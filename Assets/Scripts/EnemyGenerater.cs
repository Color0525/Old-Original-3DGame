using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerater : MonoBehaviour
{
    [SerializeField] GameObject m_mapEnemyPrefabs;
    /// <summary>
    /// 出現数
    /// </summary>
    [SerializeField] int m_instanceNum = 3;
    /// <summary>
    /// 生成する四角範囲の大きさ
    /// </summary>
    [SerializeField] float m_generateRange = 20f;
    /// <summary>
    /// プレイヤーとの最低距離
    /// </summary>
    [SerializeField] float m_playerDis = 10f;

    /// <summary>
    /// 敵を生成
    /// </summary>
    public void GenerateEnemy()
    {
        Vector3 generaterPos = transform.position;
        for (int i = 0; i < m_instanceNum; i++)
        {        
            float randomX = Random.Range(generaterPos.x - m_generateRange, generaterPos.x + m_generateRange);
            float randomZ = Random.Range(generaterPos.z - m_generateRange, generaterPos.z + m_generateRange);
            Vector3 instancePos = new Vector3(randomX, generaterPos.y, randomZ);
            Vector3 playerPos = FindObjectOfType<HumanoidController>().transform.position;
            if (Vector3.Distance(instancePos, playerPos) > m_playerDis)
            {
                Instantiate(m_mapEnemyPrefabs, instancePos, Quaternion.identity);
            }
        }
    }
}
