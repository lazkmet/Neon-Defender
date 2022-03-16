using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Char;

public class EnemySpawner : MonoBehaviour
{
    public float spawnCycleTime = 1;
    public TextAsset levelData; //Contains information about each wave and/or environment modifiers.
    //See Parse() for detailed info.
    public bool waveActive;

    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public BezierPath startRoute;

    private float currentTime;
    private string[] waves = { };
    private string currentWave = "";
    private int currentInWave = 0;
    private GameManager manager;
    private void Awake()
    {
        string rawText = levelData.text;
        waves = rawText.Split('\n');
        manager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if (currentTime >= spawnCycleTime)
        {
            ReadNext();
            currentTime = 0;
        }
        else {
            currentTime += Time.deltaTime * manager.spawnrateModifier;
        }
    }
    public void StartWave(int waveNum) {
        if (waveNum >= waves.Length) { return; }
        if (waveNum < 0) { StartWave(0); return; }
        this.enabled = true;
        currentWave = waves[waveNum];
        currentInWave = 0;
    }
    private void SpawnEnemy(int index) {
        Enemy newEnemy = Instantiate(enemyPrefabs[index % enemyPrefabs.Length], this.transform.position, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.HP = Mathf.RoundToInt(newEnemy.HP * manager.healthMultiplier);
        manager.makeRed(newEnemy.gameObject);
        BezierFollow newMovement = newEnemy.gameObject.GetComponent<BezierFollow>();
        newMovement.speedModifier *= manager.speedMultiplier;
        newMovement.startRoute = this.startRoute;
    }
    private void ReadNext() {
        if (currentInWave < currentWave.Length)
        {
            Parse(currentWave[currentInWave]);
            currentInWave++;
        }
        else {
            this.enabled = false;
            manager.CheckWaveStop();
        }
    }
    private void Parse(char a) {
        /*A numeric character from 0-9 is an index for the spawner to select a prefab 
        (thus, it can only have up to ten enemy types). A '-' character means an empty spawn cycle. A 
        '\n' character represents the end of the wave. An 'x' signifies that the next segment of chars is a modifier change. This modifier change consists of
        [type] [amount] '|'. The type is an alphabetic character from a-z (excluding x), and the amount is a 
        sequence of numbers that gets converted to a string, then added to the value. The modifier change uses a tick.
        This parsing is very fragile and requires an uncorrupted file.*/
        if (IsDigit(a))
        {
            SpawnEnemy((int)GetNumericValue(a));
        }
        else {
            switch (a) {
                case '-':
                    return;
                case 'x':
                    ParseStatus();
                    return;
                default:
                    return;
            }
        }
    }
    private void ParseStatus() {
        //s = speedMultiplier, r = spawnRateModifier, h = healthBoost
        char currentChar = ' ';
        while (currentChar != '|' && ++currentInWave < currentWave.Length) {
            currentChar = currentWave[currentInWave];
            switch (currentChar) {
                case 's':
                    manager.speedMultiplier += ParseNum();
                    return;
                case 'r':
                    manager.spawnrateModifier += ParseNum();
                    return;
                case 'h':
                    manager.healthMultiplier += ParseNum();
                    return;
                default:
                    break;
            }
        }
    }
    private float ParseNum() {
        float returnValue = 0;
        string currentWord = "";
        char currentChar = ' ';
        while (currentChar != '|' && ++currentInWave < currentWave.Length) {
            currentChar = currentWave[currentInWave];
            if (currentChar != '|') {
                currentWord += currentChar;
            }
        }
        returnValue = float.Parse(currentWord);
        return returnValue;
    }
    public void Reset()
    {
        currentWave = "";
        currentInWave = 0;
        this.enabled = false;
    }
}
