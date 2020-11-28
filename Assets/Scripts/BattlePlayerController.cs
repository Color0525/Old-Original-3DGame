using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerController : BattleUnitController
{
    public override void StartAction()
    {
        base.StartAction();
        FindObjectOfType<BattleManager>().StartCommandSelect(this);
    }

    public override void EndAction()
    {
        base.EndAction();
        FindObjectOfType<BattleManager>().EndCommandSelect();
    }

    public void AttackCommand()
    {
        BattleStatusController target = GameObject.FindObjectOfType<BattleEnemyController>().GetComponent<BattleStatusController>();
        GetComponent<BattleStatusController>().Attack(target);
        EndAction();
    }

}
