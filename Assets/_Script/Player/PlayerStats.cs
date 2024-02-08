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

 
    public Slider HPslider;
    public Slider STslider;



    public float dashSt = 0.1f;
    public float regenSt = 0.5f;




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




    IEnumerator Stamina()
    {

        yield return null;  



    }






    private void OnDie()
    {

    }
}
