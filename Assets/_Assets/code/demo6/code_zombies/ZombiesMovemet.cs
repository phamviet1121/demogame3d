using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ZombiesMovemet : MonoBehaviour
{
    private Transform playerFoot;
    public Animator anim;
    public NavMeshAgent agent;
    public float reachingRadius;

    public float rotationSpeed;
    public UnityEvent onDestinationReached;
    public UnityEvent onStartMoving;

    private bool _ismovingValue;
    public bool IsMoving
    {
        get => _ismovingValue;

        private set
        {
            if (_ismovingValue == value) return;
            _ismovingValue = value;
            OnIsMovingValueChanged();
        }
    }    
    private void OnIsMovingValueChanged()
    {
        agent.isStopped = !_ismovingValue;
        anim.SetBool("IsWalking", _ismovingValue);
        if(_ismovingValue )
        {
            onStartMoving.Invoke();
        }
        else
        {
            onDestinationReached.Invoke();
        }    
    }    
    private void Start()
    {
        playerFoot = Player.instance.playerFoot;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerFoot.position);
        
        IsMoving= distance > reachingRadius;
        if( IsMoving )
        {
            agent.SetDestination(playerFoot.position);
        }
       

        //if (distance > reachingRadius)
        //{
        //    agent.isStopped = false;
        //    agent.SetDestination(playerFoot.position);
        //    anim.SetBool("IsWalking", true);
        //}
        //else
        //{
        //    agent.isStopped = true;
        //    anim.SetBool("IsWalking", false);
        //}
        //if (anim.GetBool("die"))
        //{
        //    agent.isStopped = true;
        //}
    }
    public void Ondie()
    {
        enabled = false;
        agent.isStopped = true;
    }
    public void rotationZombie()
    {
        //Vector3 direction = (playerFoot.position - transform.position).normalized;
        //if (direction != Vector3.zero) 
        //{
        //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        //}
        // Tính toán hướng từ zombie đến player
        Vector3 direction = (playerFoot.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            // Tính toán góc cần quay
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            // Chuyển đổi góc quay hiện tại theo thời gian từng bước nhỏ
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
