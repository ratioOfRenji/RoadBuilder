using UnityEngine;

public class HouseTrigger : MonoBehaviour 
{
    [SerializeField] private RoadBulder _roadBuilder;
    [SerializeField] private Material _loseColors;
    [SerializeField] private Material _winColor;
    [SerializeField] private Material _roadColor;
    [SerializeField] private Material _roadVarColor;

    private Material[] _currentColor;
    private House[] _houseChild;
    
    public bool IsTriggerLose = false;
    public bool IsTriggerWin = false;

    private void Start()
    {
        _currentColor = gameObject.GetComponentInChildren<MeshRenderer>().materials;
        _houseChild = FindObjectsOfType<House>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Build")
        {
            for (int i = 0; i <= 1; i++)
                _currentColor[i].color = _loseColors.color;
            IsTriggerLose = true;
        }
        if (other.tag == "Finish")
        {
            for (int i = 0; i <= 1; i++)
                _currentColor[i].color = _winColor.color;

            IsTriggerWin = true;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Build")
        {
            for (int i = 0; i <= 1; i++)
                _currentColor[i].color = _loseColors.color;

            IsTriggerLose = true;
        }
        if (other.tag == "Finish")
        {
            for (int i = 0; i <= 1; i++)
                _currentColor[i].color = _winColor.color;

            IsTriggerWin = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Build")
        {
            _currentColor[0].color = _roadColor.color;
            _currentColor[1].color = _roadVarColor.color;

            IsTriggerLose = false;
        }
        if (other.tag == "Finish")
        {
            _currentColor[0].color = _roadColor.color;
            _currentColor[1].color = _roadVarColor.color;

            IsTriggerWin = false;
        }
    }

  
}

