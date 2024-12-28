using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutomaticShooting : Shooting
{
    public Animator anim;
    public int rpm;

    //public Camera aimingCamara;
    //public GameObject hitMarkerPerfab;
    //public LayerMask LayerMask;

    public UnityEvent Onshoot;
    private float lastShoot;
    private float interval;

    public GunAmmo GunAmmo;

    public Gunraycaster Gunraycaster;
    void Start()
    {
        interval=60f/rpm;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKey(KeyCode.J) || Input.GetMouseButton(0))
        {
            UpdateFiring();
        }    
       
    }


    private void UpdateFiring()
    {
        if(Time.time- lastShoot >= interval)
        {
            Shoot();
            lastShoot = Time.time;
        }
    } 
    private void Shoot()
    {
        // anim.Play("shoot", layer: 0, normalizedTime: 0);
        anim.SetTrigger("shoot");
        GunAmmo.SingleFireAmmoCounter();
        Gunraycaster.PerformRaycasting();
        Onshoot.Invoke();   


    }    

    //private void PerformRaycasting()
    //{
      
    //    Ray aimingRay=new Ray(aimingCamara.transform.position, aimingCamara.transform.forward   );
    //    Debug.DrawRay(aimingRay.origin, aimingRay.direction * 1000f, Color.red, 1f);
    //    if (Physics.Raycast(aimingRay,out RaycastHit hitInfo,1000f,LayerMask))
    //    {
    //        Quaternion effectRotation = Quaternion.LookRotation(hitInfo.normal);
    //        Instantiate(hitMarkerPerfab, hitInfo.point, effectRotation);

    //    }    
    //}    
}
