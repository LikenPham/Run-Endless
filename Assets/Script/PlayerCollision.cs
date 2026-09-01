using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement; // Để vô hiệu hóa di chuyển
    [SerializeField] private PlayerAnimation playerAnimation;

    // Hàm đặc biệt dành riêng cho CharacterController khi đâm vào vật thể có Collider
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 1. Kiểm tra xem cái mình vừa đâm vào có phải là Obstacle không?
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        // Gọi GameManager kết thúc trò chơi
        GameManager.Instance.EndGame();

        // Tắt script di chuyển để người chơi không vuốt được nữa
        if (playerMovement != null) playerMovement.enabled = false;

        if (playerAnimation != null) playerAnimation.TriggerDeath();

        // Tùy chọn: Tắt script này luôn để tránh gọi EndGame nhiều lần liên tiếp
        this.enabled = false;
    }
}