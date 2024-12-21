using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ZombiesMovemet : MonoBehaviour
{
    public Transform playerFoot;
    public Animator anim;
    public NavMeshAgent agent;
    public float reachingRadius;

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
    void Start()
    {

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
        if (anim.GetBool("die"))
        {
            agent.isStopped = true;
        }
    }

}
