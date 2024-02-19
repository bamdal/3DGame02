using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerState;

public class Player : SystemBase, IAlive
{

    public Slider[] STSlider;

    public Slider HPSlider;

    public float playerAttackDamege = 10.0f;

  
    PlayerState playerState;

   

    private void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerState.deligatePlayerState += OnPlayerState;
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

    public void Hit(float dmg)
    {
        if(IsGuard)
        {
            StaminaDamege(dmg);
        }
        else
        {

            HpHitDamege(dmg);
        }
    }
}
