using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 15f;       // 移动速度
    public float jumpForce = 6f;        // 跳跃力度

    private Rigidbody rb;
    private float moveHorizontal;
    private float moveVertical;
    private bool isGrounded;

    private Transform camTransform;     // 存储主摄像机的 Transform

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // 自动获取场景中的主摄像机
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("未找到主摄像机！请确保你的摄像机带有 'MainCamera' 标签。");
        }
    }

    void Update()
    {
        // 1. 获取键盘输入 (WASD)
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // 2. 检测跳跃 (按空格且在地面上)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (camTransform == null) return;

        // 3. 获取摄像机当前的前方和右方
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        // 4. 剔除Y轴的影响，防止球跟着摄像机往天上飞或往地下钻
        camForward.y = 0f;
        camRight.y = 0f;
        
        // 归一化向量，保证方向向量的长度为1
        camForward.Normalize();
        camRight.Normalize();

        // 5. 根据摄像机朝向和玩家按键，计算最终的移动方向
        Vector3 movement = camForward * moveVertical + camRight * moveHorizontal;

        // 防止对角线移动时速度叠加过快 (比如同时按W和D)
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        // 6. 施加力，让球滚动
        rb.AddForce(movement * moveSpeed);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 确保你已经把地面的 Tag 设置为了 "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}