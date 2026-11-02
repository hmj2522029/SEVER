
//警告マーク等を表示・非表示にするスクリプト
//
//責務
//・各マークの表示・非表示
//・マーク表示時のアニメーション管理
//・マークの表示アニメーションの開始・停止

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class AlertIcon : MonoBehaviour
{

    [SerializeField] Sprite m_alertSprite;          // !
    [SerializeField] Sprite m_lostSprite;           // ?
    [SerializeField] Vector3 m_finalCoordinates;    //最終座標
    [SerializeField] AudioClip m_alertSE;           //!音
    [SerializeField] AudioClip m_lostSE;            //?音

    private Coroutine m_coroutine;
    private SpriteRenderer m_alertIcon;


    private void Awake()
    {
        m_alertIcon = GetComponent<SpriteRenderer>();
        m_alertIcon.enabled = false;
    }


    //「！」を表示
    public void ShowAlert()
    {
        m_alertIcon.sprite = m_alertSprite;
        m_alertIcon.enabled = true;

        //!音を鳴らす
        AudioManager.Instance.PlaySE(m_alertSE, 0.8f);

        StartAnimation();
    }


    //「？」を表示
    public void ShowLost()
    {
        m_alertIcon.sprite = m_lostSprite;
        m_alertIcon.enabled = true;

        //?音を鳴らす
        AudioManager.Instance.PlaySE(m_lostSE, 0.7f);

        StartAnimation();
    }


    //非表示
    public void Hide()
    {
        m_alertIcon.enabled = false;
    }

    private void StartAnimation()
    {
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
        }

        m_coroutine = StartCoroutine(ShowAnimation());
    }

    private IEnumerator ShowAnimation()
    {
        float elapsed = 0f;     // 経過時間
        float duration = 0.2f;  // アニメーションの時間

        Vector3 targetPosition = m_finalCoordinates + new Vector3(0f, 0.5f, 0f);  
        Vector3 startPosition = m_finalCoordinates - new Vector3(0f, 0.5f, 0f);   // 上に移動するための開始位置

        //下から上に移動する
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            transform.localPosition =
                Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        elapsed = 0f;

        //上から最終座標に移動する
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;

            transform.localPosition =
                Vector3.Lerp(targetPosition, m_finalCoordinates, t);

            yield return null;
        }

        transform.localPosition = m_finalCoordinates;

        //何秒か待つ
        yield return new WaitForSeconds(1.0f);

        Hide();
    }
}   
