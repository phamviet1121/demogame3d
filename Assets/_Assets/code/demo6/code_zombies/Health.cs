using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealthPoint;
    public Animator anim;
    private int healthPoint;
    private bool IsDead => healthPoint <= 0;

    void Start()
    {
        healthPoint = maxHealthPoint;
    }

   public void TakeDamage(int damage)
    {
        if(IsDead) return;
        healthPoint -= damage;
        if(IsDead)
        {
            Die();
        }
        
    }
    private void Die() => anim.SetTrigger("die");
}
