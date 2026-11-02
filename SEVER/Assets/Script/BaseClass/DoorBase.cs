using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorBase : MonoBehaviour
{
	public abstract string Message { get; }

    //ドアが操作されたことを通知
    public event Action<DoorBase> OnInteraction;

    //実際のドアの操作
    public void ExecuteInteraction()
    {

        OnInteraction?.Invoke(this);

        Interaction();
    }

    //継承先で実装
    public abstract void Interaction();

}
