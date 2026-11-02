
// プレイヤーの視界(霧)を制御するコンポーネント
//
// 責務
// ・プレイヤーの位置をShaderへ渡す
// ・視界半径をShaderへ渡す
// ・視界のぼかし量などのパラメータをShaderへ渡す

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour
{

    [SerializeField] Material m_fogMaterial;
    private void Update()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        Vector2 normalizedPosition = new Vector2(
            viewportPosition.x ,
            viewportPosition.y 
        );


        m_fogMaterial.SetVector("_PlayerPosition", normalizedPosition);

        float aspect = (float)Screen.width / Screen.height;
        m_fogMaterial.SetFloat("_Aspect", aspect);
    }


}
