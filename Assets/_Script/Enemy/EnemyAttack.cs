using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class EnemyAttacK : MonoBehaviour, Attack
{
    public float damege = 10.0f;

    bool IsGuard = true;

    PlayerState playerState;

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
        playerState.deligatePlayerState += OnPlayerState;
    }

    private void OnPlayerState(playerState state)
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

    public void OnAttack(GameObject obj)
    {
        Debug.Log("플레이어 맞음");
        IAlive alive = obj.GetComponent<IAlive>();
        alive.Hit(damege);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnAttack(other.gameObject);
            if(IsGuard) 
            {
            }
        }
    }
}
