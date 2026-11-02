using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEvent : GameEvent
{
    [Header("Player")]
    [SerializeField] PlayerEventController m_playerController;  //プレイヤーの操作(イベント専用)

    [Header("操作するオブジェクト等")]
    [SerializeField] Door m_door;               //ドア   
    [SerializeField] CameraMovement m_camera;   //カメラ

    [Header("UI")]
    [SerializeField] GameUI m_gameUI;           //UI


    //[Header("音")]


    public override void Execute()
    {

        StartCoroutine(Event());
    }

    private IEnumerator Event()
    {
        IsFinished = false; //イベント開始

        //プレイヤーがドアの前に行き、ドアを開ける場面
        yield return StartCoroutine(PlayDoorOpeningSequence());

        //カメラが下に移動して、プレイヤーがドアから出る場面
        yield return StartCoroutine(PlayDoorExitSequence());

        //プレイヤーが左に移動し、カメラが下に移動する場面
        yield return StartCoroutine(PlayEndingSequence());

    }

    private IEnumerator PlayDoorOpeningSequence()
    {

        //プレイヤーがドアの前まで行き、ドアを開ける場面

        m_playerController.Move(Vector2.down, 6f, 2f); //プレイヤーを下に移動させる

        yield return new WaitForSeconds(3f);

        m_playerController.Move(Vector2.right, 2f, 2f); //プレイヤーを右に移動させる

        yield return new WaitForSeconds(1.1f);

        m_playerController.Move(Vector2.down, 3f, 2f); //プレイヤーをドア前まで移動させる

        yield return new WaitForSeconds(1.5f);

        m_playerController.StopMove(); //プレイヤーの移動を停止

        yield return new WaitForSeconds(0.5f);

        m_door.Open(); //ドアを開ける

    }

    private IEnumerator PlayDoorExitSequence()
    {
        //カメラが下に移動して、プレイヤーがドアから出る場面

        m_camera.Move(Vector2.down, 6f, 2f); //カメラを下に移動させる

        yield return new WaitForSeconds(2f);

        m_playerController.Move(Vector2.down, 3f, 2f); //プレイヤーをドアから出るように移動させる

        yield return new WaitForSeconds(1.5f);

        m_playerController.StopMove();  //プレイヤーの移動を停止

        yield return new WaitForSeconds(0.5f);

    }

    private IEnumerator PlayEndingSequence()
    {

        m_playerController.Move(Vector2.left, 10f, 2f); //プレイヤーを左に移動させる

        yield return new WaitForSeconds(6f);

        m_camera.Move(Vector2.down, 10f, 2f); //カメラを下に移動させる

        yield return new WaitForSeconds(5f);

        IsFinished = true; //イベント終了

    }


}
