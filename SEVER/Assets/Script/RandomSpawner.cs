using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{

    [SerializeField] Transform m_spawnPoints;       //生成する位置の親オブジェクト
    [SerializeField] GameObject m_spawnPrefab;      //生成するPrefab

    private GameObject m_spawnedObject;     //生成されたオブジェクトの参照

    public GameObject SpawnObject => m_spawnedObject;

    public void Spawn()
    {

        //子オブジェクトの数を取得
        int count = m_spawnPoints.childCount;

        //ランダムに子オブジェクトを選択
        int index = Random.Range(0, count);

        //選択した子オブジェクトの位置を取得
        Transform spawnPoint = m_spawnPoints.GetChild(index);

        //選択した位置に生成
        m_spawnedObject = Instantiate(
            m_spawnPrefab,
            spawnPoint.position,
            Quaternion.identity
        );
        

    }

}
