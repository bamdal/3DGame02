using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDieUI : MonoBehaviour
{
    Player player;
    Image image;

    bool playerDie = false;
    float color = 0.0f;
    private void Awake()
    {
        player = GameManager.Instance.Player;
        player.onDie += FadeOut;
        image = GetComponent<Image>();
        playerDie = false;
        color = 0.0f;
    }

    private void Update()
    {
        if (image != null)
        {
            if (playerDie)
            {
                color += Time.deltaTime*0.2f;
                image.color = new Color(0, 0, 0, color);
            }
        }
        if (color > 1)  // 화면이 완전히 검정색이됨
        {
            SceneManager.LoadScene("StartScene 1");
        }
    }

    private void FadeOut(bool obj)
    {

            playerDie= obj;
        
    }
}
