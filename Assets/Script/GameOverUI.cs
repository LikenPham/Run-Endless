using TMPro; // Nhớ thêm thư viện này
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI currentScoreText; // Text hiện điểm ván này
    [SerializeField] private TextMeshProUGUI highScoreText;    // Text hiện kỷ lục
    [SerializeField] private string menuSceneName = "Menu";
    private void Start()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver += ShowGameOverPanel;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameOver -= ShowGameOverPanel;
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);

        // 1. Bảo ScoreManager check và lưu điểm cao
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.CheckHighScore();

            // 2. Hiển thị lên UI
            int currentScore = Mathf.FloorToInt(ScoreManager.Instance.Score);
            int highScore = Mathf.FloorToInt(ScoreManager.Instance.HighScore);

            if (currentScoreText) currentScoreText.text = "SCORE: " + currentScore;
            if (highScoreText) highScoreText.text = "BEST: " + highScore;
        }
    }

    // Hàm cho nút "Replay" (Chơi lại ngay màn này)
    public void OnReplayButtonPressed()
    {
        // Load lại Scene hiện tại (Gameplay)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Hàm cho nút "Home" (Về Menu chính)
    public void OnHomeButtonPressed()
    {
        // Load Scene Menu
        SceneManager.LoadScene(menuSceneName);
    }
}