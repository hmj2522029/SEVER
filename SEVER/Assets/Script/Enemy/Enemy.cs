
//敵の状態を管理するクラス(指令塔)
//
//責務
//・敵の状態を管理する
//・状態に応じて各コンポーネントの更新を開始/停止する
//・視野の方向を各コンポーネントの方向に応じて更新する
//・警告アイコンの表示/非表示を制御する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private enum EnemyState
	{
		Patrol,		//巡回
		Chase,      //プレイヤーに追従
        Return,		//元の位置に戻る
        Stopped		//完全停止
	}
	private EnemyState m_state = EnemyState.Patrol; //状態

    //各コンポーネント
    private Vision m_vision;			//視野
	private Patrol m_patrol;            //巡回
	private Chase m_chase;				//追尾(プレイヤーを追いかける)
	private Return m_return;			//元の位置に戻る
    private ThreadConnection m_thread;	//糸(接続しているコアとの糸)
    private Movement m_movement;        //移動
    private AlertIcon m_alertIcon;      //警告アイコン

    public bool IsChasing => m_state == EnemyState.Chase; //追尾状態かどうか(見つかった状態)

    public bool IsStopped => m_state == EnemyState.Stopped; //完全停止状態かどうか

    private void Awake()
	{

		m_patrol = GetComponentInChildren<Patrol>();
		m_vision = GetComponentInChildren<Vision>();
		m_thread = GetComponentInChildren<ThreadConnection>();
		m_chase = GetComponentInChildren<Chase>();
		m_return = GetComponentInChildren<Return>();
        m_movement = GetComponentInChildren<Movement>();
        m_alertIcon = GetComponentInChildren<AlertIcon>();

    }

	private void Start()
	{

		m_patrol.UpdateStart();
		m_chase.UpdateStop();
		m_return.UpdateStop();

    }


	void Update()
    {
		//プレイヤーが見つかったかどうか
		Transform playerTarget = null;

		foreach (var hit in m_vision.Hits)
		{

            //プレイヤー以外のコライダーは無視する
            if (hit.collider == null || !hit.collider.CompareTag("Player"))
				continue;

			playerTarget = hit.collider.transform;
			break;
		}

		bool foundPlayer = playerTarget != null;

		switch (m_state)
		{
			case EnemyState.Patrol:
				UpdateVision(m_patrol.Dir);
                m_movement.Move(m_patrol.Dir, m_patrol.Speed);

                //プレイヤーを見つけた場合、追尾状態に切り替える
                if (foundPlayer)
				{
					ChangeStateToChase(playerTarget);   //追尾状態に切り替える
                    m_alertIcon.ShowAlert();            //警告アイコンを表示する
                }

				break;
			case EnemyState.Chase:
				UpdateVision(m_chase.Dir);
                m_movement.Move(m_chase.Dir, m_chase.Speed);

                //プレイヤーを見失った場合、元の位置に戻る状態に切り替える
                if (!foundPlayer)
				{
                    ChangeStateToReturn();    //元の位置に戻る状態に切り替える
                    m_alertIcon.ShowLost();   //はてなアイコンを表示する
                }


				break;
			case EnemyState.Return:
                UpdateVision(m_return.Dir);
                m_movement.Move(m_return.Dir, m_return.Speed);

                //戻るときにプレイヤーを見つけた場合、追尾状態に切り替える
                if (foundPlayer)
                {
                    ChangeStateToChase(playerTarget);   //追尾状態に切り替える
                    m_alertIcon.ShowAlert();            //警告アイコンを表示する
                }


                //元の位置に戻り終えた場合、巡回状態に切り替える
                if (m_return.IsReturnEnd)
				{
					ChangeStateToPatrol();  //巡回状態に切り替える
                    m_alertIcon.Hide();     //アイコンを非表示にする
                }


				break;
            case EnemyState.Stopped:

                m_movement.Stop();

				return;
		}

	}

	private void ChangeStateToPatrol()
	{
		//次の状態に切り替え
		m_state = EnemyState.Patrol;

        //一旦全部止める
        UpdateStopEverything();

        //次の状態の設定
        m_patrol.UpdateStart();

    }

	private void ChangeStateToChase(Transform target)
	{
		//次の状態に切り替え
		m_state = EnemyState.Chase;

        //一旦全部止める
        UpdateStopEverything();

        //次の状態の設定
        m_chase.SetTarget(target);
		m_chase.UpdateStart();

        //追尾した道の記録を開始する(戻るために必要)
        m_return.StartRecording();
	}


	private void ChangeStateToReturn()
	{
        //次の状態に切り替え
        m_state = EnemyState.Return;

        //一旦全部止める
        UpdateStopEverything();

        //次の状態の設定
		m_return.BeginReturn();
    }

    private void UpdateVision(Vector2 dir)
	{
		m_vision.SetDirection(dir);
	}

	private void OnThreadBroken()
	{
		m_state = EnemyState.Stopped;

		m_vision.enabled = false;
		m_patrol.enabled = false;

        UpdateStopEverything(); 
        m_vision.HideVision();
	}

    private void UpdateStopEverything()
    {
        m_chase.UpdateStop();
        m_patrol.UpdateStop();
    }


    private void OnEnable()
	{
		m_thread.OnThreadBroken += OnThreadBroken;
	}

	private void OnDisable()
	{
		m_thread.OnThreadBroken -= OnThreadBroken;
	}


}
