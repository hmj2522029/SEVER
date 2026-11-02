using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCoreOperation : MonoBehaviour
{
    [SerializeField] GameUI m_gameUI;  //UI
    [SerializeField] CanvasGroupFade m_fade;        //フェード
    [SerializeField] CanvasGroup m_messageWindow;   //メッセージ欄の枠

    private DungeonCore m_currentCore;        //今の当たっているコア


    public void DestroyDungeonCore()
    {
        if (m_currentCore == null) return;

        m_currentCore.DestroyDungeon(); //ダンジョンコアを壊す

        StartCoroutine(MessageWindowDisplay()); //メッセージを表示

        m_gameUI.Hide(GameUI.TextType.PlayerAction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        DungeonCore core = collision.GetComponent<DungeonCore>();

        if (core != null)
        {
            if(core.IsDestroyed) return;

            m_currentCore = core;

            m_gameUI.Show(m_currentCore.Message, GameUI.TextType.PlayerAction);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DungeonCore core = collision.GetComponent<DungeonCore>();

        if (core == null) return;

        if (core == m_currentCore)
        {
            m_currentCore = null;

            m_gameUI.Hide(GameUI.TextType.PlayerAction);
        }

    }


    private IEnumerator MessageWindowDisplay()
    {

        yield return new WaitForSeconds(0.5f);

        m_fade.Fade(m_messageWindow, 0f, 0.8f, 0.5f);      //メッセージウィンドウを表示
        m_gameUI.ShowUI(GameUI.TextType.MessageWindow);    //メッセージを表示

        yield return new WaitForSeconds(4.0f);

        m_fade.Fade(m_messageWindow, 0.8f, 0f, 0.5f);    //メッセージウィンドウを非表示
        m_gameUI.HideUI(GameUI.TextType.MessageWindow);  //メッセージを非表示


    }
}
