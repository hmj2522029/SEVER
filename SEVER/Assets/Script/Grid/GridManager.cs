using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine.Tilemaps;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Tilemap m_floorTilemap;    //床タイルマップ
    [SerializeField] Tilemap m_wallTilemap;     //壁タイルマップ

    private Grid m_grid;    //グリッド
    private Dictionary<Vector3Int, PathNode> m_nodes //セル座標とPathNodeの対応関係を保持する辞書
        = new Dictionary<Vector3Int, PathNode>();  


    void Update()
    {
    }

    void Start()
    {
        //TestAdjacentNode(GameObject.transform.position);
    }

    private void Awake()
    {
        m_grid = GetComponent<Grid>();

        //タイルマップの情報からPathNodeを作成
        CreateNodes();
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        //ワールド座標をセル座標に変換
        return m_grid.WorldToCell(worldPosition);   
    }

    public bool HasFloor(Vector3Int cellPosition)
    {
        //床タイルがあるかどうか
        return m_floorTilemap.HasTile(cellPosition);    
    }

    public bool HasWall(Vector3Int cellPosition)
    {
        //壁タイルがあるかどうか
        return m_wallTilemap.HasTile(cellPosition);     
    }

    public bool IsWalkable(Vector3Int cellPosition)
    {
        //床タイルがあり、壁タイルがない場合は歩行可能
        return HasFloor(cellPosition) && !HasWall(cellPosition);  
    }

    public PathNode GetNode(Vector3Int cellPosition)
    {
        //指定されたセル座標に対応するPathNodeを取得
        if (m_nodes.TryGetValue(cellPosition, out PathNode node))
        {
            return node;
        }

        //存在しない場合はnullを返す
        return null;
    }

    public List<PathNode> AdjacentNode(Vector3Int cellPosition)
    {
        //隣接ノードを格納するリスト
        List<PathNode> adjacentNodes = new List<PathNode>();

        //PathSettingsの方向ベクトルを使って各隣接ノードを取得
        foreach (Vector3Int direction in PathSettings.Directions)
        {
            //隣接するセル座標を計算
            Vector3Int adjacentCellPosition = cellPosition + direction;

            PathNode adjacentNode = GetNode(adjacentCellPosition);

            //隣接ノードが存在しない、または通行不可能な場合はスキップ
            if (adjacentNode == null || !adjacentNode.IsWalkable) continue;

            //斜め移動の判定
            if (direction.x != 0 && direction.y != 0)
            {
                //方向を反転させたベクトルを取得
                Vector3Int oppositeDirection = -direction;

                //斜め移動の際に、隣接する水平・垂直ノードが通行可能かどうかを判定
                PathNode verticalNode = GetNode(new Vector3Int(adjacentCellPosition.x, adjacentCellPosition.y + oppositeDirection.y, 0));
                PathNode horizontalNode = GetNode(new Vector3Int(adjacentCellPosition.x + oppositeDirection.x, adjacentCellPosition.y, 0));

                //通行可能であればリストに追加
                if (verticalNode != null && verticalNode.IsWalkable &&
                    horizontalNode != null && horizontalNode.IsWalkable)
                {
                    adjacentNodes.Add(adjacentNode);
                }
            }
            else
            {
                adjacentNodes.Add(adjacentNode);
            }
        }

        return adjacentNodes;
    }


    private void CreateNodes()
    {
        //タイルマップの範囲を取得
        BoundsInt bounds = m_floorTilemap.cellBounds;

        //範囲内のセルをループ
        foreach (Vector3Int cellPosition in bounds.allPositionsWithin)
        {
            //床が存在しないセルは除外
            if (!HasFloor(cellPosition)) continue;

            //通行可能かどうかを判定
            bool isWalkable = IsWalkable(cellPosition);

            //PathNodeを作成して辞書に追加
            PathNode node = new PathNode(cellPosition, isWalkable);

            //辞書に追加
            m_nodes.Add(cellPosition, node);
        }
    }



    //こっから先テスト用
    //↓↓↓↓↓↓↓
    [SerializeField] GameObject GameObject;

    public void TestWalkable(Vector3 worldPosition)
    {
        Vector3Int cellPosition = WorldToCell(worldPosition);

        Debug.Log(
            $"Cell: {cellPosition}, Walkable: {IsWalkable(cellPosition)}"
        );
    }
    private void TestAdjacentNode(Vector3 worldPosition)
    {
        Vector3Int cellPosition = WorldToCell(worldPosition);

        List<PathNode> adjacentNodes = AdjacentNode(cellPosition);

        Debug.Log($"現在のセル座標：{cellPosition}");
        Debug.Log($"取得できた隣接ノード数：{adjacentNodes.Count}");

        foreach (PathNode node in adjacentNodes)
        {
            Debug.Log($"隣接ノード：{node.Position}");
        }
    }
}
