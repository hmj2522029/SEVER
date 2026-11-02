
//糸を切るスクリプト
//
//責務
//・糸を切る
//・糸が切るときのUI表示
//・糸が切れる範囲なのかの判定

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThreadCutter : MonoBehaviour
{
    [SerializeField] GameUI m_interactionUI;  //インタラクションUI

    private ThreadConnection m_currentThread;   //今の当たっている糸

    private void Update()
    {
        
        if (m_currentThread != null)
        {
            //糸が切れていないならUI表示
            if (!m_currentThread.IsBroken)
            {
                m_interactionUI.Show(m_currentThread.Message, GameUI.TextType.PlayerAction);
            }
        }

    }

    public void CutThread()
	{
		//糸あったら切る
		if(m_currentThread != null)
		{
			m_currentThread.Break();

            m_currentThread = null;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

		ThreadConnection thread = collision.GetComponent<ThreadConnection>();

		//相手にThreadConnectionがあるなら保存
		if (thread != null )
		{

			m_currentThread = thread;

		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		ThreadConnection thread = collision.GetComponent<ThreadConnection>();

		//相手とm_currentThreadが同じならnull
		if (thread == m_currentThread)
		{

			m_currentThread = null;

            m_interactionUI.Hide(GameUI.TextType.PlayerAction);
        }

	}
}
