using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageExitDoor : DoorBase
{
    public override string Message => "「F」 出る";       //表示するメッセージ

    [SerializeField] AudioClip m_doorOpenSE;  //ドアを開ける音

    public override void Interaction()
    {

        //ドアを開ける音を再生
        AudioManager.Instance.PlaySE(m_doorOpenSE, 0.5f);

    }


}
