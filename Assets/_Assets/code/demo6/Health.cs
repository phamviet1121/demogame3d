using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealthPoint;
    // public Animator anim;


    public UnityEvent onDie;
    //  public UnityEvent<int, int> onHealth;

    public HealthBar HealthBar;
    private int _healthPointValue;
    private int healthPoint;
    //{
    //    get=> _healthPointValue;
    //   set
    //    {
    //        _healthPointValue = value;

    //        onHealth.Invoke(_healthPointValue, maxHealthPoint);
    //    }
    //}


    private bool IsDead => healthPoint <= 0;

    void Start()
    {
        healthPoint = maxHealthPoint;
        HealthBar.UpdateHealthBar(healthPoint, maxHealthPoint);
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
        HealthBar.UpdateHealthBar(healthPoint, maxHealthPoint);
    }
    private void Die()
    {

        //anim.SetTrigger("die");
        //Debug.Log(anim);

        onDie.Invoke();

    }
}
