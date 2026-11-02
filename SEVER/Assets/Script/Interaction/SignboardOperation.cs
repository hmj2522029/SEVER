
//看板の操作を行うスクリプト
//
//責務
//・看板の操作を行う
//・操作可能な看板の検知
//・看板の操作に関するUIの表示・非表示
//・看板の内容表示の切り替え

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignboardOperation : MonoBehaviour
{

    [SerializeField] GameUI m_gameUI;               //UI
    [SerializeField] CanvasGroupFade m_fade;        //フェード
    [SerializeField] CanvasGroup m_messageWindow;   //枠

    private Signboard m_currentSignboard;   //今の当たっている看板


    public void Toggle()
    {
        if (m_currentSignboard == null) return;
        
        //看板の状態の切り替え
        m_currentSignboard.Toggle();
        
        //状態に対する表示するメッセージの更新
        m_gameUI.Show(m_currentSignboard.Message, GameUI.TextType.PlayerAction);


        //看板の内容を表示・非表示にする
        if (m_currentSignboard.IsRead)
        {
            m_gameUI.Show(m_currentSignboard.Content, GameUI.TextType.MessageWindow);

            m_fade.Fade(m_messageWindow, 0f, 0.8f, 0.5f);    //フェードイン
        }
        else
        {
            m_gameUI.Hide(GameUI.TextType.MessageWindow);

            m_fade.Fade(m_messageWindow, 0.8f, 0f, 0.5f);   //フェードアウト
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Signboard signboard = collision.GetComponent<Signboard>();

        if (signboard != null)
        {
            m_currentSignboard = signboard;

            m_gameUI.Show(m_currentSignboard.Message, GameUI.TextType.PlayerAction);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Signboard signboard = collision.GetComponent<Signboard>();

        if (signboard == null) return;

        if (signboard == m_currentSignboard)
        {
            m_currentSignboard = null;
            m_gameUI.Hide(GameUI.TextType.PlayerAction);
            m_gameUI.Hide(GameUI.TextType.MessageWindow);


            if (signboard.IsRead)
            {
                m_fade.Fade(m_messageWindow, 0.8f, 0f, 0.5f);

                signboard.Return();   //看板を読みやめる
            }

        }

    }
}
