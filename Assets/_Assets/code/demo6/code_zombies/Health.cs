using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealthPoint;
    public Animator anim;
    private int healthPoint;

    public UnityEvent onDie;
    private bool IsDead => healthPoint <= 0;

    void Start()
    {
        healthPoint = maxHealthPoint;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{damage}:{healthPoint}");
        if (IsDead) return;
        healthPoint -= damage;
        if (IsDead)
        {
            Die();
        }

    }
    private void Die()
    {
     
        anim.SetTrigger("die");
        Debug.Log(anim);

        onDie.Invoke();

    }
}
