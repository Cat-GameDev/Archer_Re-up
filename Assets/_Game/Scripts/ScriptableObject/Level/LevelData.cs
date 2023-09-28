using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Player")]
    public Vector3 playerPosition; 

    [Header("Normal Enemy")]
    public int normalEnemyAmount;
    public float NormalEnemyHealth; 
    public float dameNormalEnemy;

    [Header("Explode Enemy")]
    public int explodeEnemyAmount;
    public float ExplodeEnemyHealth; 
    public float dameExplodeEnemy;

    [Header("Boss Enemy")]
    public int bossEnemyAmount;
    public float BossEnemyHealth; 
    public float dameBossEnemy;
    public Vector3 bossPosition;

    private static LevelData ins;
    public static LevelData Ins
    {
        get
        {
            if (ins == null)
            {
                LevelData[] datas = Resources.LoadAll<LevelData>("");

                if (datas.Length == 1)
                {
                    ins = datas[0];
                }
                else
                if (datas.Length == 0)
                {
                    Debug.LogError("Can find Scriptableobject LevelData");
                }
                else
                {
                    Debug.LogError("have multiple Scriptableobject LevelData");
                }
            }

            return ins;
        }
    }


}
