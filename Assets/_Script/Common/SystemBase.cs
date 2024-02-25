using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SystemBase : MonoBehaviour
{
    // HP와 체간구현
    // 체간의 회복력은 현제 HP의 비례 Hp/maxHp 에서 나온 퍼센트에 회복력을 곱해서 조절

    /// <summary>
    /// 최대 HP
    /// </summary>
    public float maxHp = 100.0f;
    /// <summary>
    /// 최대 체간
    /// </summary>
    public float maxSt = 100.0f;

    float hp;
    float st;


    /// <summary>
    /// 초당 체간 재생력
    /// </summary>
    float stRecovery = 10.0f;

    public float StRecovery 
    { 
        get => stRecovery * Hp / maxHp;

    }

    /// <summary>
    /// 가드시 기존 체간재생력에서 증가될 비율
    /// </summary>
    float stRecoveryGuard = 3.0f;

    /// <summary>
    /// 공격중인지 파악
    /// </summary>
    protected bool IsAttack = false;

    /// <summary>
    /// 가드중인지 파악
    /// </summary>
    public bool IsGuard = false;

    /// <summary>
    /// 퍼팩트가드 성공 파악
    /// </summary>
    protected bool IsPerfectGuard = false;

    /// <summary>
    /// 퍼팩트 가드시 체간데미지 경감률
    /// </summary>
    float PerfectGuardDamageReduction = 0.2f;


    /// <summary>
    /// 전투후 체간이 차는 타이밍 시간
    /// </summary>
    float nonCombatTime = 1.0f;

    /// <summary>
    /// 체간 재생 가능 - 공격, 가드, 피해받은후 nonCombatTime후에 재생가능
    /// </summary>
    bool stRecoveryEnable = true;

    /// <summary>
    /// 현재 HP (0~maxHp)
    /// </summary>
    public float Hp
    {
        get => hp;
        private set
        {
            hp = value;
            hp = Mathf.Clamp(value, 0, maxHp);
            if (hp < 0.1f)
            {
                OnDie();
            }

        }
    }

    /// <summary>
    /// 현재 체간 (0~maxSt)
    /// </summary>
    public float St
    {
        get => st;
        set
        {
            st = value;
            st = Mathf.Clamp(value, 0, maxSt);
            if(st == maxSt && !IsPerfectGuard) // 체간이 가득차고 퍼펙트가드에 실패했을때
            {
                StaminaBrokenPosture();
            }
        }
    }






#if UNITY_EDITOR
    //테스트용 text에 Hp St 출력해보기
    public TextMeshPro HpText;
    public TextMeshPro StText;

#endif
    private void Awake()
    {
        Hp = maxHp;
        St = 0;
    }

    private void Update()
    {
        StaminaRecovery();
    }

    /// <summary>
    /// 체간 재생
    /// 전투중일 때는 안차고 비전투 유지 몇초후 재생시작 가드중일때 더 잘찬다
    /// </summary>
    protected virtual void StaminaRecovery()
    {
        if(stRecoveryEnable) // 체간 회복이 가능할때
        {
            if(IsGuard) // 가드모션중
            {
                St -= Time.deltaTime * StRecovery * stRecoveryGuard;
            }
            else        // 그냥 서있을때
            {
                St -= Time.deltaTime * StRecovery;
            }
        }


    }

    IEnumerator StaminaRecoveryTime()
    {
        stRecoveryEnable = false;
        yield return new WaitForSeconds(nonCombatTime);
        stRecoveryEnable = true;
    }

    /// <summary>
    /// 가드중에 맞아서 체간이 늘어남
    /// </summary>
    /// <param name="Damege">체간 데미지</param>
    public virtual void StaminaDamege(float Damege)
    {
        if(IsPerfectGuard)
        {
            St += Damege* PerfectGuardDamageReduction;
        }
        else
        {
            St += Damege;
        }
        StopAllCoroutines();
        StartCoroutine(StaminaRecoveryTime());
    }

    /// <summary>
    /// 체간이 가득차서 자세가 무너졌을때 체간 전부 회복
    /// </summary>
    private void StaminaBrokenPosture()
    {
        st = 0.0f;
    }

    /// <summary>
    /// 공격받아서 Hp 감소
    /// </summary>
    /// <param name="Damage">공격 받은 데미지</param>
    protected virtual void HpHitDamege(float Damage)
    {
        Hp -= Damage;

        StopAllCoroutines();
        StartCoroutine(StaminaRecoveryTime());
    }

    /// <summary>
    /// Hp가 0.1f 이하 일때 사망
    /// </summary>
    protected virtual void OnDie()
    {
        
    }


#if UNITY_EDITOR
    public void TestHPSet(float hp)
    {
        HpHitDamege(hp);

    }

    public void TestIsGuard(bool g)
    {
        IsGuard = g;
    }

#endif

}
