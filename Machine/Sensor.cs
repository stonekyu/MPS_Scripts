using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
    {
    Work sensoredWork;
    [SerializeField] WorkManagerDt workManager;

    /// <summary>
    /// �ν� ��ũ ����
    /// </summary>
    /// <returns></returns>
    public Work GetWork()
        {
        return sensoredWork;
        }
    /// <summary>
    /// ��ũ ���� ���� ����
    /// </summary>
    /// <param name="workType"></param>
    public void SetMaterial(State.WorkType workType)
        {
        if (sensoredWork == null)
            return;
        sensoredWork.WorkType = workType;
        }

    /// <summary>
    /// ��ũ �ν�
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
        {
        if (other.CompareTag("Work") && other.TryGetComponent(out Work work))
            {
            this.sensoredWork = work;
            }
        }
    }
