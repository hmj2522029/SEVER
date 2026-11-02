
//オブジェクトを移動させるコンポーネント
//
//責務
//・外部から受け取った移動方向と速度による移動
//・指定距離の移動
//・移動の開始と停止
//・Rigidbody2Dを使用した移動処理

using UnityEngine;

public class Movement : MonoBehaviour
{

    private Rigidbody2D m_rb;
    private Vector2 m_move;             //移動量
    private Vector2 m_startPosition;    //開始地点
    private float m_moveDistance;       //指定距離
    private bool m_distanceMove;        //指定距離での移動かどうか

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        if (m_distanceMove)
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
    public void Move(Vector2 direction, float speed)
    {
        m_move = direction * speed;
        m_distanceMove = false;
    }

    //入力を指定する用のMove関数
    public void Move(Vector2 direction, float distance, float speed)
    {

        m_startPosition = transform.position;
        m_moveDistance = distance;
        m_distanceMove = true;

        m_move = direction * speed;

    }

    public void Stop()
    {
        m_move = Vector2.zero;
        m_distanceMove = false;
    }

}
