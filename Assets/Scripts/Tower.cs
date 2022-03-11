using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public enum upgradeType {DAMAGE, SPEED, RANGE, OTHER};
    public int[] statLevels { get; private set; }
    public int type { get; private set; }

    protected TowerManager manager;
    protected int currentDamage;
    protected float currentCooldown;
    protected float currentRange;
    private void Awake()
    {
        statLevels = new int[] {-1, -1, -1, -1};
        manager = FindObjectOfType<TowerManager>();

        Upgrade(upgradeType.DAMAGE);
        Upgrade(upgradeType.SPEED);
        Upgrade(upgradeType.RANGE);
        Upgrade(upgradeType.OTHER);
    }
    public abstract void Upgrade(upgradeType type);
    public int Stat(upgradeType type) {
        return statLevels[(int)type];
    }
}
