
//プレイヤーのアニメーション
//
//責務
//・各アニメーションの管理
//・移動中の方向アニメーションの制御
//・最後に入力された方向の保持
//・Idle時の向きの制御

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerAnimation : MonoBehaviour
{
	static private float m_updateTime = 0.05f;      //最後の入力方向を確定するまでの時間

    private BounceAnimation m_bounceAnimation;  //飛び跳ねる
    private Animator m_anime;
	private float m_time = 0;
	private Vector2 m_previousDirection;    //最後に入力された方向
    private Vector2 m_facingDirection;      //現在のキャラクターの向き
    private bool m_awaitingUpdate = false;  //最後の入力方向を更新待ちか
    private bool m_isInput = false;         //現在入力中か

    private void Awake()
	{
		m_anime = GetComponent<Animator>();
        m_bounceAnimation = GetComponentInChildren<BounceAnimation>();

	}

	void Update()
    {
        //入力方向の確定待ち時間を計測
        if (m_awaitingUpdate) m_time += Time.deltaTime;

        //一定時間入力が続いたら、最後の入力方向をIdle方向として確定
        if (m_time >= m_updateTime && m_awaitingUpdate == true && m_isInput == true)
		{

			//Idleアニメーションにするために入力状態を保存
			m_anime.SetFloat("LastMoveX", m_previousDirection.x);
			m_anime.SetFloat("LastMoveY", m_previousDirection.y);

			m_facingDirection = m_previousDirection;

			m_awaitingUpdate = false;
		}

	}

    //dirは視野などによって徐々に変化する実際の向きに使用し、
    //inputは最後に入力された方向をIdle時の向きとして使用する。
    public void Animation(Vector2 visionDirection, Vector2 inputDirection)
    {
        //キーが離された場合(停止中)
        if (inputDirection == Vector2.zero)
        {
            m_anime.SetBool("IsMove", false);
            m_isInput = false;
            return;
        }

        //アニメーション
        m_anime.SetFloat("MoveX", visionDirection.x);
        m_anime.SetFloat("MoveY", visionDirection.y);

        m_anime.SetBool("IsMove", true);

        //入力方向が変化した場合、最後の入力方向を更新する
        //Input Systemの入力開始・終了時に発生する方向のブレを防ぐ
        if (inputDirection != m_previousDirection && m_isInput == true)
        {

            m_awaitingUpdate = true;

            //過去の方向を保持
            m_time = 0;
            m_previousDirection = inputDirection;

        }

        m_isInput = true;

    }

    public void BounceAnimation()
    {
        m_bounceAnimation.Play();
    }

    public void ChangeOfDirection(Vector2 direction)
    {
        m_previousDirection = direction;
        m_facingDirection = direction;

        m_anime.SetFloat("LastMoveX", direction.x);
        m_anime.SetFloat("LastMoveY", direction.y);
    }

    public Vector2 GetFacingDirection()
	{
		return m_facingDirection;
	}
}
