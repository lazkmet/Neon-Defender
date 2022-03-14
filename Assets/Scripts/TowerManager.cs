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
    public LayerMask towerLayer;
    public UpgradeList[] lists;
    public RectTransform validScreenSpace;
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
    private GameObject selectedRange = null;
    [HideInInspector]
    public GameManager manager;
    [HideInInspector]
    public Camera cam;
    private bool placing;
    private List<Tower> activeTowers = new List<Tower>();
    private int currentToCheck = 0;
    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        cam = FindObjectOfType<Camera>();
        currentCosts = new int[numTypes];
    }
    private void Update()
    {
        if (Time.timeScale == 0) { return; }
        if (Input.GetMouseButtonDown(0)) {
            if (ValidArea()) { //mouse within valid screen area?
                if (placing)
                { //lock it in and make selected
                    if (manager.currentMoney >= currentCosts[selectedTower.type] && selectedTower.GetComponent<MouseFollow>().ValidPlacement())
                    {
                        MouseFollow temp = selectedTower.gameObject.GetComponent<MouseFollow>();
                        temp.RestoreMaterials();
                        Destroy(temp);
                        selectedTower.enabled = true;
                        activeTowers.Add(selectedTower);
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
                    TryFindTower();
                    UpdateUpgrades();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) {
            if (placing)
            {
                Destroy(selectedTower.gameObject);
                placing = false;
            }
            selectedTower = null;
            UpdateUpgrades();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            manager.StartWave();
        }

        if (activeTowers.Count > 0) {
            if (currentToCheck < activeTowers.Count) {
                activeTowers[currentToCheck].CheckArea();
            }
            else {
                currentToCheck = 0;
                activeTowers[currentToCheck].CheckArea();
            }
            currentToCheck++;
        }
    }
    public void TryUpgradeSelected(int type) {
        if (selectedTower != null) {
            selectedTower.Upgrade((Tower.upgradeType)type);
            UpdateUpgrades();
        }
    }
    private void DestroyExistingTowers()
    {
        foreach (Tower t in FindObjectsOfType<Tower>())
        {
            Destroy(t.gameObject);           
        }
        activeTowers.Clear();
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
    private void TryFindTower() {
        RaycastHit hit;
        Tower newSelected = null;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, towerLayer))
        {
            hit.collider.gameObject.TryGetComponent(out newSelected);  
        }
        selectedTower = newSelected;
    }
    public void StartPlacement(int tower) {
        placing = true;
        selectedTower = null;
        UpdateUpgrades();
        selectedTower = Instantiate(towerPrefabs[tower % towerPrefabs.Length]).GetComponent<Tower>();
        MouseFollow temp = selectedTower.gameObject.GetComponent<MouseFollow>();
        temp.floor = this.floor;
    }
    private void BuyTower(int tower) {
        int cost = Mathf.RoundToInt(currentCosts[tower % currentCosts.Length] * manager.costMultiplier);
        manager.AddMoney(-cost);
        currentCosts[tower % currentCosts.Length] += perTowerGrowth;
        UpdateButtons();
    }

    public bool ValidArea() {
        return RectTransformUtility.RectangleContainsScreenPoint(validScreenSpace, Input.mousePosition);
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

    public void TryShowArea() { 
    
    }
}
