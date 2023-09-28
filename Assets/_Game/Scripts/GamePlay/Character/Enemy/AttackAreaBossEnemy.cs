using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaBossEnemy : AttackArea
{
    public BossEnemy bossEnemy;


    void Start()
    {
        damage = bossEnemy.GetDamge();
    }

    private void Update() 
    {
        damage = bossEnemy.GetDamge();
    }
}
