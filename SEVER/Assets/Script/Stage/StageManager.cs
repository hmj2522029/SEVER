using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("フェード等")]
    [SerializeField] CanvasGroupFade m_fade;        //フェード
    [SerializeField] CanvasGroup m_image;           //画面のフェード用の画像

    [Header("ステージから出るドア")]
    [SerializeField] StageExitDoor m_stageExitDoor; //出るドア

    [Header("捕まった通知")]
    [SerializeField] PlayerCapture m_playerCapture;   //捕まった通知

    [Header("ランダムスポーン(ダンジョンコア)")]
    [SerializeField] RandomSpawner m_randomSpawner; //ランダムスポーン

    private DungeonCore m_dungeonCore;     //ダンジョンコア

    private void Start()
    {

        m_fade.FadeOut(m_image, 2.0f);

        m_randomSpawner.Spawn();    //ダンジョンコアをランダムにスポーン

        m_dungeonCore = m_randomSpawner.SpawnObject.GetComponent<DungeonCore>();   //ダンジョンコアのコンポーネントを取得
    }

    private void OnEnable()
    {
        m_stageExitDoor.OnInteraction += OnDoorInteraction;
        m_playerCapture.OnCaptured += OnPlayerCaptured;
    }

    private void OnDisable()
    {
        m_stageExitDoor.OnInteraction -= OnDoorInteraction;
        m_playerCapture.OnCaptured -= OnPlayerCaptured;
    }

    private void OnDoorInteraction(DoorBase door)
    {
        //ドアがStageExitDoorかどうかを確認
        StageExitDoor stageDoor = door as StageExitDoor;

        if (stageDoor == null) return;


        //ダンジョンコアが壊れているかどうかを確認
        if (m_dungeonCore.IsDestroyed)
        {
            //ステージクリア
            GameManager.Instance.ClearStage(GameManager.Instance.CurrentStageID);

        }

        ExitStage();
    }

    private void ExitStage()
    {
        m_fade.FadeIn(m_image, 3.0f);

        StartCoroutine(LoadStageSelect());
    }

    private void OnPlayerCaptured()
    {
        ExitStage();
    }

    private IEnumerator LoadStageSelect()
    {
        yield return new WaitForSeconds(4.5f);

        SceneManager.LoadScene("StageSelectScene");
    }

}
