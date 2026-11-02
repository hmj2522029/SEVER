
// 糸の表示方法を管理するコンポーネント
//
// 責務
// ・糸の表示状態を更新する
// ・表示方法を派生クラスへ定義させる

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThreadVisibility : MonoBehaviour
{

    protected ThreadConnection m_threadConnection;
    protected LineRenderer m_lineRenderer;


    protected virtual void Awake()
    {
        m_threadConnection = GetComponent<ThreadConnection>();
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    protected virtual void Start()
    {

        //糸の太さを設定
        //m_lineRenderer.startWidth = 1.0f;
       // m_lineRenderer.endWidth = 1.0f;

    }

    protected virtual void Update()
    {

        if (m_threadConnection.IsBroken)
        {
            m_lineRenderer.enabled = false;
            return;
        }

        UpdateVisibility();

    }

    protected abstract void UpdateVisibility();

    protected void DrawPoint(int index, Vector3 pos)
    {
        m_lineRenderer.SetPosition(index, pos);
    }

    

}
