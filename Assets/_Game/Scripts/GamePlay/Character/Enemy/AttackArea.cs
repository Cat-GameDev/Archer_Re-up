using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] protected float damage;
    protected virtual void OnTriggerEnter(Collider other) 
    {
        other.GetComponent<Player>().OnHit(damage);
    }
}
