
//UIを管理するクラス
//
//責務
//・登録されたテキストUIを管理する
//・テキストの種類と複数のテキスト対応を管理する
//・テキストの表示、非表示を管理する

using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public enum TextType
    {
        PlayerAction,       //プレイヤーアクションテキスト
        PressAnyButton,     //PressAnyButtonのテキスト
        MessageWindow      //看板の内容テキスト
    }

    //ゲーム開始時のテキストの状態
    public enum InitialState
    {
        None,       //初期状態のまま
        Hide,       //文字列を空にする
        HideUI      //UI自体を非表示にする
    }


    [System.Serializable]
    public class UIText
    {
        public TextType typeText;       //テキストの種類
        public InitialState typeHide;   //初期状態
        public TextMeshProUGUI text;    //対応するテキスト
    }

    [SerializeField] List<UIText> m_textList;   //このシーンで使用するテキスト

    private Dictionary<TextType, List<TextMeshProUGUI>> m_texts;  //テキストの種類ごとにTextMeshProUGUIを管理

    private void Awake()
    {
        m_texts = new Dictionary<TextType, List<TextMeshProUGUI>>();

        foreach (var uiText in m_textList)
        {
            //m_textsの中にuiText.typeTextというキーが存在しない場合
            if (!m_texts.ContainsKey(uiText.typeText))
            {
                m_texts.Add(uiText.typeText, new List<TextMeshProUGUI>());
            }

            m_texts[uiText.typeText].Add(uiText.text);
        }

    }

    private void Start()
    {
        foreach (var uiText in m_textList)
        {

            switch (uiText.typeHide)
            {
                case InitialState.Hide:
                    uiText.text.text = "";
                    break;

                case InitialState.HideUI:
                    uiText.text.gameObject.SetActive(false);
                    break;

            }

        }
    }

    public void Show(string message, TextType type)
    {
        //文字設定
        foreach (var uiText in m_texts[type])
        {
            uiText.text = message;
        }

    }
    public void ShowUI(TextType type)
    {
        //UI表示
        foreach (var uiText in m_texts[type])
        {
            uiText.gameObject.SetActive(true);
        }

    }


    public void Hide(TextType type)
    {
        //文字を消す
        foreach (var uiText in m_texts[type])
        {
            uiText.text = "";
        }

    }
    public void HideUI(TextType type)
    {
        //UIを非表示
        foreach (var uiText in m_texts[type])
        {
            uiText.gameObject.SetActive(false);
        }

    }
}