using UnityEngine;
using System.Collections.Generic;

public class RoadScenery : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private GameObject[] buildings; // Mảng chứa Prefab nhà/cây
    [SerializeField] private Transform[] leftSpawnPoints;  // Các vị trí bên trái
    [SerializeField] private Transform[] rightSpawnPoints; // Các vị trí bên phải

    [Header("Settings")]
    [SerializeField][Range(0f, 1f)] private float spawnChance = 0.7f; // 70% cơ hội xuất hiện nhà

    private void Start()
    {
        // Tự động trang trí ngay khi khúc đường được sinh ra
        DecorateLane(leftSpawnPoints);
        DecorateLane(rightSpawnPoints);
    }

    private void DecorateLane(Transform[] points)
    {
        if (buildings.Length == 0) return;

        foreach (Transform point in points)
        {
            // Tung xúc xắc: Có xây nhà ở điểm này không?
            if (Random.value < spawnChance)
            {
                // 1. Chọn ngẫu nhiên kiểu nhà
                GameObject prefab = buildings[Random.Range(0, buildings.Length)];

                // 2. Sinh ra nhà
                GameObject house = Instantiate(prefab, point.position, Quaternion.identity);

                // 3. Gắn vào làm con của điểm Neo (để khi xóa đường, nhà cũng mất theo)
                house.transform.SetParent(point);

                // 4. Xoay ngẫu nhiên (90, 180...) cho đỡ nhàm chán
                float randomRotY = Random.Range(0, 4) * 90f;
                house.transform.localRotation = Quaternion.Euler(0, randomRotY, 0);
            }
        }
    }
}