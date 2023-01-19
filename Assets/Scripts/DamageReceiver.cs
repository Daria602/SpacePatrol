using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class DamageReceiver : MonoBehaviour
{

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

    public int CurrentHP()
    {
        return _currentHP;

    }
    public int getMaxHP()
    {
        return maxHP;

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
        
        //if (Input.GetKeyDown(KeyCode.Tab))
        //    TakeDamage(+1);
        //if (Input.GetKeyDown(KeyCode.CapsLock))
        //    TakeDamage(-1);
        
        if(_protectionTimer>=0)
        {
            _protectionTimer -= Time.deltaTime;
        }

    }

    // To take a heart, the damage is pozitive, to add a heart, the damage is negative.
    //Note: Positive to decrease hp and negative to add hp is counter intuitive and confusing, lets keep it the right way
    //Positive values increase hp, Negative values decrease hp
    public void TakeDamage(int amount)
    {
        //Do not take damage if we just took
        //Check if we are taking damage or healing
        if (amount<0)
        {
            if (_protectionTimer > 0) return;
            _protectionTimer = damageProtectionTime;
        }
        //We should add the damage here not substract it, beacuse '-' and '-' = '+'
        //and we do not want to add Hp instead of taking it 
        _currentHP += amount;
        _currentHP = Mathf.Clamp(_currentHP, 0, maxHP);

        if(HeartsHolder!=null)
        {

            for (int i = 1; i < HeartsHolder.childCount; i++)
            {
                bool isActive = i <= _currentHP;

                if (!HeartsHolder.GetChild(i).gameObject.activeSelf && isActive)
                {
                    HeartsHolder.GetChild(i).gameObject.SetActive(true);
                }

                HeartsHolder.GetChild(i).GetComponent<Animator>().SetBool("isDestroyed", !isActive);
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
        OnTakeDamage?.Invoke(amount);
    }

    
}
