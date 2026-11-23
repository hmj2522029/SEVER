
//オブジェクトを移動させるコンポーネント
//
//責務
//・外部から受け取った移動方向と速度による移動
//・指定距離の移動
//・移動の開始と停止
//・Rigidbody2Dを使用した移動処理

using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private Vector2 m_move;             //移動量
    private Vector2 m_startPosition;    //開始地点
    private Vector2 m_moveDirection;    //現在の移動方向
    private float m_moveSpeed;          //現在の速度
    private float m_moveDistance;       //指定距離
    private bool m_isDirectionFixed;    //方向が固定されているか
    private bool m_isDistanceMove;      //指定距離での移動かどうか
    private bool m_isCanMoving;         //移動可能かどうか

    public bool IsCanMoving => m_isCanMoving;
    public float MoveSpeed => m_moveSpeed;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (m_isDistanceMove)
        {
            float distance =
                Vector2.Distance(m_startPosition, transform.position);

            if (distance >= m_moveDistance)
            {
                Stop();
                return;
            }
        }

        m_rb.velocity = m_move;
    }

    //入力用のMove関数
    public void Move(Vector2 direction)
    {
        //方向が固定されている場合は処理を行わない
        if (m_isDirectionFixed) return;

        m_moveDirection = direction;
        m_move = m_moveDirection * m_moveSpeed;
        m_isDistanceMove = false;
    }

    //入力を指定する用のMove関数
    public void Move(Vector2 direction, float distance)
    {
        m_startPosition = transform.position;
        m_moveDirection = direction;
        m_moveDistance = distance;
        m_isDistanceMove = true;

        m_move = m_moveDirection * m_moveSpeed;
    }

    public void SetSpeed(float speed)
    {
        m_moveSpeed = speed;
    }

    public void SetDirectionFixed(bool isFixed)
    {
        m_isDirectionFixed = isFixed;
    }

    public void Stop()
    {
        m_move = Vector2.zero;
        m_isDistanceMove = false;
    }

    public void SetMoveEnabled(bool enabled)
    {
        m_isCanMoving = enabled;
    }
}
