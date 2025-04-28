using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class Cylinder : MonoBehaviour
{
    [SerializeField] Transform mc;
    [SerializeField] Transform[] targetPos; 
    [SerializeField] float movSpeed = 0.2f;
    [SerializeField] AutoGrip grip;
    Coroutine c_Move;
    bool isRun = false;

    /// <summary>
    /// 기계 초기화
    /// </summary>
    public void ResetMc()
    {
        mc.position = targetPos[0].position;
    }

    /// <summary>
    /// 실린더 동작
    /// </summary>
    /// <param name="isGo"></param>
    public void MoveMc(bool isGo)
    {
        //현재 동작 중일 경우 동작 종료
        if (isRun)
        {
            StopCoroutine(c_Move);
            isRun = false;
        }

        if (isGo) //전진
        {
            if (grip != null) { grip.gameObject.SetActive(true); }
            c_Move = StartCoroutine(MovePosCo(mc, targetPos[1], movSpeed));
        }
        else //후진
        {
            if (grip != null) { grip.UnSetGrip(); grip.gameObject.SetActive(false); }
            c_Move = StartCoroutine(MovePosCo(mc, targetPos[0], movSpeed));
        }
    }

    /// <summary>
    /// 물체 이동
    /// </summary>
    /// <param name="me"></param>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <param name="isRun"></param>
    /// <returns></returns>
    protected IEnumerator MovePosCo(Transform me, Transform target, float speed)
    {
        isRun = true;

        // 목표 도착까지 이동
        // 제곱근 계산으로 거리계산
        // 기준 거리 차이에 따른 공차 [0.00001f] 조절
        while (Vector3.SqrMagnitude(me.position - target.position) >= 0.00001f)
        {
            //물체 이동
            me.position = Vector3.MoveTowards(me.position, target.position, speed * Time.smoothDeltaTime);
            yield return null;
        }
        //도착 후 최종 위치 조정
        me.position = target.position;
        isRun = false;
    }
}
