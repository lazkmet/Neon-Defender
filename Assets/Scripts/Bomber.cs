using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Tower
{
    protected override void Awake()
    {
        type = 1;
        base.Awake();
    }
    public override void Upgrade(upgradeType type)
    {
        throw new System.NotImplementedException();
    }
    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
