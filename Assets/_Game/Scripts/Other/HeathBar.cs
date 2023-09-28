using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeathBar : GameUnit
{
    [SerializeField] Image imageFill;
    [SerializeField] Vector3 offset;
    [SerializeField] TextMeshProUGUI healthText;
    float hp;
    float maxHP;
    private Transform target;
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHP, Time.deltaTime*5f);
        if(target != null)
        {
            transform.position = target.position + offset;
        }

        if (healthText != null)
        {
            healthText.text =  hp.ToString(); 
            if(hp <= 0)
            {
                OnDespawn();
            }
        }

    }
    public void OnInit(float maxHP, Transform target)
    {
        this.target = target;
        this.maxHP = maxHP;
        hp = maxHP;
        imageFill.fillAmount = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
        //imageFill.fillAmount = hp / maxHP;
    }

    public void ChangeOffset(Vector3 offset)
    {
        this.offset = offset;
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }


}
