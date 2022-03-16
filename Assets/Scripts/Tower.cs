using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public enum upgradeType {DAMAGE, SPEED, RANGE, OTHER};
    public int[] statLevels { get; private set; }
    public int type { get; protected set; }
    public LayerMask enemyLayer;
    public GameObject rangeObject;
    public string attackSFXName = "";

    protected TowerManager manager;
    protected int currentDamage;
    protected float currentMaxCooldown;
    protected float currentCooldown = 0;
    protected float currentRange;
    protected virtual void Awake()
    {
        statLevels = new int[] {-1, -1, -1, -1};
        manager = FindObjectOfType<TowerManager>();

        Upgrade(upgradeType.DAMAGE);
        Upgrade(upgradeType.SPEED);
        Upgrade(upgradeType.RANGE);
        Upgrade(upgradeType.OTHER);
    }
    private void Update()
    {
        if (currentCooldown > 0) {
            currentCooldown -= Time.deltaTime;
        }
    }
    public abstract void Upgrade(upgradeType type);
    public int Stat(upgradeType type) {
        return statLevels[(int)type];
    }
    public void ToggleRange(bool active = false) {
        rangeObject.SetActive(active);
    }
    public virtual void CheckArea() {
        if (currentCooldown > 0) {return;}
        Collider[] hits = Physics.OverlapSphere(transform.position, currentRange, enemyLayer);
        if (hits.Length > 0) {
            Attack();
            currentCooldown = currentMaxCooldown;
        }
    }
    protected abstract void Attack();
}
