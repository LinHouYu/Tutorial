using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    [Header("UI 设置")]
    public TextMeshProUGUI coinText;         // 左上角的金币计数
    public TextMeshProUGUI currentTimeText;  // 【新增】屏幕上实时显示的时间
    public GameObject victoryPanel;          
    public TextMeshProUGUI finalTimeText;    

    private int totalCoins = 0;         
    private int collectedCoins = 0;     
    
    private float timer = 0f;                
    private bool isGameActive = true;        

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); 
    }

    void Start()
    {
        Time.timeScale = 1f;
        isGameActive = true;

        totalCoins = FindObjectsOfType<Coin>().Length;
        UpdateCoinText();
        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false); 
        }
    }

    void Update()
    {
        if (isGameActive)
        {
            timer += Time.deltaTime;
            UpdateCurrentTimeUI(); // 【新增】每帧刷新实时时间
        }
    }

    // 【新增】更新屏幕上的实时时间格式
    private void UpdateCurrentTimeUI()
    {
        if (currentTimeText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            currentTimeText.text = string.Format("时间: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public void AddCoin()
    {
        collectedCoins++;
        UpdateCoinText();

        if (collectedCoins >= totalCoins)
        {
            TriggerVictory();
        }
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "金币: " + collectedCoins + " / " + totalCoins;
        }
    }

    private void TriggerVictory()
    {
        isGameActive = false; 

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (finalTimeText != null)
        {
            finalTimeText.text = "通关用时: " + niceTime;
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true); 
        }

        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}