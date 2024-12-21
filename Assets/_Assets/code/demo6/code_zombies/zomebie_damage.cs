using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zomebie_damage : MonoBehaviour
{
    public Zomebie_Attack Zomebie_Attack;
    public int i;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("co dc goi ko ");
        if (collision.gameObject.CompareTag("Player")) 
        {
            Zomebie_Attack.OnAttack(i);
            gameObject.SetActive(false);
        }
       
    }
}
