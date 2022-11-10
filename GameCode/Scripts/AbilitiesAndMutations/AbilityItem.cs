using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;

public class AbilityItem : MonoBehaviour
{
    public VisualEffect lightning;
    public UIElements ui;
    public PlayerMovement pm;
    public GameObject turret;
    public float damage;
    public GameObject mutatorPos;

    protected float lightningCd = 18, dashCd = 9, invisCd = 23, turretCd = 60;
    protected float timer;

    protected void OnUse(float duration)
    {
        Debug.Log(mutatorPos);
        if (mutatorPos.GetComponentInChildren<Mutator>() != null)
            StartCoroutine(mutatorPos.GetComponentInChildren<Mutator>().Activate(duration));
    }
}
