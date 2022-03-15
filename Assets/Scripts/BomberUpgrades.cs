using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BomberUpgrades : UpgradeList
{
    public override void Display(Tower aTower)
    {
        try
        { //Very poorly coded, but set buttons to show current levels and costs
            TextMeshProUGUI costDisplay = damageButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI degreeDisplay = damageButton.transform.Find("Upgrade Degree").GetComponent<TextMeshProUGUI>();
            int index = aTower.Stat(Tower.upgradeType.DAMAGE);
            degreeDisplay.text = Roman(index);
            if (index == towerInfo.bomberDamage.Length - 1)
            {
                damageButton.enabled = false;
                damageButton.GetComponent<Image>().color = towerInfo.manager.gray;
                costDisplay.text = "MAX";
            }
            else
            {
                damageButton.enabled = true;
                damageButton.GetComponent<Image>().color = Color.white;
                costDisplay.text = Mathf.RoundToInt(towerInfo.bomberDamage[index + 1].cost * towerInfo.manager.costMultiplier).ToString();
            }

            costDisplay = speedButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            degreeDisplay = speedButton.transform.Find("Upgrade Degree").GetComponent<TextMeshProUGUI>();
            index = aTower.Stat(Tower.upgradeType.SPEED);
            degreeDisplay.text = Roman(index);
            if (index == towerInfo.bomberCooldown.Length - 1)
            {
                speedButton.enabled = false;
                speedButton.GetComponent<Image>().color = towerInfo.manager.gray;
                costDisplay.text = "MAX";
            }
            else
            {
                speedButton.enabled = true;
                speedButton.GetComponent<Image>().color = Color.white;
                costDisplay.text = Mathf.RoundToInt(towerInfo.bomberCooldown[index + 1].cost * towerInfo.manager.costMultiplier).ToString();
            }

            costDisplay = rangeButton.transform.Find("Cost").GetComponent<TextMeshProUGUI>();
            degreeDisplay = rangeButton.transform.Find("Upgrade Degree").GetComponent<TextMeshProUGUI>();
            index = aTower.Stat(Tower.upgradeType.RANGE);
            degreeDisplay.text = Roman(index);
            if (index == towerInfo.bomberRange.Length - 1)
            {
                rangeButton.enabled = false;
                rangeButton.GetComponent<Image>().color = towerInfo.manager.gray;
                costDisplay.text = "MAX";
            }
            else
            {
                rangeButton.enabled = true;
                rangeButton.GetComponent<Image>().color = Color.white;
                costDisplay.text = Mathf.RoundToInt(towerInfo.bomberRange[index + 1].cost * towerInfo.manager.costMultiplier).ToString();
            }
        }
        catch (System.Exception)
        {
            print("Error in Button Hierarchy");
        }
        base.Display(aTower);
    }
}
