
//タイトルシーンで起こるイベントの全体の管理
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleEvent : GameEvent
{
    [Header("Player")]
    [SerializeField] PlayerEventController m_playerController;  //プレイヤーの操作(イベント専用)

    [Header("Enemy")]
    [SerializeField] EnemyEventController m_enemyController;    //敵の操作(イベント専用)

    [Header("操作するオブジェクト等")]
    [SerializeField] Door m_door;               //ドア   
    [SerializeField] CameraMovement m_camera;   //カメラ
    [SerializeField] Torchs m_torchs;           //トーチ(複数)

    [Header("UI")]
    [SerializeField] GameUI m_gameUI;           //UI
    [SerializeField] Image m_image;             //画像

    [Header("プレイヤーがテレポートする場所")]
    [SerializeField] Transform m_teleportPos;   //テレポートする場所

    [Header("音")]
    [SerializeField] AudioClip m_checkClose;        //閉まっているか確認する音
    [SerializeField] AudioClip m_hitDoor;           //ドアを叩く音
    [SerializeField] AudioClip m_closesSuddenly;    //急に閉まる音


    public override void Execute()
    {

        StartCoroutine(Event());
    }

    private IEnumerator Event()
    {
        //プレイヤーが暗かったのに急に明るくなりびっくりする場面
        yield return StartCoroutine(LightAppears());

        //プレイヤーが扉を見つけて周りを見る場面
        yield return StartCoroutine(FindDoor());

        //プレイヤーと敵が目があってプレイヤーが逃げる場面
        yield return StartCoroutine(EnemyEncounter());

        //プレイヤーが糸を見つけて不思議がり周りを見る場面
        yield return StartCoroutine(DiscoverThread());

        //また敵に見つかりプレイヤーが糸を切る場面
        yield return StartCoroutine(CutThread());

        //急に動かなくなった敵の所に行く場面
        yield return StartCoroutine(ApproachEnemy());

        //プレイヤーが扉を開ける場面
        yield return StartCoroutine(OpenDoor());

        //部屋に入ったら急に閉じ込められる場面
        yield return StartCoroutine(GetLockedIn());

        //プレイヤーが扉の前に行き開くかどうか確かめる場面
        yield return StartCoroutine(CheckDoor());

        //カメラが動く場面
        yield return StartCoroutine(CameraReveal());

        //UIを出す
        m_gameUI.Show("Press Any Button", GameUI.TextType.PressAnyButton);

        IsFinished = true;
    }

    private IEnumerator LightAppears()
    {

        //プレイヤーが暗かったのに急に明るくなりびっくりする場面

        m_playerController.Move(Vector2.left, 17.5f, 2.0f); //左に移動(プレイヤー)

        yield return new WaitForSeconds(8.8f);

        //プレイヤー
        m_playerController.StopMove();      //動きを止める
        m_playerController.ShowAlert();     //びっくりマークを出す
        m_playerController.HideVision();   //視野の見た目を消す

        m_image.enabled = false;    //画像を非表示にする

        m_torchs.Ignition();    //トーチを全部点火する

        yield return new WaitForSeconds(1.5f);
         
    }

    private IEnumerator FindDoor()
    {

        //プレイヤーが扉を見つけて周りを見る場面

        m_playerController.ChangeOfDirection(Vector2.up);   //上に向かせる(プレイヤー)

        m_enemyController.Move(Vector2.left, 12.0f, 2.2f);  //左に移動させる(敵)

        yield return new WaitForSeconds(1.0f);

        m_playerController.ShowLost();  //はてなマークを出す(プレイヤー)

        yield return new WaitForSeconds(2.0f);

        m_playerController.ChangeOfDirection(Vector2.left);     //左に向かせる(プレイヤー)

        yield return new WaitForSeconds(2.0f);

        m_playerController.ChangeOfDirection(Vector2.right);    //右に向かせる(プレイヤー)

        yield return new WaitForSeconds(0.5f);

    }

    private IEnumerator EnemyEncounter() 
    {
        //プレイヤーと敵が目があってプレイヤーが逃げる場面
        ShowBothAlert();

        yield return new WaitForSeconds(0.5f);

        //m_playerController.ChangeOfDirection(Vector2.left);   //左に向かせる(プレイヤー)
        m_playerController.Move(Vector2.left, 15.0f, 5.5f);   //左に移動(プレイヤー)

        m_enemyController.Move(Vector2.left, 20.0f, 5.5f);    //左に移動(敵)

        yield return new WaitForSeconds(3.0f);

        //プレイヤーが敵から逃げ切った場面
        m_playerController.MoveTo(m_teleportPos.transform.position); //指定した場所にテレポート(プレイヤー)

        yield return new WaitForSeconds(2.5f);

        m_playerController.Move(Vector2.left, 7.0f, 2.0f);   //左に移動(プレイヤー)

        yield return new WaitForSeconds(3.5f);

    }

    private IEnumerator DiscoverThread()
    {
        //プレイヤーが糸を見つけて不思議がり周りを見る場面

        //プレイヤー
        m_playerController.StopMove();  //動きを止める
        m_playerController.ChangeOfDirection(Vector2.up);   //上に向かせる

        //敵
        m_enemyController.Move(Vector2.right, 10.0f, 2.5f); //右に移動
        m_enemyController.ShowLost();   //はてなマークを出す

        yield return new WaitForSeconds(0.5f);

        m_playerController.ShowLost();  //はてなマークを出す(プレイヤー)

        yield return new WaitForSeconds(1.7f);

        m_playerController.ChangeOfDirection(Vector2.right);    //右に向かせる(プレイヤー)

        yield return new WaitForSeconds(1.7f);

        m_playerController.ChangeOfDirection(Vector2.left);     //左に向かせる(プレイヤー)

        yield return new WaitForSeconds(0.5f);

    }

    private IEnumerator CutThread()
    {
        //また敵に見つかりプレイヤーが糸を切る場面

        ShowBothAlert();

        yield return new WaitForSeconds(0.5f);

        m_playerController.ChangeOfDirection(Vector2.up);   //上に向かせる(プレイヤー)

        m_enemyController.Move(Vector2.right, 10.0f, 3.5f); //右に移動(敵)

        yield return new WaitForSeconds(1.0f);

        m_playerController.CutThread(); //糸を切る

        yield return new WaitForSeconds(1.0f);

        m_playerController.ChangeOfDirection(Vector2.left); //左に向かせる(プレイヤー)

        yield return new WaitForSeconds(0.5f);


    }

    private IEnumerator ApproachEnemy() 
    {
        //急に動かなくなった敵の所に行く場面

        m_playerController.ShowLost();  //はてなマークを出す(プレイヤー)

        yield return new WaitForSeconds(1.0f);

        m_playerController.Move(Vector2.up, 1.25f, 2.0f); //上に移動(プレイヤー)

        yield return new WaitForSeconds(0.6f);

        m_playerController.Move(Vector2.left, 6.0f, 2.0f);  //左に移動(プレイヤー)

        yield return new WaitForSeconds(3.0f);

        m_playerController.StopMove();  //動きを止める

        yield return new WaitForSeconds(0.5f);

        m_playerController.ShowLost();  //はてなマークを出す(プレイヤー)

        yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator OpenDoor() 
    {
        //プレイヤーが扉を開ける場面

        m_playerController.Move(Vector2.up, 1.3f, 2.0f); //上に移動(プレイヤー)

        yield return new WaitForSeconds(0.6f);

        m_playerController.Move(Vector2.left, 1.2f, 2.0f);  //左に移動(プレイヤー)

        yield return new WaitForSeconds(0.7f);

        m_playerController.Move(Vector2.up, 0.7f, 2.0f);    //上に移動(プレイヤー)

        yield return new WaitForSeconds(0.6f);

        m_playerController.StopMove();  //動きを止める(プレイヤー)

        yield return new WaitForSeconds(0.5f);

        m_door.Open();  //ドアを開ける

        yield return new WaitForSeconds(0.5f);

    }

    private IEnumerator GetLockedIn() 
    {
        //部屋に入ったら急に閉じ込められる場面

        m_playerController.Move(Vector2.up, 3.0f, 2.0f);    //上に移動(プレイヤー)

        yield return new WaitForSeconds(1.5f);

        m_playerController.StopMove();

        yield return new WaitForSeconds(0.5f);

        m_door.Close(false);

        //ドアが閉まる音を鳴らす
        AudioManager.Instance.PlaySE(m_closesSuddenly, 0.5f);

        m_playerController.ShowAlert();

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator CheckDoor()
    {
        //プレイヤーが扉の前に行き開くかどうか確かめる場面
        m_playerController.ChangeOfDirection(Vector2.down);

        yield return new WaitForSeconds(0.5f);

        m_playerController.Move(Vector2.down, 1.0f, 3.0f);

        yield return new WaitForSeconds(0.55f);

        m_playerController.StopMove();
        
        //ドアが閉まっているか確かめる音
        AudioManager.Instance.PlaySE(m_checkClose, 0.5f);

        yield return new WaitForSeconds(2.0f);

        //ドアを叩く音
        AudioManager.Instance.PlaySE(m_hitDoor);

    }

    private IEnumerator CameraReveal() 
    {
        //カメラが動く場面

        m_camera.Move(Vector2.down, 15.0f, 2.0f);

        yield return new WaitForSeconds(8.5f);
    }

    private void ShowBothAlert()
    {
        m_playerController.ShowAlert();
        m_enemyController.ShowAlert();
    }
}
