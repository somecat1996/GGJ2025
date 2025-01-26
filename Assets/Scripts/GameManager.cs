using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject buffPanel;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image expBar;

    private int level;
    private float currentExp;
    private float requiredExp;
    private float health;
    private bool gameStart;
    private bool gamePause;

    [SerializeField] private Buff[] buffs;
    [SerializeField] private TMP_Text[] buffText;
    private List<Buff> buffList;
    private float healthRegen;
    [HideInInspector] public float movingSpeed;
    [HideInInspector] public float chargingSpeed;
    [HideInInspector] public float energy;
    [HideInInspector] public float energyRegen;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameStart = false;

        mainPanel.SetActive(true);
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        buffPanel.SetActive(false);

        buffList = new List<Buff>();
    }

    private void ResetBuff()
    {
        healthRegen = 0;
        movingSpeed = 0;
        chargingSpeed = 0;
        energy = 0;
        energyRegen = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePause) Pause();
            else Resume();
        }

        if (IsRunning())
        {
            if (healthRegen > 0) Damage(-healthRegen * Time.deltaTime);
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health > playerHealth) health = playerHealth;
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

        ResetBuff();
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

    private void StartSelectBuff()
    {
        gamePause = true;
        buffPanel.SetActive(true);

        LoadBuff();
    }

    public void EndSelectBuff(int selection)
    {
        gamePause = false;
        buffPanel.SetActive(false);
        Buff buff = buffList[selection];
        switch (buff.type)
        {
            case BuffType.HealthRegen:
                healthRegen += buff.value;
                break;
            case BuffType.MovingSpeed:
                movingSpeed += buff.value;
                break;
            case BuffType.ChargingSpeed:
                chargingSpeed += buff.value;
                break;
            case BuffType.ChargingBar:
                energy += buff.value;
                break;
            case BuffType.ChargingRegen:
                energyRegen += buff.value;
                break;
            default:
                break;
        }
    }

    private void LoadBuff()
    {
        buffList.Clear();
        for (int i = 0; i < 3; i++)
        {
            Buff b = buffs[Random.Range(0, buffs.Length)];
            buffList.Add(b);
            buffText[i].text = b.descrition;
        }
    }

    public void AddExp(float e)
    {
        currentExp += e;
        if (currentExp > requiredExp)
        {
            currentExp -= requiredExp;
            level += 1;
            UpdateExpRequirement();
            StartSelectBuff();
        }
        expBar.fillAmount = currentExp / requiredExp;
    }
}
