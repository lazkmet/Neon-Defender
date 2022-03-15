using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Tower
{
    public Transform shotOrigin;
    protected override void Awake()
    {
        type = 1;
        base.Awake();
    }
    public override void Upgrade(upgradeType type)
    {
        int newIndex = statLevels[(int)type] + 1;
        float newValue = 0;
        TowerManager.upgradeValue[] array = null;
        switch (type)
        {
            case upgradeType.DAMAGE:
                array = manager.bomberDamage;
                newValue = TryGetUpgrade(array, newIndex);
                if (newValue != -1) { currentDamage = (int)newValue; statLevels[(int)type] = newIndex; }
                break;
            case upgradeType.SPEED:
                array = manager.bomberCooldown;
                newValue = TryGetUpgrade(array, newIndex);
                if (newValue != -1) { currentMaxCooldown = newValue; statLevels[(int)type] = newIndex; }
                break;
            case upgradeType.RANGE:
                array = manager.bomberRange;
                newValue = TryGetUpgrade(array, newIndex);
                if (newValue != -1)
                {
                    currentRange = newValue;
                    statLevels[(int)type] = newIndex;
                    rangeObject.transform.localScale = new Vector3(currentRange * 2, 0.1f, currentRange * 2);
                }
                break;
            case upgradeType.OTHER:
                break;
            default: //this should not happen
                throw new System.NotImplementedException();
        }
    }
    private float TryGetUpgrade(TowerManager.upgradeValue[] array, int newIndex)
    {
        float returnValue = -1;
        TowerManager.upgradeValue value;
        if (newIndex < array.Length)
        {
            value = array[newIndex];
            if (value.cost <= manager.manager.currentMoney)
            {
                manager.manager.AddMoney(-value.cost);
                returnValue = value.value;
            }
            else
            {
                //play fail noise
            }
        }
        return returnValue;
    }
    protected override void Attack()
    {
        //spawn particle effect at ORIGIN, play noise
        Invoke(nameof(Damage), 0.2f);
    }
    private void Damage() {
        Collider[] hits = Physics.OverlapSphere(shotOrigin.position, currentRange, enemyLayer);
        Enemy currentHit;
        foreach (Collider c in hits) {
            if (c.gameObject.TryGetComponent(out currentHit)) {
                currentHit.Hit(currentDamage);
            }
        }
    }
}
