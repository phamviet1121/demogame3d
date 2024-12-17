using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunraycaster : MonoBehaviour
{
    public Camera aimingCamara;
    public GameObject hitMarkerPerfab;
    public LayerMask LayerMask;

    public int damage;
    void Start()
    {
        
    }
    public void PerformRaycasting()
    {

        Ray aimingRay = new Ray(aimingCamara.transform.position, aimingCamara.transform.forward);
        Debug.DrawRay(aimingRay.origin, aimingRay.direction * 1000f, Color.red, 1f);
        if (Physics.Raycast(aimingRay, out RaycastHit hitInfo, 1000f, LayerMask))
        {
            Quaternion effectRotation = Quaternion.LookRotation(hitInfo.normal);
            Instantiate(hitMarkerPerfab, hitInfo.point, effectRotation);
            deliveDamage(hitInfo);
        }
    }

  
    private void deliveDamage(RaycastHit hitInfo)
    {
        Health health = hitInfo.collider.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }    
}
