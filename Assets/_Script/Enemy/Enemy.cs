using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : SystemBase, IAlive
{
    Animator animator;

    Player player;
    private int _animIDSpeed = Animator.StringToHash("Speed");
    private float _animationBlend;
    public float SpeedChangeRate = 10.0f;

    float targetSpeed;

    NavMeshAgent agent;

    /// <summary>
    /// 공격중 나갔는지 판별하는 애니메이션 파라미터 true면 나가있는 상태 false면 안에 있는 상태
    /// </summary>
    readonly int _animIDEnemySight = Animator.StringToHash("EnemySight");
    readonly int _animIDEnemyAttack = Animator.StringToHash("EnemyAttack");
    readonly int _animIDOnDie = Animator.StringToHash("OnDie");

    /// <summary>
    /// 플레이어가 공격범위내에 있는지 유무 (true면 있다)
    /// </summary>
    bool PlayerSight = false;

    /// <summary>
    /// 적은 strengthCount 만큼 맞을때 경직에 걸린다.
    /// </summary>
    int strengthCount = 2;

    /// <summary>
    /// 현재 강인함 수
    /// </summary>
    int currentStrengthCount;

    bool Alive = true;

    CapsuleCollider CapsuleCollider;

    /// <summary>
    /// Enemy 사망시 자동 타겟 해제용
    /// </summary>
    TargetLock TargetLock;

    /*#if UNITY_EDITOR
    #endif*/
#if UNITY_EDITOR

#endif


    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Instance.Player;
        TargetLock = player.transform.GetComponent<TargetLock>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.transform.position);
        currentStrengthCount = strengthCount;
        CapsuleCollider = GetComponent<CapsuleCollider>();
#if UNITY_EDITOR
        HpText.text = $"HP : {Hp}";
        StText.text = $"ST : {St}";
#endif
    }

    /// <summary>
    /// 플레이어가 공격 범위 내에 들어옴
    /// </summary>
    public void PlayerInSight()
    {
        if (Alive)
        {
            PlayerSight = true;
            animator.SetBool(_animIDEnemySight, false);
            agent.isStopped = true;
            animator.SetTrigger(_animIDEnemyAttack);
        }

    }


    /// <summary>
    /// 플레이어가 범위 밖으로 나감
    /// </summary>
    public void PlayerOutSight()
    {
        PlayerSight = false;



    }
    private void Update()
    {
        agent.SetDestination(player.transform.position);

    }

    /// <summary>
    /// 공격이 끝날때 마다 불리는 애니메이션 이벤트
    /// </summary>
    void EnemyEndAttack()
    {
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        if (!PlayerSight)
        {
            agent.isStopped = false;
            animator.SetBool(_animIDEnemySight, true);
        }
    }

    void EnemyFinishAttack()
    {
        currentStrengthCount = strengthCount;
        if (PlayerSight)
        {
            animator.SetTrigger(_animIDEnemyAttack);
        }
        else
        {
            agent.isStopped = false;
            animator.SetBool(_animIDEnemySight, true);
        }
    }


    private void FixedUpdate()
    {


        _animationBlend = Mathf.Lerp(agent.speed, targetSpeed, Time.fixedDeltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
        animator.SetFloat(_animIDSpeed, _animationBlend);
    }

    protected override void HpHitDamege(float Damage)
    {
        base.HpHitDamege(Damage);
#if UNITY_EDITOR
        HpText.text = $"HP : {Hp}";
        StText.text = $"ST : {St}";
#endif
    }

    public override void StaminaDamege(float Damege)
    {
        base.StaminaDamege(Damege);
#if UNITY_EDITOR
        HpText.text = $"HP : {Hp}";
        StText.text = $"ST : {St}";
#endif
    }

    public void Hit(float dmg, bool DamageCategory)
    {
        if (currentStrengthCount > 0)
        {
            animator.SetTrigger("EnemyHit");
            currentStrengthCount--;
            HpHitDamege(dmg);
        }
        else
        {
            if (DamageCategory)
            {
                HpHitDamege(dmg);
            }
            else
            {
                StaminaDamege(dmg);
            }

        }
    }

    protected override void OnDie()
    {
        if (Alive)
        {
            Alive = false;
            transform.tag = "Ground";
            TargetLock.OutTarget();
            animator.SetTrigger(_animIDOnDie);
            agent.enabled = false;
            CapsuleCollider.enabled = false;
            this.enabled = false;
        }
    }

}
