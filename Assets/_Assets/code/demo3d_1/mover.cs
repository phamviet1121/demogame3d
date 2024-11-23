//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Mover : MonoBehaviour
//{
//    public float rotationSpeed = 100f;
//    public float pitchLimitmax = 160f;
//    public float pitchLimitmin = 90;

//    public float currentPitch = 0f;
//    private float yaw = 0f;

//    // Start is called before the first frame update
//    void Start()
//    {
//        Cursor.lockState = CursorLockMode.Locked;
//        //Cursor.visible = false; 
//    }


//    void Update()
//    {
//        // Get mouse input
//        float mouseX = Input.GetAxis("Mouse X");


//        yaw = mouseX * rotationSpeed * Time.deltaTime;
//        float mouseY = Input.GetAxis("Mouse Y");

//        currentPitch = mouseY * rotationSpeed * Time.deltaTime;


//        currentPitch = Mathf.Clamp(currentPitch, -pitchLimitmin, pitchLimitmax);


//       // transform.rotation = Quaternion.Euler(currentPitch, 0f, 0f);
//        transform.Rotate(currentPitch, 0f, 0f);

//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Transform playerBody;  // Thân player (bao g?m c? th? và chân)
    public Transform playerCamera; // Camera (ho?c ph?n thân trên c?a player)

    public float rotationSpeed = 100f;
    public float pitchLimitMax =120f;
    public float pitchLimitMin = -10f;

    private float currentPitch = 0f;  // Góc pitch (lên/xu?ng)
    private float yaw = 0f;  // Góc yaw (trái/ph?i)

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Khóa chu?t
        Cursor.visible = false;  // ?n chu?t
    }

    void Update()
    {
        // L?y input chu?t (di chuy?n chu?t theo chi?u ngang và d?c)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Quay playerBody (thân player) theo tr?c Yaw (trái/ph?i)
        yaw += mouseX * rotationSpeed * Time.deltaTime;
        playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);  // Quay player theo yaw (tr?c Y)

        // Quay camera (ho?c thân trên c?a player) theo tr?c Pitch (lên/xu?ng)
        currentPitch -= mouseY * rotationSpeed * Time.deltaTime;  // Quay xu?ng khi chu?t di chuy?n xu?ng
        currentPitch = Mathf.Clamp(currentPitch, pitchLimitMin, pitchLimitMax);  // Gi?i h?n góc quay lên/xu?ng
        playerCamera.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);  // Quay camera theo pitch (tr?c X)
    }
}
