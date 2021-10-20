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
                // _colladerList.Sort();
                foreach (var tag in _colladerList)
                {
                    Debug.Log(tag.tag);
                }
            }
        }
        //  MyCollisions();
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

    void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        //      Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale);
        RaycastHit[] raycastHits;
        raycastHits = Physics.RaycastAll(rayStartPoint.transform.position,
            transform.TransformDirection(Vector3.forward) * 2.5f);
        //   Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale);
        /*   for (int i = 0; i < raycastHits.Length; i++)
           {
               RaycastHit hit = raycastHits[i];
               Collider collider = hit.collider;

               if (collider?.tag == "Build")
               {
                   Debug.Log("Build Build");


               }
               if (collider?.tag == "DefaultCollider")
               {
                   Debug.Log("DefaultCollider DefaultCollider");


               }
               if (collider?.tag == "HightPointCollider")
               {
                   Debug.Log("HightPointCollider HightPointCollider");


               }*/





        /*  foreach (var item in raycastHits)
          {
              Collider colliderRRR = item.collider;
              _colladerList.Add(colliderRRR);
              _colladerList.Sort();
              foreach (var colliderTag in _colladerList)
              {

                  if (colliderTag.tag == "Build")
                  {
                      Debug.Log("While Build");

                  }
                  if (colliderTag.tag == "DefaultCollider")
                  {
                      Debug.Log("While DefaultCollider");

                  }
                  if (colliderTag.tag == "DefaultCollider")
                  {
                      Debug.Log("While HightPointCollider");

                  }

              }
          }*/





        // int i = 0;
        //Check when there is a new collider coming into contact with the box
        /*   while (i < raycastHits.Length)
           {
               if (hitColliders[i].tag == "Build")
                   Debug.Log("While Build");

               if (hitColliders[i].tag == "DefaultCollider")
                   Debug.Log("While DefaultCollider");

               if (hitColliders[i].tag == "DefaultCollider")
                   Debug.Log("While HightPointCollider");

               // Debug.Log("Hit : " + hitColliders[i].name + i);

               i++;
           }*/
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    /*  void OnDrawGizmos()
      {
          Gizmos.color = Color.red;
      Gizmos.DrawSphere(rayStartPoint.transform.position, 0.5f);
          Vector3 direction = transform.TransformDirection(Vector3.forward) * 1.3f;           
       *//*   Gizmos.DrawRay(rayStartPoint.transform.position,
              transform.TransformDirection(Vector3.forward) * 2.5f);*//*
      }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(rayStartPoint.position, rayStartPoint.rotation, rayStartPoint.localScale);
        //  Gizmos.DrawWireCube(Vector3.zero, hitboxSize * 2);
    }


}

