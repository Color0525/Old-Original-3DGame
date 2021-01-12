using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleController継承用
/// </summary>
public class BattleUnitController : MonoBehaviour
{
    /// <summary>
    /// 行動開始
    /// </summary>
    public virtual void StartAction() 
    {
        FindObjectOfType<BattleManager>().StartActingTurn();
    }

    /// <summary>
    /// 行動開始
    /// </summary>
    public virtual void EndAction()
    {
        FindObjectOfType<BattleManager>().EndActingTurn();
    }
}
