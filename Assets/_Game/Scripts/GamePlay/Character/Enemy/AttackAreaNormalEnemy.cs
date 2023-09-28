using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaNormalEnemy : AttackArea
{
    public NormalEnemy normalEnemy;

    void Start()
    {
        damage = normalEnemy.GetDamge();
    }

    private void Update() 
    {
        damage = normalEnemy.GetDamge();
    }

}
