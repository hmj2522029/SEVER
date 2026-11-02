
//複数トーチの操作する
//
//責務
//複数のトーチの点火と消火

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchs : MonoBehaviour
{

    [SerializeField] List<Torch> m_torchList;   //操作するトーチのリスト

    public void Ignition()
    {
        foreach (var torch in m_torchList)
        {
            
            torch.Ignition();

        }
    }

    public void FireExtinguishing()
    {
        foreach (var torch in m_torchList)
        {

            torch.FireExtinguishing();

        }


    }


}
