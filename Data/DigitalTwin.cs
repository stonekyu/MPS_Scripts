using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class DigitalTwin : MonoBehaviour
    {
    DataModel dtData;
    [SerializeField] MxConnection mxCon;
    DataModel mxData;

    [SerializeField] Cylinder supplyCylinder; //���� �Ǹ���
    [SerializeField] Cylinder distributeCylinder; //�й� �Ǹ���
    [SerializeField] Cylinder drillCylinder; //���� �Ǹ���
    [SerializeField] Conveyor conveyor; //�����̾�
    [SerializeField] Gripper gripper; //�׸���
    [SerializeField] Sensor sensorWork; //��ũ �ν� ����
    [SerializeField] Cylinder StopperCylinder; //���� �Ǹ���
    [SerializeField] Cylinder storeCylinder; //â�� �Ǹ���
    [SerializeField] Cylinder removeCylinder; //���� �Ǹ���
    [SerializeField] WorkManagerDt workManager; //��ũ ����

    private void Awake()
        {
        //������ �� Null üũ
        if (!TryGetComponent(out dtData))
            {
            dtData = gameObject.AddComponent<DataModel>();
            }
        }

    private void Start()
        {
        mxData = mxCon.Data; //MxComponent ������ �Ľ�
        }

    private void Update()
        {
        //MxComponent ������ Null üũ
        if (mxData == null)
            {
            mxData = mxCon.Data;
            return;
            }

        //���� ���� ������Ʈ
        if (dtData.excState != mxData.excState)
            {
            dtData.excState = mxData.excState;
            //Reset �� DT�� Reset ��Ŵ
            if (dtData.excState == State.ExcState.RESET) ResetDt();
            }
        //���� ���� ������ ������Ʈ
        if (dtData.sensorSupply != mxData.sensorSupply)
            { dtData.sensorSupply = mxData.sensorSupply; }
        //�����̾� ���� ���� ������Ʈ
        if (dtData.conveyorMotor != mxData.conveyorMotor)
            { dtData.conveyorMotor = mxData.conveyorMotor; conveyor.isConveyRun = dtData.conveyorMotor; }
        //���� �Ǹ��� ���� ������Ʈ 
        if (dtData.supplyCylinder != mxData.supplyCylinder)
            {
            dtData.supplyCylinder = mxData.supplyCylinder;
            supplyCylinder.MoveMc(dtData.supplyCylinder);
            if (dtData.sensorSupply && dtData.supplyCylinder) workManager.SpawnWork();
            }
        //�й� �Ǹ��� ���� ������Ʈ
        if (dtData.distributeCylinder != mxData.distributeCylinder)
            { dtData.distributeCylinder = mxData.distributeCylinder; distributeCylinder.MoveMc(dtData.distributeCylinder); }
        //���� �Ǹ��� ���� ������Ʈ
        if (dtData.drillCylinder != mxData.drillCylinder)
            { dtData.drillCylinder = mxData.drillCylinder; drillCylinder.MoveMc(dtData.drillCylinder); }
        //���� �Ǹ��� ���� ������Ʈ
        if (dtData.removeCylinder != mxData.removeCylinder)
            { dtData.removeCylinder = mxData.removeCylinder; removeCylinder.MoveMc(dtData.removeCylinder); }
        //������ �Ǹ��� ���� ������Ʈ
        if (dtData.stopperCylinder != mxData.stopperCylinder)
            {
            dtData.stopperCylinder = mxData.stopperCylinder;
            StopperCylinder.MoveMc(dtData.stopperCylinder);
            }
        //�׸��� �Ǹ��� ���� ������Ʈ
        if (dtData.gripCylinder_FB != mxData.gripCylinder_FB)
            { dtData.gripCylinder_FB = mxData.gripCylinder_FB; gripper.MoveFB(dtData.gripCylinder_FB); }

        //�׸��� ���� ���� ���� ������Ʈ
        if (dtData.gripCylider_Servo != mxData.gripCylider_Servo)
            { dtData.gripCylider_Servo = mxData.gripCylider_Servo; gripper.MoveUD_Servo(dtData.gripCylider_Servo); }

        //���� ���� ������Ʈ
        if (dtData.vacuumGrip != mxData.vacuumGrip)
            { dtData.vacuumGrip = mxData.vacuumGrip; gripper.PickPlace(dtData.vacuumGrip); }

        //â�� �Ǹ��� ���� ������Ʈ
        if (dtData.storeCylinder != mxData.storeCylinder)
            { dtData.storeCylinder = mxData.storeCylinder; storeCylinder.MoveMc(dtData.storeCylinder); }

        if (dtData.sensorMetalChk != mxData.sensorMetalChk)
            {
            dtData.sensorMetalChk = mxData.sensorMetalChk;
            if (dtData.sensorMetalChk) sensorWork.SetMaterial(WorkType.Metal);
            }
        }

    /// <summary>
    /// ��� �ʱ�ȭ
    /// </summary>
    public void ResetDt()
        {
        mxData.ResetData();
        supplyCylinder.ResetMc();
        distributeCylinder.ResetMc();
        drillCylinder.ResetMc();
        conveyor.ResetMc();
        gripper.ResetMc();
        StopperCylinder.ResetMc();
        storeCylinder.ResetMc();
        removeCylinder.ResetMc();
        workManager.ResetWorks();
        }
    }
