using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : Singleton<LevelManager>
{
    public LevelData[] levelDatas;
    public Player player;
    //public GameObject skin; // tao ra skin

    public List<Enemy> enemies = new List<Enemy>();

    public Level[] levels;
    public Level currentLevel;
    public int levelIndex;
    private bool hasSpawnedExplodeEnemies = false;
    private bool hasSpawnedNomalEnemies = false;
    private bool isBoss = false;

    public bool isEndLevel = false;
    private bool isReset = false;
    
    public void Start()
    {
        UIManager.Ins.OpenUI<MainMenu>();
    }

    private void Update() 
    {
        if(!GameManager.Instance.IsState(GameState.GamePlay))
            return;
        GameScript(levelIndex);
    }

    public void OnInit()
    {
        player.OnInit();
        hasSpawnedExplodeEnemies = hasSpawnedNomalEnemies = isEndLevel = isBoss = isReset = false;
        foreach(Enemy enemy in enemies)
        {
            enemy.OnInit();
        }
    }


    public void GameScript(int levelIndex)
    {
        if(isReset)
            return;
        LevelData levelData = levelDatas[levelIndex];
        switch (levelIndex)
        {
            case 0:
                if(!hasSpawnedNomalEnemies)
                {
                    CreateMultipleNormalEnemies(levelData.normalEnemyAmount);
                    hasSpawnedNomalEnemies = true;
                } 
                else if(AreAllNormalEnemiesDead() && !hasSpawnedExplodeEnemies)
                {
                    CreateMultipleExplodeEnemy(levelData.explodeEnemyAmount);
                    hasSpawnedExplodeEnemies = true;
                }
                else if(hasSpawnedExplodeEnemies && AreAllExplodeEnemiesDead())
                {
                    isEndLevel = true;
                }
                break;
            case 1:
                if(!hasSpawnedExplodeEnemies)
                {
                    CreateMultipleExplodeEnemy(levelData.explodeEnemyAmount);
                    hasSpawnedExplodeEnemies = true;
                }
                else if(AreAllExplodeEnemiesDead() && !hasSpawnedNomalEnemies)
                {
                    CreateMultipleNormalEnemies(levelData.explodeEnemyAmount);
                    hasSpawnedNomalEnemies = true;
                }
                else if(hasSpawnedNomalEnemies && AreAllNormalEnemiesDead())
                {
                    isEndLevel = true;
                }
                break;
            case 2:
                if(!isBoss)
                {
                    CreateBossEnemy();
                    isBoss = true;
                }

                break;
            default:
                // Xử lý cho các level khác nếu cần
                break;
        }
        if(isBoss && enemies.Count == 0)
        {
            isEndLevel = true;
        }
    }


    public LevelData GetLevelData()
    {
        return this.levelDatas[levelIndex];
    }

    public List<Enemy> GetEnemies()
    {
        return this.enemies;
    }

    public void RemoveEnemyFromList(Enemy enemyToRemove)
    {
        enemies.Remove(enemyToRemove);
    }

    public void OnLoadLevel(int level)
    {
        DestroyCurrentLevel();

        currentLevel = Instantiate(levels[level]);
        OnInit();
    }

    public void DestroyCurrentLevel()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
    }

    public void OnReset()
    {
        isReset = true;
        List<Enemy> enemiesToKill = new List<Enemy>(enemies); 

        foreach (Enemy enemy in enemiesToKill)
        {
            enemy.OnDespawn();
        }

        
    }
    public void Home()
    {
        OnReset();
        UIManager.Ins.CloseAll();
        DestroyCurrentLevel();
        UIManager.Ins.OpenUI<MainMenu>();
    }


    public void OnStartGame()
    {
        GameManager.Instance.ChangeState(GameState.GamePlay);
        levelIndex = 0;
        OnLoadLevel(levelIndex);
        OnInit();
    }

    public void OnFinishGame()
    {
        if(levelIndex >= (levels.Length-1))
        {
            UIManager.Ins.OpenUI<Victory>();
            UIManager.Ins.CloseUI<Gameplay>();
            GameManager.Instance.ChangeState(GameState.Finish);
        }
        else
        {
            levelIndex++;
            OnLoadLevel(levelIndex);
        }

    }

    public void OnRetry()
    {
        OnReset();
        UIManager.Ins.CloseAll();
        DestroyCurrentLevel();
        UIManager.Ins.OpenUI<Gameplay>();
        OnStartGame();
    }

    public Vector3 RandomPoint()
    {
        Vector3 randPoint = Vector3.zero;

        for (int t = 0; t < 50; t++)
        {

            randPoint = currentLevel.RandomPoint();
            if (Vector3.Distance(randPoint, player.TF.position) < 10)
            {
                continue;
            }
        }

        return randPoint;
    }


    private void CreateMultipleNormalEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            NormalEnemy normalEnemies = SimplePool.Spawn<NormalEnemy>(PoolType.NormalEnemy, RandomPoint(), Quaternion.identity);
            normalEnemies.playerTf = player.transform;
            normalEnemies.OnInit();
            enemies.Add(normalEnemies);

        }
    }

    private void CreateMultipleExplodeEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ExplodeEnemy explodeEnemies = SimplePool.Spawn<ExplodeEnemy>(PoolType.ExplodeEnemy, RandomPoint(), Quaternion.identity);
            explodeEnemies.playerTf = player.transform;
            explodeEnemies.OnInit();
            enemies.Add(explodeEnemies);

        }
    }

    private void CreateBossEnemy()
    {
        BossEnemy bossEnemy = SimplePool.Spawn<BossEnemy>(PoolType.BossEnemy, RandomPoint(), Quaternion.identity);
        bossEnemy.playerTf = player.transform;
        bossEnemy.OnInit();
        enemies.Add(bossEnemy);

    }

    private bool AreAllNormalEnemiesDead()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy is NormalEnemy normalEnemy && !normalEnemy.IsDead)
            {
                return false; 
            }
        }
        return true; 
    }

    private bool AreAllExplodeEnemiesDead()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy is ExplodeEnemy explodeEnemy && !explodeEnemy.IsDead)
            {
                return false; 
            }
        }
        return true; 
    }
}
