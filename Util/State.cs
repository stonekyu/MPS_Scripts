using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    //자재 타입
    public enum WorkType
    {
        Metal = 1,
        NonMetal = 2,
        None
    }

    // 동작 상태
    public enum ExcState
    {
        STOP,
        RESET,
        START,
        None
    }
}
