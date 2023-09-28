using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed = 5;
    public Skin skinWeapon;
    public Weapon weapon;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    private Enemy currentTarget;
    [SerializeField] private PlayerData playerData;


    private void Start() 
    {
        skinWeapon.ChangeWeapon(WeaponType.Bow);
        weapon = skinWeapon.GetWeapon();
    }

    void Update()
    {
        if(!GameManager.Instance.IsState(GameState.GamePlay) || IsDead)
            return;
        
        if (Input.GetMouseButton(0))
        {
            isMoving = true; 
            Moving();
        }
        else
        {
            isMoving = false; 
            CheckForEnemies();  
        }

        enemies = LevelManager.Instance.GetEnemies();
    }

    public override void OnInit()
    {
        if(LevelManager.Instance.levelIndex == 0)
        {
            playerData.playerHealth = playerData.startingHealth;
            CreateHealthBar();
        }
        levelData = LevelManager.Instance.GetLevelData();
        isMoving  = isAttacking = false; 
        transform.position = levelData.playerPosition;
        ResetAnim();
    }

    protected override void CreateHealthBar()
    {
        heathBarPrefab = SimplePool.Spawn<HeathBar>(PoolType.HealthBar, offsetHealthBar, Quaternion.identity);
        UpdatePlayerHealth();
        heathBarPrefab.OnInit(hp, transform);
        offsetHealthBar = new Vector3(0f,2f,0f);
        heathBarPrefab.ChangeOffset(offsetHealthBar);
    }

    private void UpdatePlayerHealth()
    {
        hp = playerData.playerHealth;
    }

    

    private void Moving()
    {
        TF.position = JoystickControl.direct * speed * Time.deltaTime + TF.transform.position;

        if (JoystickControl.direct != Vector3.zero)
        {
            skin.forward = JoystickControl.direct;
        }
        ChangeAnim(Constants.PLAYER_RUN);
    }


    private void CheckForEnemies()
    {
        if (!isMoving && !isAttacking) 
        {
            float closestDistance = float.MaxValue;
            currentTarget = null;

            foreach (Enemy enemy in enemies)
            {
                if (enemy != null)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        currentTarget = enemy;
                    }
                }
            }

            if (currentTarget != null)
            {
                StartCoroutine(AttackCoroutine(currentTarget.transform)); 
            }
            else
            {
                ChangeAnim(Constants.PLAYER_IDLE);
            }
        }
    }


    protected override IEnumerator AttackCoroutine(Transform target)
    {
        isAttacking = true; 

        ChangeAnim(Constants.PLAYER_ATTACK);

        ChangeDirection(target);

        weapon.Shoot(target);

        yield return new WaitForSeconds(timeRate);

        isAttacking = false;
        if(!isMoving)
        {
            ResetAnim();
        } 
    }


    public override void ResetAnim()
    {
        ChangeAnim(Constants.PLAYER_IDLE);
    }

    public override void OnDeath()
    {
        ChangeAnim(Constants.PLAYER_DIE);
        UIManager.Ins.CloseUI<Gameplay>();
        UIManager.Ins.OpenUI<Fail>();
    }








}

    