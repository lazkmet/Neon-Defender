using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int startingMoney = 0;
    public int startingLife = 1;

    [Header("Brightness")]
    public Material maxBlue;
    public Material minBlue;
    public Material maxRed;
    public Material minRed;

    private int currentMoney = 0;
    private int currentLife = 0;
    public void AddMoney(int aMoneyToAdd) {
        currentMoney += aMoneyToAdd;
    }
    public void DealDamage(int aDamageTaken) {
        currentLife -= aDamageTaken;

        UpdateGlow();

        if (currentLife <= 0) {
            GameOver();
        }
    }
    public void GameOver() { 
    
    }
    private void UpdateGlow() {
        GameObject[] blues = GameObject.FindGameObjectsWithTag("Blue");
        GameObject[] reds = GameObject.FindGameObjectsWithTag("Red");
        float currentProgress = Mathf.Clamp01((float)currentLife / startingLife);

        foreach (GameObject obj in blues) {
            Renderer r = obj.GetComponent<Renderer>();
            r.material.Lerp(minBlue, maxBlue, currentProgress);
        }

        foreach (GameObject obj in reds) {
            Renderer r = obj.GetComponent<Renderer>();
            r.material.Lerp(maxRed, minRed, currentProgress);
        }
    }
}
