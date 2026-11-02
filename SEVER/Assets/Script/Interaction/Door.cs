
//ドアを管理するスクリプト
//
//責務
//・ドアの開閉状態を管理する
//・ドアの開閉状態に応じて、ドアオブジェクトの表示を切り替える
//・ドアの開閉状態に応じて、表示するメッセージを切り替える

using UnityEngine;

public class Door : DoorBase
{
	[SerializeField] GameObject m_closedDoor;   //閉まっているドアオブジェクト
	[SerializeField] GameObject m_openDoor;     //開いているドアオブジェクト

    [SerializeField] AudioClip m_open; //ドアを開ける・閉める音

    private bool m_isOpen = false;

	public override string Message => m_isOpen ? "「F」 閉じる" : "「F」 開ける";      //表示するメッセージ


    public override void Interaction()
	{
		Toggle();
	}
	

    public void Open(bool sound = true)
	{
		if(m_isOpen) return;

		m_isOpen = true;

        m_closedDoor.SetActive(false);
		m_openDoor.SetActive(true);

        if (sound)
        {
            //ドアを開ける音を鳴らす
            AudioManager.Instance.PlaySE(m_open);  
        }

    }

	public void Close(bool sound = true)
	{
		if(!m_isOpen) return;

		m_isOpen = false;

        m_closedDoor.SetActive(true);
		m_openDoor.SetActive(false);

        if (sound)
        {
            //ドアを閉める音を鳴らす
            AudioManager.Instance.PlaySE(m_open);

        }

    }

	public void Toggle()
	{

		if(m_isOpen)
		{
			Close();
		}
		else
		{
			Open();
		}

	}

}
