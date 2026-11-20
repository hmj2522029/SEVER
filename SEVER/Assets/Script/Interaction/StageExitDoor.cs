using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageExitDoor : DoorBase
{
    public override string Message => "「F」 出る";       //表示するメッセージ

    [SerializeField] AudioClip m_doorOpenSE;  //ドアを開ける音

    private bool m_isEnter = false;     //ドアに入ったかどうか

    public override void Interaction()
    {
        m_isEnter = true;

        //ドアを開ける音を再生
        AudioManager.Instance.PlaySE(m_doorOpenSE, 0.5f);
    }

    protected override bool CanInteract()
    {
        return !m_isEnter;
    }
}
