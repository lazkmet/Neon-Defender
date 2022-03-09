using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int HP = 0;
    public int killReward = 0;
    public int damage = 0;
    
    public void Hit(int aDamage = 0)
    {
        HP -= aDamage;
        if (HP <= 0) {
            Destroy(this.gameObject);
        }
        FindObjectOfType<GameManager>().AddMoney(killReward);
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Exit")) {
            FindObjectOfType<GameManager>().DealDamage(damage);
        }
    }
}
