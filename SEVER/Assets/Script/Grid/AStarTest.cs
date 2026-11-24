using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
	[SerializeField] private Transform m_startObject;
	[SerializeField] private Transform m_goalObject;

	private GridManager m_gridManager;
	private AStar m_aStar;

	private void Start()
	{
		m_gridManager = GetComponent<GridManager>();
		m_aStar = GetComponent<AStar>();

		TestFindPath();
	}

	private void TestFindPath()
	{
		//オブジェクトのワールド座標をセル座標に変換
		Vector3Int startPosition = m_gridManager.WorldToCell(m_startObject.position);
		Vector3Int goalPosition = m_gridManager.WorldToCell(m_goalObject.position);

		//セル座標からPathNodeを取得
		PathNode startNode = m_gridManager.GetNode(startPosition);
		PathNode goalNode = m_gridManager.GetNode(goalPosition);

		//経路探索
		List<PathNode> path = m_aStar.FindPath(startNode, goalNode);

		for (int i = 0; i < path.Count - 1; i++)
		{
			//セル座標をワールド座標に変換
			Vector3 start = m_gridManager.CellToWorld(path[i].Position);
			Vector3 end = m_gridManager.CellToWorld(path[i + 1].Position);

			Debug.DrawLine(start, end, Color.green, 40f);
		}

		Debug.Log($"探索結果：{path.Count}個のノード");
	}
}
