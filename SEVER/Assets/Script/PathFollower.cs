using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PathFollower : MonoBehaviour
{
	private AStar m_aStar;
	private GridManager m_gridManager;
	private List<PathNode> m_pathNodes;		//経路
	private Vector2 m_direction;			//現在の方向
	private Vector3 m_previousTarget;		//前回の目的地
	private int m_currentIndex;				//現在のインデックス(経路のインデックス)
	private bool m_isStartExploring;		//探索が開始したかどうか

	public bool IsStartExploring => m_isStartExploring;
	public Vector2 Dir => m_direction;

	private void Exploring(Vector3 currentPos, Vector3 targetPos)
	{
		//探索準備
		PreparationExploration(currentPos, targetPos);  //一番最初の現在位置をスタート位置とする

		//現在の目的地を取得
		Vector3 currentTarget = m_gridManager.CellToWorld(m_pathNodes[m_currentIndex].Position);

		//前回と現在のターゲットが同じ場合
		if(currentTarget == m_previousTarget) return;

	}

	private void PreparationExploration(Vector3 startPos, Vector3 targetPos)
	{
		if(m_isStartExploring) return;

		m_isStartExploring = true;
		m_currentIndex = 0;

		//オブジェクトのワールド座標をセル座標に変換
		Vector3Int startPosition = m_gridManager.WorldToCell(startPos);
		Vector3Int goalPosition = m_gridManager.WorldToCell(targetPos);

		//セル座標からPathNodeを取得
		PathNode startNode = m_gridManager.GetNode(startPosition);
		PathNode goalNode = m_gridManager.GetNode(goalPosition);

		//経路探索
		m_pathNodes = m_aStar.FindPath(startNode, goalNode);

		DebugLog();
	}

	public void Setting(AStar aStar, GridManager gridManager)
	{
		m_aStar = aStar;
		m_gridManager = gridManager;
	}

	private void DebugLog()
	{
		for (int i = 0; i < m_pathNodes.Count - 1; i++)
		{
			//セル座標をワールド座標に変換
			Vector3 start = m_gridManager.CellToWorld(m_pathNodes[i].Position);
			Vector3 end = m_gridManager.CellToWorld(m_pathNodes[i + 1].Position);

			Debug.DrawLine(start, end, Color.green, 20f);
		}

		Debug.Log($"探索結果：{m_pathNodes.Count}個のノード");

	}
}
