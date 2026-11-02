
// 糸の描画方法を管理するコンポーネント
//
// 責務
// ・LineRendererの共通処理を管理する
// ・糸の描画方法を派生クラスへ定義させる

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionThreadVisibility : ThreadVisibility
{
    
    [SerializeField] Vision m_vision;   //誰の視野に入った時に糸を表示するかを決めるためのVisionコンポーネント
    
    private List<Vector3> m_visiblePoints = new List<Vector3>();  //視野内にある糸の分割点を保存するリスト

    protected override void UpdateVisibility()
    {

        //視野内にある糸の分割点を保存するリストをクリアする
        m_visiblePoints.Clear();

       var points = m_threadConnection.Points;

        if(points == null || points.Count == 0)
        {
            m_lineRenderer.positionCount = 0;
            return;
        }

        for (int i = 0; i < points.Count; i++)
        {

            if(m_vision.IsVisible(points[i]))
            {
                m_visiblePoints.Add(points[i]);
            }

        }

        //視野内にある分割点の数が変わった場合はLineRendererのpositionCountを更新する
        if (m_lineRenderer.positionCount != m_visiblePoints.Count)
        {
            m_lineRenderer.positionCount = m_visiblePoints.Count;
        }


        //視野内にある分割点を描画する]
        for (int i = 0; i < m_visiblePoints.Count; i++)
        {
            DrawPoint(i, m_visiblePoints[i]);
        }

    }

}
