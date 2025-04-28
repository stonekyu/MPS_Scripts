using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class Work : MonoBehaviour
    {
    [SerializeField] Material NonMetal_Material;
    [SerializeField] Material Metal_Material;

    //자재 타입
    private WorkType workType;
    public WorkType WorkType
        {
        get { return workType; }
        set
            {
            //자재 타입 변경 시 색상 변경
            workType = value;
            ChangeColor(workType);
            }
        }

    /// <summary>
    /// 자재 타입별 색상 변경
    /// </summary>
    /// <param name="workType"></param>
    void ChangeColor(WorkType workType)
        {
        transform.TryGetComponent<Renderer>(out Renderer render);
        switch (workType)
            {
            case WorkType.NonMetal:
                render.material = NonMetal_Material;
                break;
            case WorkType.Metal:
                render.material = Metal_Material;
                break;
            case WorkType.None:
                render.material = NonMetal_Material;
                break;
            }
        }
    }
