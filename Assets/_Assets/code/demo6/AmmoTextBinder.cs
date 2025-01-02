using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoTextBinder : MonoBehaviour
{
    public TMP_Text loadedAmmoText;
    public GunAmmo gunAmmo;

    void Start()
    {
        onstart();
        updateGunAmmo();
    }
    public void onstart() => gunAmmo.loadeedAmmoChanged.AddListener(updateGunAmmo);

    //public void updateGunAmmo()
    //{
    //    loadedAmmoText.text = gunAmmo.loadedAmmo.ToString();
    //}
    public void updateGunAmmo() => loadedAmmoText.text = gunAmmo.loadedAmmo.ToString();

}
