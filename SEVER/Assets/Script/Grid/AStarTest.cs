using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
	[SerializeField] private Transform m_startObject1;
	[SerializeField] private Transform m_goalObject1;

	[SerializeField] private Transform m_startObject2;
	[SerializeField] private Transform m_goalObject2;

	private GridManager m_gridManager;
	private AStar m_aStar;

	private void Start()
	{
		m_gridManager = GetComponent<GridManager>();
		m_aStar = GetComponent<AStar>();

		StartCoroutine(ExploreAgain(m_startObject1, m_goalObject1, m_startObject2, m_goalObject2));
	}

	private void TestFindPath(Transform startObject, Transform goalObject)
	{
		//オブジェクトのワールド座標をセル座標に変換
		Vector3Int startPosition = m_gridManager.WorldToCell(startObject.position);
		Vector3Int goalPosition = m_gridManager.WorldToCell(goalObject.position);

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

			Debug.DrawLine(start, end, Color.green, 20f);
		}

		Debug.Log($"探索結果：{path.Count}個のノード");
	}

	private IEnumerator ExploreAgain(Transform startObject1, Transform goalObject1, Transform startObject2, Transform goalObject2)
	{
		TestFindPath(startObject1, goalObject1);

		yield return new WaitForSeconds(10f);

		TestFindPath(startObject2, goalObject2);
	}
}
