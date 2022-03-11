using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SniperUpgrades : UpgradeList
{
    public Button damageButton;
    public Button speedButton;
    public Button rangeButton;
    public Button triShotButton;
    public Color gray;
    private TowerManager towerInfo;
    protected override void Awake()
    {
        towerInfo = FindObjectOfType<TowerManager>();
        base.Awake();
    }
    public override void Display(Tower aTower) {
        try
        { //Very poorly coded, but set buttons to show current levels and costs
            TextMeshProUGUI costDisplay = damageButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();           
            TextMeshProUGUI degreeDisplay = damageButton.transform.Find("Upgrade Degree").GetComponent<TextMeshProUGUI>();
            int index = aTower.Stat(Tower.upgradeType.DAMAGE);
            degreeDisplay.text = Roman(index);
            if (index == towerInfo.sniperDamage.Length - 1)
            {
                damageButton.enabled = false;
                damageButton.GetComponent<Image>().color = gray;
                costDisplay.text = "MAX";
            }
            else
            {
                damageButton.enabled = true;
                damageButton.GetComponent<Image>().color = Color.white;
                costDisplay.text = Mathf.RoundToInt(towerInfo.sniperDamage[index + 1].cost * towerInfo.manager.costMultiplier).ToString();
            }
           
            costDisplay = speedButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            degreeDisplay = speedButton.transform.Find("Upgrade Degree").GetComponent<TextMeshProUGUI>();
            index = aTower.Stat(Tower.upgradeType.SPEED);
            degreeDisplay.text = Roman(index);
            if (index == towerInfo.sniperCooldown.Length - 1)
            {
                speedButton.enabled = false;
                speedButton.GetComponent<Image>().color = gray;
                costDisplay.text = "MAX";
            }
            else
            {
                speedButton.enabled = true;
                speedButton.GetComponent<Image>().color = Color.white;
                costDisplay.text = Mathf.RoundToInt(towerInfo.sniperCooldown[index + 1].cost * towerInfo.manager.costMultiplier).ToString();
            }

            costDisplay = rangeButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            degreeDisplay = rangeButton.transform.Find("Upgrade Degree").GetComponent<TextMeshProUGUI>();
            index = aTower.Stat(Tower.upgradeType.RANGE);
            degreeDisplay.text = Roman(index);
            if (index == towerInfo.sniperRange.Length - 1)
            {
                rangeButton.enabled = false;
                rangeButton.GetComponent<Image>().color = gray;
                costDisplay.text = "MAX";
            }
            else
            {
                rangeButton.enabled = true;
                rangeButton.GetComponent<Image>().color = Color.white;
                costDisplay.text = Mathf.RoundToInt(towerInfo.sniperRange[index + 1].cost * towerInfo.manager.costMultiplier).ToString();
            }

            costDisplay = triShotButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            index = aTower.Stat(Tower.upgradeType.OTHER);
            if (index < 1)
            {
                triShotButton.enabled = true;
                triShotButton.GetComponent<Image>().color = Color.white;
                costDisplay.text = Mathf.RoundToInt(towerInfo.sniperTriShot[1].cost * towerInfo.manager.costMultiplier).ToString();
            }
            else {
                triShotButton.enabled = false;
                triShotButton.GetComponent<Image>().color = gray;
                costDisplay.text = "MAX";
            }
        }
        catch (System.Exception) {
            print("Error in Button Hierarchy");
        }
        base.Display(aTower);
    }

    private string Roman(int numeral) {
        string returnValue = "";
        switch (numeral + 1) {
            case 1:
                returnValue = "I";
                break;
            case 2:
                returnValue = "II";
                break;
            case 3:
                returnValue = "III";
                break;
            case 4:
                returnValue = "IV";
                break;
            case 5:
                returnValue = "V";
                break;
            default:
                returnValue = "ERROR";
                break;
        }
       
        return returnValue;
    }
}
