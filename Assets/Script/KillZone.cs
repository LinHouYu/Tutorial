using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理，用于重新加载场景

public class Killzone : MonoBehaviour
{
    [Header("陷阱设置")]
    public AudioClip deathSound;      // 碰到陷阱时的音效
    public float restartDelay = 1f;   // 延迟几秒后重置游戏（为了让音效播完）

    private bool isDead = false;      // 标记是否已经触发，防止短时间内重复触发

    // 当其他带有刚体的物体【碰撞】到这个方块时触发
    void OnCollisionEnter(Collision collision)
    {
        // 检查撞到方块的是不是玩家，并且确保还没死
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            isDead = true; // 标记为已死亡

            // 1. 播放死亡音效
            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }

            // 2. 延迟调用 RestartGame 方法
            Invoke("RestartGame", restartDelay);
        }
    }

    // （可选）如果你把方块设置成了 Is Trigger（触发器/虚体），就取消下面这段代码的注释
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            isDead = true;
            if (deathSound != null) AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Invoke("RestartGame", restartDelay);
        }
    }
    */

    // 重新开始游戏的方法
    void RestartGame()
    {
        // 重新加载当前所在的关卡
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}