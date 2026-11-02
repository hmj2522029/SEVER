
//画像の点滅スクリプト
//
//責務
//・画像の点滅処理

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PressAnyButton : MonoBehaviour
{
    [SerializeField] float m_blinkSpeed = 2.0f;     //点滅速度     

    private TextMeshProUGUI m_image;   //点滅させる画像

    private float m_timer;

    private void Awake()
    {
        m_image = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //点滅処理
        m_timer += Time.deltaTime * m_blinkSpeed;
        float alpha = Mathf.Abs(Mathf.Sin(m_timer));


        Color color = m_image.color;
        color.a = alpha;
        m_image.color = color;

    }
}
