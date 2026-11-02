
//イベントの基底クラス

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    public bool IsFinished { get; protected set; }

    //継承先に書かせる
    public abstract void Execute(); 
}
