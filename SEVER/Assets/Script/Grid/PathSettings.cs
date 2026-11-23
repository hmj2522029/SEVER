using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathSettings
{
    private static readonly Vector3Int[] m_directions =
   {
        new Vector3Int( 0,  1, 0), // 上
        new Vector3Int( 0, -1, 0), // 下
        new Vector3Int(-1,  0, 0), // 左
        new Vector3Int( 1,  0, 0), // 右

        new Vector3Int(-1,  1, 0), // 左上
        new Vector3Int( 1,  1, 0), // 右上
        new Vector3Int(-1, -1, 0), // 左下
        new Vector3Int( 1, -1, 0)  // 右下
    };

    //移動コスト
    private const int m_straightCost = 10;  //上下左右
    private const int m_diagonalCost = 14;  //斜め

    public static IReadOnlyList<Vector3Int> Directions => m_directions;
    public static int StraightCost => m_straightCost;
    public static int DiagonalCost => m_diagonalCost;

}
