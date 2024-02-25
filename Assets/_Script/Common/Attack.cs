using UnityEngine;

interface Attack
{
    /// <summary>
    /// 상대에게 데미지를 주는 인터페이스
    /// </summary>
    /// <param name="obj">맞는 대상</param>
    /// <param name="DamageCategory">데미지의 유형 true면 체력감소 false면 체간감소</param>
    void OnAttack(GameObject obj,bool DamageCategory);
}