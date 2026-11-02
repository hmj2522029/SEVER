
// 敵とコアを繋ぐ糸を管理するスクリプト
//
//責務
//・糸の当たり判定を更新する
//・糸が切れた状態を管理する
//・糸を切る音を出す

using System.Collections.Generic;
using UnityEngine;
using System;

public class ThreadConnection : MonoBehaviour 
{
	public event Action OnThreadBroken;

	[SerializeField] Transform m_core;				//接続先のコア
    [SerializeField] float m_segmentLength = 0.3f;  //糸の分割長さ
    [SerializeField] int m_maxSegmentCount = 30;    //糸の最大分割数   
    [SerializeField] AudioClip m_cut;               //糸を切る音 

    private EdgeCollider2D m_edgeCollider;

    private Vector3[] m_threadPoints;               //糸の分割点のワールド座標
    private Vector2[] m_points = new Vector2[2];    //EdgeCollider2Dに設定するローカル座標
    private int m_minSegmentCount = 2;              //糸の最小分割数
    private int m_segmentCount;		                //糸の分割数(プレイヤーの視野内だけ糸を表示させるため)
	private bool m_isBroken = false;				//糸が切れているかどうか

    private string m_message = "「E」 切る";        //プレイヤーのアクション表示用のテキスト

    public string Message => m_message;

    public bool IsBroken => m_isBroken;
    public IReadOnlyList<Vector3> Points => m_threadPoints;
    public int SegmentCount => m_segmentCount;

    private void Awake()
	{

		m_edgeCollider = GetComponent<EdgeCollider2D>();

	}


	private void LateUpdate()
	{
		if(m_isBroken) return;

		UpdateLine();
        UpdateCollider();

	}

	private void UpdateLine()
	{

        //糸の長さを計算
		float distance = Vector3.Distance(transform.position, m_core.position);

        //糸の分割数を計算
        m_segmentCount = Mathf.Clamp(
            Mathf.CeilToInt(distance / m_segmentLength),
            m_minSegmentCount,  //最小分割数
            m_maxSegmentCount   //最大分割数
        );


        //必要な場合にのみ配列を作成
        if (m_threadPoints == null || m_threadPoints.Length != m_segmentCount)
        {
            m_threadPoints = new Vector3[m_segmentCount];
        }


        //糸の位置を更新
        for (int i = 0; i < m_segmentCount; i++)
        {
            //糸の分割数に応じて、0から1までの値を計算
            float t = (float)i / (m_segmentCount - 1);

            Vector3 pos = Vector3.Lerp(
                transform.position,
                m_core.position,
                t
            );

            m_threadPoints[i] = pos;
            
        }
    }

	private void UpdateCollider()
	{

        //EdgeColliderはローカル座標で設定するため変換
        m_points[0] = Vector2.zero;
        m_points[1] = transform.InverseTransformPoint(m_core.position);

        //当たり判定を糸の位置に合わせて更新
        m_edgeCollider.points = m_points;

    }


	public void Break()
	{

		if(m_isBroken) return;

		m_edgeCollider.enabled = false;
		m_isBroken = true;

        //糸を切る音を再生
        AudioManager.Instance.PlaySE(m_cut);

        OnThreadBroken?.Invoke();

	}



}
