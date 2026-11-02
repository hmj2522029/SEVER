using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectDoor : DoorBase
{

	[SerializeField] int m_stageID;             //ステージのID
    [SerializeField] string m_sceneName;        //ステージのシーン名
    [SerializeField] Transform m_returnPoint;   //ステージから出た時の場所
    [SerializeField] Vector2 m_returnDirection; //ステージから出た時の方向
    [SerializeField] int m_requiredStageID;     //入るために必要なクリアステージのID
    [SerializeField] AudioClip m_doorOpenSE;    //ドアを開ける音

   

    public int StageID => m_stageID;

    public string SceneName => m_sceneName;

    public Transform ReturnPoint => m_returnPoint;

    public Vector2 ReturnDirection => m_returnDirection;

    public int RequiredStageID => m_requiredStageID;

    public override string Message => "「F」 入る";       //表示するメッセージ

    private bool m_isLocked; //ドアがロックされているかどうか

    public override void Interaction()
	{

        if (m_isLocked) return;

        AudioManager.Instance.PlaySE(m_doorOpenSE, 0.5f);

    }

    public void LockDoor()
    {
        m_isLocked = true;
    }

    public void UnlockDoor()
    {
        m_isLocked = false;
    }

}
