using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Chicken : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển của Chicken
    private SpriteRenderer spriteRenderer; // Để lật Sprite
    public TMP_Text scoreText; // Tham chiếu đến UI Text
    private int score = 0; // Biến lưu điểm số
    public TMP_Text crackText;
    private int crack = 0; // Biến lưu số lần crack
    public GameObject entryPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public TMP_Text winLevelText; // Tham chiếu tới TMP_Text trong panel Win
    public TMP_Text loseLevelText; // Tham chiếu tới TMP_Text trong panel Lose
    private int currentLevel = 0; // Biến lưu trữ số level

    // Các biến mới để theo dõi số điểm và số crack mỗi level
    private int scoreMission = 10; // Điểm yêu cầu để hoàn thành mission mỗi level
    private int crackMission = 10; // Số lần crack cho phép mỗi level
    public TMP_Text missionText;// Text trong panel Win để hiển thị thông tin mission

    void Start()
    {
        Time.timeScale = 0;
        // Lấy SpriteRenderer từ GameObject Chicken
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Kiểm tra xem SpriteRenderer có tồn tại không
        if (spriteRenderer == null)
        {
            Debug.LogError("Không tìm thấy SpriteRenderer trên GameObject!");
        }
        // Khởi tạo điểm số
        UpdateScoreUI();
    }

    void Update()
    {
        // Lấy đầu vào từ bàn phím để di chuyển Chicken sang trái/phải
        float moveX = Input.GetAxis("Horizontal");

        // Di chuyển Chicken theo trục X (trái/phải)
        transform.Translate(Vector2.right * moveX * speed * Time.deltaTime);

        // Lật hình ảnh khi di chuyển trái/phải
        if (moveX > 0)
            spriteRenderer.flipX = false; // Hướng phải
        else if (moveX < 0)
            spriteRenderer.flipX = true; // Hướng trái
    }

    public void AddScore(int points)
    {
        // Cộng điểm và cập nhật giao diện
        score += points;
        UpdateScoreUI();
    }

    public void AddCrack(int points)
    {
        crack += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        // Hiển thị điểm số trên giao diện
        scoreText.text = score.ToString();
        crackText.text = crack.ToString();

        // Kiểm tra điều kiện thắng/thua
        if (score >= scoreMission) // Hoàn thành score mission
        {
            WinGame();
        }
        else if (crack >= crackMission) // Quá số lần crack cho phép
        {
            LoseGame();
        }
    }

    public void OnPlayButtonClicked()
    {
        // Tắt EntryPanel
        entryPanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        // Bắt đầu game (tiếp tục TimeScale nếu đã dừng)
        Time.timeScale = 1;
        score = 0;
        crack = 0;

        // Cập nhật điểm và crack theo level
        scoreMission = 10 + (currentLevel * 2); // Tăng điểm mỗi level
        crackMission = Mathf.Max(1, 10 - currentLevel); // Giảm số lần crack cho phép mỗi level (tối thiểu 1)
    }

    private void WinGame()
    {
        // Tăng level lên sau khi thắng
        currentLevel++;

        // Cập nhật điểm và crack yêu cầu cho level tiếp theo
        scoreMission = 10 + (currentLevel * 2); // Cập nhật điểm yêu cầu cho level tiếp theo
        crackMission = Mathf.Max(1, 10 - currentLevel); // Giảm số lần crack cho phép

        // Cập nhật missionText trong winPanel
        if (missionText != null)
        {
            missionText.text = "Mission: score = " + scoreMission + " & crack < " + crackMission + " !";
        }

        // Hiển thị Panel thắng và dừng game
        winPanel.SetActive(true);

        // Cập nhật số level trong winPanel
        if (winLevelText != null)
        {
            winLevelText.text = "Level: " + currentLevel;
        }

        Time.timeScale = 0;
    }

    private void LoseGame()
    {
        // Hiển thị Panel thua và dừng game
        losePanel.SetActive(true);

        // Cập nhật số level trong losePanel
        if (loseLevelText != null)
        {
            loseLevelText.text = "Level: " + currentLevel;
        }

        currentLevel = 0; // Reset level về 0 khi thua
        Time.timeScale = 0;
    }
}

