using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class Conveyor : MonoBehaviour
{
  //  Work work;
    float movSpeed = 0.11f;
    public bool isConveyRun;

    /// <summary>
    /// ��� �ʱ�ȭ
    /// </summary>
    public void ResetMc()
    {
        isConveyRun = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isConveyRun) return; //�����̾� ���ۿ��� ����

        //�����̾� �� ��ũ �̵�
        if (other.CompareTag("Work"))
        {
            other.transform.localPosition += Vector3.right * Time.deltaTime * movSpeed;
        }
    }
}
