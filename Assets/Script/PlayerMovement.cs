using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SwipeInputReader inputReader; // Kéo thả script input vào đây
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerAnimation playerAnimation;

    [Header("Settings")]
    //[SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float laneDistance = 3f; // Khoảng cách giữa các lane
    [SerializeField] private float laneChangeSpeed = 10f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 20f;

    private int _currentLane = 1; // 0: Trái, 1: Giữa, 2: Phải
    private Vector3 _verticalVelocity;
    private float _targetX;

    private void Start()
    {
        // Đăng ký nhận lệnh từ Input Reader
        inputReader.OnSwipeLeft += MoveLeft;
        inputReader.OnSwipeRight += MoveRight;
        inputReader.OnSwipeUp += Jump;
        inputReader.OnSwipeDown += Slide;

        UpdateTargetLane();
    }

    // Đừng quên hủy đăng ký để tránh lỗi bộ nhớ
    private void OnDestroy()
    {
        inputReader.OnSwipeLeft -= MoveLeft;
        inputReader.OnSwipeRight -= MoveRight;
        inputReader.OnSwipeUp -= Jump;
        inputReader.OnSwipeDown -= Slide;
    }

    private void Update()
    {
        // 1. Tính toán di chuyển làn (X)
        // Di chuyển mượt mà từ vị trí hiện tại sang vị trí lane đích
        Vector3 targetPosition = transform.position;
        targetPosition.x = Mathf.MoveTowards(targetPosition.x, _targetX, laneChangeSpeed * Time.deltaTime);

        // 2. Tính toán di chuyển thẳng (Z)
        //Vector3 moveVector = Vector3.forward * forwardSpeed;
        // Kiểm tra null để tránh lỗi khi test riêng lẻ nhân vật mà quên bỏ GameManager vào Scene
        float speed = GameManager.Instance != null ? GameManager.Instance.CurrentSpeed : 0f;
        Vector3 moveVector = Vector3.forward * speed;
        moveVector.x = (targetPosition.x - transform.position.x) / Time.deltaTime; // Tính vận tốc ngang cần thiết

        // 3. Xử lý trọng lực (Y)
        if (characterController.isGrounded && _verticalVelocity.y < 0)
        {
            _verticalVelocity.y = -2f; // Giữ nhân vật bám đất
        }
        else
        {
            _verticalVelocity.y -= gravity * Time.deltaTime;
        }

        // 4. Thực thi di chuyển cuối cùng
        characterController.Move((moveVector + _verticalVelocity) * Time.deltaTime);
    }

    // --- Các hàm Logic Game ---

    private void MoveLeft()
    {
        if (!GameManager.Instance.IsGameStarted) return;
        if (_currentLane > 0)
        {
            _currentLane--;
            UpdateTargetLane();
        }
    }

    private void MoveRight()
    {
        if (!GameManager.Instance.IsGameStarted) return;
        if (_currentLane < 2)
        {
            _currentLane++;
            UpdateTargetLane();
        }
    }

    private void Jump()
    {
        if (!GameManager.Instance.IsGameStarted) return;
        _verticalVelocity.y = jumpForce;
        if (playerAnimation != null)
        {
            playerAnimation.TriggerJump();
        }
    }

    private void Slide()
    {
        if (!GameManager.Instance.IsGameStarted) return;
        // Logic cuộn người, giảm collider height, v.v.
        StartCoroutine(SlideRoutine());
    }

    private System.Collections.IEnumerator SlideRoutine()
    {
        // 1. Lưu lại kích thước ban đầu (để tí nữa trả lại)
        float originalHeight = characterController.height;
        float originRadius = characterController.radius;

        // 2. Thiết lập kích thước khi Trượt (Slide)
        // Giả sử Height trượt là 0.5f (Rất thấp để chui qua rào)
        float slideHeight = 0.2f;
        float slideRadius = 0.2f;

        characterController.height = slideHeight;
        characterController.radius = slideRadius;
        // QUAN TRỌNG: Tâm phải bằng 1/2 chiều cao để đáy capsule luôn ở vị trí 0 (mặt đất)

        // Gọi Animation hiển thị
        if (playerAnimation != null) playerAnimation.SetSliding(true);

        // 3. Chờ hết thời gian trượt
        yield return new WaitForSeconds(1.0f);

        // 4. Trả về kích thước đứng ban đầu
        characterController.height = originalHeight;
        characterController.radius = originRadius;

        if (playerAnimation != null) playerAnimation.SetSliding(false);
    }

    private void UpdateTargetLane()
    {
        // Lane 0 = -3, Lane 1 = 0, Lane 2 = 3
        _targetX = (_currentLane - 1) * laneDistance;
    }
}