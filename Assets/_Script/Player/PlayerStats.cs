using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]

public class PlayerStats : MonoBehaviour
{


    Rigidbody2D rigid2d;

    public float maxHp = 5.0f;
    public float maxSt = 10.0f;
    public Slider HPslider;
    public Slider STslider;
    float hp;
    float st;


    public float dashSt = 0.1f;
    public float regenSt = 0.5f;


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

    public float St
    {
        get => st;
        set
        {
            st = value;
            st = Mathf.Clamp(value, 0, maxSt);
        }
    }

    int score = 0;
    public Action<int> onScoreChange;
    public int Score
    {
        get => score; // 읽기는 public
        private set // 쓰기는 private
        {
            if (score != value)
            {

                score = Mathf.Min(value, 99999);

                onScoreChange?.Invoke(score);
            }

        }
    }


    private void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();

        Hp = maxHp;
        St = maxSt;
        HPslider.maxValue = maxHp;
        HPslider.value = Hp;
        STslider.maxValue = maxSt;
        STslider.value = St;

    }

    IEnumerator Stamina()
    {

        yield return null;  



    }






    private void OnDie()
    {

    }
}
