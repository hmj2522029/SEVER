using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyEventController : MonoBehaviour
{

    private Movement m_movement;        //移動
    private ThreadConnection m_thread;	//糸(接続しているコアとの糸)
    private AlertIcon m_alertIcon;      //警告アイコン

    private void Awake()
    {
        m_movement = GetComponentInChildren<Movement>();
        m_thread = GetComponentInChildren<ThreadConnection>();
        m_alertIcon = GetComponentInChildren<AlertIcon>(); 
    }

    public void Move(Vector2 direction, float distance, float speed)
    {
        m_movement.Move(direction, distance, speed);
    }

    public void ShowAlert()
    {
        m_alertIcon.ShowAlert();
    }

    public void ShowLost()
    {
        m_alertIcon.ShowLost();
    }

    private void OnThreadBroken()
    {
        m_movement.Stop();
    }

    private void OnEnable()
    {
        m_thread.OnThreadBroken += OnThreadBroken;
    }

    private void OnDisable()
    {
        m_thread.OnThreadBroken -= OnThreadBroken;
    }


}
