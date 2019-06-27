using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;



public class NetworkClient
{


    //由于本系统必须和服务器部署在一台机器上，所以一定是 127.0.0.1 端口为2900 暂不可配置，正式部署要改为可配置的
    private string ip = "127.0.0.1";
    private int port = 2900;
    public bool isConnect = false;
    public bool reconnect = false;
    public const int MAX_BUFFER_SIZE = 64 * 1024 * 2;

    //通讯用的套接字
    private Socket m_socket;

    [System.NonSerialized]
    public byte[] m_buffer = new byte[MAX_BUFFER_SIZE];

    /// <summary>
    /// 缓存接收到的消息
    /// </summary>
    public List<SocketMessage> m_messages = new List<SocketMessage>();

    public List<CAMARE_INFO> cameraList;
    /// <summary>
    /// 接受buffer的偏移，表示上一条未接受完的包的已接收大小
    /// </summary>
    private int m_lastFragmentDataLen = 0;
    private int m_iHeaderLen = (int)Marshal.SizeOf(typeof(ALLST_GENERIC));
    private static NetworkClient instance;
    public static NetworkClient Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetworkClient();
            }
            return instance;
        }
    }

    private void HandleMessages(SocketMessage sm)
    {
        NetworkMsgAnalysis.GetInstance().Analysis(sm);
    }

    public void NetUpdate()
    {
        if (isConnect)
        {
            if (m_messages.Count > 0)
            {
                while (m_messages.Count > 0)
                {
                    HandleMessages(m_messages[0]);
                    m_messages.RemoveAt(0);
                }
            }
        }
        else
        {
            Logger.Instance.WriteLog("与服务器断开连接");
        }
    }

    public void Connect()
    {
        try
        {
            CloseConnect();
            //创建socket
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //链接服务器
            m_socket.Connect(ip, port);
            Logger.Instance.WriteLog("连接服务器成功");
            isConnect = true;

            m_socket.BeginReceive(m_buffer, m_lastFragmentDataLen, MAX_BUFFER_SIZE - m_lastFragmentDataLen, SocketFlags.None, ReceiveCallBack, null);
        }
        catch (Exception e)
        {
            isConnect = false;
            Logger.Instance.WriteLog("连接服务器失败 " + e.Message);
        }
    }

    public void CloseConnect()
    {
        if (m_socket != null && m_socket.Connected)
        {
            m_socket.Shutdown(SocketShutdown.Both);
            m_socket.Close();
        }
        m_socket = null;
        Logger.Instance.WriteLog("关闭与服务器的连接");
    }

    private void ReceiveCallBack(IAsyncResult asyn)
    {
        SocketError sError = SocketError.Success;
        //消息读取
        try
        {
            if (asyn != null)
            {
                int count = 0;

                count = m_socket.EndReceive(asyn, out sError);
                if (sError == SocketError.Success)
                {
                    if (count > 0)
                    {
                        // 数据总长度
                        int msgLen = count + m_lastFragmentDataLen;
                        // 数据偏移
                        int msgOffset = 0;
                        // 下一条数据偏移
                        int nextOffset = 0;
                        // 单条数据长度
                        int dataLen = 0;
                        // 头（int32/uint32）长度
                        int headLen = Marshal.SizeOf(typeof(ALLST_GENERIC));

                        while (msgOffset + headLen < msgLen)
                        {
                            // 获取单条数据长度
                            dataLen = BitConverter.ToInt32(m_buffer, msgOffset + Marshal.SizeOf(typeof(UInt32)) + Marshal.SizeOf(typeof(UInt16)) + Marshal.SizeOf(typeof(UInt32)));
                            // 计算下一条数据的偏移
                            nextOffset = msgOffset + headLen + dataLen;

                            if (nextOffset < msgLen)
                            {
                                m_messages.Add(new SocketMessage(m_buffer, msgOffset, headLen + dataLen));
                                msgOffset = nextOffset;
                            }
                            else if (nextOffset == msgLen)
                            {
                                m_messages.Add(new SocketMessage(m_buffer, msgOffset, headLen + dataLen));
                                m_lastFragmentDataLen = 0;
                                break;
                            }
                            else
                            {
                                // 计算未接收完的数据的已接收部分的长度
                                m_lastFragmentDataLen = msgLen - msgOffset;
                                // 将这个已接收的部分copy到接收buffer里，用于下一次的接收
                                Buffer.BlockCopy(m_buffer, msgOffset, m_buffer, 0, m_lastFragmentDataLen);
                                break;
                            }

                        }
                    }
                }
                else
                {
                    Debug.Log(sError.ToString());
                }
            }

            // 第三个参数代表需要接受的字节数，所以要减掉偏移量，否则报Argument out of range的错误
            m_socket.BeginReceive(m_buffer, m_lastFragmentDataLen, MAX_BUFFER_SIZE - m_lastFragmentDataLen, SocketFlags.None, ReceiveCallBack, null);
        }
        catch (Exception e)
        {
            m_socket.Close();
            Logger.Instance.WriteLog("receive error : " + e.StackTrace + " ; " + e.Message);
            isConnect = false;
            Logger.Instance.WriteLog(sError.ToString());
            Logger.Instance.WriteLog("重新连接服务器");
            // 连接失败后重新连接
            Connect();
            reconnect = isConnect;
        }
    }

    bool Send(byte[] bytes)
    {
        bool bRet = false;
        try
        {
            if (isConnect)
            {
                bRet = m_socket.Send(bytes) > 0;
            }
        }
        catch (Exception e)
        {
            Logger.Instance.WriteLog("= = = =  ERROR : send " + e.Message);
            CloseConnect();
        }
        return bRet;
    }

    public static byte[] StructToBytes(object structObj)
    {
        int size = Marshal.SizeOf(structObj);
        byte[] bytes = new byte[size];
        IntPtr structPtr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(structObj, structPtr, false);
        Marshal.Copy(structPtr, bytes, 0, size);
        Marshal.FreeHGlobal(structPtr);
        return bytes;
    }

    public bool loginToCMS(string name, string passwd, string ip, UInt16 port, UInt16 Protocol, ReturnCallback CallbackFunction)
    {
        NetworkMsgAnalysis.GetInstance().UserLoginCallback = CallbackFunction;
        CTOS_CLIENTLOGIN st = new CTOS_CLIENTLOGIN(name, passwd, ip, port, Protocol);

        ALLST_GENERIC stHeader = new ALLST_GENERIC((UInt16)CmdNum.CTOS_CLIENTLOGIN_MSG, (Int32)Marshal.SizeOf(st));

        byte[] all = new byte[m_iHeaderLen + stHeader.iMessageLen];

        byte[] data1 = StructToBytes(stHeader);
        byte[] data2 = StructToBytes(st);

        Buffer.BlockCopy(data1, 0, all, 0, m_iHeaderLen);
        Buffer.BlockCopy(data2, 0, all, m_iHeaderLen, (int)stHeader.iMessageLen);

        return Send(all);

    }

    public bool LogoffCMS()
    {
        ALLST_GENERIC stHeader = new ALLST_GENERIC((UInt16)CmdNum.CTOS_LOGINOUT_MSG, 0);
        byte[] msg;
        msg = StructToBytes(stHeader);
        return Send(msg);
    }

    public bool GetCameraList()
    {
        ALLST_GENERIC stHeader = new ALLST_GENERIC((UInt16)CmdNum.CTOS_GETVIDEODATA_MSG, 0);
        byte[] msg;
        msg = StructToBytes(stHeader);
        return Send(msg);
    }

    public bool OpenCamera(int cameraId, CMS_GUID guid, ReturnCallback CallbackFunction)
    {
        NetworkMsgAnalysis.GetInstance().OpenCameraCallback = CallbackFunction;
        ALLST_GENERIC stHeader = new ALLST_GENERIC((UInt16)CmdNum.CTOS_OPENCAMERA_MSG, Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(CMS_GUID)));
        byte[] msg = new byte[m_iHeaderLen + stHeader.iMessageLen];
        byte[] b_header = StructToBytes(stHeader);
        byte[] b_id = StructToBytes(cameraId);
        byte[] b_guid = StructToBytes(guid);
        Buffer.BlockCopy(b_header, 0, msg, 0, m_iHeaderLen);
        Buffer.BlockCopy(b_guid, 0, msg, m_iHeaderLen, b_guid.Length);
        Buffer.BlockCopy(b_id, 0, msg, m_iHeaderLen + b_guid.Length, b_id.Length);
        return Send(msg);

    }

    public bool CloseCamera(int cameraId, CMS_GUID guid, ReturnCallback CallbackFunction)
    {
        NetworkMsgAnalysis.GetInstance().CloseCameraCallback = CallbackFunction;
        ALLST_GENERIC stHeader = new ALLST_GENERIC((UInt16)CmdNum.CTOS_CLOSECAMERA_MSG, Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(CMS_GUID)));
        byte[] msg = new byte[m_iHeaderLen + stHeader.iMessageLen];
        byte[] b_header = StructToBytes(stHeader);
        byte[] b_id = StructToBytes(cameraId);
        byte[] b_guid = StructToBytes(guid);
        Buffer.BlockCopy(b_header, 0, msg, 0, m_iHeaderLen);
        Buffer.BlockCopy(b_guid, 0, msg, m_iHeaderLen, b_guid.Length);
        Buffer.BlockCopy(b_id, 0, msg, m_iHeaderLen + b_guid.Length, b_id.Length);
        return Send(msg);
    }

    public bool SetPTZControl(int cameraId, CMS_GUID guid, PTZOperation nCtrlType, UInt16 nSpeedX, UInt16 nSpeedY, UInt16 nSpeedZ, int nPreset, ReturnCallback CallbackFunction)
    {
        NetworkMsgAnalysis.GetInstance().SetPTZCtrlCallback = CallbackFunction;
        CTOS_PTZ_INFO PTZinfo = new CTOS_PTZ_INFO();
        PTZinfo.cameraId = cameraId;
        PTZinfo.camareGuid = guid;
        PTZinfo.type = (UInt16)nCtrlType;
        PTZinfo.speedX = nSpeedX;
        PTZinfo.speedY = nSpeedY;
        PTZinfo.speedZ = nSpeedZ;
        PTZinfo.nPreset = nPreset;
        ALLST_GENERIC stHeader = new ALLST_GENERIC((UInt16)CmdNum.CTOS_SETPTZCTRL_MSG, Marshal.SizeOf(typeof(CTOS_PTZ_INFO)));
        byte[] msg = new byte[m_iHeaderLen + stHeader.iMessageLen];
        byte[] b_header = StructToBytes(stHeader);
        byte[] b_PTZinfo = StructToBytes(PTZinfo);
        Buffer.BlockCopy(b_header, 0, msg, 0, m_iHeaderLen);
        Buffer.BlockCopy(b_PTZinfo, 0, msg, m_iHeaderLen, b_PTZinfo.Length);
        return Send(msg);
    }

    public bool SetPresetPosition(int cameraId, CMS_GUID guid, UInt16 index, ReturnCallback CallbackFunction)
    {
        NetworkMsgAnalysis.GetInstance().SetPresetPosCallback = CallbackFunction;
        ALLST_GENERIC stHeader = new ALLST_GENERIC((UInt16)CmdNum.CTOS_SETPREPOSITION_MSG, Marshal.SizeOf(typeof(CTOS_SETPREPOSITION_INFO)));
        CTOS_SETPREPOSITION_INFO info = new CTOS_SETPREPOSITION_INFO();
        info.cameraId = cameraId;
        info.camareGuid = guid;
        info.index = index;
        info.name = Encoding.ASCII.GetBytes("preset" + index);
        byte[] msg = new byte[m_iHeaderLen + stHeader.iMessageLen];
        byte[] b_header = StructToBytes(stHeader);
        byte[] b_info = StructToBytes(info);
        Buffer.BlockCopy(b_header, 0, msg, 0, m_iHeaderLen);
        Buffer.BlockCopy(b_info, 0, msg, m_iHeaderLen, b_info.Length);
        return Send(msg);
    }

    public bool GotoPresetPosition(int cameraId, CMS_GUID guid, UInt16 index, ReturnCallback CallbackFunction)
    {
        NetworkMsgAnalysis.GetInstance().GotoPresetPosCallback = CallbackFunction;
        ALLST_GENERIC stHeader = new ALLST_GENERIC((UInt16)CmdNum.CTOS_GOTOPREPOSITION_MSG, Marshal.SizeOf(typeof(CTOS_GTOPREPOSITION_INFO)));
        CTOS_GTOPREPOSITION_INFO info = new CTOS_GTOPREPOSITION_INFO();
        info.cameraId = cameraId;
        info.camareGuid = guid;
        info.index = index;
        byte[] msg = new byte[m_iHeaderLen + stHeader.iMessageLen];
        byte[] b_header = StructToBytes(stHeader);
        byte[] b_info = StructToBytes(info);
        Buffer.BlockCopy(b_header, 0, msg, 0, m_iHeaderLen);
        Buffer.BlockCopy(b_info, 0, msg, m_iHeaderLen, b_info.Length);
        return Send(msg);
    }

    public void GetPassengerFlow(DelPassengerFlow callback)
    {
        NetworkMsgAnalysis.GetInstance().PassengerFlowCallback = callback;
    }
}
