using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public EnemyManager enemyManager;
    public float playerHealth = 5f;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image expBar;

    private int level;
    private float currentExp;
    private float requiredExp;
    private float health;
    private bool gameStart;
    private bool gamePause;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameStart = false;

        mainPanel.SetActive(true);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePause) Pause();
            else Resume();
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / playerHealth;
        if (health <= 0)
        {
            GameOver();
        }
    }

    public void GameStart()
    {
        level = 1;
        UpdateExpRequirement();
        currentExp = 0;
        expBar.fillAmount = 0;

        gameStart = true;
        gamePause = false;
        enemyManager.Initiate();
        health = playerHealth;

        mainPanel.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        healthBar.fillAmount = health / playerHealth;
    }

    public void GameOver()
    {
        gameStart = false;

        mainPanel.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(true);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf) enemy.GetComponent<EnemyController>().ReturnToPool();
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Pause()
    {
        gamePause = true;

        mainPanel.SetActive(false);
        pausePanel.SetActive(true);
        endPanel.SetActive(false);
    }

    public void Resume()
    {
        gamePause = false;

        mainPanel.SetActive(false);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    public bool IsRunning()
    {
        return gameStart && !gamePause;
    }

    private void UpdateExpRequirement()
    {
        requiredExp = Mathf.Pow(level, 2);
    }

    public void AddExp(float e)
    {
        currentExp += e;
        if (currentExp > requiredExp)
        {
            currentExp -= requiredExp;
            level += 1;
            UpdateExpRequirement();
        }
        expBar.fillAmount = currentExp / requiredExp;
    }
}
