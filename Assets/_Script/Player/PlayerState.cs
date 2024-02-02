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
}
