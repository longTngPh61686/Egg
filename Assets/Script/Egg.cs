using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour
{
    private Animator animator; // Animator để điều khiển animation
    public float delayBeforeDestroy = 0.1f; // Thời gian chờ trước khi phá hủy
    private bool isBroken = false; // Trạng thái kiểm tra nếu trứng đã chạm NonTouch

    void Start()
    {
        // Lấy Animator từ GameObject
        animator = GetComponent<Animator>();

        // Kiểm tra Animator có tồn tại không
        if (animator == null)
        {
            Debug.LogError("Không tìm thấy Animator trên GameObject!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu trứng đã vỡ, bỏ qua các xử lý khác
        if (isBroken) return;

        if (collision.gameObject.CompareTag("NonTouch"))
        {
            // Đặt trạng thái trứng đã vỡ
            isBroken = true;
            Chicken chicken = FindObjectOfType<Chicken>();
            if (chicken != null)
            {
                // Cộng điểm Crack
                chicken.AddCrack(1);
            }


            // Bắt đầu Coroutine để play animation và phá hủy
            StartCoroutine(PlayAnimationAndDestroy());
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Destroy(rb); // Loại bỏ Rigidbody2D để đối tượng không còn bị ảnh hưởng bởi vật lý
            }

            // Đảm bảo Collider2D vẫn có thể tương tác với các đối tượng khác
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = false; // Nếu cần, bạn có thể đặt Collider thành "Trigger" để cho phép va chạm mà không thay đổi vị trí
            }
        }
        else if (collision.gameObject.CompareTag("Bag"))
        {
            Debug.Log("Chạm vào Bag!");
            Chicken chicken = FindObjectOfType<Chicken>();

            if (chicken != null)
            {
                // Cộng điểm Score
                chicken.AddScore(1);
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator PlayAnimationAndDestroy()
    {
        // Chạy animation (trong Animator phải có trigger "Break")
        animator.SetTrigger("Crack");

        // Chờ delayBeforeDestroy giây
        yield return new WaitForSeconds(delayBeforeDestroy);

        // Phá hủy đối tượng
        Destroy(gameObject);
    }
}

