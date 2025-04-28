using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class Gripper : MonoBehaviour
{
    [SerializeField] Transform mc_ud; //수직 이동 [서보모터]
    [SerializeField] Transform originPos; //서보 모터 기준위치 (초기위치)
    [SerializeField] Transform mc_fb; //수평 이동 [실린더] 
    [SerializeField] Transform[] targetPos_fb; // 0:Back, 1:Font
    [SerializeField] Grip grip; //흡착
    float movSpeed = 0.3f;
    Coroutine moveUDCo;
    Coroutine moveFBCo;


    bool isRunUD = false;
    bool isRunFB = false;

    /// <summary>
    /// 기계초기화
    /// </summary>
    public void ResetMc()
    {
        mc_ud.position = originPos.position; //서보 모터 위치 초기화
        mc_fb.position = targetPos_fb[0].position; //실린더 초기화
        grip.StopAllCoroutines(); //흡착 초기화
    }

    /// <summary>
    /// 서보모터 값 방식 동작
    /// </summary>
    /// <param name="servo"></param>
    public void MoveUD_Servo(float servo)
    {
        if (isRunUD)
        {
            StopCoroutine(moveUDCo);
            isRunUD = false;
        }
        Vector3 targetPos = originPos.position + Vector3.down * servo * 0.001f;
        moveUDCo = StartCoroutine(MovePosUDCo(mc_ud, targetPos, 0.52f));
    }

    /// <summary>
    /// 그리퍼 실린더 동작
    /// </summary>
    /// <param name="isFront"></param>
    public void MoveFB(bool isFront)
    {
        if (isRunFB)
        {
            StopCoroutine(moveFBCo);
            isRunFB = false;
        }

        if (isFront) //실린더 전진
            moveFBCo = StartCoroutine(MovePosFBCo(mc_fb, targetPos_fb[1], movSpeed));
        else //실린더 후진
            moveFBCo = StartCoroutine(MovePosFBCo(mc_fb, targetPos_fb[0], movSpeed));
    }

    /// <summary>
    /// 그리퍼 흡착
    /// </summary>
    /// <param name="isPick"></param>
    public void PickPlace(bool isPick)
    {
        if (isPick) //집기
        {
            grip.StopAllCoroutines();
            grip.StartCoroutine(grip.PickCo());
        }
        else //놓기
        {
            grip.StopAllCoroutines();
            grip.StartCoroutine(grip.PlaceCo());
        }
    }

    /// <summary>
    /// 물체 이동 [거리에 따른 속도 조절]
    /// </summary>
    /// <param name="me"></param>
    /// <param name="target"></param>
    /// <param name="time"></param>
    /// <param name="isRun"></param>
    /// <returns></returns>
    private IEnumerator MovePosUDCo(Transform me, Vector3 target, float time)
    {
        isRunUD = true;

        while (Vector3.SqrMagnitude(me.position - target) >= 0.0001f)
        {
            float speed = Vector3.Distance(me.position, target) / time;
            me.position = Vector3.MoveTowards(me.position, target, speed * Time.smoothDeltaTime);
            yield return null;
        }

        me.position = target;
        isRunUD = false;

        yield break;
    }

    /// <summary>
    /// 물체 이동
    /// </summary>
    /// <param name="me"></param>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <param name="isRun"></param>
    /// <returns></returns>
    protected IEnumerator MovePosFBCo(Transform me, Transform target, float speed)
    {
        isRunFB = true;

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
        isRunFB = false;
    }
}
