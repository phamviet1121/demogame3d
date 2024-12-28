using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera _camera;
    void Start()
    {
        _camera = Camera.main;
        looktowardcamara();
    }

    // Update is called once per frame
    void Update()
    {
        looktowardcamara();
    }
    private void looktowardcamara()
    {
        transform.forward = _camera.transform.forward;
    }
}
