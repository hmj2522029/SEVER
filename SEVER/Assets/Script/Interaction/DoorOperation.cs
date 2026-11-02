
//ドアの操作を行うスクリプト
//
//責務
//・ドアの操作を行う
//・ドアの操作に関するUIを表示・非表示にする
//・ドアを開けれる範囲に入ったかどうかを判定する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOperation : MonoBehaviour
{
    [SerializeField] GameUI m_gameUI;  //UI

    private DoorBase m_currentDoor;        //今の当たっているドア


    public void ToggleDoor()
    {
        if (m_currentDoor == null) return;
        
        //ドアの状態の切り替え
        m_currentDoor.ExecuteInteraction();

        //状態に対する表示するメッセージの更新
        m_gameUI.Show(m_currentDoor.Message, GameUI.TextType.PlayerAction);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

		DoorBase door = collision.GetComponent<DoorBase>();

        if (door != null)
        {
            m_currentDoor = door;

            m_gameUI.Show(m_currentDoor.Message, GameUI.TextType.PlayerAction);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
		DoorBase door = collision.GetComponent<DoorBase>();

        if (door == null) return;

        if (door == m_currentDoor)
        {
            m_currentDoor = null;

            m_gameUI.Hide(GameUI.TextType.PlayerAction);
        }

    }


}
