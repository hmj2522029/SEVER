
//視野の描画と当たり判定を行うコンポーネント
//
//責務
//・Meshを使って視野の描画の管理
//・視野の当たり判定の管理
//・視野に入ったオブジェクトの管理
//・視野の見た目の切り替え

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{

	[SerializeField] float m_viewDistance = 5.0f;   //視野の長さ
	[SerializeField] float m_viewAngle = 90.0f;     //視野角
	[SerializeField] int m_division = 20;           //扇形を何分割するか(扇形は三角形の集合体)
	[SerializeField] LayerMask m_targetLayer;		//視野に入ったレイヤー(例(用途) : 敵の視野にプレイヤーが入ったら追いかける)
	[SerializeField] LayerMask m_obstacleLayer;     //当たり判定に入るレイヤー(障害物)

    [SerializeField] private Material m_visibleMaterial;    //通常の視野のマテリアル
    [SerializeField] private Material m_hiddenMaterial;     //止めている時の視野のマテリアル


    private MeshRenderer m_meshRenderer;    //マテリアル(視野を止めるとき見た目だけを消す)
    private Mesh m_mesh;        			//扇形を描画するMesh
	private int[] m_triangles;  			//三角形を構成する頂点の番号
	private Vector3[] m_vertices;			//扇形を構成する頂点の座標
	private  List<RaycastHit2D> m_hits;		//当たったコライダーを保存する用
    private Vector2 m_direction;            //視野の向き(ベクトル)
    private float m_angle;                  //視野の向き(角度)
    private float m_targetAngle;            //視野の向き(ターゲットの角度)
    private float m_rotateSpeed = 500.0f;   //視野の回転速度


    public IReadOnlyList<RaycastHit2D> Hits => m_hits;

    public Vector2 Dir => m_direction;


	private void Awake()
	{
		m_mesh = new Mesh();
		m_mesh.name = "VisionMesh";

        m_meshRenderer = GetComponent<MeshRenderer>();
    }

	private void Start()
	{
		GetComponent<MeshFilter>().mesh = m_mesh;

		m_vertices = new Vector3[m_division + 2];   //外側の点 : 21個, 中心の点 : 1個
		m_vertices[0] = Vector3.zero;               //扇形の中心となる頂点

		//(※Meshは一次配列しか受け付けないため二次配列にしない)
		m_triangles = new int[m_division * 3];      //三角形は一つ作るのに三つの頂点がいるから

		m_hits = new List<RaycastHit2D>();


		//m_trianglesは一度作れば変わらないから最初で作成
		for (int i = 0; i < m_division; i++)
		{
			int index = i * 3;

			//三角形を構成する頂点番号を設定
			m_triangles[index] = 0;             //中心
			m_triangles[index + 1] = i + 2;     //現在の頂点 
			m_triangles[index + 2] = i + 1;     //次の頂点
		}

	}

    void Update()
    {
        //現在の角度からターゲットの角度に向かって回転するようにする
        m_angle = Mathf.MoveTowardsAngle(
            m_angle,
            m_targetAngle,
            m_rotateSpeed * Time.deltaTime
        );

        float rad = m_angle * Mathf.Deg2Rad;

        //視野の向きを計算
        m_direction = new Vector2(
            Mathf.Cos(rad),
            Mathf.Sin(rad)
        );

    }

    public void SetDirection(Vector2 direction)
	{
        m_hits.Clear(); //現在の当たっている奴を調べるために前の奴を消す

        //ターゲットの方向がゼロベクトルでない場合にのみ角度を計算
        if (direction != Vector2.zero)
        {
            m_targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        float angleStep = m_viewAngle / m_division; //1つ隣の頂点との角度の差

		//※毎フレーム呼ぶと毎回全部計算するため後々必要の時だけMeshへ反映に変更
		//各頂点の座標の保存(m_vertices[0]以外)
		for (int i = 1; i < m_vertices.Length; i++)
		{

			//現在の頂点の角度
			float angle = -m_viewAngle * 0.5f + (i - 1) * angleStep;

			//扇形の角度に現在の向きを加える
			float rad = angle * Mathf.Deg2Rad + m_angle * Mathf.Deg2Rad;

			Vector2 dir = new Vector2(
				Mathf.Cos(rad),
				Mathf.Sin(rad)
			);


			RaycastHit2D hit = Physics2D.Raycast(
				transform.position, //どこから飛ばすか
                dir,				//飛ばす方向
				m_viewDistance,     //どこまで飛ばすか
				m_obstacleLayer | m_targetLayer
			);

			//障害物に当たったかどうか
			if (hit.collider != null && IsLayer(hit.collider.gameObject, m_obstacleLayer))
			{
				
				//障害物に当たった所を頂点として保存
				m_vertices[i] = transform.InverseTransformPoint(hit.point);

                //障害物に当たったかどうかをわかりやすくするためにデバック用の線を引く
                Debug.DrawLine(
					transform.position,
					hit.point,
					Color.green
				);
			
			}
			else　
			{

				//障害物が無いので最大視野距離の位置を頂点として保存
				m_vertices[i] = new Vector3(
                    dir.x * m_viewDistance,
                    dir.y * m_viewDistance,
					0
				);

				Debug.DrawRay(
					transform.position,
                    dir * m_viewDistance,
					Color.red
				);

				//ターゲットが視野内に入ったかどうか
				if(hit.collider != null && IsLayer(hit.collider.gameObject, m_targetLayer))
				{

                    //ターゲットが視野内に入ったので保存
                    m_hits.Add(hit);

                    //ターゲットに当たったかどうかをわかりやすくするためにデバック用の線を引く
                    Debug.DrawLine(
						transform.position,
						hit.point,
						Color.blue
					);


				}

			}

		}

		m_mesh.Clear();

		m_mesh.vertices = m_vertices;
		m_mesh.triangles = m_triangles;

		m_mesh.RecalculateBounds();     //Meshの範囲更新
		m_mesh.RecalculateNormals();

	}

	private bool IsLayer(GameObject obj, LayerMask mask)
	{
        //複数のレイヤーを判定するためにビット演算を使用
        return ((1 << obj.layer) & mask) != 0;
	}

	public void HideVision()
	{
        m_meshRenderer.material = m_hiddenMaterial;
    }

    public void ShowVision()
    {
        m_meshRenderer.material = m_visibleMaterial;
    }

    public bool IsVisible(Vector2 point)
    {
        if (m_direction == Vector2.zero) return false;

        //中心点からターゲットまでの距離を計算
        float distance = Vector2.Distance(transform.position, point);

        //距離が視野の長さより大きい場合は視野外
        if (distance > m_viewDistance) return false;

        //中心点からターゲットまでの方向を計算
        Vector2 pointDirection = (point - (Vector2)transform.position).normalized;

        //中心点からターゲットまでの角度を計算
        float angle = Vector2.Angle(m_direction, pointDirection);

        //角度が視野角の半分より大きい場合は視野外
        if (angle > m_viewAngle * 0.5f) return false;

        //障害物があるかどうかを判定
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            pointDirection,
            distance,
            m_obstacleLayer
        );

        //障害物に当たった場合は視野外
        if (hit.collider != null) return false;

        return true;

    }

    public Vector2 Direction()
    {
        return m_direction;
    }

}
