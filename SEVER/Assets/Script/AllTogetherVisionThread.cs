using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllTogetherVisionThread : MonoBehaviour
{
    [SerializeField] Vision m_vision;   //誰の視野に入った時に糸を表示するかを決めるためのVisionコンポーネント

    private List<VisionThreadVisibility> m_visionThreads;   //視野内にある糸の描画方法を管理するコンポーネントのリスト

    private void Awake()
    {
        //子オブジェクトからVisionThreadVisibilityコンポーネントを取得する
        m_visionThreads = new List<VisionThreadVisibility>(GetComponentsInChildren<VisionThreadVisibility>());
        //各VisionThreadVisibilityコンポーネントにVisionコンポーネントを設定する
        foreach (var visionThread in m_visionThreads)
        {
            visionThread.SetVision(m_vision);
        }
    }

}
