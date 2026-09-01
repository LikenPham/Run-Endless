using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Flow")]
    [SerializeField] private float startDelay = 2f;

    [Header("Speed Settings")]
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private string menuSceneName = "Menu";

    // Event để báo game bắt đầu
    public event Action OnGameStarted;

    // --- [QUAN TRỌNG] THÊM DÒNG NÀY ĐỂ SỬA LỖI BÊN UI ---
    // Event để báo game kết thúc cho GameOverUI biết mà hiện lên
    public event Action OnGameOver;
    // ----------------------------------------------------

    public float CurrentSpeed { get; private set; }
    public bool IsGameStarted { get; private set; }
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        CurrentSpeed = 0f;
        IsGameStarted = false;
        IsGameOver = false;

        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        Debug.Log("Waiting...");
        yield return new WaitForSeconds(startDelay);

        IsGameStarted = true;
        CurrentSpeed = initialSpeed;

        OnGameStarted?.Invoke();

        Debug.Log("RUN!");
    }

    private void Update()
    {
        if (!IsGameStarted || IsGameOver) return;

        if (CurrentSpeed < maxSpeed)
        {
            CurrentSpeed += acceleration * Time.deltaTime;
        }
    }

    // Hàm này giữ lại phòng hờ bạn muốn dùng nút bấm restart nhanh
    public void StartGame()
    {
        IsGameStarted = true;
        CurrentSpeed = initialSpeed;
        Debug.Log("GAME STARTED!");
    }

    public void EndGame()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        CurrentSpeed = 0f;

        // --- SỬA ĐOẠN NÀY ---

        // 1. Bắn sự kiện để hiện bảng UI (Cái này sửa lỗi Null Reference bên kia)
        OnGameOver?.Invoke();

        // 2. Tạm thời COMMENT dòng này lại. 
        // Vì nếu để dòng này, game sẽ tự load về Menu sau 2 giây, người chơi chưa kịp nhìn điểm số.
        // Hãy để việc chuyển scene cho cái nút "Home" ở GameOverUI lo.
        // Invoke(nameof(RestartGame), 2f); 
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}