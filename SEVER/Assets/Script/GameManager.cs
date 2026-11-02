using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }    //シングルトン

    public int CurrentStageID { get; private set; } = 0;        //今入っているステージID

    private HashSet<int> m_clearStages = new HashSet<int>();    //クリアしているステージ番号

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetStage(int stageID)
    {
        CurrentStageID = stageID;
    }

    public void ClearStage(int stageID)
    {
        m_clearStages.Add(stageID);
    }

    public bool IsStageCleared(int stageID)
    {
        return m_clearStages.Contains(stageID);
    }


    public bool IsAllStagesCleared(int numberStages)
    {
        for (int i = 1; i <= numberStages; i++)
        {
            if (!m_clearStages.Contains(i))
            {
                return false;
            }
        }
        return true;
    }
}
