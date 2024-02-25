

interface IAlive
{
    /// <summary>
    /// 데미지를 받는 오브젝트가 가지는 인터페이스
    /// </summary>
    /// <param name="dmg">데미지 크기</param>
    /// <param name="DamageCategory">데미지의 유형 true면 체력감소 false면 체간감소</param>
    void Hit(float dmg, bool DamageCategory);
}
