using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class Gripper : MonoBehaviour
{
    [SerializeField] Transform mc_ud; //���� �̵� [��������]
    [SerializeField] Transform originPos; //���� ���� ������ġ (�ʱ���ġ)
    [SerializeField] Transform mc_fb; //���� �̵� [�Ǹ���] 
    [SerializeField] Transform[] targetPos_fb; // 0:Back, 1:Font
    [SerializeField] Grip grip; //����
    float movSpeed = 0.3f;
    Coroutine moveUDCo;
    Coroutine moveFBCo;


    bool isRunUD = false;
    bool isRunFB = false;

    /// <summary>
    /// ����ʱ�ȭ
    /// </summary>
    public void ResetMc()
    {
        mc_ud.position = originPos.position; //���� ���� ��ġ �ʱ�ȭ
        mc_fb.position = targetPos_fb[0].position; //�Ǹ��� �ʱ�ȭ
        grip.StopAllCoroutines(); //���� �ʱ�ȭ
    }

    /// <summary>
    /// �������� �� ��� ����
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
    /// �׸��� �Ǹ��� ����
    /// </summary>
    /// <param name="isFront"></param>
    public void MoveFB(bool isFront)
    {
        if (isRunFB)
        {
            StopCoroutine(moveFBCo);
            isRunFB = false;
        }

        if (isFront) //�Ǹ��� ����
            moveFBCo = StartCoroutine(MovePosFBCo(mc_fb, targetPos_fb[1], movSpeed));
        else //�Ǹ��� ����
            moveFBCo = StartCoroutine(MovePosFBCo(mc_fb, targetPos_fb[0], movSpeed));
    }

    /// <summary>
    /// �׸��� ����
    /// </summary>
    /// <param name="isPick"></param>
    public void PickPlace(bool isPick)
    {
        if (isPick) //����
        {
            grip.StopAllCoroutines();
            grip.StartCoroutine(grip.PickCo());
        }
        else //����
        {
            grip.StopAllCoroutines();
            grip.StartCoroutine(grip.PlaceCo());
        }
    }

    /// <summary>
    /// ��ü �̵� [�Ÿ��� ���� �ӵ� ����]
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
    /// ��ü �̵�
    /// </summary>
    /// <param name="me"></param>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <param name="isRun"></param>
    /// <returns></returns>
    protected IEnumerator MovePosFBCo(Transform me, Transform target, float speed)
    {
        isRunFB = true;

        // ��ǥ �������� �̵�
        // ������ ������� �Ÿ����
        // ���� �Ÿ� ���̿� ���� ���� [0.00001f] ����
        while (Vector3.SqrMagnitude(me.position - target.position) >= 0.00001f)
            {
            //��ü �̵�
            me.position = Vector3.MoveTowards(me.position, target.position, speed * Time.smoothDeltaTime);
            yield return null;
            }
        //���� �� ���� ��ġ ����
        me.position = target.position;
        isRunFB = false;
    }
}
