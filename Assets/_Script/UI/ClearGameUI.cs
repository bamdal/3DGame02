using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ClearGameUI : MonoBehaviour
{
    public Enemy enemy;
    Image image;
    public float color = 0.5f;
    Transform child;

    void Start()
    {
        enemy.onDie += ClearGame;
        image = GetComponent<Image>();
        child = GetComponent<Transform>();
        child.gameObject.SetActive(false);

    }

    private void ClearGame(bool obj)
    {
        image.color = new Color(0, 0, 0, color);
        child.gameObject.SetActive(obj);
        StartCoroutine(RetrunScene());
    }

    IEnumerator RetrunScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("StartScene 1");
    }
}
