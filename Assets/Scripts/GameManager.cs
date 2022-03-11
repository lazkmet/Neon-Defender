using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerManager))]
public class GameManager : MonoBehaviour
{
    public int difficultyValue = 0;

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

    private int currentLife;
    private TowerManager towers;
    
    public void Awake()
    {
        towers = this.GetComponent<TowerManager>();
        Reset();
    }
    public void AddMoney(int aMoneyToAdd) {
        currentMoney += aMoneyToAdd;
    }
    public void DealDamage(int aDamageTaken) {
        SetLife(currentLife - aDamageTaken);

        if (currentLife <= 0) {
            GameOver();
        }
    }
    public void GameOver() {
        Time.timeScale = 0;
    }
    public void Reset()
    {
        currentMoney = StartMoney();
        SetLife(StartLife());
        ResetModifiers();
        foreach (EnemySpawner e in FindObjectsOfType<EnemySpawner>()) {
            e.Reset();
        }
        foreach (UpgradeList ul in FindObjectsOfType<UpgradeList>()) {
            ul.Hide();
        }
        towers.Reset();
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
