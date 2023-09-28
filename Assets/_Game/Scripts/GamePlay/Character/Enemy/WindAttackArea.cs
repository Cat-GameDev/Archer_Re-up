using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAttackArea : AttackArea
{
    [SerializeField] private float push;
    protected override void OnTriggerEnter(Collider other)
    {
        Vector3 direction = other.transform.position - transform.position;
        direction.Normalize();

        other.transform.position += direction * push;
        other.GetComponent<Player>().ResetAnim();
        
    }
}