
//CanvasGroupを使ったFadeのスクリプト
//
//責務
//・フェードイン、フェードアウトの実行
//・フェード時間の管理
//・フェード中かどうかの状態管理

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupFade : MonoBehaviour
{

    //フェード中かどうかのフラグ
    private bool m_isFading = false;        //フェード中かどうか

    //フェード中かどうかを取得
    public bool GetIsFading() { return m_isFading; }


    public void FadeIn(CanvasGroup group, float duration)
    {
        StartCoroutine(FadeUI(group, 0f, 1f, duration));
    }

    public void FadeOut(CanvasGroup group, float duration)
    {
        StartCoroutine(FadeUI(group, 1f, 0f, duration));
    }

    public void Fade(CanvasGroup group, float start, float end, float duration)
    {
        StartCoroutine(FadeUI(group, start, end, duration));
    }


    public IEnumerator FadeOutCoroutine(CanvasGroup group, float duration)
    {
        yield return FadeUI(group, 1f, 0f, duration);
    }
    public IEnumerator FadeInCoroutine(CanvasGroup group, float duration)
    {
        yield return FadeUI(group, 0f, 1f, duration);
    }

    private IEnumerator FadeUI(CanvasGroup group, float start, float end, float duration)
    {

        float elapsedTime = 0.0f;
        m_isFading = true; //フェード中フラグを立てる

        //フェードアウトの場合は、フェード開始と同時にUIの操作を無効化する
        group.interactable = false;
        group.blocksRaycasts = false;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(start, end, elapsedTime / duration);

            //キャンバスグループのアルファ値を更新
            group.alpha = alpha;
            elapsedTime += Time.deltaTime;

            yield return null; //次のフレームまで待つ
        }

        //最終的な状態を確実に設定
        group.alpha = end;
        m_isFading = false; //フェード終了フラグを下ろす

        // 最終状態に合わせる
        if (end == 1f)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        }
    }
}
