using ActUtlType64Lib;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class MxConnection : MonoBehaviour
    {
    public DataModel Data;
    Thread thMxLoad;
    [SerializeField] int stationNumber = 0;
    bool isSearching;

    private void Start()
        {
        Set();
        }

    /// <summary>
    /// 데이터 셋
    /// </summary>
    public void Set()
        {
        //데이터 모델 Null 체크
        if (!TryGetComponent(out Data))
            Data = gameObject.AddComponent<DataModel>();

        //검색 시작
        if (!isSearching)
            {
            //'Mx Component 데이터 읽기' 쓰레드 생성 및 동작
            isSearching = true;
            thMxLoad = new Thread(SearchPLCData);
            thMxLoad.IsBackground = true;
            thMxLoad.Start();
            }
        }

    /// <summary>
    /// MxComponent 데이터 읽기
    /// </summary>
    private void SearchPLCData()
        {
        try
            {
            while (isSearching)
                {
                ReadDataALL();

                Thread.Sleep(100);
                }
            }
        catch (System.Exception ex)
            {
            Debug.Log(ex);
            }
        }

    /// <summary>
    /// MxComponent 전체 동작 데이터 읽기
    /// </summary>
    private void ReadDataALL()
        {
        ActUtlType64 mxConn;
        mxConn = new ActUtlType64();
        mxConn.ActLogicalStationNumber = stationNumber;

        if (mxConn.Open() != 0)
            {
            Debug.Log("Disconneting");
            mxConn = null;
            Data.IsConnected = false;
            return;
            }

        Data.IsConnected = true;
        //공급 센서
        if (ReadDataOnce(mxConn, "X0F") == 1) Data.sensorSupply = true;
        else Data.sensorSupply = false;
        //분배 센서
        if (ReadDataOnce(mxConn, "X10") == 1) Data.sensorDistribute = true;
        else Data.sensorDistribute = false;
        //용량 센서
        if (ReadDataOnce(mxConn, "X11") == 1) Data.sensorWorkChk = true;
        else Data.sensorWorkChk = false;
        //유도 센서
        if (ReadDataOnce(mxConn, "X12") == 1) Data.sensorMetalrslt = 1;
        else Data.sensorMetalrslt = 2;
        //정지 센서
        if (ReadDataOnce(mxConn, "X13") == 1) Data.sensorStopper = true;
        else Data.sensorStopper = false;

        if (ReadDataOnce(mxConn, "B0") == 1) Data.sensorMetalChk = true;
        else Data.sensorMetalChk = false;

        #region Input
        //컨베이어
        if (ReadDataOnce(mxConn, "Y21") == 1) Data.conveyorMotor = true;
        else Data.conveyorMotor = false;
        //공급 실린더
        if (ReadDataOnce(mxConn, "Y22") == 1) Data.supplyCylinder = true;
        if (ReadDataOnce(mxConn, "Y23") == 1) Data.supplyCylinder = false;
        //분배 실린더
        if (ReadDataOnce(mxConn, "Y24") == 1) Data.distributeCylinder = true;
        if (ReadDataOnce(mxConn, "Y25") == 1) Data.distributeCylinder = false;
        //가공 실린더
        if (ReadDataOnce(mxConn, "Y26") == 1) Data.drillCylinder = true;
        else Data.drillCylinder = false;
        //배출 실린더
        if (ReadDataOnce(mxConn, "Y27") == 1) Data.removeCylinder = true;
        else Data.removeCylinder = false;

        if (ReadDataOnce(mxConn, "X08") == 1) Data.stopperCylinder = true;
        if (ReadDataOnce(mxConn, "X09") == 1) Data.stopperCylinder = false;
        //흡착 실린더
        if (ReadDataOnce(mxConn, "Y2A") == 1) Data.gripCylinder_FB = true;
        if (ReadDataOnce(mxConn, "Y2B") == 1) Data.gripCylinder_FB = false;
        //저장 실린더
        if (ReadDataOnce(mxConn, "Y2C") == 1) Data.storeCylinder = true;
        if (ReadDataOnce(mxConn, "Y2D") == 1) Data.storeCylinder = false;
        //흡착
        if (ReadDataOnce(mxConn, "Y2E") == 1) Data.vacuumGrip = true;
        else Data.vacuumGrip = false;
        //서보모터
        Data.gripCylider_Servo = (float)ReadDataOnce(mxConn, "D2000");
        #endregion

        //MPS 상태
        if (ReadDataOnce(mxConn, "D9600.A") == 1) Data.excState = State.ExcState.START;
        if (ReadDataOnce(mxConn, "D9600.B") == 1) Data.excState = State.ExcState.STOP;
        if (ReadDataOnce(mxConn, "D9600.C") == 1) Data.excState = State.ExcState.RESET;

        mxConn.Close();
        }

    /// <summary>
    /// MxComponent 데이터 읽기
    /// </summary>
    /// <param name="mxConn"></param>
    /// <param name="device"></param>
    /// <returns></returns>
    private short ReadDataOnce(ActUtlType64 mxConn, string device)
        {
        int iReturnCode;
        short value;
        //iReturnCode = mxConn.ReadDeviceRandom2(device, 1, out value);
        iReturnCode = mxConn.GetDevice2(device, out value);
        return value;
        }

    /// <summary>
    /// MxComponent 데이터 읽기 종료
    /// </summary>
    protected void StopSearch()
        {
        try
            {
            //검색 쓰레드 해제
            isSearching = false;
            thMxLoad.Join();
            thMxLoad.Interrupt();
            }
        catch (System.Exception ex)
            {
            Debug.Log(ex);
            }
        }

    protected new void OnApplicationQuit()
        {
        if (isSearching)
            {
            StopSearch();
            }
        }
    }
