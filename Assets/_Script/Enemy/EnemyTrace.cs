using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrace : MonoBehaviour
{
    Enemy parentEnemy;

    private void Awake()
    {
        parentEnemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            parentEnemy.StopPlayerFind();
           
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            parentEnemy.StartPlayerFind();
            Debug.Log("나감");
        }
    }
}
