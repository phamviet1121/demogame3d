using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Shooting
{
    private const int LeftMouseButton=0;
    public GameObject bulletprefab;
    public Transform firingPos;
    public float bulletSpeed;
    public Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.J))
        {
            shootbulet();
        }
    }
    private void shootbulet() => anim.SetTrigger("shoot");
    public void addprojectile()
    {
        GameObject bullet=Instantiate(bulletprefab, firingPos.position, firingPos.rotation);
        bullet.GetComponent<Rigidbody>().velocity = firingPos.forward* bulletSpeed;
    }
}
