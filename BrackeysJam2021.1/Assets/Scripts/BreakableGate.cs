using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGate : BaseUnit
{
    public override void Start()
    {
        base.Start();

        UnitManager.instance.AddHumanToAttackableList(transform); // enroll in attackable by animals
    }

    public override void Die(float timeUntilDestroy)
    {
        UnitManager.instance.RemoveHumanFromAttackableList(this.transform);

        GetComponent<BreakableGateInteractable>().RemoveConnectionsForDeath();

        // recalculate A* paths
        AstarPath.active.Scan();

        base.Die(timeUntilDestroy);
    }
}
