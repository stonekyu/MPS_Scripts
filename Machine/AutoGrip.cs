using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGrip : MonoBehaviour
{
    /// <summary>
    /// 자식 객체 해제
    /// 실린더가 후진 시 실행
    /// </summary>
    public void UnSetGrip()
    {
        // 하위 자식 객체 해제
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).SetParent(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 실린더 전진 시 워크를 자식으로 물고 전진
        if (other.CompareTag("Work"))
        {
            other.transform.SetParent(transform);
        }
    }
}
