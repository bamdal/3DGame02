using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrace : MonoBehaviour
{
    Enemy parentEnemy;
    Player player;
    StarterAssets.StarterAssetsInputs assetsInputs;

    bool playerinsight = false;

    private void Awake()
    {
        parentEnemy = GetComponentInParent<Enemy>();
        player = GameManager.Instance.Player;
        assetsInputs = player.GetComponent<StarterAssetsInputs>();
        assetsInputs.deligatePlayerState += EnemyState;
    }

    private void EnemyState(PlayerState.playerState state)
    {
        if(playerinsight)
        {
            switch (state) 
            {
                case PlayerState.playerState.Attack:
                    parentEnemy.EnemyGuard(); // 적이 가드하는것 가져와서 넣기
                    break;
            
            }


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            parentEnemy.PlayerInSight();
            playerinsight = true;
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            parentEnemy.PlayerOutSight();
            playerinsight = false;
        }
    }

}
