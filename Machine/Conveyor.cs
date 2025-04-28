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
    /// 기계 초기화
    /// </summary>
    public void ResetMc()
    {
        isConveyRun = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isConveyRun) return; //컨베이어 동작여부 구분

        //컨베이어 위 워크 이동
        if (other.CompareTag("Work"))
        {
            other.transform.localPosition += Vector3.right * Time.deltaTime * movSpeed;
        }
    }
}
