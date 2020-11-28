using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCommandController : MonoBehaviour
{
    public BattlePlayerController m_bpc; 

    public void PlayCommand()
    {
        m_bpc.AttackCommand();
    }

}
