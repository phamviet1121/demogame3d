using UnityEngine;
using UnityEngine.UI; // Thêm để sử dụng UI Image

public class RaycastZombie : MonoBehaviour
{
    public Camera aimingCamera; // Camera để bắn ray
    public Image tamImg; // UI Image sẽ đổi màu
    public Color hitColor = Color.red; // Màu khi trúng zombie
    public Color defaultColor = Color.white; // Màu mặc định
    public float rayDistance = 1000f; // Khoảng cách tối đa của raycast

    void Update()
    {
        // Tạo ray từ camera
        Ray aimingRay = new Ray(aimingCamera.transform.position, aimingCamera.transform.forward);
        RaycastHit hit;

        // Kiểm tra nếu ray trúng vật
        if (Physics.Raycast(aimingRay, out hit, rayDistance))
        {
            // Kiểm tra nếu trúng zombie (đối tượng có tag "Zombie")
            if (hit.collider.CompareTag("zomebie"))
            {
                // Đổi màu tam_img
                tamImg.color = hitColor;
            }
            else
            {
                // Trả về màu mặc định nếu không trúng zombie
                tamImg.color = defaultColor;
            }
        }
        else
        {
            // Trả về màu mặc định nếu ray không trúng gì
            tamImg.color = defaultColor;
        }
    }
}
