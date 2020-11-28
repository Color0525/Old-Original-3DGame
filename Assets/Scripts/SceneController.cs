using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController m_Instance { get; private set; }

    public GameObject[] m_playerPrefabs;
    public GameObject[] m_enemyPrefabs;

    public Vector3 m_playerMapPosition = Vector3.zero;
    public Quaternion m_playerMapRotation = Quaternion.identity;

    void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadBattleScene(GameObject[] playerPrefabs, GameObject[] enemyPrefabs, Vector3 playerMapPos, Quaternion playerMapRotate)
    {
        foreach (var player in FindObjectsOfType<HumanoidController>())
        {
            player.StopControl();
        }
        foreach (var enemy in FindObjectsOfType<MapEnemyController>())
        {
            enemy.StopControl();
        }

        m_playerPrefabs = playerPrefabs;
        m_enemyPrefabs = enemyPrefabs;
        m_playerMapPosition = playerMapPos;
        m_playerMapRotation = playerMapRotate;

        SceneManager.LoadScene("Battle");
        //StartCoroutine(LoadScene("Battle"));
    }

    public void LoadMapScene()
    {
        SceneManager.LoadScene("Map");
        //StartCoroutine(LoadScene("Map"));
    }

    IEnumerator LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        yield return null;
    }
}
