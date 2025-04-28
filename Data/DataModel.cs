using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class DataModel : MonoBehaviour
    {
    public ExcState excState; //���� ���� Stop/Reset/Start

    [Header("Machine Data")]
    public bool drillMotor; //���� ����
    public bool conveyorMotor; //�����̾� ����
    public bool supplyCylinder; //���� �Ǹ���
    public bool distributeCylinder; //�й� �Ǹ��� 
    public bool drillCylinder; //���� �Ǹ���
    public bool removeCylinder; //���� �Ǹ��� , removeCylinder
    public bool stopperCylinder; //���� �Ǹ���
    public bool storeCylinder; //â�� �Ǹ���
    public bool gripCylinder_FB; //���� �Ǹ���
    public float gripCylider_Servo; //���� �������� ��, ����: ���� ��ġ������ ��ġ �ٲ�
    public bool vacuumGrip; //���� ����

    [Header("Sensor Data")]
    public bool sensorSupply; //���� ����
    public bool sensorDistribute; //�й� ����
    public bool sensorStopper; //���� ����
    public bool sensorWorkChk; //�뷮 ����
    public bool sensorMetalChk; //�ݼ� ����
    public int sensorMetalrslt; //���� ����� : 0 / 1 / 2
    public bool IsConnected; //���� ����

    public DataModel()
        {
        ResetData();
        excState = ExcState.None;
        IsConnected = false;
        }

    /// <summary>
    /// ������ �ʱ�ȭ
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
