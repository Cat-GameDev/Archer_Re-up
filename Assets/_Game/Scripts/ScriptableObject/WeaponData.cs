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

    // public WeaponType NextType(WeaponType weaponType)
    // {

    // }

    // public WeaponType PrevType(WeaponType weaponType)
    // {
        
    // }

    [System.Serializable]
    public class WeaponItem
    {
        public string name;
        public WeaponType type;
        public int cost;
    }




}


