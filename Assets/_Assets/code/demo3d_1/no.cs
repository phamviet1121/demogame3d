using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class no : MonoBehaviour
{
    public GameObject boom;
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
            vuno();
            Destroy(gameObject);
          
        }
    }
    public void OnCollisionEnter(Collision collision)
    {

        Invoke("vuno", 2f);
    }
    void vuno()
    {
        GameObject newboom = Instantiate(boom, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
