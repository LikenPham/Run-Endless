using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform playerTransform; // Kéo nhân vật vào đây

    [Header("Settings")]
    [SerializeField] private Vector3 offset; // Khoảng cách lệch so với nhân vật
    [SerializeField] private float smoothTime = 0.25f; // Độ mượt (càng lớn càng chậm)
    [SerializeField] private bool lockX = true; // Subway Surfers style: Camera luôn ở giữa đường

    private Vector3 _currentVelocity; // Biến dùng cho hàm SmoothDamp

    private void Start()
    {
        // Nếu chưa set offset bằng tay, tự động tính toán dựa trên vị trí hiện tại
        if (offset == Vector3.zero && playerTransform != null)
        {
            offset = transform.position - playerTransform.position;
        }
    }

    // Dùng LateUpdate để đảm bảo Nhân vật di chuyển xong rồi Camera mới đi theo
    // Tránh hiện tượng nhân vật bị rung lắc (Jitter)
    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // 1. Tính toán vị trí đích mà Camera muốn tới
        Vector3 targetPosition = playerTransform.position + offset;

        // 2. Logic đặc biệt cho game Endless Runner:
        if (lockX)
        {
            // Giữ Camera luôn ở giữa làn đường (X = 0), bất kể nhân vật đang ở làn nào
            // Nếu bạn muốn camera lệch nhẹ thì có thể thay số 0 bằng transform.position.x ban đầu
            targetPosition.x = 0f;
        }

        // 3. Di chuyển mượt mà từ vị trí hiện tại tới vị trí đích
        // SmoothDamp giúp camera không bị giật cục khi nhân vật nhảy hoặc tăng tốc
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref _currentVelocity,
            smoothTime
        );
    }
}