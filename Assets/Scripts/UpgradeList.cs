using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public abstract class UpgradeList : MonoBehaviour
{
    private CanvasGroup menuItems;
    protected virtual void Awake()
    {
        menuItems = this.GetComponent<CanvasGroup>();
        Hide();
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
}
