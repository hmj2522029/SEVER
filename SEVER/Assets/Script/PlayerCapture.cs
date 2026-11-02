using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCapture : MonoBehaviour
{
    public event Action OnCaptured;

    [SerializeField] AudioClip m_getCaught; //捕まったときのSE

    private bool m_isCaptured = false;

    public bool IsCaptured => m_isCaptured;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (m_isCaptured) return;

        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();

            //敵が停止しているか、追跡状態(見つかっていない)でない場合は捕獲されない
            if (enemy.IsStopped || !enemy.IsChasing) return;

            AudioManager.Instance.PlaySE(m_getCaught, 0.7f);

            m_isCaptured = true;
            OnCaptured?.Invoke();
        }
    }


}
