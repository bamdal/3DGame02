using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
 
    Image imageHp;
    Image imageSt;

    public Enemy enemy;
    Transform UITransform;
    void Start()
    {
        Transform child = transform.GetChild(0);
        imageHp = child.GetChild(0).GetComponent<Image>();

        child = transform.GetChild(1);
        imageSt = child.GetChild(0).GetComponent<Image>();

        enemy.EnemyChangeHP += ChangeHP;
        enemy.EnemyChangeST += ChangeST;
        UITransform = enemy.transform.GetChild(6);
        enemy.onDie += UIDisable;
    }

    private void UIDisable(bool obj)
    {
        gameObject.SetActive(!obj);
    }

    private void ChangeHP(float obj)
    {
        imageHp.fillAmount = obj / enemy.maxHp;

    }

    private void ChangeST(float obj)
    {
        imageSt.fillAmount =  (obj / enemy.maxSt);

    }

    private void Update()
    {
        if (enemy != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(UITransform.position);

            RectTransform rt = GetComponent<RectTransform>();
            rt.position = new Vector2(screenPos.x, screenPos.y); 
        }
    }
}
