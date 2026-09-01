using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject[] roadPrefabs; // Mảng chứa các kiểu đường khác nhau

    [Header("Settings")]
    [SerializeField] private float roadLength = 30f;   // Chiều dài chính xác của 1 khúc đường
    [SerializeField] private int maxSegments = 5;      // Số lượng khúc đường luôn hiển thị
    [SerializeField] private float safeZone = 35f;     // Khoảng cách an toàn sau lưng nhân vật để xóa/tái chế

    private List<GameObject> _activeSegments = new List<GameObject>();
    private float _spawnZ = 0f; // Tọa độ Z để đặt khúc đường tiếp theo

    private void Start()
    {
        // Khởi tạo sẵn đường băng lúc đầu game
        for (int i = 0; i < maxSegments; i++)
        {
            SpawnSegment();
        }
    }

    private void Update()
    {
        // Kiểm tra xem nhân vật đã chạy qua khúc đường đầu tiên chưa
        // Logic: Vị trí Player > (Vị trí bắt đầu của khúc đường đầu - SafeZone)
        if (playerTransform.position.z - safeZone > (_spawnZ - maxSegments * roadLength))
        {
            SpawnSegment();
            RemoveSegment();
        }
    }

    private void SpawnSegment(int prefabIndex = -1)
    {
        GameObject go;

        // Chọn ngẫu nhiên kiểu đường (nếu có nhiều kiểu)
        if (prefabIndex == -1)
            go = Instantiate(roadPrefabs[Random.Range(0, roadPrefabs.Length)]);
        else
            go = Instantiate(roadPrefabs[prefabIndex]);

        // Đặt nó làm con của Manager cho gọn Hierarchy
        go.transform.SetParent(transform);

        // Đặt vị trí nối tiếp vào đuôi
        go.transform.position = Vector3.forward * _spawnZ;

        // Cập nhật tọa độ cho lần sau
        _spawnZ += roadLength;

        // Thêm vào danh sách quản lý
        _activeSegments.Add(go);

        // Lấy script RoadSegment vừa gắn vào đường và bảo nó sinh vật cản đi
        RoadSegment segmentScript = go.GetComponent<RoadSegment>();
        if (segmentScript != null)
        {
            // Nếu là khúc đường đầu tiên thì ĐỪNG sinh vật cản (để người chơi không chết ngay khi vào game)
            if (_activeSegments.Count > 1)
            {
                segmentScript.SpawnObstacles();
            }
        }
    }

    private void RemoveSegment()
    {
        // Xóa khúc đường cũ nhất (đầu danh sách)
        GameObject oldSegment = _activeSegments[0];
        _activeSegments.RemoveAt(0);

        Destroy(oldSegment);
        // LƯU Ý: Để tối ưu cực đại cho mobile, thay vì Destroy, 
        // bạn nên dùng Object Pooling (Tắt đi rồi tái sử dụng).
        // Nhưng ở bước này dùng Destroy cho code dễ hiểu trước đã.
    }
}