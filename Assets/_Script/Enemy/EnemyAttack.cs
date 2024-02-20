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

    public ParticleSystem Hit;
    public ParticleSystem Guard;

    private void Awake()
    {
        playerState = GameManager.Instance.Player.GetComponent<PlayerState>();
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
        Debug.Log($"{obj.name} 맞음");
        IAlive alive = obj.GetComponent<IAlive>();
        alive.Hit(damege);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnAttack(other.gameObject);
            Vector3 point = other.ClosestPoint(transform.position);
            if (IsGuard) 
            {
                // 파티클 Instantiate로 꺼내서 쓰기
                Guard.transform.position = point;
                Guard.Play();
            }
            else
            {
                Hit.transform.position = point;
                Hit.Play();
            }
        }
    }
}
