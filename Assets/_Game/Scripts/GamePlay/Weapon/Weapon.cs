using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint; 
    [SerializeField] private Bullet bulletPrefab; 
    [SerializeField] private float bulletSpeed = 10f; 

    public void Shoot(Transform target)
    {
        Bullet bullet = SimplePool.Spawn<Bullet>(PoolType.Arrow, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        if (bullet != null)
        {
            bullet.InitiateBullet(target, bulletSpeed);
        }
        
    }
}
