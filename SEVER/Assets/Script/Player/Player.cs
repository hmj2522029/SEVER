
//プレイヤーの指令塔のスクリプト
//
//責務
//・プレイヤーに必要なコンポーネントの管理
//・各コンポーネントへの指示
//・プレイヤーの各機能の連携


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] Vector2 m_direction;       //プレイヤーが最初に向いている方向
    [SerializeField] float m_speed;             //速度

    private Vision m_vision;                        //視野
    private PlayerAnimation m_playerAnimation;      //アニメーション
    private PlayerInput m_playerInput;              //プレイヤーの入力
    private PlayerInteraction m_playerInteraction;  //プレイヤーのインタラクション
    private Movement m_movement;                    //プレイヤーの移動
    private PlayerCapture m_playerCapture;          //プレイヤーが捕まったかどうか

    private void Awake()
    {

        m_movement = GetComponent<Movement>();
        m_playerInput = GetComponent<PlayerInput>();
        m_playerAnimation = GetComponent<PlayerAnimation>();
        m_playerInteraction = GetComponent<PlayerInteraction>();
        m_vision = GetComponentInChildren<Vision>();
        m_playerCapture = GetComponentInChildren<PlayerCapture>();

    }

    private void Start()
    {

        m_playerAnimation.ChangeOfDirection(m_direction);

    }

    private void Update()
    {

        if(m_playerCapture.IsCaptured)
        {
            m_movement.Stop();

            return;
        }

        //移動
        m_movement.Move(m_playerInput.Input, m_speed);

        //アニメーション
        m_playerAnimation.Animation(m_vision.Dir, m_playerInput.Input);

        //視野
        m_vision.SetDirection(m_playerAnimation.GetFacingDirection());

        //ドアを開閉のインタラクション
        if (m_playerInput.DoorOperationDown)
        {
            m_playerInteraction.DoorInteract();
        }

        //糸を切るインタラクション
        if (m_playerInput.CutThreadDown)
        {
            m_playerInteraction.ThreadCutInteract();
        }

        //看板のインタラクション
        if (m_playerInput.SignboardOperationDown)
        {
            m_playerInteraction.SignboardInteract();
        }

        //ダンジョンコアのインタラクション
        if (m_playerInput.DungeonCoreOperationDown)
        {
            m_playerInteraction.DungeonCoreInteract();
        }


    }

    public void Respawn(Vector2 position)
    {
        transform.position = position;
    }

    public void ViewingDirection(Vector2 direction)
    {
        m_playerAnimation.ChangeOfDirection(direction);
    }


}
