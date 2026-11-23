using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector3Int Position { get; }  //位置情報(セル情報)
    public bool IsWalkable { get; }      //通行可能かどうか

    public int GCost { get; set; }  //スタート地点からの実際に通った経路のコスト
    public int HCost { get; set; }  //ゴールまでの残りの推定コスト

    public int FCost => GCost + HCost;  //総コスト

    public PathNode Parent { get; set; }    //経路を辿るための直前のノード

    public PathNode(Vector3Int position, bool isWalkable)
    {
        Position = position;
        IsWalkable = isWalkable;
    }
}
