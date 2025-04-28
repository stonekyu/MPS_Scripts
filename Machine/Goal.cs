using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Action<GameObject> onFinishWork; //도착종료 이벤트
    WaitForSeconds wait = new WaitForSeconds(1.5f);

    private void OnTriggerEnter(Collider other)
    {
        //워크 자재 인식
        if (other.CompareTag("Work"))
        {
            if (onFinishWork != null)
            {
                StartCoroutine(WaitForFinishWork(other.gameObject));
            }
        }
    }

    /// <summary>
    /// 대기 후 도착 이벤트 실행
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    IEnumerator WaitForFinishWork(GameObject obj)
    {
        yield return wait;

        onFinishWork(obj);
    }
}
