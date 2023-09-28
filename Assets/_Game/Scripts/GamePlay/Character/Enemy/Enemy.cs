using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using Unity.VisualScripting;



public class Enemy : Character
{
    protected StateMachine stateMachine = new StateMachine();
    [SerializeField] protected NavMeshAgent navMeshAgent;
    public Transform playerTf;
    protected Transform currentTarget; 
    [SerializeField] protected float rangeVision;
    [SerializeField] protected float rangeAttack;
    [SerializeField] protected GameObject attackArea;
    protected float damge;

    
    // void Start()
    // {
    //     stateMachine.ChangeState(IdleState);
    // }

   
    // void Update()
    // {
    //     if(IsDead)
    //         return; 

    //     stateMachine?.Execute();
    // }

    // public override void OnInit()
    // {
    //     base.OnInit();
    // }

    public float GetDamge()
    {
        return this.damge;
    }



    protected virtual void StopMoving()
    {
        navMeshAgent.enabled = false;
        ChangeAnim(Constants.ENEMY_IDLE);
        isMoving = false;
    }

    protected virtual void FollowPlayer()
    {
        if(isAttacking || IsDead)
            return;
        navMeshAgent.enabled = true;
        isMoving = true;
        ChangeAnim(Constants.ENEMY_RUN);

        navMeshAgent.SetDestination(playerTf.position);
        
    }


    private void SetNewTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        navMeshAgent.SetDestination(newTarget.position);
    }


    protected virtual void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackCoroutine(playerTf));
            ChangeAnim(Constants.ENEMY_ATTACK);
        }
    }

    protected virtual void ActionAttack()
    {
        attackArea.SetActive(true);
        
    }

    protected virtual void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }


    protected override IEnumerator AttackCoroutine(Transform playerTf)
    {
        isAttacking = true;
        ChangeDirection(playerTf);
        
        ActionAttack();
        yield return new WaitForSeconds(timeRate);
        ResetAnim();
        DeActiveAttack();
        isAttacking = false;
    }

    public override void ResetAnim()
    {
        ChangeAnim(Constants.ENEMY_IDLE);
    }

    public override void OnDeath()
    {
        LevelManager.Instance.RemoveEnemyFromList(this);
        StopMoving();
        ChangeAnim(Constants.ENEMY_DEATH);
        Invoke(nameof(OnDespawn), 1f);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        LevelManager.Instance.RemoveEnemyFromList(this);
    }

    protected virtual bool PlayerIsInRange(float range)
    {
        float distance = Vector3.Distance(playerTf.position, transform.position);
        
        if (distance <= range)
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }



    protected virtual void IdleState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float ramdonTime = Random.Range(0.25f, 0.5f); 
        float idleTimer = 0f;

        onEnter = () =>
        {
            idleTimer = 0f; 
            StopMoving();
        };

        onExecute = () =>
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= ramdonTime)
            {
                stateMachine.ChangeState(PatrolState);
            }

        };

        onExit = () =>
        {
        };
    }

    protected virtual void PatrolState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onExecute = () =>
        {
            if(PlayerIsInRange(rangeVision))
            {
                stateMachine.ChangeState(AttackState);
            }
            else
            {
                FollowPlayer();
            }

        };

    }

    
    protected virtual void AttackState(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {

        };

        onExecute = () =>
        {
            if(IsDead)
                return;
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


}
