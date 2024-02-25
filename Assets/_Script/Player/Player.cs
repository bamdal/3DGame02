using Cinemachine;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerState;

public class Player : SystemBase, IAlive
{

    public Slider[] STSlider;

    public Slider HPSlider;

    public float playerAttackDamege = 10.0f;

  
    PlayerState playerState;

    Animator animator;

    StarterAssets.ThirdPersonController ThirdPersonController;

    readonly int _animIDOnDie = Animator.StringToHash("OnDie");

    bool Alive = true;

    private void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerState.deligatePlayerState += OnPlayerState;
        animator = GetComponent<Animator>();
        ThirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void OnPlayerState(PlayerState.playerState state)
    {
        switch (state)
        {
            case PlayerState.playerState.Guard:
                IsGuard = true;
                break;
            case PlayerState.playerState.NonGuard:
                IsGuard = false;
                break;
        }
    }

    protected override void StaminaRecovery()
    {
        base.StaminaRecovery();
        if (STSlider != null)
        {
            for (int i = 0; i < STSlider.Length; i++)
            {
                STSlider[i].value = St / maxSt;
            }
        }
    }

    protected override void HpHitDamege(float Damage)
    {
        base.HpHitDamege(Damage);
        if (HPSlider != null)
        {
            HPSlider.value = Hp / maxHp;
        }
    }

    public void Hit(float dmg, bool DamageCategory)
    {
        if(DamageCategory)
        {
            HpHitDamege(dmg);
            
        }
        else
        {

            StaminaDamege(dmg);
        }
    }

    protected override void OnDie()
    {
        if (Alive)
        {
            Alive = false;
            animator.SetTrigger(_animIDOnDie);
            ThirdPersonController.enabled = false;


        }
    }
}
