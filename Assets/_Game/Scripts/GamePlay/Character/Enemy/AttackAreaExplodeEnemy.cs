using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class AttackAreaExplodeEnemy : AttackArea
{
    public ExplodeEnemy explodeEnemy;
    void Start()
    {
        damage = explodeEnemy.GetDamge();
    }

    private void Update() 
    {
        damage = explodeEnemy.GetDamge();
    }


}
