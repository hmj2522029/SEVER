using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource m_seSource2D; // 効果音用
    [SerializeField] AudioSource m_bgmSource;  // BGM用(一つしか鳴らせない(鳴らしたいならもう一個用意する))
    [SerializeField] int m_poolSize = 10;      // 3D効果音用のAudioSourceのプールサイズ


    private List<AudioSource> m_seSource3D = new List<AudioSource>();

    void Awake()
    {
        // シングルトン化
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 3D効果音用のAudioSourceをプールする
        for (int i = 0; i < m_poolSize; i++)
        {
            //新しいGameObjectを作成
            GameObject obj = new GameObject("SE3D_" + i);

            //親子関係を設定
            obj.transform.parent = transform;

            //AudioSourceコンポーネントを追加して設定
            AudioSource source = obj.AddComponent<AudioSource>();

            //3Dサウンドに設定
            source.spatialBlend = 1.0f;

            m_seSource3D.Add(source);
        }
    }

    private AudioSource GetAvailableSource()
    {
        // 使用していないAudioSourceを探す
        foreach (var source in m_seSource3D)
        {
            // 再生中でないAudioSourceを返す
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return null; // 全部使用中
    }

    // 効果音再生(2D(ループしない))
    public void PlaySE(AudioClip clip, float volume = 1f)
    {
        m_seSource2D.PlayOneShot(clip, volume);
    }

    // 効果音再生(3D(ループしない))
    public void PlaySE3D(AudioClip clip, Vector3 position, float volume = 1f)
    {
        // 空いているAudioSourceを取得
        AudioSource source = GetAvailableSource();

        // 空いているAudioSourceがない場合はログを出して終了
        if (source == null)
        {
            Debug.Log("空きAudioSourceがない");
            return;
        }

        // AudioSourceの位置とクリップ、音量を設定
        source.transform.position = position;   // 位置設定
        source.clip = clip;                     // 鳴らす音を設定
        source.volume = volume;                 // 音量設定

        // 距離設定
        source.minDistance = 1f;                            // 音が最大音量で聞こえる距離
        source.maxDistance = 10f;                           // 音が聞こえなくなる距離
        source.rolloffMode = AudioRolloffMode.Logarithmic;  // 音の減衰方法を設定

        source.Play();

    }

    public void PlayBGM(AudioClip clip)
    {
        if (m_bgmSource.clip == clip) return; // 同じなら何もしない

        m_bgmSource.clip = clip;
        m_bgmSource.loop = true;
        m_bgmSource.Play();
    }

}
