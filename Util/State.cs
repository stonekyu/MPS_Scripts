using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    //���� Ÿ��
    public enum WorkType
    {
        Metal = 1,
        NonMetal = 2,
        None
    }

    // ���� ����
    public enum ExcState
    {
        STOP,
        RESET,
        START,
        None
    }
}
