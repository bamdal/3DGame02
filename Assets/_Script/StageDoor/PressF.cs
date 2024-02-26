using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressF : MonoBehaviour
{
    Transform presstext;
    Player player;

    private void Awake()
    {
        player = GameManager.Instance.Player;
        presstext = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            presstext.gameObject.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            presstext.gameObject.SetActive(false);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene("StartScene 2");
            }
        }
    }

}
