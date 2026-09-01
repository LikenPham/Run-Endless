using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float scoreMultiplier = 1f; // Hệ số nhân điểm (ví dụ ăn x2 thì tăng cái này)

    public float Score { get; private set; }
    public float HighScore { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        Score = 0f;

        // Load điểm cao nhất từ bộ nhớ máy (nếu chưa có thì mặc định là 0)
        HighScore = PlayerPrefs.GetFloat("HighScore", 0);
    }

    private void Update()
    {
        // Chỉ tính điểm khi game ĐANG CHẠY và CHƯA GAME OVER
        if (GameManager.Instance.IsGameStarted && !GameManager.Instance.IsGameOver)
        {
            // Công thức: Điểm cộng thêm = Tốc độ hiện tại * Thời gian * Hệ số
            // Cách này hay ở chỗ: Về sau game chạy nhanh -> Điểm nhảy vù vù -> Kích thích người chơi
            float speed = GameManager.Instance.CurrentSpeed;
            Score += speed * Time.deltaTime * scoreMultiplier;
        }
    }

    // Hàm gọi khi chết để lưu điểm cao
    public void CheckHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetFloat("HighScore", HighScore);
            PlayerPrefs.Save(); // Lưu xuống ổ cứng ngay lập tức
        }
    }
}