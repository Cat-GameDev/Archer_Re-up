using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const string PLAYER_RUN = "run";
    public const string PLAYER_IDLE = "idle";
    public const string PLAYER_DANCE = "dance";
    public const string PLAYER_ATTACK = "attack";
    public const string PLAYER_DIE = "die";

    public const string ENEMY_IDLE = "idle";
    public const string ENEMY_RUN = "run";
    public const string ENEMY_ATTACK = "attack";
    public const string ENEMY_DEATH = "die";
    public const string ENEMY_DASHUP = "dashup";
    public const string ENEMY_FLYIDLE = "flyidle";
    public const string ENEMY_FLY = "fly";
    public const string ENEMY_FLYATTACK = "attackfly";



}

public enum BulletType
{
    Normal = 0,
    Fire = 1,
    Ice = 2,
    Grass = 3
}

public enum WeaponType
{
    Bow = 0

}

public enum GameObjectList
{
    WarningFire = 0,
    WindAttackArea = 1,
    TornadoAttack = 2,
    FlyBreathFire = 3,
    FireAttack = 4
}
