using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IAlive
{
    Animator animator;
    Transform EnemyWapon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        EnemyWapon = transform.GetChild(5);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Katana"))
        {
            animator.SetTrigger("EnemyHit");
        }
    }
    private void FixedUpdate()
    {
        // 적 공격 회전 테스트
        EnemyWapon.Rotate(Time.fixedDeltaTime * 360.0f * Vector3.up);
    }

    public void Hit(float dmg)
    {
        
    }
}
