using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, Attack
{
    // EnemyAttack 거 가져와서 공격할때 가드인지 아닌지 파악후 이펙트 생성
    float playerDamege;
    private void Awake()
    {
        playerDamege = GameManager.Instance.Player.playerAttackDamege;
    }
    public void OnAttack(GameObject obj)
    {
        Debug.Log("Enemy 맞음");
        IAlive alive = obj.GetComponent<IAlive>();
        alive.Hit(playerDamege);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnAttack(other.gameObject);
            if (other.gameObject.GetComponent<SystemBase>().IsGuard)
            {
            }
        }
    }
}

