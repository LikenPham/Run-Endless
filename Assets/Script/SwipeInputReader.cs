using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SwipeInputReader : MonoBehaviour
{
    // Cấu hình độ nhạy
    [SerializeField] private float swipeThreshold = 50f; // Khoảng cách tối thiểu để tính là vuốt

    // Các Events để Controller lắng nghe
    public event Action OnSwipeLeft;
    public event Action OnSwipeRight;
    public event Action OnSwipeUp;   // Nhảy
    public event Action OnSwipeDown; // Trượt (Roll)

    private RunnerInput _inputActions;
    private Vector2 _startPosition;
    private bool _isSwiping;

    private void Awake()
    {
        _inputActions = new RunnerInput();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        // Lắng nghe sự kiện nhấn xuống
        _inputActions.Gameplay.TouchPress.started += OnTouchStarted;
        // Lắng nghe sự kiện nhấc tay lên (hoặc kết thúc)
        _inputActions.Gameplay.TouchPress.canceled += OnTouchEnded;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _inputActions.Gameplay.TouchPress.started -= OnTouchStarted;
        _inputActions.Gameplay.TouchPress.canceled -= OnTouchEnded;
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        // Lưu vị trí bắt đầu
        _startPosition = _inputActions.Gameplay.TouchPosition.ReadValue<Vector2>();
        _isSwiping = true;
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        if (!_isSwiping) return;

        Vector2 endPosition = _inputActions.Gameplay.TouchPosition.ReadValue<Vector2>();
        DetectSwipe(_startPosition, endPosition);
        _isSwiping = false;
    }

    private void DetectSwipe(Vector2 start, Vector2 end)
    {
        Vector2 delta = end - start;

        // Nếu vuốt quá ngắn thì bỏ qua (chống rung tay)
        if (delta.magnitude < swipeThreshold) return;

        // Xác định hướng vuốt (Ngang hay Dọc ưu thế hơn?)
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            // Vuốt Ngang
            if (delta.x > 0) OnSwipeRight?.Invoke();
            else OnSwipeLeft?.Invoke();
        }
        else
        {
            // Vuốt Dọc
            if (delta.y > 0) OnSwipeUp?.Invoke();
            else OnSwipeDown?.Invoke();
        }
    }
}