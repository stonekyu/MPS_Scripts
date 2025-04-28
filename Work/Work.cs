using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class Work : MonoBehaviour
    {
    [SerializeField] Material NonMetal_Material;
    [SerializeField] Material Metal_Material;

    //���� Ÿ��
    private WorkType workType;
    public WorkType WorkType
        {
        get { return workType; }
        set
            {
            //���� Ÿ�� ���� �� ���� ����
            workType = value;
            ChangeColor(workType);
            }
        }

    /// <summary>
    /// ���� Ÿ�Ժ� ���� ����
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
