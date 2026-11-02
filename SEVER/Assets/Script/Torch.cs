
//トーチを操作する
//
//責務
//・トーチの火の点火と消火
//・状態の管理

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Torch : MonoBehaviour
{
    [SerializeField] bool m_isIgnition = false;  //点火しているかどうか
    [SerializeField] AudioClip m_fire;          //火をつける音

    private Animator m_torch;


    private void Awake()
    {
        m_torch = GetComponent<Animator>();
    }

    private void Start()
    {
        //最初の状態
        m_torch.SetBool("isIgnition", m_isIgnition);
    }

    public void Ignition()
    {
        m_isIgnition = true;
        m_torch.SetBool("isIgnition", true);

        //火をつける音を鳴らす
        AudioManager.Instance.PlaySE(m_fire, 0.5f);
    }

    public void FireExtinguishing()
    {
        m_isIgnition = false;
        m_torch.SetBool("isIgnition", false);
    }

    public void Toggle()
    {
        if (m_isIgnition)
        {
            m_torch.SetBool("isIgnition", true);
        }
        else
        {
            m_torch.SetBool("isIgnition", false);
        }
    }

}
