using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class DigitalTwin : MonoBehaviour
    {
    DataModel dtData;
    [SerializeField] MxConnection mxCon;
    DataModel mxData;

    [SerializeField] Cylinder supplyCylinder; //공급 실린더
    [SerializeField] Cylinder distributeCylinder; //분배 실린더
    [SerializeField] Cylinder drillCylinder; //가공 실린더
    [SerializeField] Conveyor conveyor; //컨베이어
    [SerializeField] Gripper gripper; //그리퍼
    [SerializeField] Sensor sensorWork; //워크 인식 센서
    [SerializeField] Cylinder StopperCylinder; //정지 실린더
    [SerializeField] Cylinder storeCylinder; //창고 실린더
    [SerializeField] Cylinder removeCylinder; //배출 실린더
    [SerializeField] WorkManagerDt workManager; //워크 관리

    private void Awake()
        {
        //데이터 모델 Null 체크
        if (!TryGetComponent(out dtData))
            {
            dtData = gameObject.AddComponent<DataModel>();
            }
        }

    private void Start()
        {
        mxData = mxCon.Data; //MxComponent 데이터 파싱
        }

    private void Update()
        {
        //MxComponent 데이터 Null 체크
        if (mxData == null)
            {
            mxData = mxCon.Data;
            return;
            }

        //실행 상태 업데이트
        if (dtData.excState != mxData.excState)
            {
            dtData.excState = mxData.excState;
            //Reset 시 DT도 Reset 시킴
            if (dtData.excState == State.ExcState.RESET) ResetDt();
            }
        //공급 센서 데이터 업데이트
        if (dtData.sensorSupply != mxData.sensorSupply)
            { dtData.sensorSupply = mxData.sensorSupply; }
        //컨베이어 모터 동작 업데이트
        if (dtData.conveyorMotor != mxData.conveyorMotor)
            { dtData.conveyorMotor = mxData.conveyorMotor; conveyor.isConveyRun = dtData.conveyorMotor; }
        //공급 실린더 동작 업데이트 
        if (dtData.supplyCylinder != mxData.supplyCylinder)
            {
            dtData.supplyCylinder = mxData.supplyCylinder;
            supplyCylinder.MoveMc(dtData.supplyCylinder);
            if (dtData.sensorSupply && dtData.supplyCylinder) workManager.SpawnWork();
            }
        //분배 실린더 동작 업데이트
        if (dtData.distributeCylinder != mxData.distributeCylinder)
            { dtData.distributeCylinder = mxData.distributeCylinder; distributeCylinder.MoveMc(dtData.distributeCylinder); }
        //가공 실린더 동작 업데이트
        if (dtData.drillCylinder != mxData.drillCylinder)
            { dtData.drillCylinder = mxData.drillCylinder; drillCylinder.MoveMc(dtData.drillCylinder); }
        //배출 실린더 동작 업데이트
        if (dtData.removeCylinder != mxData.removeCylinder)
            { dtData.removeCylinder = mxData.removeCylinder; removeCylinder.MoveMc(dtData.removeCylinder); }
        //스토퍼 실린더 동작 업데이트
        if (dtData.stopperCylinder != mxData.stopperCylinder)
            {
            dtData.stopperCylinder = mxData.stopperCylinder;
            StopperCylinder.MoveMc(dtData.stopperCylinder);
            }
        //그리퍼 실린더 동작 업데이트
        if (dtData.gripCylinder_FB != mxData.gripCylinder_FB)
            { dtData.gripCylinder_FB = mxData.gripCylinder_FB; gripper.MoveFB(dtData.gripCylinder_FB); }

        //그리퍼 서보 모터 동작 업데이트
        if (dtData.gripCylider_Servo != mxData.gripCylider_Servo)
            { dtData.gripCylider_Servo = mxData.gripCylider_Servo; gripper.MoveUD_Servo(dtData.gripCylider_Servo); }

        //흡착 동작 업데이트
        if (dtData.vacuumGrip != mxData.vacuumGrip)
            { dtData.vacuumGrip = mxData.vacuumGrip; gripper.PickPlace(dtData.vacuumGrip); }

        //창고 실린더 동작 업데이트
        if (dtData.storeCylinder != mxData.storeCylinder)
            { dtData.storeCylinder = mxData.storeCylinder; storeCylinder.MoveMc(dtData.storeCylinder); }

        if (dtData.sensorMetalChk != mxData.sensorMetalChk)
            {
            dtData.sensorMetalChk = mxData.sensorMetalChk;
            if (dtData.sensorMetalChk) sensorWork.SetMaterial(WorkType.Metal);
            }
        }

    /// <summary>
    /// 기계 초기화
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
