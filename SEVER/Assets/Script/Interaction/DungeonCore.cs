using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCore : MonoBehaviour
{
    [SerializeField] AudioClip m_break;     //壊す

    private bool m_isDestroyed = false; //ダンジョンコアが壊れているかどうか

    public bool IsDestroyed => m_isDestroyed;

    public string Message => m_isDestroyed ? "" : "「F」 壊す";      //表示するメッセージ


    public void DestroyDungeon()
    {

        AudioManager.Instance.PlaySE(m_break, 0.7f); //壊すSEを再生
        m_isDestroyed = true;
    }

}
