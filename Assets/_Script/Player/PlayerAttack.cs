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
    public void OnAttack(GameObject obj, bool DamageCategory)
    {
        Debug.Log($"{obj.name} 맞음");
        IAlive alive = obj.GetComponent<IAlive>();
        alive.Hit(playerDamege, DamageCategory);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Attack)
        {
           

            if (other.CompareTag("Enemy"))
            {

                Vector3 point = other.ClosestPoint(transform.position);
                Vector3 playerRoot = other.ClosestPoint(transform.root.position);
                Vector3 directionToPoint = (transform.root.position - playerRoot).normalized;
                if (other.gameObject.GetComponent<Enemy>().IsGuard && Vector3.Angle(directionToPoint, other.transform.root.forward) < 70.0f)
                {
                    OnAttack(other.gameObject,false);
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

