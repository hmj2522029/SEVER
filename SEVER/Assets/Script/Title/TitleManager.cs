using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("イベント等")]
    [SerializeField] EventDirector m_eventDirector;    //イベント開始
    [SerializeField] GameEvent m_gameEvent;            //どのイベントか 

    [Header("開始ボタン")]
    [SerializeField] InputAction m_pressAnyButton;  //PressAnyButton

    [Header("フェード等")]
    [SerializeField] CanvasGroupFade m_fade;        //フェード
    [SerializeField] CanvasGroup m_image;           //画面のフェード用の画像

    [Header("音")]
    [SerializeField] AudioClip m_click;     //クリック音

    private void Start()
    {
        m_eventDirector.StartEvent(m_gameEvent);

        //初期状態は見えないようにする
        m_image.alpha = 0.0f;
    }

    private void Update()
    {
        
        if(m_pressAnyButton.WasPressedThisFrame() && m_gameEvent.IsFinished)
        {
            //クリック音を鳴らす
            AudioManager.Instance.PlaySE(m_click);

            // シーン切り替え
            StartCoroutine(ChangeScene());

        }

    }

    private IEnumerator ChangeScene()
    {

        m_fade.FadeIn(m_image, 2f);     //見えなる

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("StageSelectScene");

    }

    private void OnEnable()
    {
        m_pressAnyButton.Enable();
    }

    private void OnDisable()
    {
        m_pressAnyButton.Disable();
    }

}
