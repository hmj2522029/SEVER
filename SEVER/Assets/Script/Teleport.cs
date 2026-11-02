
//テレポートさせるスクリプト
//
//責務
//・オブジェクトを指定の場所に移動させる

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public void MoveTo(Vector2 position)
    {
        transform.position = position;
    }

}
