
//通った道を記録するコンポーネント
//
//責務
//・通った道を管理
//・通った道記録の開始と停止
//・通った道の記録のリセット

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PathRecorder : MonoBehaviour
{
    private List<Vector2> m_path = new List<Vector2>(); //通った道を記録するリスト
    private bool m_isUpdate = false;

    public IReadOnlyList<Vector2> Path => m_path;

    private void Update()
    {

        if (!m_isUpdate) return;

        //現在の位置を記録する(何も記録していない場合)
        if(m_path.Count == 0)
        {
            m_path.Add(transform.position);
            return;
        }
        else
        {

            //前回の位置と現在の位置が一定距離以上離れている場合のみ記録する
            if(Vector2.Distance(m_path[m_path.Count - 1], transform.position) > 1.0f)
            {
                m_path.Add(transform.position);
            }

        }
    }

    private void OnDrawGizmos()
    {

        if (m_path == null || m_path.Count < 2) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < m_path.Count - 1; i++)
        {

            //敵が追尾した道を線で結ぶ(デバックしやすくするため)
            Gizmos.DrawLine(
                m_path[i],       //現在のポイント
                m_path[i + 1]    //次のポイント
            );

        }

    }


    public void StartRecording()
    {
        m_isUpdate = true;
    }

    public void StopRecording()
    {
        m_isUpdate = false;
    }

    public void Clear()
    {
        if (m_path.Count == 0) return;

        m_path.Clear();
    }
    public void RemoveLastPoint(int index)
    {
        if (m_path.Count == 0) return;

        m_path.RemoveAt(index);
    }

}
