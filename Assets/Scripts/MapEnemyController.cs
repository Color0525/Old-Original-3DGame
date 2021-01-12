﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapEnemyController : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float m_turnSpeed = 3f;

    /// <summary>
    /// 索敵コライダー
    /// </summary>
    [SerializeField] SphereCollider m_discoverCollider;
    /// <summary>
    /// 索敵範囲
    /// </summary>
    [SerializeField] float m_discoverDis = 10f;
    /// <summary>
    /// 動く最大時間
    /// </summary>
    [SerializeField] float m_maxMoveTime = 5f;
    /// <summary>
    /// 止まる最大時間
    /// </summary>
    [SerializeField] float m_maxStopTime = 5f;

    Vector3 m_dir = Vector3.zero;
    Rigidbody m_rb;
    NavMeshAgent m_nma;

    /// <summary>
    /// 制御状態
    /// </summary>
    bool m_stop = false;
    /// <summary>
    /// 追尾状態
    /// </summary>
    bool m_chase = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_nma = GetComponent<NavMeshAgent>();
        m_discoverCollider.radius = m_discoverDis;
        StartCoroutine(SetMoveDir(m_maxMoveTime, m_maxStopTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (m_stop)
        {
            return;
        }
        if (m_chase)
        {
            return;
        }

        if (m_dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
        }
        else
        {
            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(m_dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);  // Slerp を使うのがポイント

            Vector3 velo = m_dir.normalized * m_movingSpeed; // 入力した方向に移動する
            velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_chase = true;
        }
    }

    /// <summary>
    /// 索敵範囲内にplayerが侵入したとき追尾する
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (m_stop)
        {
            return;
        }

        if (other.gameObject.tag == "Player")
        {
            m_nma.SetDestination(other.gameObject.transform.position);


            //Vector3 playerPosition = other.transform.position;
            //playerPosition.y = this.transform.position.y;
            //this.transform.LookAt(playerPosition);

            //m_dir = other.transform.position - this.transform.position;
            //m_dir.y = m_rb.velocity.y;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_chase = false;
        }
    }

    /// <summary>
    /// playerと接触時バトルシーンへ移行
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneController.m_Instance.LoadBattleScene(
                collision.gameObject.GetComponent<MapPlayerInformation>().m_playerPrefabs,
                gameObject.GetComponent<MapEnemyInformation>().m_enemyPrefabs,
                collision.gameObject.transform.position,
                collision.gameObject.transform.rotation);
        }
    }

    /// <summary>
    /// 制御停止
    /// </summary>
    public void StopControl()
    {
        m_stop = true;
    }

    /// <summary>
    /// m_dirにランダム方向をランダム秒数で入れ、動く止まるを繰り返す
    /// </summary>
    /// <param name="maxMoveTime"></param>
    /// <param name="maxStopTime"></param>
    /// <returns></returns>
    IEnumerator SetMoveDir(float maxMoveTime, float maxStopTime)
    {
        while (true)
        {
            m_dir = GetRandomDir();
            yield return new WaitForSeconds(Random.Range(0, maxMoveTime));
            m_dir = Vector3.zero;
            yield return new WaitForSeconds(Random.Range(0, maxStopTime));
        }
    }
    /// <summary>
    /// ランダムな平面方向を取得
    /// </summary>
    /// <returns></returns>
    Vector3 GetRandomDir()
    {
        float v = Random.Range(-1f, 1f);
        float h = Random.Range(-1f, 1f);

        return Vector3.forward * v + Vector3.right * h;
    }
}
