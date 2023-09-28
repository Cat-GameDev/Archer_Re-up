using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour
{
    Weapon currentWeapon;
    [SerializeField] private Transform leftHand;
    [SerializeField] private WeaponData weaponData;

    public void ChangeWeapon(WeaponType weaponType)
    {
        if(currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = Instantiate(weaponData.GetWeapon(weaponType), leftHand);
    }

    public Weapon GetWeapon()
    {
        return currentWeapon;
    }
}
