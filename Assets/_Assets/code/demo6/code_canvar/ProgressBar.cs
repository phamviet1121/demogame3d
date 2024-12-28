using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public RectTransform rectTransform; // Vùng hiển thị tổng thể của thanh
    public RectTransform mask;          // Vùng mặt nạ để hiển thị thanh tiến trình
    public RectTransform progressImage; // Ảnh hoặc màu nền cho tiến trình

    private float maxWidth;  // Chiều rộng tối đa của thanh
    private float maxHeight; // Chiều cao tối đa của thanh

    private void Awake()
    {
        // Lấy giá trị kích thước tối đa từ mask
        maxWidth = mask.rect.width;
        maxHeight = mask.rect.height;
    }

    public void SetProgressValue(float progress)
    {
        // Đảm bảo giá trị tiến trình nằm trong khoảng [0, 1]
        progress = Mathf.Clamp01(progress);

        // Tính toán chiều rộng hiện tại theo tiến trình
        float currentWidth = progress * maxWidth;

        // Cập nhật kích thước của mask để thể hiện tiến trình
        mask.sizeDelta = new Vector2(currentWidth, maxHeight);
    }
}