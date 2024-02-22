using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, Attack
{
    // EnemyAttack 거 가져와서 공격할때 가드인지 아닌지 파악후 이펙트 생성
    float playerDamege;

    float coolTime = 0.2f;
    float currentCoolTime;

    bool Attack => currentCoolTime < 0.0f;

    private void Awake()
    {
        playerDamege = GameManager.Instance.Player.playerAttackDamege;
    }

    private void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }
    public void OnAttack(GameObject obj)
    {
        Debug.Log($"{obj.name} 맞음");
        IAlive alive = obj.GetComponent<IAlive>();
        alive.Hit(playerDamege);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Attack)
        {
           

            if (other.CompareTag("Enemy"))
            {
                OnAttack(other.gameObject);
                Vector3 point = other.ClosestPoint(transform.position);
                if (other.gameObject.GetComponent<Enemy>().IsGuard)
                {
                    Factory.Instance.GetGuard(point);
                }
                else
                {

                    Factory.Instance.GetHit(point);
                }
                currentCoolTime = coolTime;
            }
        }
    }
}

