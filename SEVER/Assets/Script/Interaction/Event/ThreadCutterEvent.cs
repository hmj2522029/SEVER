using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadCutterEvent : MonoBehaviour
{
    private ThreadConnection m_currentThread;   //今の当たっている糸

    public void CutThread()
    {
        //糸あったら切る
        if (m_currentThread != null)
        {
            m_currentThread.Break();

            m_currentThread = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ThreadConnection thread = collision.GetComponent<ThreadConnection>();

        //相手にThreadConnectionがあるなら保存
        if (thread != null)
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
        }

    }
}
