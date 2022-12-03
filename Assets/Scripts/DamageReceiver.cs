using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class DamageReceiver : MonoBehaviour
{
    public Animator animator;

    [SerializeField]
    protected int maxHP;

    /// <summary>
    /// Check this if you want to destroy the gameobject when the hp reaches 0
    /// </summary>
    [SerializeField]
    protected bool destroyOnDead=false;

    /// <summary>
    /// The time in seconds in wich you are protected from taking damage again
    /// </summary>
    [SerializeField]
    protected float damageProtectionTime;

    [Header("Health Panel References")]
    public RectTransform HeartsHolder;
    public RectTransform HeartPrefab;

    public Action<int> OnTakeDamage;
    public Action OnDead;

    private float _protectionTimer;

    protected int _currentHP;

    public int CurrentHP
    {
        get => _currentHP;

    }

    protected void Awake()
    {
        _currentHP = maxHP;
        SetupHearts();
    }
    private void SetupHearts()
    {
        if (HeartsHolder == null || HeartPrefab == null) return;
        HeartPrefab.gameObject.SetActive(false);
        for (int i = 1; i < HeartsHolder.childCount; i++)
        {
            Destroy(HeartsHolder.GetChild(i).gameObject);
        }
        for (int i = 0; i < maxHP; i++)
        {
            var instance = Instantiate(HeartPrefab, HeartsHolder);
            instance.gameObject.SetActive(true);
        }
    }

    public void Update()
    {
        // For debug
        /*
        if (Input.GetKeyDown(KeyCode.Tab))
            TakeDamage(+1);
        if (Input.GetKeyDown(KeyCode.CapsLock))
            TakeDamage(-1);
        */

        if(_protectionTimer>=0)
        {
            _protectionTimer -= Time.deltaTime;
        }

    }

    // To take a heart, the damage is pozitive, to add a heart, the damage is negative.
    public void TakeDamage(int damage)
    {
        //Do not take damage if we just took
        if (damage>0)
        {
            if (_protectionTimer > 0) return;
            _protectionTimer = damageProtectionTime;
        }
        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, maxHP);

        if(HeartsHolder!=null)
        {
            for(int i = 1; i < HeartsHolder.childCount; i++)
            {
                animator.SetBool("isTakingDamage", false);
                HeartsHolder.GetChild(i).gameObject.SetActive(i <= _currentHP);
                animator.SetBool("isTakingDamage", true);
                //HeartsHolder.GetChild(i).
            }
        }
        
        if(_currentHP<=0)
        {
            OnDead?.Invoke();
            if(destroyOnDead)
            {
                Destroy(this.gameObject);
            }
        }
        OnTakeDamage?.Invoke(damage);
    }
}
