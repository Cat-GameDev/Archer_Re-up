using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponData" , order = 1)]
public class WeaponData : ScriptableObject
{
    [SerializeField] private Weapon[] weaponItems;
    public Weapon GetWeapon(WeaponType weaponType)
    {
        return weaponItems[(int)weaponType];
    }
}


