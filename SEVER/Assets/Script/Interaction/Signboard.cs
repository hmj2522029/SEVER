
//看板
//
//責務
//・看板の内容の管理
//・看板の閲覧状態の管理
//・閲覧状態の切り替え
//・閲覧状態に応じたメッセージの管理

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signboard : MonoBehaviour
{

    [SerializeField] string m_content;  //看板に書いてある内容

    private bool m_isRead = false;  //看板を読んでいるか

    public string Message => m_isRead ? "「F」 読みやめる" : "「F」 読む";       //表示するメッセージ

    public string Content => m_content;

    public bool IsRead  => m_isRead;

    //看板を読む
    public void Read()
    {
        if (m_isRead) return;

        m_isRead = true;

    }

    //看板を読みやめて戻る
    public void Return()
    {
        if (!m_isRead) return;

        m_isRead = false;


    }

    public void Toggle()
    {

        if (m_isRead)
        {
            Return();
        }
        else
        {
            Read();
        }

    }
}
