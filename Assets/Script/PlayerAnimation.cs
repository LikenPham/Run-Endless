using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        // Đăng ký lắng nghe sự kiện từ GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStarted += PlayRunAnimation;
            // Nếu muốn xử lý Game Over thì đăng ký thêm event EndGame ở đây
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký khi object bị hủy để tránh lỗi
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStarted -= PlayRunAnimation;
        }
    }

    private void PlayRunAnimation()
    {
        // Bật biến IsRunning lên true -> Animator sẽ tự chuyển từ Idle sang Run
        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }
    }

    // Hàm phụ: Nếu muốn nhân vật ngã khi chết
    public void PlayDeathAnimation()
    {
        if (animator != null) animator.SetTrigger("Die");
    }

    public void TriggerJump()
    {
        if (animator) animator.SetTrigger("Jump");
    }

    // 2. Hàm gọi trượt (có bật/tắt)
    public void SetSliding(bool isSliding)
    {
        if (animator) animator.SetBool("IsSliding", isSliding);
    }

    // 3. Hàm gọi chết
    public void TriggerDeath()
    {
        if (animator) animator.SetTrigger("Die");
    }
}