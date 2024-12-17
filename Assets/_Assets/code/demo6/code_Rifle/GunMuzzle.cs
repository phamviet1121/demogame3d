using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMuzzle : MonoBehaviour
{
    public Transform muzzleImage;
    public float duration;

    void Start()
    {
        HideMuzzle();
    }

    // Update is called once per frame
   public void Showmuzzle()
    {
        muzzleImage.gameObject.SetActive(true);
        float angle =Random.Range(0,360);
        muzzleImage.localEulerAngles=new Vector3(0,0,angle);
        CancelInvoke();
        Invoke(nameof(HideMuzzle), duration);

    }    
    private void HideMuzzle()=> muzzleImage.gameObject.SetActive(false);
}
