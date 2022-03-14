using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP = 0;
    public int killReward = 0;
    public int damage = 0;
    private GameManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }
    public void Hit(int aDamage = 0)
    {
        HP -= aDamage;
        if (HP <= 0) {
            manager.AddMoney(killReward);
            manager.Invoke("CheckWaveStop", 0.01f);
            Destroy(this.gameObject);
        }       
    }

    public void OnTriggerExit(Collider other)
    {
        manager.DealDamage(damage);
        manager.Invoke("CheckWaveStop", 0.01f);
        Destroy(this.gameObject);
    }
}
