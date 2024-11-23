using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ban : MonoBehaviour
{
    public GameObject luudan;
    public Transform vitri;
    public float luc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra xem người chơi có nhấn phím "J" không
        if (Input.GetKeyDown(KeyCode.J))
        {
            ThrowGrenade();
        }
    }
    void ThrowGrenade()
    {
        // Tạo lựu đạn tại vị trí ném
        GameObject grenade = Instantiate(luudan, vitri.position, vitri.rotation);

        // Lấy Rigidbody của lựu đạn

        grenade.GetComponent<Rigidbody>().AddForce(vitri.forward * luc, ForceMode.Impulse);


    }
}
