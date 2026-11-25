using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AStar : MonoBehaviour
{
    private List<PathNode> m_openList = new List<PathNode>();		//探索候補のノード
    private List<PathNode> m_closedList = new List<PathNode>();		//探索済みのノード
	private List<PathNode> m_searchedNodes = new List<PathNode>();	//前回で検索したノード

	private GridManager m_gridManager;  

    private void Awake()
    {
        m_gridManager = GetComponent<GridManager>();
    }

	private PathNode GetLowestFCostNode()
    {
        //OpenListからFCostが最も小さいノードを取得
        
        PathNode lowestFCostNode = m_openList[0];//最初のノードを仮の最小値として設定

        foreach (PathNode node in m_openList)
        {
            if (node.FCost < lowestFCostNode.FCost ||
				(node.FCost == lowestFCostNode.FCost && node.HCost < lowestFCostNode.HCost))
            {
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }

    private void MoveToClosedList(PathNode node)
    {
        //探索済みリストに追加
        m_closedList.Add(node);

        //探索候補リストから削除
        m_openList.Remove(node);
    }

    private int GetMoveCost(PathNode currentNode, PathNode adjacentNode)
    {
        //移動コストを計算
        int moveCost = currentNode.GCost;

        //隣接ノードが斜め方向かどうかを判定
        if (currentNode.Position.x != adjacentNode.Position.x && 
            currentNode.Position.y != adjacentNode.Position.y)
        {
            moveCost += PathSettings.DiagonalCost; //斜め移動のコストを加算
        }
        else
        {
            moveCost += PathSettings.StraightCost; //上下左右の移動コストを加算
        }

        return moveCost;
    }

    private int GetDistanceCost(PathNode currentNode, PathNode goalNode)
    {
        //距離コストを計算
        int xDistance = Mathf.Abs(currentNode.Position.x - goalNode.Position.x);
        int yDistance = Mathf.Abs(currentNode.Position.y - goalNode.Position.y);

        //斜め移動の回数を計算
        int diagonalCount = Mathf.Min(xDistance, yDistance);

        //直線移動の回数を計算
        int straightCount = Mathf.Abs(xDistance - yDistance);

        //総コストを計算
        return PathSettings.DiagonalCost * diagonalCount + PathSettings.StraightCost * straightCount;
    }

    private void UpdateNodeCost(PathNode currentNode,PathNode adjacentNode,PathNode goalNode)
    {
        //現在のGCostを計算
        int moveCost = GetMoveCost(currentNode, adjacentNode);

        //既に探索済みのノードの場合はスキップ
        if (m_closedList.Contains(adjacentNode)) return;

        //隣接ノードがOpenListに存在するかどうか
        if (m_openList.Contains(adjacentNode))
        {
            //既にOpenListに存在する場合、GCostが小さい場合のみ更新
            if (moveCost < adjacentNode.GCost)
            {
                adjacentNode.GCost = moveCost;
                adjacentNode.Parent = currentNode;
            }
        }
        else
        {
            //OpenListに存在しない場合、新規に追加
            adjacentNode.GCost = moveCost;
            adjacentNode.HCost = GetDistanceCost(adjacentNode, goalNode);
            adjacentNode.Parent = currentNode;
            m_openList.Add(adjacentNode);
			m_searchedNodes.Add(adjacentNode);
        }
    }

    public List<PathNode> FindPath(PathNode startNode, PathNode goalNode)
    {
		//初期化
		InitializationSearchedNodes();
		m_openList.Clear();
        m_closedList.Clear();

		//スタートまたはゴールが無いか通行不可能な場合
		if (startNode == null || goalNode == null ||
			!startNode.IsWalkable || !goalNode.IsWalkable)
		{
			return new List<PathNode>();
		}

        //初回の探索候補を設定
        FirstSearchCandidate(startNode, goalNode);

        //探索候補が存在する限りループ
        while (m_openList.Count > 0)
        {
            //FCostが最も小さいノードを取得
            PathNode smallestFCost = GetLowestFCostNode();

            //ゴールノードに到達した場合は実際の経路を返す
            if (smallestFCost == goalNode) return RetracePath(goalNode); 

            //探索済みリストに移動
            MoveToClosedList(smallestFCost);

            //隣接ノードを取得
            List<PathNode> adjacentNodes = m_gridManager.AdjacentNode(smallestFCost.Position);

            foreach (PathNode adjacentNode in adjacentNodes)
            {
                //隣接ノードのコストを更新
                UpdateNodeCost(smallestFCost, adjacentNode, goalNode);
            }
        }

		//ゴールに達成出来なかった
		return new List<PathNode>();
    }

    private List<PathNode> RetracePath(PathNode goalNode)
    {
        //経路を格納するリスト
        List<PathNode> pathNodes = new List<PathNode>();

        PathNode currentNode = goalNode;

        //ゴールノードからスタートノードまで親ノードを辿って経路を作成
        while (currentNode != null && currentNode.Parent != null)
        {
            pathNodes.Add(currentNode);
            currentNode = currentNode.Parent;
        }

		pathNodes.Reverse();
		return pathNodes;
    }

    private void FirstSearchCandidate (PathNode startNode, PathNode goalNode)
    {
        //初回の探索候補を設定
        startNode.GCost = 0;
        startNode.HCost = GetDistanceCost(startNode, goalNode);
        startNode.Parent = null;
        m_openList.Add(startNode);
		m_searchedNodes.Add(startNode);
	}

	private void InitializationSearchedNodes()
	{
		foreach(PathNode node in m_searchedNodes)
		{
			node.GCost = int.MaxValue;
			node.HCost = int.MaxValue;
			node.Parent = null;
		}

		m_searchedNodes.Clear();
	}
}
