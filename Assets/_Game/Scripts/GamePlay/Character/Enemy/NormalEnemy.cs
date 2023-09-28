using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NormalEnemy : Enemy
{

    void Start()
    {
        stateMachine.ChangeState(IdleState);
    }

    private void Update() 
    {
        if(IsDead || !GameManager.Instance.IsState(GameState.GamePlay))
            return; 
        stateMachine?.Execute();
    }

    public override void OnInit()
    {
        base.OnInit();
        hp = levelData.NormalEnemyHealth;
        heathBarPrefab.OnInit(hp, transform);
        offsetHealthBar = new Vector3(0f,1.5f,0f);
        heathBarPrefab.ChangeOffset(offsetHealthBar);
        damge = levelData.dameNormalEnemy;
    }

    public override void OnDeath()
    {
        DeActiveAttack();
        LevelManager.Instance.RemoveEnemyFromList(this);
        StopMoving();
        ChangeAnim(Constants.ENEMY_DEATH);
        Invoke(nameof(OnDespawn), 1f);
    }



}
