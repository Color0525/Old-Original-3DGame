using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] State m_state = State.StartTurn;

    [SerializeField] GameObject m_CommandWindow;
    [SerializeField] Transform m_playerBattleTransform;
    [SerializeField] Transform m_enemyBattleTransform;

    public GameObject[] m_playerPrefabs;
    public GameObject[] m_enemyPrefabs;

    [SerializeField] List<GameObject> m_playerUnits = new List<GameObject>();
    [SerializeField] List<GameObject> m_enemyUnits = new List<GameObject>();
    [SerializeField] List<GameObject> m_allUnits = new List<GameObject>();
    int m_nowNum = 0;

    bool inBattle = true;
    bool won = false;


    // Start is called before the first frame update
    void Start()
    {
        SceneController sc = SceneController.m_Instance;
        if (sc.m_playerPrefabs.Length > 0)
        {
            m_playerPrefabs = sc.m_playerPrefabs;
        }
        if (sc.m_enemyPrefabs.Length > 0)
        {
            m_enemyPrefabs = sc.m_enemyPrefabs;
        }

        foreach (var unit in m_playerPrefabs)
        {
            GameObject player = Instantiate(unit, m_playerBattleTransform.position, m_playerBattleTransform.rotation);
            m_playerUnits.Add(player);
            m_allUnits.Add(player);
        }
        foreach (var unit in m_enemyPrefabs)
        {
            GameObject enemy = Instantiate(unit, m_enemyBattleTransform.position, m_enemyBattleTransform.rotation);
            m_enemyUnits.Add(enemy);
            m_allUnits.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case State.StartTurn:
                m_allUnits[m_nowNum].GetComponent<BattleUnitController>().StartAction();
                break;

            case State.ActingTurn:
                break;

            case State.EndTurn:
                if (!inBattle)
                {
                    Debug.Log(won ? "Win" : "Lose");
                    m_state = State.AfterBattle;
                    return;
                }

                m_nowNum++;
                if (m_nowNum >= m_allUnits.Count)
                {
                    m_nowNum = 0;
                }
                m_state = State.StartTurn;
                break;

            case State.AfterBattle:
                if (Input.anyKeyDown)
                {
                    SceneController.m_Instance.LoadMapScene();
                }
                break;

            default:
                break;
        }
    }

    public void StartActingTurn()
    {
        m_state = State.ActingTurn;
    }

    public void EndActingTurn()
    {
        m_state = State.EndTurn;
    }

    public void StartCommandSelect(BattlePlayerController bpc)
    {
        m_CommandWindow.SetActive(true);
        m_CommandWindow.GetComponent<PlayCommandController>().m_bpc = bpc;
    }

    public void EndCommandSelect()
    {
        m_CommandWindow.GetComponent<PlayCommandController>().m_bpc = null;
        m_CommandWindow.SetActive(false);
    }

    public void DeleteUnitsList(GameObject deadUnit)
    {
        m_allUnits.Remove(deadUnit);
        if (deadUnit.GetComponent<BattleEnemyController>())
        {
            m_enemyUnits.Remove(deadUnit);
            if (m_enemyUnits.Count == 0)
            {
                inBattle = false;
                won = true;
            }
        }
        else if (deadUnit.GetComponent<BattlePlayerController>())
        {
            m_playerUnits.Remove(deadUnit);
            if (m_playerUnits.Count == 0)
            {
                inBattle = false;
                won = false;
            }
        }
    }

    public enum State
    {
        StartTurn,
        ActingTurn,
        EndTurn,
        AfterBattle,
    }
}
