using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class EnemyAttacK : MonoBehaviour, Attack
{
    public float damege = 10.0f;

    bool IsGuard = false;

    PlayerState playerState;

    float coolTime = 0.2f;
    float currentCoolTime;
    bool Attack => currentCoolTime < 0.0f;


    private void Awake()
    {
        playerState = GameManager.Instance.Player.GetComponent<PlayerState>();
        playerState.deligatePlayerState += OnPlayerState;

    }
    private void Update()
    {
        currentCoolTime -= Time.deltaTime;
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

    public void OnAttack(GameObject obj,bool DamageCategory)
    {
        Debug.Log($"{obj.name} 맞음");
        IAlive alive = obj.GetComponent<IAlive>();
        alive.Hit(damege, DamageCategory);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Attack)
        {
            if (other.CompareTag("Player"))
            {

                Vector3 point = other.ClosestPoint(transform.position);
                Vector3 playerRoot = other.ClosestPoint(transform.root.position);
                Vector3 directionToPoint = (transform.root.position - playerRoot).normalized;

            
                if (IsGuard && Vector3.Angle(directionToPoint,other.transform.root.forward) < 70.0f)
                {
                    OnAttack(other.gameObject, false);
                    Factory.Instance.GetGuard(point);

                }
                else
                {
                    OnAttack(other.gameObject,true);
                    Factory.Instance.GetHit(point);
                }
                currentCoolTime = coolTime;
            }
        }
    }
}
