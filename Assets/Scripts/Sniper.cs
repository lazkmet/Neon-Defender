using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Tower
{
    public override void Upgrade(upgradeType type)
    {
        int newIndex = ++statLevels[(int)type];
        float moneyAvailable = manager.manager.currentMoney;
        float value;

        switch (type) {
            case upgradeType.DAMAGE:
                value = manager.sniperDamage[newIndex].value;
                break;
            case upgradeType.SPEED:
                value = manager.sniperCooldown[newIndex].value;
                break;
            case upgradeType.RANGE:
                value = manager.sniperRange[newIndex].value;
                break;
            case upgradeType.OTHER:
                value = manager.sniperTriShot[newIndex].value;
                break;
            default:
                value = 0;
                break;
        }
        print("Upgrade: Index " + newIndex + ", Value " + value);
    }
}
