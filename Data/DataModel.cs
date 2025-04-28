using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class DataModel : MonoBehaviour
    {
    public ExcState excState; //실행 상태 Stop/Reset/Start

    [Header("Machine Data")]
    public bool drillMotor; //가공 모터
    public bool conveyorMotor; //컨베이어 모터
    public bool supplyCylinder; //공급 실린더
    public bool distributeCylinder; //분배 실린더 
    public bool drillCylinder; //가공 실린더
    public bool removeCylinder; //배출 실린더 , removeCylinder
    public bool stopperCylinder; //정지 실린더
    public bool storeCylinder; //창고 실린더
    public bool gripCylinder_FB; //흡착 실린더
    public float gripCylider_Servo; //흡착 서보모터 값, 주의: 모터 수치에따라 위치 바뀜
    public bool vacuumGrip; //흡착 여부

    [Header("Sensor Data")]
    public bool sensorSupply; //공급 센서
    public bool sensorDistribute; //분배 센서
    public bool sensorStopper; //정지 센서
    public bool sensorWorkChk; //용량 센서
    public bool sensorMetalChk; //금속 센서
    public int sensorMetalrslt; //자재 결과값 : 0 / 1 / 2
    public bool IsConnected; //연결 상태

    public DataModel()
        {
        ResetData();
        excState = ExcState.None;
        IsConnected = false;
        }

    /// <summary>
    /// 데이터 초기화
    /// </summary>
    public void ResetData()
        {
        excState = ExcState.RESET;
        drillMotor = false;
        conveyorMotor = false;
        supplyCylinder = false;
        distributeCylinder = false;
        drillCylinder = false;
        removeCylinder = false;
        stopperCylinder = false;
        storeCylinder = false;
        gripCylinder_FB = false;
        gripCylider_Servo = 0;
        vacuumGrip = false;

        sensorSupply = false;
        sensorDistribute = false;
        sensorStopper = false;
        sensorWorkChk = false;
        sensorMetalChk = false;
        sensorMetalrslt = 0;
        }
    }
