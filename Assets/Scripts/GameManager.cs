using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TowerManager))]
public class GameManager : MonoBehaviour
{
    public int difficultyValue = 0;
    public int numWaves = 0;
    //[HideInInspector]
    public int currentWave = 0;

    [Header("Game Parameters")]
    [SerializeField]
    private int[] startingMoney = { 0, 0, 0 };
    [SerializeField]
    private int[] startingLife = { 1, 1, 1 };
    [SerializeField]
    private float[] spawnMultiplier = { 1, 1, 1 };
    [SerializeField]
    private float[] enemySpeedMultiplier = { 1, 1, 1 };
    [SerializeField]
    private float[] healthBoost = { 1, 1, 1 };
    [SerializeField]
    private float[] itemCostMultiplier = { 1, 1, 1 };

    [Header("Brightness")]
    public Material maxBlue;
    public Material minBlue;
    public Material maxRed;
    public Material minRed;

    [HideInInspector]
    public float spawnrateModifier;
    [HideInInspector]
    public float speedMultiplier;
    [HideInInspector]
    public float healthMultiplier;
    [HideInInspector]
    public float costMultiplier;
    [HideInInspector]
    public int currentMoney { get; private set; }

    [Header("Menu Items")]
    public TextMeshProUGUI moneyDisplay;
    public Button waveButton;
    public Button lifeButton;
    public Color gray;

    private int currentLife;
    private bool waveOngoing;
    private bool gameOver;
    private TowerManager towers;
    private EnemySpawner[] spawners = { };
    
    public void Awake()
    {
        towers = this.GetComponent<TowerManager>();
        spawners = FindObjectsOfType<EnemySpawner>();
    }
    public void Start()
    {
        Reset();
    }
    public void AddMoney(int aMoneyToAdd) {
        currentMoney += aMoneyToAdd;
        UpdateDisplay();
    }
    public void StartWave() {
        if (!waveOngoing && currentWave < numWaves) {
            waveOngoing = true;
            foreach (EnemySpawner e in spawners) {
                e.NextWave();
            }
            currentWave++;
            waveButton.gameObject.GetComponent<Image>().color = gray;
            waveButton.enabled = false;
            TextMeshProUGUI waveText;
            try
            {
                if (waveButton.transform.Find("Wave Text").TryGetComponent(out waveText))
                {
                    string waveMessage = "In Progress\n(" + currentWave + "/" + numWaves + ")";
                    waveText.text = waveMessage;
                };
            }
            catch (System.NullReferenceException)
            {
                print("Error in button hierarchy");
            }
        }
    }
    public void CheckWaveStop() {
        bool waveOver = true;
        foreach (EnemySpawner e in spawners) {
            if (e.enabled) {
                waveOver = false;
            }
        }
        if (FindObjectsOfType<Enemy>().Length > 0) {
            waveOver = false;
        }
        waveOngoing = !waveOver;
        if (waveOver) {
            if (currentWave >= numWaves && !gameOver)
            {
                GameWon();
            }
            else { 
                UpdateDisplay(); 
            }
        }
    }
    public void DealDamage(int aDamageTaken) {
        SetLife(currentLife - aDamageTaken);

        if (currentLife <= 0) {
            GameOver();
        }
    }
    public void GameOver() {
        gameOver = true;
        Time.timeScale = 0;
    }
    private void GameWon() {
        Time.timeScale = 0;
        print("Congratulations! Win!");
    }
    public void Reset()
    {
        currentMoney = StartMoney();
        SetLife(StartLife());
        ResetModifiers();
        foreach (EnemySpawner e in spawners) {
            e.Reset();
        }
        foreach (UpgradeList ul in FindObjectsOfType<UpgradeList>()) {
            ul.Hide();
        }
        towers.Reset();
        waveOngoing = false;
        gameOver = false;
        UpdateDisplay();
        Time.timeScale = 1;
    }
    private void SetLife(int aAmount) {
        currentLife = aAmount;

        GameObject[] blues = GameObject.FindGameObjectsWithTag("Blue");
        GameObject[] reds = GameObject.FindGameObjectsWithTag("Red");

        foreach (GameObject obj in blues) {
            Renderer r = obj.GetComponent<Renderer>();
            r.material.Lerp(minBlue, maxBlue, Mathf.Clamp01((float)currentLife / (StartLife() * 1.2f)));
        }

        foreach (GameObject obj in reds) {
            Renderer r = obj.GetComponent<Renderer>();
            r.material.Lerp(maxRed, minRed, Mathf.Clamp01((float)currentLife / StartLife()));
        }
    }

    private void UpdateDisplay() {
        moneyDisplay.text = currentMoney.ToString();

        //Update life button

        if (!waveOngoing) {
            waveButton.enabled = true;
            waveButton.gameObject.GetComponent<Image>().color = Color.white;
            TextMeshProUGUI waveText;
            try
            {
                if (waveButton.transform.Find("Wave Text").TryGetComponent(out waveText))
                {
                    string waveMessage = "Start Wave\n(" + (currentWave + 1) + "/" + numWaves + ")";
                    waveText.text = waveMessage;
                };
            }
            catch (System.NullReferenceException) {
                print("Error in button hierarchy");
            }
        }
    }

    //data modification
    private int StartMoney() {
        return startingMoney[difficultyValue % 3];
    }
    private int StartLife() {
        return startingLife[difficultyValue % 3];
    }
    private void ResetModifiers() {
        spawnrateModifier = spawnMultiplier[difficultyValue % 3];
        speedMultiplier = enemySpeedMultiplier[difficultyValue % 3];
        healthMultiplier = healthBoost[difficultyValue % 3];
        costMultiplier = itemCostMultiplier[difficultyValue % 3];
    }
}
