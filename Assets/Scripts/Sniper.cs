using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Tower
{
    public Transform rotator;
    public Transform shotOrigin;
    public GameObject bulletPrefab;
    public GameObject triShot;
    public float[] triShotAngles = {0, 0};
    public string boostedAttackSFXName = "";
    private bool targetLast = false;
    private float[] shotAngles = {0};
    private float damageMultiplier = 1;
    private Transform currentTarget = null;
    protected override void Awake()
    {
        type = 0;
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
                array = manager.sniperDamage;
                newValue = TryGetUpgrade(array, newIndex);
                if (newValue != -1) { currentDamage = (int)newValue; statLevels[(int)type] = newIndex; }
                break;
            case upgradeType.SPEED:
                array = manager.sniperCooldown;
                newValue = TryGetUpgrade(array, newIndex);
                if (newValue != -1) { currentMaxCooldown = newValue; statLevels[(int)type] = newIndex; }
                break;
            case upgradeType.RANGE:
                array = manager.sniperRange;
                newValue = TryGetUpgrade(array, newIndex);
                if (newValue != -1) { 
                    currentRange = newValue; 
                    statLevels[(int)type] = newIndex;
                    rangeObject.transform.localScale = new Vector3(currentRange * 2, 0.1f, currentRange * 2);
                }
                break;
            case upgradeType.OTHER:
                array = manager.sniperTriShot;
                newValue = TryGetUpgrade(array, newIndex);
                if (newValue != -1) { if (newValue == 1) { EnableTriShot(); } statLevels[(int)type] = newIndex; }
                break;
            default: //this should never happen
                return;
        }
    }
    private float TryGetUpgrade(TowerManager.upgradeValue[] array, int newIndex) {
        float returnValue = -1;
        TowerManager.upgradeValue value;
        if (newIndex < array.Length)
        {
            value = array[newIndex];
            if (value.cost <= manager.manager.currentMoney)
            {
                manager.manager.AddMoney(-value.cost);
                returnValue = value.value;
                if (newIndex > 0)
                {
                    manager.manager.audioManager.Play("Upgrade");
                }
            }
            else
            {
                manager.manager.audioManager.Play("Error");
            }
        }
        return returnValue;
    }
    private void EnableTriShot() {
        try
        {
            shotAngles = new float[] { shotAngles[0], triShotAngles[0], triShotAngles[1] };
            damageMultiplier = 0.75f;
            triShot.SetActive(true);
        }
        catch (System.IndexOutOfRangeException) {
            print("Error: trishot index invalid");
        }
    }
    public override void CheckArea()
    {
        if (currentCooldown > 0) {return;}
        Collider[] hits = Physics.OverlapSphere(transform.position, currentRange, enemyLayer);
        if (hits.Length > 0) {
            Retarget(hits);
            Attack();
            currentCooldown = currentMaxCooldown;
        }
    }
    public void NewAlg() {
        targetLast = !targetLast;
    }
    private void Retarget(Collider[] hits) {
        if (targetLast) {
            RT_MinDistance(hits);
        }
        else { RT_MaxDistance(hits); }
    }
    private void RT_MinDistance(Collider[] hits) {
        throw new System.NotImplementedException();
    }
    private void RT_MaxDistance(Collider[] hits) { 
        /*Determines the enemy that is closest to the exit.
         Will only work when passed an array of colliders to objects with an Enemy script*/
        int maxIndex = -1;
        Transform currentMax = null;
        int currentPathIndex;
        BezierFollow currentEnemy;
        foreach (Collider c in hits) {
            if (c.gameObject.TryGetComponent(out currentEnemy)) {
                currentPathIndex = currentEnemy.currentRoute.transform.GetSiblingIndex();
                if (currentPathIndex > maxIndex) { //Whichever route is the farthest along will have the highest index, because the path hierarchy is top to bottom
                    currentMax = c.transform;
                    maxIndex = currentPathIndex;
                }
                else if (currentPathIndex == maxIndex) { //If both objects are on the same route, the one with the highest parameter value is the farthest along
                    if (currentEnemy.tParam > currentMax.GetComponent<BezierFollow>().tParam) {
                        currentMax = c.transform;
                    }
                }
            }
        }
        currentTarget = currentMax;
    }

    protected override void Attack()
    {
        Vector3 targetPosition = currentTarget.position;
        targetPosition.y = rotator.position.y;
        rotator.LookAt(targetPosition);
        RaycastHit[] hits;
        Enemy currentEnemyHit;
        int currentCalculatedDamage = Mathf.RoundToInt(currentDamage * damageMultiplier);
        float shotVelocity = 150;
        foreach (float turnValue in shotAngles)
        {
            GameObject newBullet = Instantiate(bulletPrefab, shotOrigin.position, shotOrigin.transform.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward*shotVelocity, ForceMode.VelocityChange);
            Destroy(newBullet, currentRange / shotVelocity);
            manager.manager.audioManager.Play(Stat(upgradeType.DAMAGE) > 3 ? boostedAttackSFXName:attackSFXName);
            hits = Physics.RaycastAll(shotOrigin.position, Quaternion.Euler(0, turnValue, 0) * shotOrigin.forward, currentRange, enemyLayer);
            foreach (RaycastHit h in hits) {
                if (h.collider.gameObject.TryGetComponent(out currentEnemyHit)) {
                    currentEnemyHit.Hit(currentCalculatedDamage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (this.enabled) {
            Gizmos.color = Color.red;
            foreach (float turnValue in shotAngles) {
                Gizmos.DrawRay(shotOrigin.position, Quaternion.Euler(0, turnValue, 0) * shotOrigin.forward * 30);
            }
        }
    }
}
