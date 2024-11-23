using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banggocquay : MonoBehaviour
{
    public Transform bk;
    public Transform toado;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion offset = Quaternion.Euler(-60f, 0f, 0f); 
        transform.rotation = bk.rotation * offset;
       // transform.rotation = bk.rotation;
        transform.position=toado.position;
    }
}
