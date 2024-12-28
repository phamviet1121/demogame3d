using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    public ProgressBar progressBar; // Thanh hiển thị tiến trình
    public string sceneName;        // Tên của cảnh cần tải
    public float fakeDuration = 2f; // Thời gian giả lập tải cảnh

    private AsyncOperation loadingOperation;
    private float startTime;

    public void StartLoadScene()
    {
        // Kích hoạt đối tượng và thiết lập thời gian bắt đầu
        gameObject.SetActive(true);
        DontDestroyOnLoad(this);
        startTime = Time.unscaledTime;

        // Bắt đầu tải cảnh không đồng bộ
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        Time.timeScale = 0; // Tạm dừng thời gian trò chơi
    }

    private void Update()
    {
        if (loadingOperation == null) return;

        // Tính toán tiến trình giả lập
        float fakeProgress = (Time.unscaledTime - startTime) / fakeDuration;

        // Tính toán tiến trình thực tế (giới hạn bởi tiến trình tải thực)
        float finalProgress = Mathf.Min(fakeProgress, loadingOperation.progress);

        // Cập nhật thanh tiến trình
        progressBar.SetProgressValue(finalProgress);

        // Hoàn thành tải cảnh khi đủ điều kiện
        if (loadingOperation.isDone && finalProgress >= 1f)
        {
            FinishLoading();
        }
    }

    private void FinishLoading()
    {
        // Khôi phục thời gian trò chơi và phá hủy đối tượng
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}