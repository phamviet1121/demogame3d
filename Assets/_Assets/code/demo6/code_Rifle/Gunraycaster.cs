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
            //Quaternion effectRotation = Quaternion.LookRotation(hitInfo.normal);
            //Instantiate(hitMarkerPerfab, hitInfo.point, effectRotation);
            ShowHitEffect(hitInfo);
            deliveDamage(hitInfo);
            AddForceToObject(hitInfo);


        }
    }

  private void ShowHitEffect(RaycastHit hitInfo)
    {
        HitSurface hitSurface= hitInfo.collider.GetComponent<HitSurface>();
        if(hitSurface!=null)
        {
            GameObject effectPrefab=HitEffectManager.instance.GetEffectPrefab(hitSurface.surfaceType);
            if(effectPrefab!=null)
            {
                Quaternion effectRotation = Quaternion.LookRotation(hitInfo.normal);
                Instantiate(effectPrefab, hitInfo.point, effectRotation);
            }
        }    
    }    
    private void deliveDamage(RaycastHit hitInfo)
    {
        Debug.Log($"đã bắn chưa  "); 
        Health health = hitInfo.collider.GetComponentInParent<Health>();
        if (health != null)
        { 
            Debug.Log($"có chay ko ");
            health.TakeDamage(damage);
        }
    }
    //private void AddForceToObject(Collider affecterObject)
    //{
    //    Rigidbody rb = affecterObject.attachedRigidbody;
    //    if (rb)
    //    {
    //        rb.AddExplosionForce(0.1f, transform.position, 0.1f, 1, ForceMode.Impulse);
    //    }
    //}
    private void AddForceToObject(RaycastHit hitInfo)
    {
        Rigidbody rb = hitInfo.collider.attachedRigidbody;
        if (rb != null)
        {
           
            Vector3 forceDirection = hitInfo.normal; 
            // Tác dụng lực tại vị trí va chạm
            rb.AddForceAtPosition(forceDirection * 5f, hitInfo.point, ForceMode.Impulse);
        }
    }
}
