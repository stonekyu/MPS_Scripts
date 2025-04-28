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
    /// ��� �ʱ�ȭ
    /// </summary>
    public void ResetMc()
    {
        mc.position = targetPos[0].position;
    }

    /// <summary>
    /// �Ǹ��� ����
    /// </summary>
    /// <param name="isGo"></param>
    public void MoveMc(bool isGo)
    {
        //���� ���� ���� ��� ���� ����
        if (isRun)
        {
            StopCoroutine(c_Move);
            isRun = false;
        }

        if (isGo) //����
        {
            if (grip != null) { grip.gameObject.SetActive(true); }
            c_Move = StartCoroutine(MovePosCo(mc, targetPos[1], movSpeed));
        }
        else //����
        {
            if (grip != null) { grip.UnSetGrip(); grip.gameObject.SetActive(false); }
            c_Move = StartCoroutine(MovePosCo(mc, targetPos[0], movSpeed));
        }
    }

    /// <summary>
    /// ��ü �̵�
    /// </summary>
    /// <param name="me"></param>
    /// <param name="target"></param>
    /// <param name="speed"></param>
    /// <param name="isRun"></param>
    /// <returns></returns>
    protected IEnumerator MovePosCo(Transform me, Transform target, float speed)
    {
        isRun = true;

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
        isRun = false;
    }
}
