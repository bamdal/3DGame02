using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using StarterAssets;
using System;
using UnityEngine.Windows;

public class TargetLock : MonoBehaviour
{
    [Header("Objects")]
    [Space]
    [SerializeField] private Camera mainCamera;            // your main camera object.
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera; //cinemachine free lock camera object.
    [Space]
    [Header("UI")]
    [SerializeField] private Image aimIcon;  // ui image of aim icon u can leave it null.
    [Space]
    [Header("Settings")]
    [Space]
    [SerializeField] private string enemyTag; // the enemies tag.
    [SerializeField] private Vector2 targetLockOffset;
    [SerializeField] private float minDistance; // minimum distance to stop rotation if you get close to target
    [SerializeField] private float maxDistance;

    public bool isTargeting;

    private float maxAngle;
    public Transform currentTarget;
    private float mouseX;
    private float mouseY;

    /// <summary>
    /// 컨트롤러에서 카메라 잠그기 가져오는용
    /// </summary>
    StarterAssets.ThirdPersonController thirdPersonController;

    /// <summary>
    /// 플레이어 카메라 팔로우 트랜스폼
    /// </summary>
    public Transform playerCameraRoot;

    private StarterAssetsInputs _input;

    public Action<bool> Targeting;

    public float coolTime = 1.0f;
    float currentCoolTime;

    bool LookAtCoolTime => currentCoolTime < 0.0f;
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        playerCameraRoot = transform.GetChild(0);
        maxAngle = 90f; // always 90 to target enemies in front of camera.
                        //cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
                        // cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
        _input = GetComponent<StarterAssetsInputs>();

    }

    private void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (isTargeting)
        {

            NewInputTarget(currentTarget);
        }

        if (aimIcon)
            aimIcon.gameObject.SetActive(isTargeting);

       // cinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseX;
       // cinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseY;

        if (_input.lookOn && LookAtCoolTime)
        {
            currentCoolTime = coolTime;
            AssignTarget();

            // playerCameraRoot.transform.rotation = Quaternion.LookRotation(transform.forward);
         
        }
    }

    /// <summary>
    /// 타게팅 강제 온오프 시도
    /// </summary>
    public void OutTarget()
    {
        AssignTarget();

    }


    private void AssignTarget()
    {
        if (isTargeting)
        {
            //cinemachineVirtualCamera.Priority = 5;
            isTargeting = false;
            Targeting?.Invoke(isTargeting);
      

            cinemachineVirtualCamera.LookAt = null;
            currentTarget = null;
            return;
        }


           
     

        if (ClosestTarget())
        {
            currentTarget = ClosestTarget().transform;
            isTargeting = true;

            Targeting?.Invoke(isTargeting);
        }

    }

    private void NewInputTarget(Transform target) // sets new input value.
    {
        if (!currentTarget) return;

        Vector3 viewPos = mainCamera.WorldToViewportPoint(target.position);

        if (aimIcon)
            aimIcon.transform.position = mainCamera.WorldToScreenPoint(target.position);

        if ((target.position - transform.position).magnitude < minDistance) return;
        //cinemachineVirtualCamera.Priority = 11;
        cinemachineVirtualCamera.LookAt = target.GetChild(0);
        playerCameraRoot.transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized);


    }


    private GameObject ClosestTarget() // this is modified func from unity Docs ( Gets Closest Object with Tag ). 
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float distance = maxDistance;
        float currAngle = maxAngle;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                Vector3 viewPos = mainCamera.WorldToViewportPoint(go.transform.position);
                Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                if (Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle)
                {
                    closest = go;
                    currAngle = Vector3.Angle(diff.normalized, mainCamera.transform.forward.normalized);
                    distance = curDistance;
                }
            }
        }
        return closest;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}