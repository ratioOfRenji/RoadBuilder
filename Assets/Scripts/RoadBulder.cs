using Assets.Scripts.Interfaces;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoadBulder : ObjectSpawner, IRotator, IUnseen, IVisible
{
    [SerializeField] private Transform _currentRoad;
    [SerializeField] private Transform _ground;
    [SerializeField] private Camera _myCamera;
    [SerializeField] public int scoreDefault = 10;
    [SerializeField] public int scoreIncrease = 20;
    [SerializeField] private RayStartPoint rayStartPoint;

    private House _house;
    private HouseTrigger _trigger;
    private bool IsCanSpawn = false;

    public bool IsDefaultCollider = false;
    public UnityAction<int> OnRoadBuild;

    private void Awake()
    {
        _currentRoad = SpawnRoad();
        SetDefaultRoad();
        SetUnseenObj();
    }
    void Start()
    {    
        Rotate();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            CheckObstacle();
            SetEnvironment();
            if (!IsCanSpawn)
            {
                AnchorObj();
                SetNewPointPosition();
            }
        }
    }

    public void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Tween tween = transform.DORotate(new Vector3(0, 360, 0), 3.5f, RotateMode.LocalAxisAdd).
            SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
    public void SetUnseenObj()
    {
        GameObject carChild = GetCar();
        carChild.gameObject.SetActive(false);

    }

    private void SetDefaultRoad()
    {
        GameObject roadChild = _currentRoad.GetChild(1).GetChild(0).gameObject;
        roadChild.gameObject.SetActive(true);
    }

    public void SetVisibleObj()
    {
        int randomChild = Random.Range(0, 3);
        GameObject roadChild = _currentRoad.GetChild(1).GetChild(0).gameObject;
        roadChild.gameObject.SetActive(false);

        roadChild = GetRoad();
        roadChild.gameObject.SetActive(true);

        roadChild = _currentRoad.GetChild(1).GetChild(randomChild).gameObject;
        roadChild.gameObject.SetActive(true);

        GameObject carChild = GetCar();
        carChild.gameObject.SetActive(true);

        carChild = _currentRoad.GetChild(0).GetChild(randomChild).gameObject;
        carChild.gameObject.SetActive(true);
    }

    private GameObject GetCar() => _currentRoad.GetChild(0).gameObject;
    private GameObject GetRoad() => _currentRoad.GetChild(1).gameObject;
    private GameObject GetBusStation() => _currentRoad.GetChild(2).GetChild(0).gameObject;
    private GameObject GetFlower() => _currentRoad.GetChild(3).GetChild(0).gameObject;
    private GameObject GetLamp() => _currentRoad.GetChild(4).GetChild(0).gameObject;
    private GameObject GetTree() => _currentRoad.GetChild(5).GetChild(0).gameObject;
   
    private void AnchorObj()
    {
        _currentRoad.SetParent(_ground, true);
        SetVisibleObj();
    }
    private void SetNewPointPosition()
    {
        _currentRoad = SpawnRoad();
        Transform endPoint = _currentRoad.GetComponentInChildren<EndPointOfSpawn>().transform;
        transform.position = endPoint.position;
        SetUnseenObj();

    }
    private void CheckTriggerLose()
    {
        _trigger = gameObject.GetComponentInChildren<HouseTrigger>();
        if (_trigger.IsTriggerLose)
        {
            IsCanSpawn = true;
            transform.DOPause();
            _myCamera.GetComponent<UIManager>().Lose();
        }
    }
    private void CheckTriggerWin()
    {
        _trigger = gameObject.GetComponentInChildren<HouseTrigger>();
        if (_trigger.IsTriggerWin)
        {
            IsCanSpawn = true;
            transform.DOPause();
            _myCamera.GetComponent<UIManager>().Win();
        }
    }
    private void SetEnvironment()
    {
        GameObject busChild = GetBusStation();
        GameObject flowerChild = GetFlower();
        GameObject lampChild = GetLamp();
        GameObject treeChild = GetTree();

        Collider[] hitBusColliders = Physics.OverlapSphere(busChild.transform.position, transform.localScale.x / 2f);
        RayCast.DrawPlus(busChild.transform.position, transform.localScale / 2f, Color.red, 1000);

        for (int i = 0; i < hitBusColliders.Length; i++)
        {
            if (hitBusColliders[i].tag == "Build")
                busChild.gameObject.SetActive(false);
            else
                busChild.gameObject.SetActive(true);
        }

        Collider[] hitFlowerColliders = Physics.OverlapSphere(flowerChild.transform.position, transform.localScale.x / 2f);

        for (int i = 0; i < hitFlowerColliders.Length; i++)
        {
            if (hitFlowerColliders[i]?.tag == "Build")
                flowerChild?.gameObject.SetActive(false);
            else
                flowerChild?.gameObject.SetActive(true);
        }

        Collider[] hitLampColliders = Physics.OverlapSphere(lampChild.transform.position, transform.localScale.x / 2f);

        for (int i = 0; i < hitLampColliders.Length; i++)
        {
            if (hitLampColliders[i]?.tag == "Build")
                lampChild?.gameObject.SetActive(true);
            else
            {
                lampChild?.gameObject.SetActive(false);
            }
        }

        Collider[] hitTreeColliders = Physics.OverlapSphere(treeChild.transform.position, transform.localScale.x / 2f);

        for (int i = 0; i < hitTreeColliders.Length; i++)
        {
            if (hitTreeColliders[i]?.tag == "Build")
                treeChild?.gameObject.SetActive(true);
            else
                treeChild?.gameObject.SetActive(false);
        }
    }

    private void CheckObstacle()
    {       
        int score = 0;
        var position = _currentRoad.position;

        RaycastHit[] raycastHits;
        raycastHits = Physics.SphereCastAll(rayStartPoint.transform.position + position, 0.5f,
            transform.TransformDirection(Vector3.forward), 3f);

        Debug.DrawLine(rayStartPoint.transform.position + position, (transform.TransformDirection(Vector3.forward) * 3)
            + rayStartPoint.transform.position + position, Color.yellow, 1000);

        for (int i = 0; i < raycastHits.Length; i++)
        {
            RaycastHit hit = raycastHits[i];
            Collider collider = hit.collider;

            if (collider?.tag == "Build")
            {             
                CheckTriggerLose();
                return;
            }
            if (collider?.tag == "Finish")
            {
                CheckTriggerWin();
                return;
            }

            if (collider?.tag == "DefaultCollider")
            {
                SetVisibleSideWalk(collider);
                DisableCollider(collider);
                score += scoreDefault;             
            }

            if (collider?.tag == "HightPointCollider")
            {
                SetVisibleSideWalk(collider);
                DisableCollider(collider);
                score += scoreIncrease;             
            }
        }
        if (score != 0)
        {
            TextScoreUI.Instance.AddText(score, _currentRoad.position);
            OnRoadBuild?.Invoke(score);
        }
    }

    private static void DisableCollider(Collider collider)
    {
        var defaultCollider = collider?.gameObject.GetComponent<Collider>();
        defaultCollider.enabled = false;
    }

    private static void SetVisibleSideWalk(Collider collider)
    {
        collider?.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}

