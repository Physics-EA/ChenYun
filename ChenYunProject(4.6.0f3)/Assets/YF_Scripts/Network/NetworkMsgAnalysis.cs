using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Runtime.InteropServices;



public delegate void AnalysisMsg(SocketMessage sm);
public delegate void ReturnCallback(int cameraId, CMS_GUID guid, bool success);
public delegate void DelPassengerFlow(int channel, int ruleId, int count, string ipFrom);


class NetworkMsgAnalysis
{
    public Dictionary<UInt16, AnalysisMsg> m_MsgMap = new Dictionary<UInt16, AnalysisMsg>();
    public ReturnCallback UserLoginCallback;
    public ReturnCallback OpenCameraCallback;
    public ReturnCallback CloseCameraCallback;
    public ReturnCallback SetPTZCtrlCallback;
    public ReturnCallback SetPresetPosCallback;
    public ReturnCallback GotoPresetPosCallback;
    public DelPassengerFlow PassengerFlowCallback;
    private static NetworkMsgAnalysis instance;
    public static NetworkMsgAnalysis GetInstance()
    {
        if (instance == null)
        {
            instance = new NetworkMsgAnalysis();
        }
        return instance;
    }

    public NetworkMsgAnalysis()
    {
        m_MsgMap.Add((UInt16)(CmdNum.STOC_CLIENTLOGIN_MSG), ReturnUserLogin);//
        m_MsgMap.Add((UInt16)(CmdNum.STOC_GETVIDEODATA_MSG), ReturnCameraList);
        m_MsgMap.Add((UInt16)(CmdNum.STOC_OPENCAMERA_MSG), ReturnOpenCamera);
        m_MsgMap.Add((UInt16)(CmdNum.STOC_CLOSECAMERA_MSG), ReturnCloseCamera);
        m_MsgMap.Add((UInt16)(CmdNum.STOC_SETPREPOSITION_MSG), ReturnSetPresetPosition);
        m_MsgMap.Add((UInt16)(CmdNum.STOC_GOTOPREPOSITION_MSG), ReturnGotoPresetPosition);
        m_MsgMap.Add((UInt16)(CmdNum.STOC_LOGINOUT_MSG), ReturnUserLogoff);
        m_MsgMap.Add((UInt16)(CmdNum.STOC_SETPTZCTRL_MSG), ReturnSetPTZCtrl);
        m_MsgMap.Add((UInt16)(CmdNum.STOC_PASSENGER_FLOW_MSG), ReturnPassengerFlow);
    }

    public void Analysis(SocketMessage sm)
    {
        if (sm == null)
        {
            Logger.Instance.WriteLog("sm结构体为空");
            return;
        }

        if (sm.realHeader == null)
        {
            Logger.Instance.WriteLog("sm.realHeader值为空");
            return;
        }

        UInt16 cmd = sm.realHeader.wType;
        if (m_MsgMap.ContainsKey(cmd))
        {
            AnalysisMsg am = (AnalysisMsg)m_MsgMap[cmd];
            am(sm);
        }
        else
        {
            Logger.Instance.WriteLog("没有对应的Cmd指令 :" + sm.realHeader.ToString());
        }
    }

    void ReturnUserLogin(SocketMessage sm)
    {
        //NetworkClient.Instance;
        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {
            if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
            {
                UserLoginCallback(0, new CMS_GUID(), true);
            }
            else
            {
                UserLoginCallback(0, new CMS_GUID(), false);
            }
        }
        else
        {

        }
    }

    void ReturnUserLogoff(SocketMessage sm)
    {
        //NetworkClient.Instance;
        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {

        }
        else
        {

        }
    }

    void ReturnCameraList(SocketMessage sm)
    {
        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {
            NetworkClient.Instance.cameraList = new List<CAMARE_INFO>();
            int sum = BitConverter.ToInt32(sm.realMsg, Marshal.SizeOf(sm.realHeader));
            for (int i = 0; i < sum; i++)
            {
                CAMARE_INFO info = SocketDataTool.GetStruct<CAMARE_INFO>(sm.realMsg, Marshal.SizeOf(sm.realHeader) + Marshal.SizeOf(typeof(int)) + i * Marshal.SizeOf(typeof(CAMARE_INFO)));
                NetworkClient.Instance.cameraList.Add(info);
            }
        }
    }

    void ReturnOpenCamera(SocketMessage sm)
    {
        STOC_OPENCAMERA info = SocketDataTool.GetStruct<STOC_OPENCAMERA>(sm.realMsg, Marshal.SizeOf(sm.realHeader));
        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {
            OpenCameraCallback(info.cameraId, info.camareGuid, true);
        }
        else
        {
            OpenCameraCallback(info.cameraId, info.camareGuid, false);
        }
    }

    void ReturnCloseCamera(SocketMessage sm)
    {
        STOC_CLOSECAMERA info = SocketDataTool.GetStruct<STOC_CLOSECAMERA>(sm.realMsg, Marshal.SizeOf(sm.realHeader));
        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {
            CloseCameraCallback(info.cameraId, info.camareGuid, true);
        }
        else
        {
            CloseCameraCallback(info.cameraId, info.camareGuid, false);
        }
    }

    void ReturnSetPTZCtrl(SocketMessage sm)
    {
        STOC_SETPTZCTRL info = SocketDataTool.GetStruct<STOC_SETPTZCTRL>(sm.realMsg, Marshal.SizeOf(sm.realHeader));
        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {
            SetPTZCtrlCallback(info.cameraId, info.camareGuid, true);
        }
        else
        {
            SetPTZCtrlCallback(info.cameraId, info.camareGuid, false);
        }
    }

    void ReturnSetPresetPosition(SocketMessage sm)
    {
        STOC_SETPREPOSITION info = SocketDataTool.GetStruct<STOC_SETPREPOSITION>(sm.realMsg, Marshal.SizeOf(sm.realHeader));
        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {
            SetPresetPosCallback(info.cameraId, info.camareGuid, true);
        }
        else
        {
            SetPresetPosCallback(info.cameraId, info.camareGuid, false);
        }
    }

    void ReturnGotoPresetPosition(SocketMessage sm)
    {
        STOC_GOTOPREPOSITION info = SocketDataTool.GetStruct<STOC_GOTOPREPOSITION>(sm.realMsg, Marshal.SizeOf(sm.realHeader));
        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {
            GotoPresetPosCallback(info.cameraId, info.camareGuid, true);
        }
        else
        {
            GotoPresetPosCallback(info.cameraId, info.camareGuid, false);
        }
    }

    void ReturnPassengerFlow(SocketMessage sm)
    {
        STOC_PASSENGER_FLOW info = SocketDataTool.GetStruct<STOC_PASSENGER_FLOW>(sm.realMsg, Marshal.SizeOf(sm.realHeader));

        if (sm.realHeader.nError == (UInt16)GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS)
        {
            if (PassengerFlowCallback != null)

                PassengerFlowCallback(info.channel, info.RuleId, info.count, System.Text.Encoding.Default.GetString(Trim(info.ipFrom)));
        }
    }

    byte[] Trim(byte[] src)
    {
        byte[] newByte;
        for (int i = src.Length - 1; i >= 0; i--)
        {
            if (src[i] == 0)
            {
                continue;
            }
            newByte = new byte[i + 1];
            Array.Copy(src, newByte, i + 1);
            return newByte;
        }
        return new byte[0];
    }
}
