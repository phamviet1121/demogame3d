using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class GunAmmo : MonoBehaviour
{
   // public TextMeshProUGUI soddan;
   
    public int magsize;
    public Shooting Shooting;
    //  public GrenadeLauncher gun;
    private int _loadedAmmo;
    public UnityEvent loadeedAmmoChanged;
    public Animator anim;
    public int loadedAmmo
    {
        get => _loadedAmmo;
        set
        {
            _loadedAmmo=value;
            loadeedAmmoChanged.Invoke();
            if (_loadedAmmo<=0)
            {
                Reload();
            }
            else
            {
                UnlockShooting();
            }    
        }
    }    
    void Start()
    {
        RefillAmmo();
    }

    public void SingleFireAmmoCounter() => loadedAmmo--;
    private void LockShooting() { Shooting.enabled = false; }
    private void UnlockShooting() { Shooting.enabled = true; }
    public void OnGunSelected() { UpdateShootingLock(); loadeedAmmoChanged.Invoke(); }
    private void UpdateShootingLock()=>Shooting.enabled = _loadedAmmo > 0;
    //private void UpdateShootingLock() => Shooting.enabled =( _loadedAmmo > 0)?true:false;

    private void Update()
    {
       // soddan.text = loadedAmmo.ToString();


        if (Input.GetKeyDown(KeyCode.U))
        {
            Reload();
        }    
    }
    public void Reload()
    {
        anim.SetTrigger("reload");
        LockShooting();
    }    
    public void addAmmo()
    {
        RefillAmmo();
    }    
  private void RefillAmmo()
    {
        loadedAmmo = magsize;
    }

}
