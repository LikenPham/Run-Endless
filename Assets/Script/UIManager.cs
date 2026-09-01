using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Cần thư viện này để thao tác với UI cũ hoặc TMP

public class UIManager : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    [Header("UI Elements")]
    [SerializeField] private GameObject startMenuPanel; // Chứa nút Start, Logo game...

    // Hàm này sẽ gán vào sự kiện OnClick của Button
    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }
}