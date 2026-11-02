
//メインカメラを動かすスクリプト
//
//責務
//・指定距離の移動
//・移動の開始と停止

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Vector2 m_move;             //移動量
    private Vector2 m_startPosition;    //開始地点
    private float m_moveDistance;       //指定距離
    private bool m_distanceMove;        //指定距離での移動かどうか

    private void Update()
    {

        if (m_distanceMove)
        {
            float distance =
                Vector2.Distance(m_startPosition, transform.position);

            if (distance >= m_moveDistance)
            {
                Stop();
                return;
            }

            transform.position += (Vector3)m_move * Time.deltaTime;
        }

    }

    //入力を指定する用のMove関数
    public void Move(Vector2 direction, float distance, float speed)
    {

        m_startPosition = transform.position;
        m_moveDistance = distance;
        m_distanceMove = true;

        m_move = direction * speed;

    }

    private void Stop()
    {
        m_move = Vector2.zero;
        m_distanceMove = false;
    }

}
