using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDoor : MonoBehaviour
{
    Transform TextBar;
    Player player;

    private void Awake()
    {
        player = GameManager.Instance.Player;
        TextBar = transform.GetChild(0);
    }

    private void Update()
    {
        transform.forward = -(player.transform.position - transform.position);
    }
}
