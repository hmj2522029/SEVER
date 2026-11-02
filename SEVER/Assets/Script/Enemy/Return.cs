
//戻る処理を行うコンポーネント
//
// 責務
// ・記録された移動経路を辿って元の位置へ戻る
// ・戻る方向を計算する
// ・戻り終わったかどうかを判定する
// ・戻る処理の開始・停止を管理する

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor.Purchasing;
using UnityEngine;

public class Return : MonoBehaviour
{
    private float WaitTime = 1.0f;

    [SerializeField] float m_speed;

    private PathRecorder m_pathRecorder;    //通った道を記録するコンポーネント

    private Vector2 m_target;
    private Vector2 m_dir;
    private bool m_isUpdate = false;
    private bool m_isReturnEnd = false;
    private float m_waitTime;           //移動前の待機時間
    private int m_pathIndex;            //現在の目標のインデックス

    public Vector2 Dir => m_dir;
    public float Speed => m_speed;  
    public bool IsReturnEnd => m_isReturnEnd;

    private void Awake()
    {
        m_pathRecorder = GetComponent<PathRecorder>();
    }

    private void Start()
    {
        m_pathRecorder.StopRecording();

        m_waitTime = WaitTime;  //移動前の待機時間を初期化

    }

    private void Update()
    {
        if (!m_isUpdate) return;

        //移動前の待機時間を減算する
        if (m_waitTime > 0)
        {
            m_waitTime -= Time.deltaTime;
            return;
        }

        //目標の方向を計算する
        m_dir = (m_target - (Vector2)transform.position).normalized;

        //目標の位置に到達した場合、次の目標に進む
        if (Vector2.Distance(transform.position, m_target) < 0.1f)
        {
            m_pathRecorder.RemoveLastPoint(m_pathIndex);

            m_pathIndex--;

            if (m_pathIndex < 0)
            {
                UpdateStop();
                m_pathRecorder.Clear();
                return;
            }

            //次の目標の位置を設定する
            m_target = m_pathRecorder.Path[m_pathIndex];
        }
    }

    private void UpdateStart()
    {

        if (m_pathRecorder.Path.Count == 0) return;

        m_isUpdate = true;
        m_waitTime = WaitTime;  //移動前の待機時間を初期化

        //移動記録の最後の位置を目標にする(一番近い位置だから)
        m_pathIndex = m_pathRecorder.Path.Count - 1;

        //次の目標の位置を設定する
        m_target = m_pathRecorder.Path[m_pathIndex];

    }

    public void UpdateStop()
    {
        m_isUpdate = false;
        m_target = Vector2.zero;
        m_isReturnEnd = true;
    }

    public void StartRecording()
    {
        //通った道の記録を開始する
        m_pathRecorder.StartRecording();

        m_isReturnEnd = false;
    }
    public void BeginReturn()
    {

        //通った道の記録を停止する(戻った道を記録させないため)
        m_pathRecorder.StopRecording();
        UpdateStart();

    }

}
