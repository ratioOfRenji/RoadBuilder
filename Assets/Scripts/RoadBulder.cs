using Assets.Scripts.Interfaces;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoadBulder : ObjectSpawner, IRotator, IUnseen, IVisible
{
    private const string SCORE_COLLIDER = "ScoreCollider";
    private const string SIDE_WALK_COLLIDER = "SideWalkCollider";
    private const string BUILD = "Build";
    private const string FINISH = "Finish";

    [SerializeField] private Transform _currentRoad;
    [SerializeField] private Transform _ground;
    [SerializeField] private Camera _myCamera;
    [SerializeField] public int scoreDefault = 10;
    [SerializeField] public int scoreIncrease = 20;
    [SerializeField] private RayStartPoint rayStartPoint;

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
            if (!IsCanSpawn)
            {
                SetEnvironment();
                AnchorObj();
                SetNewPointPosition();
            }
        }
    }

    public void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Tween tween = transform.DORotate(new Vector3(0, 360, 0), 2.5f, RotateMode.LocalAxisAdd).
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

    public void SetVisibleObjects()
    {
        int randomChild = Random.Range(0, 3);
        SetRoad(randomChild);
        SetCar(randomChild);
        GameObject roadSideWalk = _currentRoad.GetChild(1).GetChild(0).GetChild(0).gameObject;
        roadSideWalk.gameObject.SetActive(true);
    }

    private void SetCar(int randomChild)
    {
        GameObject carChild = GetCar();
        carChild.gameObject.SetActive(true);

        carChild = _currentRoad.GetChild(0).GetChild(randomChild).gameObject;
        carChild.gameObject.SetActive(true);
    }

    private void SetRoad(int randomChild)
    {
        GameObject roadChild = _currentRoad.GetChild(1).GetChild(0).gameObject;
        roadChild.gameObject.SetActive(false);

        roadChild = GetRoad();
        roadChild.gameObject.SetActive(true);

        roadChild = _currentRoad.GetChild(1).GetChild(randomChild).gameObject;
        roadChild.gameObject.SetActive(true);
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
        SetVisibleObjects();
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
        SetBusStation();
        SetFlower();
        SetLamp();
        SetTree();
    }
    private void SetBusStation()
    {
        GameObject busChild = GetBusStation();

        Collider[] hitBusColliders = Physics.OverlapSphere(busChild.transform.position, transform.localScale.x / 2f);
        RayCast.DrawPlus(busChild.transform.position, transform.localScale / 3f, Color.red, 1000);

        for (int i = 0; i < hitBusColliders.Length; i++)
        {
            if (hitBusColliders[i].tag == BUILD)
                busChild.gameObject.SetActive(false);
            else
                busChild.gameObject.SetActive(true);
        }
    }
    private void SetTree()
    {
        GameObject treeChild = GetTree();

        Collider[] hitTreeColliders = Physics.OverlapSphere(treeChild.transform.position, transform.localScale.x / 2f);

        for (int i = 0; i < hitTreeColliders.Length; i++)
        {
            if (hitTreeColliders[i]?.tag == BUILD)
                treeChild?.gameObject.SetActive(false);
            else
                treeChild?.gameObject.SetActive(true);
        }
    }
    private void SetLamp()
    {
        GameObject lampChild = GetLamp();

        Collider[] hitLampColliders = Physics.OverlapSphere(lampChild.transform.position, transform.localScale.x / 2f);

        for (int i = 0; i < hitLampColliders.Length; i++)
        {
            if (hitLampColliders[i]?.tag == BUILD)
                lampChild?.gameObject.SetActive(false);
            else
                lampChild?.gameObject.SetActive(true);
        }
    }
    private void SetFlower()
    {
        GameObject flowerChild = GetFlower();

        Collider[] hitFlowerColliders = Physics.OverlapSphere(flowerChild.transform.position, transform.localScale.x / 2f);

        for (int i = 0; i < hitFlowerColliders.Length; i++)
        {
            if (hitFlowerColliders[i]?.tag == BUILD)
                flowerChild?.gameObject.SetActive(false);
            else
                flowerChild?.gameObject.SetActive(true);
        }
    }
    private void CheckObstacle()
    {
        var position = _currentRoad.position;
        int score = 0;

        RaycastHit[] raycastHits;
        raycastHits = Physics.SphereCastAll(rayStartPoint.transform.position + position, 0.5f,
            transform.TransformDirection(Vector3.forward), 3f);

        Debug.DrawLine(rayStartPoint.transform.position + position, (transform.TransformDirection(Vector3.forward) * 3)
            + rayStartPoint.transform.position + position, Color.yellow, 1000);

        for (int i = 0; i < raycastHits.Length; i++)
        {
            RaycastHit hit = raycastHits[i];
            Collider collider = hit.collider;

            if (collider?.tag == BUILD)
            {
                CheckTriggerLose();
                return;
            }
            if (collider?.tag == FINISH)
            {
                CheckTriggerWin();
                return;
            }

            if (collider?.tag == SCORE_COLLIDER)
            {
                DisableCollider(collider);
                score += scoreDefault;
                SetPointsVisible(score);
            }

            if (collider?.tag == SIDE_WALK_COLLIDER)
            {
                SetVisibleSideWalk(collider);
                DisableCollider(collider);
            }
        }
    }

    private void SetPointsVisible(int score)
    {
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

