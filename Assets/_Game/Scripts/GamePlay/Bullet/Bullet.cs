using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit
{
    private Transform target;
    private float speed;

    [SerializeField] private float damage;

    public PlayerData damagePlayer;
    void Update()
    {
        BulletFly();
        damage = damagePlayer.damePlayer;
    }
    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    protected void BulletFly()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            transform.position += direction * speed * Time.deltaTime;
            
            Quaternion newRotation =  Quaternion.LookRotation(direction);
            transform.rotation = newRotation;
            
        }

    }

    public void InitiateBullet(Transform tgt, float spd)
    {
        target = tgt;
        speed = spd;
        Invoke(nameof(OnDespawn), 3f);
    }

    private void OnTriggerEnter(Collider other) 
    {
        other.GetComponent<Enemy>().OnHit(damage);
        OnDespawn();
    }



}
