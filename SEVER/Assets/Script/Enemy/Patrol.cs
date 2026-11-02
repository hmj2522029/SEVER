
//敵を巡回させるコンポーネント
//
//責務
//・巡回ポイントに沿って移動する
//・現在の巡回先を管理する
//・巡回方向を管理する
//・現在の移動方向を提供する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{

	[SerializeField] float m_speed;         //移動速度
	[SerializeField] Transform[] m_points;  //巡回ポイント


	private int m_index = 0;		//今向かっているポイント
	private Transform m_target;		//現在の移動目標地点
	private int m_direction = 1;	//巡回方向(1: 順方向, -1: 逆方向)
	private Vector2 m_dir;          //方向ベクトル
	private bool m_stop = false;	//更新を止める

    public Vector2 Dir => m_dir;
    public float Speed => m_speed;

	private void Start()
	{
		m_target = m_points[0];
	}

	private void Update()
	{
		if (m_stop) return;

		//目的地点との距離を確認し、到着していたら次の巡回地点へ変更
		if(Vector2.Distance(m_target.position, transform.position) <= 0.1f)
		{
			//端の地点に到達した場合、巡回方向を反転
			if (m_target == m_points[m_points.Length - 1])
			{
				m_direction = -1;
			}
			else if (m_target == m_points[0])
			{
				m_direction = 1;
			}

			//次に向かう巡回地点を設定
			m_index = m_index + m_direction;
			m_target = m_points[m_index];

		}
		
	}

	private void FixedUpdate()
	{
		if (m_stop) return;

		//現在位置から目的地への方向ベクトル
		m_dir = (m_target.position - transform.position).normalized;
		
    }


	private void OnDrawGizmos()
	{

		if (m_points == null || m_points.Length < 2) return;

		Gizmos.color = Color.green;

		for(int i = 0; i < m_points.Length - 1; i++)
		{

			//敵が巡回するポイントを線で結ぶ(デバックしやすくするため)
			Gizmos.DrawLine(
				m_points[i].position,		//現在のポイント
				m_points[i + 1].position	//次のポイント
			);

		}

	}

    public void UpdateStop()
	{
		m_stop = true;
	}

	public void UpdateStart()
	{
		m_stop = false;
	}

}
