using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
    private void Update()
    {
        // 적 공격 회전 테스트
    }
}
