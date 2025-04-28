using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Action<GameObject> onFinishWork; //�������� �̺�Ʈ
    WaitForSeconds wait = new WaitForSeconds(1.5f);

    private void OnTriggerEnter(Collider other)
    {
        //��ũ ���� �ν�
        if (other.CompareTag("Work"))
        {
            if (onFinishWork != null)
            {
                StartCoroutine(WaitForFinishWork(other.gameObject));
            }
        }
    }

    /// <summary>
    /// ��� �� ���� �̺�Ʈ ����
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    IEnumerator WaitForFinishWork(GameObject obj)
    {
        yield return wait;

        onFinishWork(obj);
    }
}
