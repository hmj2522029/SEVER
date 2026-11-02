
//イベント中のプレイヤーの操作
//
//責務
//・イベントからの指示を受け取り、Playerの各コンポーネントへ指示を送る
//・イベント中のプレイヤーの移動を制御する
//・イベント中のプレイヤーの向きを制御する
//・イベント中の視野を制御する
//・イベント中のアニメーションを制御する
//・イベント用UIを制御する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEventController : MonoBehaviour
{

    private PlayerAnimation m_playerAnimation;  //プレイヤーアニメーション
    private Movement m_movement;                //移動
    private Vision m_vision;                    //視野
    private AlertIcon m_alertIcon;              //マーク
    private Teleport m_teleport;                //テレポート
    private ThreadCutterEvent m_threadCutter;   //糸を切る(イベント専用)

    private Vector2 m_input;

    private void Awake()
    {
        m_playerAnimation = GetComponent<PlayerAnimation>();
        m_movement = GetComponent<Movement>();
        m_teleport = GetComponent<Teleport>();
        m_alertIcon = GetComponentInChildren<AlertIcon>();
        m_vision = GetComponentInChildren<Vision>();
        m_threadCutter = GetComponentInChildren<ThreadCutterEvent>();

    }

    private void Update()
    {
        //アニメーション
        m_playerAnimation.Animation(m_vision.Dir, m_input);

        //視野
        m_vision.SetDirection(m_playerAnimation.GetFacingDirection());

    }

    public void Move(Vector2 direction, float distance, float speed)
    {
        m_input = direction;
        m_movement.Move(direction, distance, speed);
    }

    public void StopMove()
    {
        m_input = Vector2.zero;
    }

    public void HideVision()
    {
        m_vision.HideVision();
    }

    public void ShowAlert()
    {
        m_alertIcon.ShowAlert();
        m_playerAnimation.BounceAnimation();
    }

    public void CutThread()
    {
        m_threadCutter.CutThread();
    }

    public void ShowLost()
    {
        m_alertIcon.ShowLost();
    }

    public void ChangeOfDirection(Vector2 direction)
    { 
        m_playerAnimation.ChangeOfDirection(direction);
    }

    public void MoveTo(Vector2 position)
    {
        m_teleport.MoveTo(position);
    }

   
}
