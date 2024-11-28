using UnityEngine;
using System.Collections.Generic; // Thêm thư viện để sử dụng List

public class EggSpawnPoint : MonoBehaviour
{
    public GameObject[] bubblePrefabs; // Các prefab trứng
    public float spawnInterval = 2f; // Thời gian giữa các lần spawn (ban đầu)
    public float spawnRangeX = 5f; // Giới hạn vùng spawn trên trục X
    public float spawnHeight = -5f; // Vị trí cố định trên trục Y
    public float levelIntervalDecrease = 0.1f; // Số giảm spawn interval mỗi lần next level

    private float initialSpawnInterval; // Lưu giá trị spawn interval ban đầu
    private List<GameObject> spawnedBubbles = new List<GameObject>(); // Danh sách lưu trữ các quả trứng đã spawn

    void Start()
    {
        initialSpawnInterval = spawnInterval; // Lưu giá trị spawnInterval ban đầu
        // Bắt đầu spawn liên tục từ đầu
        InvokeRepeating(nameof(SpawnBubbles), 0f, spawnInterval);
    }

    void SpawnBubbles()
    {
        if (bubblePrefabs.Length == 0)
        {
            Debug.LogWarning("Chưa có prefab nào trong mảng bubblePrefabs!");
            return;
        }

        // Chọn một prefab ngẫu nhiên từ mảng
        GameObject randomBubblePrefab = bubblePrefabs[Random.Range(0, bubblePrefabs.Length)];

        // Xác định vị trí spawn ngẫu nhiên trên trục X
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);

        // Tạo đối tượng từ prefab và lưu vào danh sách spawnedBubbles
        GameObject spawnedBubble = Instantiate(randomBubblePrefab, spawnPosition, Quaternion.identity);
        spawnedBubbles.Add(spawnedBubble);
    }

    public void NextLevel()
    {
        // Giảm thời gian spawn interval mỗi lần nhấn Next Level (tối thiểu 0.5 giây)
        spawnInterval = Mathf.Max(0.5f, spawnInterval - levelIntervalDecrease);

        // Dừng việc gọi InvokeRepeating cũ
        CancelInvoke(nameof(SpawnBubbles));

        // Xóa tất cả các quả trứng cũ
        foreach (GameObject bubble in spawnedBubbles)
        {
            if (bubble != null)
            {
                Destroy(bubble); // Xóa đối tượng quả trứng
            }
        }
        spawnedBubbles.Clear(); // Xóa danh sách lưu trữ các quả trứng cũ

        // Bắt đầu lại với spawnInterval mới ngay lập tức
        InvokeRepeating(nameof(SpawnBubbles), 0f, spawnInterval);
    }

    public void LosePlayAgain()
    {
        // Reset thời gian spawn về giá trị ban đầu
        spawnInterval = initialSpawnInterval;

        // Dừng việc gọi InvokeRepeating
        CancelInvoke(nameof(SpawnBubbles));

        // Xóa tất cả các quả trứng cũ
        foreach (GameObject bubble in spawnedBubbles)
        {
            if (bubble != null)
            {
                Destroy(bubble); // Xóa đối tượng quả trứng
            }
        }
        spawnedBubbles.Clear(); // Xóa danh sách lưu trữ các quả trứng cũ

        // Bắt đầu lại với spawnInterval ban đầu
        InvokeRepeating(nameof(SpawnBubbles), 0f, spawnInterval);

        // Nếu cần, reset lại các giá trị game khác như điểm số, trạng thái v.v
        // GameManager.Instance.ResetGame(); // Thực hiện reset các giá trị game
    }

    // Vẽ Gizmos trong Scene để hiển thị vùng spawn
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Màu sắc của vùng spawn
        Vector3 center = new Vector3(0f, spawnHeight, 0f);
        Vector3 size = new Vector3(spawnRangeX * 2, 0.1f, 1f); // Chiều dài vùng spawn dựa trên spawnRangeX
        Gizmos.DrawWireCube(center, size); // Vẽ vùng spawn là một hình chữ nhật rỗng
    }
}








