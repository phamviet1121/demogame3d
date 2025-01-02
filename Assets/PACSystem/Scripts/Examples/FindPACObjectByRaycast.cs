using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orbitami.PacSystem;

public class FindPACObjectByRaycast : MonoBehaviour
{
    public Transform raycastOriginPoint;
    public float raycastDistance;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            PointAndClickSystem.FireRaycast(raycastOriginPoint.position, raycastOriginPoint.transform.forward, raycastDistance);
            //Debug.Log("Detected Button Down. Firing Point and Click System Raycast Method.");
        }
    }
}
