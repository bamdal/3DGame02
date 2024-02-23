using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : SystemBase, IAlive
{
    Animator animator;

    Player player;
    private int _animIDSpeed = Animator.StringToHash("Speed");
    private float _animationBlend;
    public float SpeedChangeRate = 10.0f;

    float targetSpeed;

    NavMeshAgent agent;


    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Instance.Player;

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.transform.position);


    }

    public void StartPlayerFind()
    {
        agent.isStopped = false;
        // 제대로 구현 안됨 2번 안돌아옴
    }

    public void StopPlayerFind()
    {
        agent.isStopped = true;
        animator.SetTrigger("EnemyAttack");
    }
    private void OnTriggerEnter(Collider other)
    {

    }
    // 시야 범위 내에 오면 블렌드트리에 이동 적용해서 달리는 애니메이션 나오게 하기 다가오면 공격
    private void FixedUpdate()
    {

        
        _animationBlend = Mathf.Lerp(agent.speed, targetSpeed, Time.fixedDeltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
            animator.SetFloat(_animIDSpeed, _animationBlend);
    }
    public void Hit(float dmg)
    {
        animator.SetTrigger("EnemyHit");
    }
}
