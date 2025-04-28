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
    /// ������ ��
    /// </summary>
    public void Set()
        {
        //������ �� Null üũ
        if (!TryGetComponent(out Data))
            Data = gameObject.AddComponent<DataModel>();

        //�˻� ����
        if (!isSearching)
            {
            //'Mx Component ������ �б�' ������ ���� �� ����
            isSearching = true;
            thMxLoad = new Thread(SearchPLCData);
            thMxLoad.IsBackground = true;
            thMxLoad.Start();
            }
        }

    /// <summary>
    /// MxComponent ������ �б�
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
    /// MxComponent ��ü ���� ������ �б�
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
        //���� ����
        if (ReadDataOnce(mxConn, "X0F") == 1) Data.sensorSupply = true;
        else Data.sensorSupply = false;
        //�й� ����
        if (ReadDataOnce(mxConn, "X10") == 1) Data.sensorDistribute = true;
        else Data.sensorDistribute = false;
        //�뷮 ����
        if (ReadDataOnce(mxConn, "X11") == 1) Data.sensorWorkChk = true;
        else Data.sensorWorkChk = false;
        //���� ����
        if (ReadDataOnce(mxConn, "X12") == 1) Data.sensorMetalrslt = 1;
        else Data.sensorMetalrslt = 2;
        //���� ����
        if (ReadDataOnce(mxConn, "X13") == 1) Data.sensorStopper = true;
        else Data.sensorStopper = false;

        if (ReadDataOnce(mxConn, "B0") == 1) Data.sensorMetalChk = true;
        else Data.sensorMetalChk = false;

        #region Input
        //�����̾�
        if (ReadDataOnce(mxConn, "Y21") == 1) Data.conveyorMotor = true;
        else Data.conveyorMotor = false;
        //���� �Ǹ���
        if (ReadDataOnce(mxConn, "Y22") == 1) Data.supplyCylinder = true;
        if (ReadDataOnce(mxConn, "Y23") == 1) Data.supplyCylinder = false;
        //�й� �Ǹ���
        if (ReadDataOnce(mxConn, "Y24") == 1) Data.distributeCylinder = true;
        if (ReadDataOnce(mxConn, "Y25") == 1) Data.distributeCylinder = false;
        //���� �Ǹ���
        if (ReadDataOnce(mxConn, "Y26") == 1) Data.drillCylinder = true;
        else Data.drillCylinder = false;
        //���� �Ǹ���
        if (ReadDataOnce(mxConn, "Y27") == 1) Data.removeCylinder = true;
        else Data.removeCylinder = false;

        if (ReadDataOnce(mxConn, "X08") == 1) Data.stopperCylinder = true;
        if (ReadDataOnce(mxConn, "X09") == 1) Data.stopperCylinder = false;
        //���� �Ǹ���
        if (ReadDataOnce(mxConn, "Y2A") == 1) Data.gripCylinder_FB = true;
        if (ReadDataOnce(mxConn, "Y2B") == 1) Data.gripCylinder_FB = false;
        //���� �Ǹ���
        if (ReadDataOnce(mxConn, "Y2C") == 1) Data.storeCylinder = true;
        if (ReadDataOnce(mxConn, "Y2D") == 1) Data.storeCylinder = false;
        //����
        if (ReadDataOnce(mxConn, "Y2E") == 1) Data.vacuumGrip = true;
        else Data.vacuumGrip = false;
        //��������
        Data.gripCylider_Servo = (float)ReadDataOnce(mxConn, "D2000");
        #endregion

        //MPS ����
        if (ReadDataOnce(mxConn, "D9600.A") == 1) Data.excState = State.ExcState.START;
        if (ReadDataOnce(mxConn, "D9600.B") == 1) Data.excState = State.ExcState.STOP;
        if (ReadDataOnce(mxConn, "D9600.C") == 1) Data.excState = State.ExcState.RESET;

        mxConn.Close();
        }

    /// <summary>
    /// MxComponent ������ �б�
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
    /// MxComponent ������ �б� ����
    /// </summary>
    protected void StopSearch()
        {
        try
            {
            //�˻� ������ ����
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
