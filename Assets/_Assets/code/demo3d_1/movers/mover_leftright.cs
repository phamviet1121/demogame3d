using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover_leftright : MonoBehaviour
{
    public float rotationSpeed = 100f;  // Tốc độ xoay
    public float pitchLimitMax = 80f;  // Giới hạn góc trên (độ)
    public float pitchLimitMin = -80f; // Giới hạn góc dưới (độ)

    private float yaw = 0f;            // Góc xoay ngang (trục Y)
    private float pitch = 0f;          // Góc xoay dọc (trục X)

    void Update()
    {
        // Lấy giá trị chuột từ Input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Tính toán góc xoay ngang (yaw)
        yaw += mouseX * rotationSpeed * Time.deltaTime;

        // Tính toán góc xoay dọc (pitch)
        pitch -= mouseY * rotationSpeed * Time.deltaTime; // Dùng dấu trừ vì trục Y của chuột thường ngược hướng xoay
        pitch = Mathf.Clamp(pitch, pitchLimitMin, pitchLimitMax); // Giới hạn pitch trong khoảng min/max

        // Áp dụng góc xoay cho đối tượng
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
