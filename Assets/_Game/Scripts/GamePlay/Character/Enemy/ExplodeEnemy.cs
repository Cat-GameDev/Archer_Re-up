using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplodeEnemy : Enemy
{
    [SerializeField] private GameObject explodeVFX;
    
    void Start()
    {
        stateMachine.ChangeState(IdleState);
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
        hp = levelData.ExplodeEnemyHealth;
        heathBarPrefab.OnInit(hp, transform);
        offsetHealthBar = new Vector3(0f,1.5f,0f);
        heathBarPrefab.ChangeOffset(offsetHealthBar);
        damge = levelData.dameExplodeEnemy;
    }

    protected override void FollowPlayer()
    {
        if (!isAttacking)
        {
            navMeshAgent.enabled = true;
            ChangeAnim(Constants.ENEMY_RUN);

            navMeshAgent.SetDestination(playerTf.position);
            Invoke(nameof(Attack), 3f);
        }
    }

    protected override void Attack()
    {
        if(!isAttacking && !IsDead)
        {
            StopMoving();
            Explode(); 
        }
        isAttacking = true;
    }

    private void Explode()
    {
        ActionAttack();
        OnDeath();
        Invoke(nameof(DeActiveAttack), 0.3f);
    }

    public override void OnDeath()
    {
        LevelManager.Instance.RemoveEnemyFromList(this);
        StopMoving();
        ParticlePool.Play(ParticleType.FireExplodeRedVFX,transform.position, transform.rotation);
        ChangeAnim(Constants.ENEMY_DEATH);
        Invoke(nameof(OnDespawn), 0.3f);
    }



    








}
