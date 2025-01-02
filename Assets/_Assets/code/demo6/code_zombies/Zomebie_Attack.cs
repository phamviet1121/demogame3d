//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Zomebie_Attack : MonoBehaviour
//{
//    public Animator anim;
//    public int damage;
//    public Health playerhealth;

//    private bool isPlayerInRange = false;
//    public void StartAttack()
//    {

//        anim.SetBool("IsAttack", true);
//        Debug.Log("vả đi ");

//    }
//    public void StopAttack()
//    {
//        anim.SetBool("IsAttack", false);
//        Debug.Log("tắt rồi ");
//    }

//    //private void OnCollider(Collider other)
//    //{
//    //    Debug.Log("co dc goi ko ");
//    //    if (other.CompareTag("Player")) // Assuming the player has a tag "Player"
//    //    {
//    //        isPlayerInRange = true;
//    //        Debug.Log("dã chạm vào player");
//    //    }
//    //    else
//    //    {
//    //        isPlayerInRange = false;
//    //        Debug.Log("chưa chạm vào player");
//    //    }
//    //}
//    //private void OnCollisionEnter(Collision collision)
//    //{
//    //    Debug.Log("co dc goi ko ");
//    //    if (collision.gameObject.CompareTag("Player")) // Assuming the player has a tag "Player"
//    //    {
//    //        isPlayerInRange = true;
//    //        Debug.Log("dã chạm vào player");
//    //    }
//    //    else
//    //    {
//    //        isPlayerInRange = false;
//    //        Debug.Log("chưa chạm vào player");
//    //    }
//    //}
//    //private void OnTransformChildrenChanged()
//    //{

//    //}




//    public void OnAttack(int index)

//    {
       
        
//            playerhealth.TakeDamage(damage);
//            Debug.Log("có gây damage ko ");


//            if (index == 1)
//            {
//                Player.instance.playerUI.ShowLeftScratch();
//            }
//            else
//            {
//                Player.instance.playerUI.ShowRightScratch();
//            }
        
//    }


//    public void attack_left()
//    {
//        OnAttack(1);
//    }
//    public void attack_right()
//    {
       
//        OnAttack(0);
//    }


//}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zomebie_Attack : MonoBehaviour
{
    public Animator anim;
    public int damage;
    private Health playerhealth;




    private bool isPlayerInRangeLeft = false;
    private bool isPlayerInRangeRight = false;

    public GameObject palm_zomebie_left;
    public GameObject palm_zomebie_right;
    public float explosionRadius;

    private void Start()
    {
        playerhealth=Player.instance.health;
    }
    public void StartAttack()
    {

        anim.SetBool("IsAttack", true);
      //  Debug.Log("vả đi ");

    }
    public void StopAttack()
    {
        anim.SetBool("IsAttack", false);
       // Debug.Log("tắt rồi ");
    }

    //private void Update()
    //{
    //    OncolliderZomebie_Right();
    //    OncolliderZomebie_left();
    //    Debug.Log($"isPlayerInRangeLeft:{isPlayerInRangeLeft} ");
    //    Debug.Log($"isPlayerInRangeLeft:{isPlayerInRangeLeft} ");

    //}


    //public void OncolliderZomebie_left()
    //{
    //    Collider[] affectedObjects = Physics.OverlapSphere(palm_zomebie_left.transform.position, explosionRadius);
    //    for (int i = 0; i < affectedObjects.Length; i++)
    //    {
    //        if (affectedObjects[i].CompareTag("Player"))
    //        {
    //            isPlayerInRangeLeft = true;

    //        }
    //        else
    //        {
    //            isPlayerInRangeLeft = false;

    //        }

    //    }

    //}
    //public void OncolliderZomebie_Right()
    //{
    //    Collider[] affectedObjects = Physics.OverlapSphere(palm_zomebie_right.transform.position, explosionRadius);
    //    for (int i = 0; i < affectedObjects.Length; i++)
    //    {
    //        if (affectedObjects[i].CompareTag("Player"))
    //        {
    //            isPlayerInRangeRight = true;

    //        }
    //        else
    //        {
    //            isPlayerInRangeRight = false;

    //        }

    //    }

    //}
    //private void OnDrawGizmos()
    //{
    //    // Chọn màu cho Gizmos
    //    Gizmos.color = Color.red;

    //    // Vẽ vòng tròn tại vị trí của palm_zomebie với bán kính explosionRadius
    //    Gizmos.DrawWireSphere(palm_zomebie_left.transform.position, explosionRadius);
    //    Gizmos.DrawWireSphere(palm_zomebie_right.transform.position, explosionRadius);
    //}



    public void OnAttack(int index)

    {

        playerhealth.TakeDamage(damage);
       // Debug.Log("có gây damage ko ");


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
        //    if (isPlayerInRangeLeft)
        //    {
        //        OnAttack(1);
        //    }
        OnAttack(1);
    }
    public void attack_right()
    {
        //if (isPlayerInRangeRight)
        //{
        //    OnAttack(0);
        //}
        OnAttack(0);
    }


}