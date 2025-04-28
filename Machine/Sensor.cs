using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
    {
    Work sensoredWork;
    [SerializeField] WorkManagerDt workManager;

    /// <summary>
    /// 인식 워크 전달
    /// </summary>
    /// <returns></returns>
    public Work GetWork()
        {
        return sensoredWork;
        }
    /// <summary>
    /// 워크 자재 선택 지정
    /// </summary>
    /// <param name="workType"></param>
    public void SetMaterial(State.WorkType workType)
        {
        if (sensoredWork == null)
            return;
        sensoredWork.WorkType = workType;
        }

    /// <summary>
    /// 워크 인식
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
