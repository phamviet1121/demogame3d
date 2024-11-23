using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huong : MonoBehaviour
{
    public Vector3 target;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lookATCursor();


    }

    void lookATCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;

        }
        transform.LookAt(target);
    }
}
