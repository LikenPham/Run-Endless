using UnityEngine;
using TMPro; // Bắt buộc dòng này để dùng TextMeshPro

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Kéo Text điểm số vào đây

    private void Update()
    {
        // Cập nhật text liên tục mỗi frame
        if (ScoreManager.Instance != null && scoreText != null)
        {
            // Chuyển float thành int để cho đẹp (bỏ số lẻ)
            int scoreInt = Mathf.FloorToInt(ScoreManager.Instance.Score);

            // Format "D6" nghĩa là luôn hiện 6 chữ số: 000123
            scoreText.text = scoreInt.ToString("D6");
        }
    }
}