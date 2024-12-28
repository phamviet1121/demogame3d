using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Start()
    {
        // Ẩn con trỏ chuột và khóa nó vào giữa màn hình
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Nhấn phím Escape để hiển thị lại con trỏ chuột
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
