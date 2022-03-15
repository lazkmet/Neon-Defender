using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public abstract class UpgradeList : MonoBehaviour
{
    private CanvasGroup menuItems;
    public Button damageButton;
    public Button speedButton;
    public Button rangeButton;
    protected TowerManager towerInfo;
    
    private void Awake()
    {
        towerInfo = FindObjectOfType<TowerManager>();
        menuItems = this.GetComponent<CanvasGroup>();
    }
    public virtual void Display(Tower aTower)
    { //fixed with https://answers.unity.com/questions/971009/make-ui-elements-invisible.html
        menuItems.alpha = 1f;
        menuItems.blocksRaycasts = true;
    }
    public void Hide() {
        menuItems.alpha = 0f;
        menuItems.blocksRaycasts = false;
    }
    protected string Roman(int numeral)
    {
        string returnValue = "";
        switch (numeral + 1)
        {
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
