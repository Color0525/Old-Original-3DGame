using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitController : MonoBehaviour
{
    public virtual void StartAction() 
    {
        FindObjectOfType<BattleManager>().StartActingTurn();
    }

    public virtual void EndAction()
    {
        FindObjectOfType<BattleManager>().EndActingTurn();
    }
}
