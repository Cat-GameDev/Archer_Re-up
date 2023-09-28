using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class Character : GameUnit
{
    public LevelData levelData;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform skin;
    [SerializeField] protected HeathBar heathBarPrefab;
    [SerializeField] protected CombatText combatTextPrefab;
    [SerializeField] protected float timeRate = 1f;
    protected Vector3 offsetHealthBar;
    protected bool isMoving = false;

    protected string currentAnim;
    public float hp;
    public bool IsDead => hp <= 0;
    protected bool isAttacking = false ;

    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    public virtual void OnInit()
    {
        isAttacking = false;
        levelData = LevelManager.Instance.GetLevelData();
        CreateHealthBar();
    }

    protected virtual void CreateHealthBar()
    {
        if(heathBarPrefab != null)
        {
            heathBarPrefab = SimplePool.Spawn<HeathBar>(PoolType.HealthBar, offsetHealthBar, Quaternion.identity);
        }
    }

    public HeathBar GetHeathBar()
    {
        return this.heathBarPrefab;
    }

    public virtual void OnDespawn()
    {
        SimplePool.Despawn(this);
        heathBarPrefab.OnDespawn();
    }

    public virtual void OnDeath()
    {
        
    }

    public virtual void OnHit(float damage)
    {
        if(!IsDead)
        {
            hp -= damage;

            if(IsDead)
            {
                hp = 0;
                OnDeath();
            }
            heathBarPrefab.SetNewHp(hp);
            SimplePool.Spawn<CombatText>(PoolType.CombatText, transform.position + new Vector3(1f,0f,0f) + Vector3.up , Quaternion.identity).OnInit(damage);

        }
    }

    public virtual void ResetAnim()
    {

    }

    protected virtual IEnumerator AttackCoroutine(Transform target)
    {
        yield return new WaitForSeconds(0f);
    }

    protected void ChangeDirection(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        skin.LookAt(target);
        skin.eulerAngles = new Vector3(0, skin.eulerAngles.y, 0);
    }





}
