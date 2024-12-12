using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroygameobject : MonoBehaviour
{
   
        
    public float time;
    public float newtime = 10f;
    void Start()
    {
        time = newtime;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
           
            Destroy(gameObject);

        }
    }
}
