
//追尾するコンポーネント
//
//責務
//・追尾するオブジェクトの方向を計算
//・追尾するオブジェクトの方向に移動する


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{

	[SerializeField] float m_speed;     //移動速度

	private Transform m_target;         //追尾するオブジェクト
	private Vector2 m_dir;              //方向ベクトル
	private bool m_isUpdate = false;    //更新を止める
    private float m_currentSpeed;       //現在の速度

    public Vector2 Dir => m_dir;
    public float Speed => m_currentSpeed; 


	private void Update()
	{
		if(m_isUpdate) return;

        //ターゲットが設定されていない場合は処理を行わない
		if (m_target == null) return;

        //ターゲットの方向を計算
		m_dir = (m_target.position - transform.position).normalized;

    }

    private void FixedUpdate()
	{
		if(m_isUpdate) return;

        //ターゲットとの距離が1.0以上の場合のみ移動する
        if ((m_target.position - transform.position).magnitude > 1.2)
		{

            m_currentSpeed = m_speed;

        }
		else
		{
            m_currentSpeed = 0;
        }



    }

    public void SetTarget(Transform transform)
	{
		m_target = transform;
	}

	public void UpdateStop()
	{
        m_isUpdate = true;
	}
	public void UpdateStart()
	{
        m_isUpdate = false;
	}

}
