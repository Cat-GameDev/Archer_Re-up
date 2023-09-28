using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



public class BossEnemy : Enemy
{
    [SerializeField] private List<Transform> firePointList = new List<Transform>();
    [SerializeField] private List<GameObject> gameObjectList = new List<GameObject>();
    public bool isWindAttack;
    public bool isTornadaAttack;
    public bool isRageState;
    void Start()
    {
        stateMachine.ChangeState(AttackState);
    }

    void Update()
    {

        if(IsDead || !GameManager.Instance.IsState(GameState.GamePlay))
            return; 

        stateMachine?.Execute();
    }
    

    public override void OnInit()
    {
        base.OnInit();
        isTornadaAttack = isWindAttack = false;
        transform.position = levelData.bossPosition;
        hp = levelData.BossEnemyHealth;
        heathBarPrefab.OnInit(hp, transform);
        offsetHealthBar = new Vector3(0f,3f,0f);
        heathBarPrefab.ChangeOffset(offsetHealthBar);
        damge = levelData.dameBossEnemy;
        DeActiveWindAttack();

    }

    public GameObject GetGameObject(GameObjectList gameObject)
    {
        return gameObjectList[(int)gameObject];
    }

    
    protected override void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackCoroutine(playerTf));
            if(!isRageState)
            {
                ChangeAnim(Constants.ENEMY_ATTACK);
            }
            else
            {
                ChangeAnim(Constants.ENEMY_FLYATTACK);
            }

        }
    }



    protected override IEnumerator AttackCoroutine(Transform playerTf)
    {
        isAttacking = true;
        ChangeDirection(playerTf);
        GetGameObject(GameObjectList.WarningFire).SetActive(true);
        if(!isRageState)
        {
            Invoke(nameof(DeActiveWarningFire), 0.3f);
            Invoke(nameof(ActionAttack), 1.2f);
        }
        else
        {
            //Invoke(nameof(DeActiveFlyBreathAttack), 0.3f);
            Invoke(nameof(ActiveFlyBreathAttack), 1.2f);
        }

        Invoke(nameof(FireBreathAttack), 0.5f);
        
        yield return new WaitForSeconds(timeRate);
        
        ResetAnim();
        DeActiveFlyBreathAttack();
        isAttacking = false;
    }

    public override void OnDeath()
    {
        //stateMachine.ChangeState(null);
        LevelManager.Instance.RemoveEnemyFromList(this);
        StopMoving();
        ChangeAnim(Constants.ENEMY_DEATH);
        Invoke(nameof(OnDespawn), 1.2f);
        
    }

    public override void ResetAnim()
    {
        if(isWindAttack)
            return;
        if(!isRageState)
        {
            ChangeAnim(Constants.ENEMY_IDLE);
        }
        else
        {
            ChangeAnim(Constants.ENEMY_FLYIDLE);
        }
        
        
    }

    private void DeActiveFlyBreathAttack()
    {
        GetGameObject(GameObjectList.FlyBreathFire).SetActive(false);
    }

    private void ActiveFlyBreathAttack()
    {
        GetGameObject(GameObjectList.FlyBreathFire).SetActive(true);
    }

    private void DeActiveWarningFire()
    {
        GetGameObject(GameObjectList.WarningFire).SetActive(false);
    }

    private void DeActiveWindAttack()
    {
        isWindAttack = false;
        GetGameObject(GameObjectList.WindAttackArea).SetActive(false);
    }

    private void ActiveWindAttack()
    {
        isWindAttack = true;
        GetGameObject(GameObjectList.WindAttackArea).SetActive(true);
    }

    private void ActiveTornadoAttack()
    {
        GetGameObject(GameObjectList.TornadoAttack).SetActive(true);
    }

    private void DeActiveTornadoAttack()
    {
        isTornadaAttack = false;
        GetGameObject(GameObjectList.TornadoAttack).SetActive(false);
    }

    protected override void StopMoving()
    {
        navMeshAgent.enabled = false;
        isMoving = false;
    }

    private void FireBreathAttack()
    {
        if(!isRageState)
        {
            ParticlePool.Play(ParticleType.FireBreathVFX,firePointList[0].position, transform.rotation);
        }
        else 
        {
            ParticlePool.Play(ParticleType.FireBreathVFX,firePointList[1].position, transform.rotation);
            ParticlePool.Play(ParticleType.FireBreathVFX,firePointList[2].position, transform.rotation);
            ParticlePool.Play(ParticleType.FireBreathVFX,firePointList[3].position, transform.rotation);
        }
        
    }

    protected override void FollowPlayer()
    {
        if(isAttacking || isWindAttack || isTornadaAttack)
            return;
        isMoving = true;
        if(!isRageState)
        {
            ChangeAnim(Constants.ENEMY_RUN);
        }
        else 
        {
            ChangeAnim(Constants.ENEMY_FLY);
        }
        
        navMeshAgent.enabled = true;
        
        navMeshAgent.SetDestination(playerTf.position);
        
    }

    
    private void TornadoAttack()
    {
        isTornadaAttack = true;
        int numTornadoes = 4;

        float angleStep = 360f / numTornadoes;

        Vector3[] tornadoPositions = new Vector3[numTornadoes];

        for (int i = 0; i < numTornadoes; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad; 
            float distance = 5f; 
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;
            tornadoPositions[i] = transform.position + offset;

            ParticlePool.Play(ParticleType.FireTornadaVFX, tornadoPositions[i], Quaternion.Euler(-90f, 0f, 0f));
        }
        ActiveTornadoAttack();
        Invoke(nameof(DeActiveTornadoAttack), 3f);
        ResetAnim();
    }

    private void FlyAttack()
    {
        ChangeAnim(Constants.ENEMY_FLYATTACK);
        Debug.Log(1);
    }


    private void Appear()
    {
        
    }

    protected override void AttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {

        onEnter = () =>
        {
            Appear();
        };

        onExecute = () =>
        {
            if(hp<=700)
            {
                stateMachine.ChangeState(RageState);
            }
            if(PlayerIsInRange(rangeAttack))
            {
                StopMoving();
                Attack();
            }
            else
            {
                FollowPlayer();
            }
        };

        onExit = () =>
        {

        };
    }

    public void RageState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float randomTime = 0f;
        float timer = 0f;
        onEnter = () =>
        {
            timer = 0f;
            randomTime = Random.Range(0.5f, 1f);
            isRageState = true;
            hp+=200;
            ChangeAnim(Constants.ENEMY_DASHUP);    
            ActiveWindAttack();

        };

        onExecute = () =>
        {
            timer += Time.deltaTime;
            if(isWindAttack)
            {
                Invoke(nameof(DeActiveWindAttack), 0.5f);
            }
            else 
            {
                TornadoAttack();
                if(timer < randomTime)
                {
                    FollowPlayer();
                } 
                else 
                {
                    stateMachine.ChangeState(FlyAttackState);
                }
            }
            
        };

        onExit = () =>
        {

        };
    }

    public void TornadoAttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {

        onEnter = () =>
        {

        };

        onExecute = () =>
        {
            TornadoAttack();
            stateMachine.ChangeState(FlyAttackState);
            
        };
    }

    public void FlyAttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        // TO DO: phun lua 3 tia lua 
        float attackTimer = 5f;
        float timer = 0f;
        onEnter = () =>
        {

        };

        onExecute = () =>
        {
            timer += Time.deltaTime;
            if(PlayerIsInRange(rangeAttack))
            {
                StopMoving();
                Attack();
            }
            else 
            {
                if(timer < attackTimer)
                {
                    FollowPlayer();
                } 
                else 
                {
                    stateMachine.ChangeState(TornadoAttackState);
                }
            }
            
        };
    }





    


}
