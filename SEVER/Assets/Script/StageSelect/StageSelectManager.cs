using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{


    [Header("フェード等")]
    [SerializeField] CanvasGroupFade m_fade;        //フェード
    [SerializeField] CanvasGroup m_background;      //画面のフェード用の画像
    [SerializeField] CanvasGroup m_messageWindow;   //メッセージ欄の枠

    [Header("ステージ選択ドア")]
    [SerializeField] StageSelectDoor[] m_stageDoors;    

    [Header("プレイヤー")]
    [SerializeField] Player m_player;

    [Header("UI")]
    [SerializeField] GameUI m_gameUI;  //UI
    [SerializeField] string m_message; //メッセージ

    private bool m_isMessageWindowDisplayed = false;   //メッセージウィンドウが表示されているか

    private void Start()
    {
        m_fade.FadeOut(m_background, 2.0f);

        foreach(var door in m_stageDoors)
        {


            
            if(door.StageID == GameManager.Instance.CurrentStageID)
            {
                //ステージに入る前の指定した位置にプレイヤーを移動させる
                m_player.Respawn(door.ReturnPoint.transform.position);

                //ステージに入る前に指定した方向にプレイヤーを向ける
                m_player.ViewingDirection(door.ReturnDirection);
            }
        }

        //全てのステージをクリアしている場合
        if (GameManager.Instance.IsAllStagesCleared(m_stageDoors.Length))
        {
            SceneManager.LoadScene("ClearScene");
        }

    }

    private void OnEnable()
    {
        foreach (var door in m_stageDoors)
        {
            door.OnInteraction += OnDoorInteraction;
        }
    }

    private void OnDisable()
    {
        foreach (var door in m_stageDoors)
        {
            door.OnInteraction -= OnDoorInteraction;
        }
    }

    private void OnDoorInteraction(DoorBase door)
    {
        StageSelectDoor stageDoor = door as StageSelectDoor;

        //StageSelectDoorではない場合
        if (stageDoor == null) return;

        //入るために必要なステージをクリアしていない場合
        if (stageDoor.RequiredStageID != 0 &&
            !GameManager.Instance.IsStageCleared(stageDoor.RequiredStageID)
            )
        {
            if (m_isMessageWindowDisplayed) return; //メッセージウィンドウが表示されている場合は何もしない

            m_isMessageWindowDisplayed = true;      //メッセージウィンドウが表示されている状態にする
            StartCoroutine(MessageWindowDisplay()); //メッセージを表示
            return;
        }

        EnterStage(stageDoor);
    }

    private void EnterStage(StageSelectDoor stageDoor)
    {
        //どのステージに入ったかをIDで保存
        GameManager.Instance.SetStage(stageDoor.StageID);

        m_fade.FadeIn(m_background, 3.0f);

        StartCoroutine(LoadStage(stageDoor.SceneName));
    }

    private IEnumerator LoadStage(string sceneName)
    {
        yield return new WaitForSeconds(4.5f);

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator MessageWindowDisplay()
    {

        m_fade.Fade(m_messageWindow, 0f, 0.8f, 0.5f);               //メッセージウィンドウを表示
        m_gameUI.Show(m_message, GameUI.TextType.MessageWindow);    //メッセージを表示

        yield return new WaitForSeconds(2.0f);

        m_fade.Fade(m_messageWindow, 0.8f, 0f, 0.5f);    //メッセージウィンドウを非表示
        m_gameUI.Hide(GameUI.TextType.MessageWindow);    //メッセージを非表示

        yield return new WaitForSeconds(1.0f);

        m_isMessageWindowDisplayed = false;              //メッセージウィンドウが表示されていない状態にする

    }

}
