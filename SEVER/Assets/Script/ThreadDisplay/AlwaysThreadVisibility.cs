
//糸を常に表示するコンポーネント
//
//責務
//・糸の全ての分割点をLineRendererへ設定する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysThreadVisibility : ThreadVisibility
{

    protected override void UpdateVisibility()
    {
        var points = m_threadConnection.Points; //糸の分岐点

        //分岐点がない場合
        if (points == null || points.Count == 0)
        {
            m_lineRenderer.positionCount = 0;
            return;
        }

        //糸の分割点数をLineRendererに設定
        m_lineRenderer.positionCount = points.Count;

        //全ての分割点を描画
        for (int i = 0; i < points.Count; i++)
        {
            DrawPoint(i, points[i]);
        }

    }

}
