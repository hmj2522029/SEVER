
//飛び跳ねるアニメーション
//
//責務
//・オブジェクトを指定した高さまで上下に移動させる
//・飛び跳ねるアニメーションの再生を制御する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    [SerializeField] Transform m_target;        //飛び跳ねさせるオブジェクト
    [SerializeField] float m_height = 0.2f;     //飛び跳ねる高さ
    [SerializeField] float m_duration = 0.2f;   //飛び跳ねる時間

    private Vector3 m_startPosition;    //アニメーション開始時の位置
    private float m_time;               //アニメーションの経過時間
    private bool m_isPlaying;           //アニメーションが開始しているか

    public void Play()
    {
        m_startPosition = m_target.position;
        m_time = 0;
        m_isPlaying = true;
    }

    private void Update()
    {
        if (!m_isPlaying) return;

        if (m_time >= m_duration)
        {
            m_isPlaying = false;
            return;
        }

        m_time += Time.deltaTime;

        //経過時間を0～1に変換
        float t = m_time / m_duration;

        //最初と最後を0にした上下運動を作る
        float y = Mathf.Sin(t * Mathf.PI) * m_height;

        m_target.position = m_startPosition + Vector3.up * y;
    }

}
