using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AllTogetherVisionThread : MonoBehaviour
{
    [SerializeField] Vision m_vision;   //誰の視野に入った時に糸を表示するかを決めるためのVisionコンポーネント
	[SerializeField] AStar m_aStar;
	[SerializeField] GridManager m_gridManager;

    private List<VisionThreadVisibility> m_visionThreads;   //視野内にある糸の描画方法を管理するコンポーネントのリスト
	private List<PathFollower> m_pathFollowers;

    private void Awake()
    {
        //子オブジェクトから各コンポーネントを取得する
        m_visionThreads = new List<VisionThreadVisibility>(GetComponentsInChildren<VisionThreadVisibility>());
		m_pathFollowers = new List<PathFollower>(GetComponentsInChildren<PathFollower>());

		//各コンポーネントを設定する
		foreach(var visionThread in m_visionThreads)
        {
            visionThread.SetVision(m_vision);
        }
		foreach(var pathFollower in m_pathFollowers)
		{
			pathFollower.Setting(m_aStar, m_gridManager);
		}
    }

}
