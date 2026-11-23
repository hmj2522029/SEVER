using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] Movement m_movement;       //どのMovementコンポーネントを操作するか
    [SerializeField] float m_dashMultiplier;    //ダッシュ倍率
    [SerializeField] float m_dashDuration;      //ダッシュにかける時間
    [SerializeField] float m_decelerationTime;  //減速にかける時間

    private float m_firstSpeed;     //ダッシュ前の速度  
    private bool m_isDashing;       //ダッシュ中か

    public void StartDash(Vector2 direction, float speed)
    {
        //すでにダッシュ中なら何もしない
        if (m_isDashing) return;

        m_isDashing = true;

        m_firstSpeed = speed;
        m_movement.SetSpeed(speed * m_dashMultiplier);
        m_movement.Move(direction);
        m_movement.SetDirectionFixed(true); //ダッシュ中は方向を固定する

        StartCoroutine(Deceleration());
    }

    private IEnumerator Deceleration()
    {
        //ダッシュ時間待機
        yield return new WaitForSeconds(m_dashDuration);

        //減速処理
        float currentSpeed = m_movement.MoveSpeed;
        float targetSpeed = m_firstSpeed;
        float elapsedTime = 0f;     //経過時間

        while (elapsedTime < m_decelerationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / m_decelerationTime;
            float newSpeed = Mathf.Lerp(currentSpeed, targetSpeed, t);
            m_movement.SetSpeed(newSpeed);
            yield return null;
        }

        m_movement.SetSpeed(targetSpeed);
        m_isDashing = false;
        m_movement.SetDirectionFixed(false); //ダッシュ終了後は方向固定を解除
    }
}
