using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatText : GameUnit
{
    [SerializeField] private TextMeshProUGUI hpText;
    public Canvas canvas;

    public void OnInit(float damage)
    {
        //LoadCamera();
        hpText.text = damage.ToString();
        Invoke(nameof(OnDespawn), 1f);
    }

    public void LoadCamera()
    {
        canvas.worldCamera = Camera.main;
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}
