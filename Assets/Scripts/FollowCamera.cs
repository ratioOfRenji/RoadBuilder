using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] float _smoothFactor;
    [SerializeField] Transform _targetTransform;
    [SerializeField] CameraSheker _camaraShaker;

    private Vector3 _cameraOffset;
    private void Awake()
    {
        _cameraOffset = transform.position + new Vector3(0,0,-1)*2;
    }
 
    private void Update()
    {      
        MoveCamera();
        RotateCamera();
    }
    private void RotateCamera()
    {
        Vector3 startEulerAngles = transform.eulerAngles;
        transform.LookAt(_targetTransform.position);

        Vector3 deltaEulerAngles = new Vector3(Mathf.DeltaAngle(startEulerAngles.x, transform.eulerAngles.x),
            Mathf.DeltaAngle(startEulerAngles.y, transform.eulerAngles.y),
            Mathf.DeltaAngle(startEulerAngles.z, transform.eulerAngles.z));
        transform.eulerAngles = startEulerAngles + deltaEulerAngles;
    }

    private void MoveCamera()
    {
        Vector3 newPosition = _targetTransform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, _smoothFactor);
    }
}
