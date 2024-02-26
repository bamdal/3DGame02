using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // 플레이어의 전투 상태를받고 단순 이동, 아무것도 안한다, 땅에 있는다등 공격이 가능하면
    // queue에 받은 공격,대쉬,가드 명령을 수행
    // 이미 진행중이라면 공격은 q에 3개 까지 쌓아서 선입력진행
    // 대쉬 가드, 점프는 현재 q를 지워버리고 다음차례로 대기
    // Player인풋으로 받는다 가드 공격 대쉬중에는 움직임과 점프 막기


    /// <summary>
    /// 사용가능 여부, 쿨타임이 0 이상 일시 저스트가드
    /// </summary>
    public bool CanParry => currentCoolTime > 0.0f;

    /// <summary>
    /// 저스트 가드 지속시간
    /// </summary>
    public float justParryTime = 0.1f;
    /// <summary>
    /// 현재 남아있는 쿨타임
    /// </summary>
    float currentCoolTime = 0;

    public enum playerState
    {
        Idle,
        Attack,
        Guard,
        NonGuard,
        Dash,
        Jump,
        Hit
    }

    /// <summary>
    /// 현재상태의 방향 저장 버퍼
    /// </summary>
    Queue<Vector3> inputDirectionBuffer = new Queue<Vector3>(queueSize);

    /// <summary>
    /// 현재 어떤 상태인지 저장하는 버퍼
    /// </summary>
    Queue<playerState> inputStateBuffer = new Queue<playerState>(queueSize);

    /// <summary>
    /// 상태저장버퍼의 최대값
    /// </summary>
    static int queueSize = 1;

    public ThirdPersonController ThirdPersonController;

    playerState state = playerState.Idle;
    Vector3 dir;

    public Animator animator;
    private int _animIDIsGuard = Animator.StringToHash("IsGuard");
    private int _animIDIsAttack= Animator.StringToHash("IsAttack");
    private int _animIDIsHit= Animator.StringToHash("IsHit");
    private int _animIDIsGuradTrigger= Animator.StringToHash("IsGuradTrigger");
    private int _animIDIsJump = Animator.StringToHash("IsJump");

    /// <summary>
    /// 무기 트랜스폼
    /// </summary>
    public Transform Katana;

    public Action<playerState> deligatePlayerState;

    /// <summary>
    /// _Lock 이 true면 현재 락온중 
    /// </summary>
    bool _Lock => _targetLock.isTargeting;

    /// <summary>
    /// 락온시 봐야하는 방향벡터
    /// </summary>
    Vector3 LockLookAt => _targetLock.currentTarget.transform.position - transform.position;

    /// <summary>
    /// 조작가능 불가능 상태
    /// </summary>
    bool Operation => !GameManager.Instance.Player.onReaction; 

    protected TargetLock _targetLock;
    protected bool TargetLock => _targetLock.isTargeting;
    private void Awake()
    {
        _targetLock = GetComponent<TargetLock>();
    }


    /// <summary>
    /// 현재 상태를 최대 queueSize 까지만 저장하고 현재 방향키에 입력된 방향을 받음
    /// </summary>
    /// <param name="dir">현재 방향키가 향하는 방향</param>
    protected virtual void PlayerStateEnQueue(playerState type, Vector3 dir)
    {
        if (inputDirectionBuffer.Count < queueSize) // 최대 queue를 넘지 않았을때
        {
            inputStateBuffer.Enqueue(type);
            inputDirectionBuffer.Enqueue(dir);

        }
        else
        {
            Debug.Log("넘치게 입력했음");
        }
    }

    /// <summary>
    /// 각 행동별 개별 코드 여기 작성
    /// 현재상태버퍼 빼기 빠질때마다 다음 동작이 불려야 한다.
    /// 공격,대쉬,가드 이후 다시 실행됨
    /// </summary>
    public void PlayerStateDeQueue()
    {

        if (inputDirectionBuffer.Count > 0 && inputStateBuffer.Count > 0 && Operation)
        {
            state = inputStateBuffer.Dequeue();
            dir = inputDirectionBuffer.Dequeue();
            ThirdPersonController.currentSpeed = 0.0f; // 애니메이션중 움직임 금지
            switch (state)
            {
                case playerState.Attack:
                    deligatePlayerState?.Invoke(playerState.Attack);
                    animator.SetTrigger(_animIDIsAttack);
                    if(_Lock)
                        transform.rotation = Quaternion.LookRotation(LockLookAt);
                  
                    break;
                case playerState.Guard:
                    deligatePlayerState?.Invoke(playerState.Guard);
                    animator.SetTrigger(_animIDIsGuradTrigger);
                    animator.SetBool(_animIDIsGuard, true);
                    if (_Lock)
                        transform.rotation = Quaternion.LookRotation(LockLookAt);

                    Parrying();
                    break;
                case playerState.Dash:
                    deligatePlayerState?.Invoke(playerState.Dash);
                    transform.rotation = Quaternion.LookRotation(dir);
                    ReMove();
                    break;
                case playerState.Jump:
                    deligatePlayerState?.Invoke(playerState.Jump);
                    ReMove();
                    animator.SetBool(_animIDIsJump, true);
                    break;
                case playerState.Hit:
                    deligatePlayerState?.Invoke(playerState.Hit);

                    break;
            }
        }
        else
        {
            ReMove();
        }

    }



    protected virtual void PlayerStateQueueClear()
    {
        inputStateBuffer.Clear();
        inputDirectionBuffer.Clear();
    }

    /// <summary>
    /// 상태에 맞게 현재상태버퍼에 값 넣기 
    /// </summary>
    /// <param name="type">현재 들어온 상태</param>
    /// <param name="dir">들어올 당시의 방향키 방향</param>
    protected virtual void GetPlayerBattle(playerState type, Vector3 dir)
    {
        switch (type)
        {
            case playerState.Attack:
                if (state == playerState.Idle) // 전투 명령이 없는 상태일시
                {
                    PlayerStateEnQueue(type, dir);
                    PlayerStateDeQueue();
                }
                else
                {
                    PlayerStateEnQueue(type, dir);
                }
                Debug.Log("공격");
                break;
            case playerState.Guard:
                if (state == playerState.Idle) // 전투 명령이 없는 상태일시
                {
                    PlayerStateEnQueue(type, dir);
                    PlayerStateDeQueue();
                }
                else
                {
                    PlayerStateQueueClear();        // 플레이어가 동작중이고 다음 행동을 강제로 바꿈
                    PlayerStateEnQueue(type, dir);
                }
                Debug.Log("가드");
                break;
            case playerState.NonGuard:
                deligatePlayerState?.Invoke(playerState.NonGuard);
                animator.SetBool(_animIDIsGuard, false);
                ReMove();
                break;
            case playerState.Dash:
                if (state == playerState.Idle) // 전투 명령이 없는 상태일시
                {
                    PlayerStateEnQueue(type, dir);
                    PlayerStateDeQueue();
                }
                else
                {
                    PlayerStateQueueClear();        // 플레이어가 동작중이고 다음 행동을 강제로 바꿈
                    PlayerStateEnQueue(type, dir);
                }
                Debug.Log("대쉬(미구현)");
                break;
            case playerState.Jump:
                if (state == playerState.Idle) // 전투 명령이 없는 상태일시
                {
                    PlayerStateEnQueue(type, dir);
                    PlayerStateDeQueue();
                }
                else
                {
                    PlayerStateQueueClear();        // 플레이어가 동작중이고 다음 행동을 강제로 바꿈
                    PlayerStateEnQueue(type, dir);
                }
                Debug.Log("점프(미구현)");
                break;
            case playerState.Hit:
                PlayerStateQueueClear();
                PlayerStateEnQueue(type, dir); // 즉각 적인 피드백이 필요 이녀석만을 위한 코드 필요
                Debug.Log("Hit(미구현)");
                break;
            default:
                break;

        }

    }


    /// <summary>
    /// 전투동작이 끝나고 다시 평소 움직이기 
    /// </summary>
    private void ReMove()
    {
        PlayerStateQueueClear();
        state = playerState.Idle;
        ThirdPersonController.currentSpeed = ThirdPersonController.MoveSpeed;
    }

    /// <summary>
    /// 패링 기능 구현 0.1초 저스트가드 그 이후 초기화
    /// </summary>
    private void Parrying()
    {
        currentCoolTime = justParryTime; // 쿨타임 초기화
    }

    
    private void FixedUpdate()
    {
        currentCoolTime -= Time.fixedDeltaTime;
    }
}
