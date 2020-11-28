﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatusController : MonoBehaviour
{
    [SerializeField] int m_nowLife = 10;
    [SerializeField] int m_power = 3;

    public void Attack(BattleStatusController target)
    {
        Debug.Log(this.gameObject.name + " Attack");
        target.Damage(m_power);
    }

    void Damage(int power)
    {
        m_nowLife = Mathf.Max(m_nowLife - power, 0);
        Debug.Log($"{this.gameObject.name} {power}Damage @{m_nowLife}");
        if (m_nowLife == 0)
        {
            Debug.Log(this.gameObject.name + " Dead");
            FindObjectOfType<BattleManager>().DeleteUnitsList(this.gameObject);
        }
    }
}
