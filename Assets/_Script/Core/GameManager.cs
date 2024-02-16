using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;

    public Player Player
    {
        get
        {
            if (player == null)
            {
                OnInitialize();
            }
            return player;
        }
    }


    protected override void OnInitialize()
    {
        base.OnInitialize();
        player = FindAnyObjectByType<Player>();
    }

    bool isClear = false;

    public Action onClear;

    public void GameClear()
    {
        if (!isClear)
        {
            onClear?.Invoke();
            isClear = true;
        }
    }
       
    bool isGameOver = false;

    public Action onGameOver;

    public void GameOver()
    {
        if (!isGameOver)
        {
            onGameOver?.Invoke();
            isGameOver = true;
        }
    }

}
