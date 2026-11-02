
//イベントの操作をするスクリプト
//
//責務
//・各イベントの開始

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDirector : MonoBehaviour
{

    //イベントの開始
    public void StartEvent(GameEvent gameEvent)
    {
        gameEvent.Execute();
    }

}
