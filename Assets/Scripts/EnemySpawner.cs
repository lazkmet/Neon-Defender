using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnCycleTime = 1;
    public TextAsset levelData; //Contains information about each wave and/or environment modifiers
    //A numeric byte from 0-9 is an index for the spawner to select a prefab 
    //(thus, it can only have up to ten enemy types). A '-' character means an empty spawn cycle. A 
    //'\n' character represents the end of the wave. An 'x' signifies that the next segment of bytes is a modifier change. This modifier change consists of
    //[type] [amount] '|'. The type is an alphabetic character from a-z (excluding x), and the amount is a 
    //sequence of numbers that gets converted to a string, then added to the value. This parsing is very fragile and requires an 
    //uncorrupted file.
    
    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public BezierPath startRoute;

    private float currentTime;
    private byte[] byteInfo = { };
    private int byteIndex = 0;
    private GameManager manager;
    private void Awake()
    {
        byteInfo = levelData.bytes;
        manager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if (currentTime >= spawnCycleTime)
        {
            //ParseByte();
            SpawnEnemy(1);
            currentTime = 0;
        }
        else {
            currentTime += Time.deltaTime * manager.spawnrateModifier;
        }
    }
    public void NextWave() {
        this.enabled = true;
    }
    private void SpawnEnemy(int index) {
        Enemy newEnemy = Instantiate(enemyPrefabs[index % enemyPrefabs.Length], this.transform.position, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.HP = Mathf.RoundToInt(newEnemy.HP * manager.healthMultiplier);
        BezierFollow newMovement = newEnemy.gameObject.GetComponent<BezierFollow>();
        newMovement.speedModifier *= manager.speedMultiplier;
        newMovement.startRoute = this.startRoute;
    }
    public void Reset()
    {
        byteIndex = 0;
        this.enabled = false;
    }
}
