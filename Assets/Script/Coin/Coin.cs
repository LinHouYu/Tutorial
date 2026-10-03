using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("旋转设置")]
    public float rotationSpeed = 150f; 

    [Header("音效设置")]
    public AudioClip collectSound; 

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // 【新增这一行】：调用 GameManager 增加金币计数
            if (GameManager.instance != null)
            {
                GameManager.instance.AddCoin();
            }

            Destroy(gameObject);
        }
    }
}