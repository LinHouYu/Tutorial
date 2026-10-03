using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;        // 绑定的目标（拖入你的球体）
    public float distance = 5f;     // 摄像机与球的距离

    [Header("Rotation Settings")]
    public float sensitivity = 3f;  // 鼠标灵敏度
    public float pitchMin = -10f;   // 上下旋转的最小角度（限制在稍微仰视）
    public float pitchMax = 80f;    // 上下旋转的最大角度（限制在头顶俯视，防止360度翻转）

    private float yaw = 0f;         // 左右旋转角度 (Y轴)
    private float pitch = 30f;      // 上下旋转角度 (X轴)

    void Start()
    {
        // 隐藏鼠标指针并锁定在屏幕中心，按 Esc 键可以解锁
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 获取鼠标输入
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity; // 注意这里是减，否则鼠标上下反转

        // 核心：限制摄像机的上下旋转角度，防止360度翻转
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // 计算摄像机在空间中的目标旋转
        Quaternion currentRotation = Quaternion.Euler(pitch, yaw, 0f);
        
        // 计算摄像机的位置（基于球的位置，向后退一段距离）
        Vector3 offset = currentRotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;
        
        // 确保摄像机始终盯着球
        transform.LookAt(target.position); 
    }
}