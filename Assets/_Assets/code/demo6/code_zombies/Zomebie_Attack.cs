using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zomebie_Attack : MonoBehaviour
{
    public Animator anim;
    public int damage;
    public Health playerhealth;

    private bool isPlayerInRange = false;
    public void StartAttack()
    {

        anim.SetBool("IsAttack", true);
        Debug.Log("vả đi ");

    }
    public void StopAttack()
    {
        anim.SetBool("IsAttack", false);
        Debug.Log("tắt rồi ");
    }

    //private void OnCollider(Collider other)
    //{
    //    Debug.Log("co dc goi ko ");
    //    if (other.CompareTag("Player")) // Assuming the player has a tag "Player"
    //    {
    //        isPlayerInRange = true;
    //        Debug.Log("dã chạm vào player");
    //    }
    //    else
    //    {
    //        isPlayerInRange = false;
    //        Debug.Log("chưa chạm vào player");
    //    }
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("co dc goi ko ");
    //    if (collision.gameObject.CompareTag("Player")) // Assuming the player has a tag "Player"
    //    {
    //        isPlayerInRange = true;
    //        Debug.Log("dã chạm vào player");
    //    }
    //    else
    //    {
    //        isPlayerInRange = false;
    //        Debug.Log("chưa chạm vào player");
    //    }
    //}
    //private void OnTransformChildrenChanged()
    //{

    //}




    public void OnAttack(int index)

    {
       
        
            playerhealth.TakeDamage(damage);
            Debug.Log("có gây damage ko ");


            if (index == 1)
            {
                Player.instance.playerUI.ShowLeftScratch();
            }
            else
            {
                Player.instance.playerUI.ShowRightScratch();
            }
        
    }


    public void attack_left()
    {
        OnAttack(1);
    }
    public void attack_right()
    {
       
        OnAttack(0);
    }

      
}
