using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : SystemBase, IAlive
{
    Animator animator;

    Player player;
    private int _animIDSpeed;
    private float _animationBlend;
    public float SpeedChangeRate = 10.0f;
    float targetSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Instance.Player;
        _animIDSpeed = Animator.StringToHash("Speed");
    }
    private void OnTriggerEnter(Collider other)
    {

    }
    // 시야 범위 내에 오면 블렌드트리에 이동 적용해서 달리는 애니메이션 나오게 하기 다가오면 공격
    private void FixedUpdate()
    {

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.fixedDeltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
        animator.SetFloat(_animIDSpeed, _animationBlend);
    }
    public void Hit(float dmg)
    {
        animator.SetTrigger("EnemyHit");
    }
}
