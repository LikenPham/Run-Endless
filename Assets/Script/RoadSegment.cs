using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    [Header("Ground Obstacles (Dưới Đất)")]
    [SerializeField] private GameObject[] groundPrefabs; // Đổi tên biến cũ cho rõ nghĩa

    [Header("Air Obstacles (Trên Không)")]
    [SerializeField] private GameObject[] flyingPrefabs; // Mảng mới cho vật bay
    [SerializeField] private float flyHeight = 2.5f;     // Độ cao so với mặt đường (Player cao 2m thì cái này nên > 2m)
    [SerializeField][Range(0f, 1f)] private float chanceToFly = 0.3f; // 30% tỉ lệ ra vật bay

    [Header("General Settings")]
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float chanceToSpawn = 0.5f;

    public void SpawnObstacles()
    {
        // ... (Logic chọn làn an toàn giữ nguyên như bài trước) ...
        List<int> availableLanes = new List<int> { -1, 0, 1 };
        int safeLaneIndex = Random.Range(0, availableLanes.Count);
        availableLanes.RemoveAt(safeLaneIndex);

        foreach (int laneIndex in availableLanes)
        {
            if (Random.value < chanceToSpawn)
            {
                SpawnOnLane(laneIndex);
            }
        }
    }

    private void SpawnOnLane(int laneIndex)
    {
        // 1. Quyết định xem sinh ra con "Đi bộ" hay con "Bay"
        // Nếu có prefab bay VÀ random trúng tỉ lệ bay -> Sinh ra trên trời
        bool isFlying = (flyingPrefabs.Length > 0) && (Random.value < chanceToFly);

        GameObject prefabToSpawn;
        float yPos;

        if (isFlying)
        {
            // Chọn vật bay
            prefabToSpawn = flyingPrefabs[Random.Range(0, flyingPrefabs.Length)];
            yPos = flyHeight; // Đặt độ cao
        }
        else
        {
            // Chọn vật dưới đất (Logic cũ)
            if (groundPrefabs.Length == 0) return;
            prefabToSpawn = groundPrefabs[Random.Range(0, groundPrefabs.Length)];
            yPos = 1f; // Sát đất
        }

        // 2. Tạo vật cản
        GameObject obstacle = Instantiate(prefabToSpawn, transform);

        // 3. Đặt vị trí (Lưu ý tham số Y)
        float xPos = laneIndex * laneDistance;
        obstacle.transform.localPosition = new Vector3(xPos, yPos, 0);
    }
}