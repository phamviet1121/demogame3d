using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarSpriter;
    
    //public void UpdateHealthBar(float _healthPointValue, float maxHealthPoint)
    //{
    //    HealthBarSpriter.fillAmount = _healthPointValue / maxHealthPoint;
    //}
    public void UpdateHealthBar(int _healthPointValue, int maxHealthPoint)
    {
        HealthBarSpriter.fillAmount =1f*_healthPointValue / maxHealthPoint;
    }
}
