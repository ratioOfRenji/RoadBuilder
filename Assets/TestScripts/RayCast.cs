using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    [SerializeField] private Transform _currentRoad;
    [SerializeField] private Transform rayStartPoint;
    bool m_Started;
    public LayerMask m_LayerMask;
    private List<Collider> _colladerList;
    public Vector3 hitboxSize = Vector3.one;

    void Start()
    {
        _colladerList = new List<Collider>();     
        m_Started = true;

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit[] raycastHits;
            raycastHits = Physics.SphereCastAll(rayStartPoint.transform.position, 0.5f,
                transform.TransformDirection(Vector3.forward), 2f);

            foreach (var item in raycastHits)
            {
                Collider colliderRRR = item.collider;
                _colladerList.Add(colliderRRR);
              
                foreach (var tag in _colladerList)
                {
                    Debug.Log(tag.tag);
                }
            }
        }
       
    }

    public static void DrawPlus(Vector3 vector3, Vector3 size, Color color, float duration)
    {
        Vector3 v1 = vector3 - new Vector3(-size.x, 0, 0);
        Vector3 v2 = vector3 - new Vector3(size.x, 0, 0);
        Vector3 v3 = vector3 - new Vector3(0, -size.y, 0);
        Vector3 v4 = vector3 - new Vector3(0, size.y, 0);
        Vector3 v5 = vector3 - new Vector3(0, 0, -size.z);
        Vector3 v6 = vector3 - new Vector3(0, 0, size.z);

        Debug.DrawLine(v1,v2,color,duration);
        Debug.DrawLine(v3,v4,color,duration);
        Debug.DrawLine(v5,v6,color,duration);
    }

  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(rayStartPoint.position, rayStartPoint.rotation, rayStartPoint.localScale);
        //  Gizmos.DrawWireCube(Vector3.zero, hitboxSize * 2);
    }


}

