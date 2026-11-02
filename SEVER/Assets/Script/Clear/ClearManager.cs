using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour
{
    [Header("イベント等")]
    [SerializeField] EventDirector m_eventDirector;    //イベント開始
    [SerializeField] GameEvent m_gameEvent;            //どのイベントか 


    [Header("フェード等")]
    [SerializeField] CanvasGroupFade m_fade;        //フェード
    [SerializeField] CanvasGroup m_image;           //画面のフェード用の画像


    private void Start()
    {

        m_eventDirector.StartEvent(m_gameEvent);

        m_fade.FadeOut(m_image, 2f);

        StartCoroutine(TitleTransition());
    }

    private IEnumerator TitleTransition()
    {
        //イベントが終了するまで待機
        yield return new WaitUntil(() => m_gameEvent.IsFinished);

        yield return new WaitForSeconds(2f);

        m_fade.FadeIn(m_image, 2f); 

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("TitleScene");

    }

}
