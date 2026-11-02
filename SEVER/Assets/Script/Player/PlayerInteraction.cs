
//プレイヤーのインタラクションを管理するスクリプト
//
//責務
//各インタラクションのスクリプトを管理する
//インタラクションの入力を受け取る


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{

    private ThreadCutter m_threadCutter;                    //糸を切るスクリプト
    private DoorOperation m_doorOperation;                  //ドアの操作を行うスクリプト
    private SignboardOperation m_signboardOperation;        //看板の操作を行うスクリプト
    private DungeonCoreOperation m_dungeonCoreOperation;    //ダンジョンコアの操作を行うスクリプト

    private void Awake()
    {
        m_threadCutter = GetComponentInChildren<ThreadCutter>();
        m_doorOperation = GetComponentInChildren<DoorOperation>();
        m_signboardOperation = GetComponentInChildren<SignboardOperation>();
        m_dungeonCoreOperation = GetComponentInChildren<DungeonCoreOperation>();

    }

    public void DoorInteract()
    {
        m_doorOperation.ToggleDoor();
    }

    public void ThreadCutInteract()
    {
        m_threadCutter.CutThread();
    }

    public void SignboardInteract()
    {
        m_signboardOperation.Toggle();
    }

    public void DungeonCoreInteract()
    {
        m_dungeonCoreOperation.DestroyDungeonCore();
    }
}
