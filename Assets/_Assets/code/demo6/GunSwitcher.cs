using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitcher : MonoBehaviour
{
    public GameObject[] guns;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
               // Debug.Log($"co chy ko{i}");
                SetActiveGun(i);
            }
        }

    }

    private void SetActiveGun(int gunIndex)
    {
      //  Debug.Log($"co {gunIndex}");
        for (int i = 0; i < guns.Length; i++)
        {
            //Debug.Log($" ko{i}");
           //Debug.Log("la doi chua");
            bool isAtive = (i == gunIndex);
            guns[i].SetActive(isAtive);
            //Debug.Log($"  dcm ko{isAtive}");
            if (isAtive)
            {
                guns[i].SendMessage("OnGunSelected", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
