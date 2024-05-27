using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [HideInInspector] public bool invincible;

    protected override void Update()
    {
        base.Update();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void DealDamage(float damageAmount)
    {
        if (!invincible) base.DealDamage(damageAmount);
    }
}
