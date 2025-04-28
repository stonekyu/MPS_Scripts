using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGrip : MonoBehaviour
{
    /// <summary>
    /// �ڽ� ��ü ����
    /// �Ǹ����� ���� �� ����
    /// </summary>
    public void UnSetGrip()
    {
        // ���� �ڽ� ��ü ����
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
        // �Ǹ��� ���� �� ��ũ�� �ڽ����� ���� ����
        if (other.CompareTag("Work"))
        {
            other.transform.SetParent(transform);
        }
    }
}
