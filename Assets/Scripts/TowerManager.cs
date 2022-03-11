using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerManager : MonoBehaviour
{
    public enum towerType {SNIPER, BOMBER}
    private int numTypes = 2;

    public Transform floor;
    public GameObject[] towerPrefabs;
    public UpgradeList[] lists;
    public int perTowerGrowth = 0;

    [System.Serializable]
    public struct upgradeValue {
        public float value;
        public int cost;
    }

    [Header("Sniper")]
    public upgradeValue[] sniperDamage = { };
    public upgradeValue[] sniperCooldown = { };
    public upgradeValue[] sniperRange = { };
    public upgradeValue[] sniperTriShot = { };
    public Button sniperButton;
    public int sniperCost = 0;

    [Header("Bomber")]
    public upgradeValue[] bomberDamage = { };
    public upgradeValue[] bomberCooldown = { };
    public upgradeValue[] bomberRange = { };
    public Button bomberButton;
    public int bomberCost = 0;

    private int[] currentCosts;
    private Tower selectedTower;
    [HideInInspector]
    public GameManager manager;
    private bool placing;
    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        currentCosts = new int[numTypes];
    }
    private void Update()
    {
        if (Time.timeScale == 0) { return; }
        if (Input.GetMouseButtonDown(0)) {
            if (true) { //mouse within valid screen area?
                if (placing)
                { //lock it in and make selected
                    if (manager.currentMoney >= currentCosts[selectedTower.type] && selectedTower.GetComponent<MouseFollow>().ValidPlacement())
                    {
                        MouseFollow temp = selectedTower.gameObject.GetComponent<MouseFollow>();
                        temp.RestoreMaterials();
                        Destroy(temp);
                        selectedTower.enabled = true;
                        BuyTower(selectedTower.type);
                        placing = false;
                        UpdateUpgrades();
                    }
                    else { 
                        //error noise
                    }
                }
                else
                { //raycast for tower
                  //if there is a selected tower, becomes active and displays proper upgrade list
                    UpdateUpgrades();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)){
            if (placing)
            {
                Destroy(selectedTower);
                placing = false;
            }
            selectedTower = null;
            UpdateUpgrades();
        }
    }
    public void TryUpgradeSelected(Tower.upgradeType type) {
        if (selectedTower != null) {
            selectedTower.Upgrade(type);
            UpdateUpgrades();
        }
    }
    private void DestroyExistingTowers()
    {
        foreach (Tower t in FindObjectsOfType<Tower>())
        {
            Destroy(t.gameObject);
        }
    }
    public void Reset()
    {
        selectedTower = null;
        UpdateUpgrades();
        placing = false;
        DestroyExistingTowers();
        currentCosts[(int)towerType.SNIPER] = sniperCost;
        currentCosts[(int)towerType.BOMBER] = bomberCost;
        UpdateButtons();
    }
    public void StartPlacement(int tower) {
        placing = true;
        selectedTower = Instantiate(towerPrefabs[tower % towerPrefabs.Length]).GetComponent<Tower>();
        selectedTower.gameObject.GetComponent<MouseFollow>().floor = this.floor;
    }
    private void BuyTower(int tower) {
        int cost = Mathf.RoundToInt(currentCosts[tower % currentCosts.Length] * manager.costMultiplier);
        manager.AddMoney(-cost);
        currentCosts[tower % currentCosts.Length] += perTowerGrowth;
        UpdateButtons();
    }
    public void UpdateButtons() {
        try
        {
            TextMeshProUGUI costDisplay = sniperButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costDisplay.text = currentCosts[(int)towerType.SNIPER].ToString();

            costDisplay = bomberButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            costDisplay.text = currentCosts[(int)towerType.BOMBER].ToString();
        }
        catch (System.NullReferenceException) { }
    }
    public void UpdateUpgrades() {
        if (selectedTower == null)
        {
            foreach (UpgradeList ul in lists) {
                ul.Hide();
            }
        }
        else {
            System.Type t = selectedTower.GetType(); //should be either Sniper or Bomber, which both are Towers
            if (t == typeof(Sniper))
            {
                lists[(int)towerType.SNIPER].Display(selectedTower);
            }
            else if (t == typeof(Bomber))
            {
                lists[(int)towerType.BOMBER].Display(selectedTower);
            }
            else {
                print("Unexpected type: " + t.Name);
            }
        }
    }
}
