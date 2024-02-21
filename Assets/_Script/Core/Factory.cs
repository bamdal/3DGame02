using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PoolObjectType
{
    Hit,
    Guard
}

public class Factory : Singleton<Factory>
{
    HitPool HitPool;
    GuardPool GuardPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

       HitPool = GetComponentInChildren<HitPool>(true);
        if(HitPool != null)
            HitPool.Initialize();
        GuardPool = GetComponentInChildren<GuardPool>(true);
        if (GuardPool != null)
            GuardPool.Initialize();
    }

    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.Hit:
                result = HitPool.GetObject(position, euler).gameObject;
                break;
            default:
                break;
        }
        return result;
    }

    public Hit GetHit()
    {
        return HitPool.GetObject();
    }

    public Hit GetHit(Vector3 position, float angle = 0.0f)
    {
        return HitPool.GetObject(position, angle * Vector3.forward); 
    }

    public Guard GetGuardt()
    {
        return GuardPool.GetObject();
    }

    public Guard GetGuard(Vector3 position, float angle = 0.0f)
    {
        return GuardPool.GetObject(position, angle * Vector3.forward);
    }
}

// 팩토리 만들기
// - 이전 프로젝트 펙토리 이식
