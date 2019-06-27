using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class NetConnectVideo : MonoBehaviour {



    public GameObject TextButton1;
    public GameObject TextButton2;
    public GameObject TextButton3;
    public GameObject TextButton4;

    public static Texture2D txt2dtest;
    public static Texture2D sectxt2dtest;

    private static Socket clientSocket;

    public static IntPtr hCurOperateCamera = IntPtr.Zero;

    public static IntPtr hFirOperateCamera = IntPtr.Zero;
    public static IntPtr hSecOperateCamera = IntPtr.Zero;

    //public static string hCurCameraName = "";

    //public static string hSecCameraName = "";

    public static byte[]  CurCameraName = new byte[64];

    public static byte[] FirCameraNam = new byte[64];
    public static byte[] SecCameraNam = new byte[64];



    public static List<NetProtocol.CAMERAINFO> ServerCameraList = new List<NetProtocol.CAMERAINFO>();

    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr OpenFileMapping(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, string lpName);
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr MapViewOfFile(IntPtr hFileMapping, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool UnmapViewOfFile(IntPtr pvBaseAddress);
    [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool CloseHandle(IntPtr handle);
    const int FILE_MAP_READ = 0x0004;
    const int BUF_SIZE = 1280 * 720 * 4;

    public static bool mIsReadShareMemory = false;
    public static bool mSecIsReadShareMemory = false;
    public static bool mIsRecieve = true;

	// Use this for initialization
	void Start () {

        string ip = "127.0.0.1";
        int port = 7999;
        InitSocket(ip, port);

        txt2dtest = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        GameObject.FindGameObjectWithTag("Game1BG").GetComponent<UITexture>().mainTexture = txt2dtest;
        sectxt2dtest = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        GameObject.FindGameObjectWithTag("Game2BG").GetComponent<UITexture>().mainTexture = sectxt2dtest;
        UIEventListener.Get(TextButton1).onClick = onLeftButtonClick;
        UIEventListener.Get(TextButton2).onClick = onRightButtonClick;
        UIEventListener.Get(TextButton3).onClick = onUpButtonClick;
        UIEventListener.Get(TextButton4).onClick = onDownButtonClick;

	}

    void onLeftButtonClick(GameObject button)
    {
        Debug.Log("click");

        hCurOperateCamera = hSecOperateCamera;
        CurCameraName = SecCameraNam;
        openCamera(hSecOperateCamera, SecCameraNam.ToString());

    }

    void onRightButtonClick(GameObject button)
    {

    }

    void onUpButtonClick(GameObject button)
    {

    }
    void onDownButtonClick(GameObject button)
    {

    }

    static float mtimer = 0.1f;
	// Update is called once per frame
	void Update () {


        ReciveMsg();
        if (mIsRecieve)
        {
           
        }
        

        if (mtimer > 0)
        {
            mtimer -= Time.deltaTime;
        }
        else
        {
            mtimer = 0.1f;
            if (mIsReadShareMemory)
            {
                IntPtr hMappingHandle = IntPtr.Zero;
                IntPtr hVoid = IntPtr.Zero;
                hMappingHandle = OpenFileMapping(FILE_MAP_READ, false, CurCameraName.ToString());
                if (hMappingHandle == IntPtr.Zero)
                {
                    //没有打开共享内存
                }
                hVoid = MapViewOfFile(hMappingHandle, FILE_MAP_READ, 0, 0, BUF_SIZE);
                if (hVoid == IntPtr.Zero)
                {
                    //读取内存失败
                }

                //读取共享内存中的Buff然后做渲染
                byte[] bytes = new byte[BUF_SIZE];
                Marshal.Copy(hVoid, bytes, 0, bytes.Length);
                txt2dtest.LoadRawTextureData(bytes);
                txt2dtest.Apply();

                if (hVoid != IntPtr.Zero)
                {
                    UnmapViewOfFile(hVoid);
                    hVoid = IntPtr.Zero;
                }
                if (hMappingHandle != IntPtr.Zero)
                {
                    CloseHandle(hMappingHandle);
                    hMappingHandle = IntPtr.Zero;
                }
            }

            if (mSecIsReadShareMemory)
            {
                IntPtr hMappingHandle = IntPtr.Zero;
                IntPtr hVoid = IntPtr.Zero;
                hMappingHandle = OpenFileMapping(FILE_MAP_READ, false, SecCameraNam.ToString());
                if (hMappingHandle == IntPtr.Zero)
                {
                    //没有打开共享内存
                }
                hVoid = MapViewOfFile(hMappingHandle, FILE_MAP_READ, 0, 0, BUF_SIZE);
                if (hVoid == IntPtr.Zero)
                {
                    //读取内存失败
                }

                //读取共享内存中的Buff然后做渲染
                byte[] bytes = new byte[BUF_SIZE];
                Marshal.Copy(hVoid, bytes, 0, bytes.Length);
                sectxt2dtest.LoadRawTextureData(bytes);
                sectxt2dtest.Apply();

                if (hVoid != IntPtr.Zero)
                {
                    UnmapViewOfFile(hVoid);
                    hVoid = IntPtr.Zero;
                }
                if (hMappingHandle != IntPtr.Zero)
                {
                    CloseHandle(hMappingHandle);
                    hMappingHandle = IntPtr.Zero;
                }
            }
        }
       
	}


        public static void InitSocket(string Ip, int port)
        {
            IPAddress ip = IPAddress.Parse(Ip);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, port)); //配置服务器IP与端口  

            }
            catch
            {
                
                return ;
            }

            LaunchSocket("MyClinet");
            return ;

        }

        //获取socket的头包
        public static byte[] GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL type, Int16 MessageSize)
        {

            byte[] headbuff = new byte[NetProtocol.Protocol.ALLST_GENERIC_LEN];

            object objhead = NetTools.BytesToStuct(headbuff, typeof(NetProtocol.ALLST_GENERIC));
            NetProtocol.ALLST_GENERIC pMsgGeneric = (NetProtocol.ALLST_GENERIC)objhead;
            pMsgGeneric.dwTimeStamp = 0;
            pMsgGeneric.wType = (UInt16)type;
            pMsgGeneric.iMessageLen = MessageSize;
            pMsgGeneric.iPlayerID = 1001;
            pMsgGeneric.nError = (Int16)NetProtocol.GAMESERVER_ERROR.ERR_GAMESERVER_SUCCESS;

            byte[] Readheadbuff = NetTools.GetBytes(pMsgGeneric);
            return Readheadbuff;

            
        }

        
        //socket 握手通信
        private static void LaunchSocket(string Name)
        {
            Stream str;
            str = new MemoryStream();
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_CLIENTLOGIN_MSG, (Int16)NetProtocol.Protocol.CTOS_CLIENTLOGIN_LEN);

            byte[] Msgbuff = new byte[NetProtocol.Protocol.CTOS_CLIENTLOGIN_LEN];
            object objMsg = NetTools.BytesToStuct(Msgbuff, typeof(NetProtocol.CTOS_CLIENTLOGIN));
            NetProtocol.CTOS_CLIENTLOGIN pMsg = (NetProtocol.CTOS_CLIENTLOGIN)objMsg;
            pMsg.PlayerName = NetTools.CodeBytes(Name, NetProtocol.ConstVar.ZZ_NAME_MAX + 3);
            pMsg.playerUID = 1001;
            byte[] ReadMsgbuff = NetTools.GetBytes(pMsg);

            byte[] PacketBuff = new byte[NetProtocol.Protocol.CTOS_CLIENTLOGIN_LEN + headBuff.Length];
            str.Write(headBuff, 0, headBuff.Length);
            str.Write(ReadMsgbuff, 0, ReadMsgbuff.Length);
            str.Position = 0;
            int r = str.Read(PacketBuff, 0, PacketBuff.Length);
            
            if (r > 0)
            {
                clientSocket.Send(PacketBuff, PacketBuff.Length, SocketFlags.None);
            }
            return;
        }

        public static int currentBuff = 0;
        public static int count = 0;

        public static byte [] RGBBuff = new byte[1280*720*4];

        private static void Dispatch(NetProtocol.ALLST_GENERIC pMsgGeneric, byte[] dataBuff, int ReciveSize)
        {

            byte[] Msgbuff = new byte[NetProtocol.ConstVar.RECIVEBUFFSIZE_MAX];
            Array.Copy(dataBuff, NetProtocol.Protocol.ALLST_GENERIC_LEN, Msgbuff, 0, ReciveSize - NetProtocol.Protocol.ALLST_GENERIC_LEN);
            switch (pMsgGeneric.wType)
            {
                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_CLIENTLOGIN_MSG:
                    {
                        //连接成功
                        object obj = NetTools.BytesToStuct(Msgbuff, typeof(NetProtocol.STOC_CLIENTLOGIN));
                        LogonServer("172.22.20.232","admin","");
                    }
                    break;
                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_INITVIDEO_MSG:
                    {
                        //检测连接视频服务器是否成功
                        object obj = NetTools.BytesToStuct(Msgbuff, typeof(NetProtocol.STOC_INITVIDEO));
                        NetProtocol.STOC_INITVIDEO pInfo = (NetProtocol.STOC_INITVIDEO)obj;
                        if (pInfo.Ret > 0)
                        {
                            //连接成功
                            GetCameraList();
               
                        }
                        else
                        {
                            //连接失败
                        }
                    }
                    break;
                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_GETVIDEODATA_MSG:
                    {
                        //获取视频数据
                        object obj = NetTools.BytesToStuct(Msgbuff, typeof(NetProtocol.STOC_VIDEOINFO));
                        NetProtocol.STOC_VIDEOINFO pInfo = (NetProtocol.STOC_VIDEOINFO)obj;
                        ServerCameraList.Clear();
                        foreach (NetProtocol.CMS_NodeData data in pInfo.VideoData)
                        {
                            NetProtocol.CAMERAINFO camera= new NetProtocol.CAMERAINFO();
                            camera.cName = data.cName;
                            camera.hNode = data.hNode;
                            camera.bPTZ = data.bPTZ;
                            camera.nStatus = data.nStatus;
                            ServerCameraList.Add(camera);
      
                        }

                        foreach (NetProtocol.CMS_NodeData data in pInfo.VideoData)
                        {
                            if (data.nStatus == 1&& data.bPTZ == 0)
                            {
                                hFirOperateCamera = data.hNode;
                                hCurOperateCamera = data.hNode;
                                FirCameraNam = data.cName;
                                CurCameraName = data.cName;
                                break;
                            }

                        }
                         foreach (NetProtocol.CMS_NodeData data in pInfo.VideoData)
                        {
                            if (data.nStatus == 1 && data.bPTZ == 1)
                            {
                                hSecOperateCamera = data.hNode;
                                SecCameraNam = data.cName;
                                break;
                            }

                        }






                         openCamera(hCurOperateCamera, CurCameraName.ToString());
                    
                        

                    }
                    break;
                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_OPENCAMERA_MSG:
                    {
                        //打开视频返回的消息（获取当前预览的句柄）
                        object obj = NetTools.BytesToStuct(Msgbuff, typeof(NetProtocol.CTOS_CAMERAINFO));
                        NetProtocol.CTOS_CAMERAINFO pInfo = (NetProtocol.CTOS_CAMERAINFO)obj;
                        if (pInfo.camerHander != IntPtr.Zero)
                        {
                            // 成功打开
                            if (hCurOperateCamera != IntPtr.Zero)
                            {
                                SetVideoCallBackFunc(hCurOperateCamera,CurCameraName);
                            }
                        }


                    }
                    break;

                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_CLOSECAMERA_MSG:
                    {
                        //关闭当前视频


                    }
                    break;
                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_SETPREPOSITION_MSG:
                    {
                        //设置预设位返回的消息


                    }
                    break;
                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_GOTOPREPOSITION_MSG:
                    {
                        //打开预设位返回的消息


                    }
                    break;
                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_LOGINOFFSERVER_MSG:
                    {
                        //关闭服务器返回的消息



                    }
                    break;
                case (UInt16)NetProtocol.NET_GM_PROTPCOL.STOC_GETCALLBACKDATA_MSG:
                    {
                        //获取回调数据

                        object obj = NetTools.BytesToStuct(Msgbuff, typeof(NetProtocol.STOC_CAMERANAME));
                        NetProtocol.STOC_CAMERANAME pData = (NetProtocol.STOC_CAMERANAME)obj;
                     //   Array.Copy(源数据, 源数据开始复制处索引, 接收数据, 接收数据开始处索引, 复制多少个数据);
                        byte[] CompareByte = new byte[NetProtocol.Protocol.STOC_CAMERANAME_LEN];
                        Array.Copy(pData.pName, 0, CompareByte, 0, pData.size);

                        byte[] CompareFirCameraByte = new byte[NetProtocol.Protocol.STOC_CAMERANAME_LEN];

                        Array.Copy(FirCameraNam, 0, CompareFirCameraByte, 0, pData.size);


                        byte[] CompareSecCameraByte = new byte[NetProtocol.Protocol.STOC_CAMERANAME_LEN];

                        Array.Copy(SecCameraNam, 0, CompareSecCameraByte, 0, pData.size);

                        if (NetTools.PasswordEquals(CompareByte, CompareFirCameraByte))
                        {
                            mIsReadShareMemory = true;
                        }
                        if (NetTools.PasswordEquals(CompareByte, CompareSecCameraByte))
                        {
                            mSecIsReadShareMemory = true;
                        }
                        //int index = count * NetProtocol.Protocol.STOC_RGB_LEN;
   
                        //currentBuff += NetProtocol.Protocol.STOC_RGB_LEN;
                        //count++;
                        //Debug.Log("currentBuff =" + currentBuff);
                        //Debug.Log("count =" + count);
                        //txt2dtest.LoadRawTextureData(pData.pRGB);
                        //txt2dtest.Apply();


                    }
                    break;
                default:
                    break;
            }
        }

        //通过clientSocket接收数据  
        private static void ReciveMsg()
        {
            //DynamicBufferManager receiveBuffer = m_asyncSocketUserToken.ReceiveBuffer;  
            byte[] dataBuff = new byte[NetProtocol.ConstVar.RECIVEBUFFSIZE_MAX];
            //while (true)
            {

                Array.Clear(dataBuff, 0, dataBuff.Length);
                int receiveLength = clientSocket.Receive(dataBuff, dataBuff.Length, SocketFlags.None);

                if (receiveLength >= NetProtocol.Protocol.ALLST_GENERIC_LEN)
                {
                    //接收server 发来的数据
                    object objhead_1 = NetTools.BytesToStuct(dataBuff, typeof(NetProtocol.ALLST_GENERIC));
                    NetProtocol.ALLST_GENERIC pMsgGeneric = (NetProtocol.ALLST_GENERIC)objhead_1;
                   // if (pMsgGeneric.iMessageLen + Protocol.ALLST_GENERIC_LEN <= receiveLength)
                    {
                        Dispatch(pMsgGeneric, dataBuff, receiveLength);
                    }
                }

            }
           
        }

        //ip 172.22.20.232 userName admin pwd ""登录服务器
        public static void LogonServer(string ip,string userName,string pwd)
        {
           
            Stream str;
            str = new MemoryStream();
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_INITVIDEO_MSG, (Int16)NetProtocol.Protocol.CTOS_INITVIDEO_LEN);

            byte[] MSG = new byte[NetProtocol.Protocol.CTOS_INITVIDEO_LEN];
            object objMsg = NetTools.BytesToStuct(MSG, typeof(NetProtocol.CMS_LogonInfo));
            NetProtocol.CMS_LogonInfo pMsg = (NetProtocol.CMS_LogonInfo)objMsg;

            pMsg.nStructVersion = 201402170;
            pMsg.srvIP = NetTools.CodeBytes(ip, 64);
            pMsg.User = NetTools.CodeBytes(userName, 64);
            pMsg.Passwd = NetTools.CodeBytes(pwd, 64);
            pMsg.nProtocol = 1;
            pMsg.nRelayMode = 0;
            byte[] ReadMsgbuff = NetTools.GetBytes(pMsg);

            byte[] PacketBuff = new byte[NetProtocol.Protocol.CTOS_INITVIDEO_LEN + headBuff.Length];
            str.Write(headBuff, 0, headBuff.Length);
            str.Write(ReadMsgbuff, 0, ReadMsgbuff.Length);
            str.Position = 0;
            int r = str.Read(PacketBuff, 0, PacketBuff.Length);

            if (r > 0)
            {
                clientSocket.Send(PacketBuff, PacketBuff.Length, SocketFlags.None);
            }
        }

        // 关闭视频服务器
        public static void LogonOffServer()
        {

            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_LOGINOFFSERVER_MSG, 0);
            clientSocket.Send(headBuff, headBuff.Length, SocketFlags.None);
        }

        //获取服务器相机组
        public static void GetCameraList()
        {

            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_GETVIDEODATA_MSG, 0);
            clientSocket.Send(headBuff, headBuff.Length, SocketFlags.None);

        }

        //------------------------------------------云台控制接口--------------------------------------------------------//
        /**@brief 控制云台运动
        * @param [IN]	hCamera			镜头句柄。
        * @param [IN]	nCtrlType		控制类型，参见：PTZOperation枚举。
        * @param [IN]	nSpeedX			X方向速度。
        * @param [IN]	nSpeedY			Y方向速度。
        * @param [IN]	nSpeedZ			Z方向速度，如果设备不支持Z方向速度，此值无效。
        * @param [IN]	nReserve		保留参数。
        * @note
        *	1.为了兼容不容的设备，不论是左右移动或者上下移动的时候，请俩个速度都填写，速度值范围是1-10
        */
//        public static void SetPTZCtrl(NetProtocol.PTZOperation nCtrlType, int nSpeedX, int nSpeedY, int nSpeedZ, int nPreset)
//        {
//            Stream str;
//            str = new MemoryStream();
//            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_SETPTZCTRL_MSG, (Int16)NetProtocol.Protocol.SETPTZOPERATIONINFO_LEN);
//
//            byte[] MSG = new byte[NetProtocol.Protocol.SETPTZOPERATIONINFO_LEN];
//            object objMsg = NetTools.BytesToStuct(MSG, typeof(NetProtocol.SETPTZOPERATIONINFO));
//            NetProtocol.SETPTZOPERATIONINFO pMsg = (NetProtocol.SETPTZOPERATIONINFO)objMsg;
//            pMsg.camerHander = hCurOperateCamera;
//            pMsg.operationType = (int)nCtrlType;
//            pMsg.SpeedX = nSpeedX;
//            pMsg.SpeedY = nSpeedY;
//            pMsg.SpeedZ = nSpeedZ;
//            pMsg.nPreset = nPreset;
//           
//            byte[] ReadMsgbuff = NetTools.GetBytes(pMsg);
//
//            byte[] PacketBuff = new byte[NetProtocol.Protocol.SETPTZOPERATIONINFO_LEN + headBuff.Length];
//            str.Write(headBuff, 0, headBuff.Length);
//            str.Write(ReadMsgbuff, 0, ReadMsgbuff.Length);
//            str.Position = 0;
//            int r = str.Read(PacketBuff, 0, PacketBuff.Length);
//
//            if (r > 0)
//            {
//                clientSocket.Send(PacketBuff, PacketBuff.Length, SocketFlags.None);
//            }
//        }

            
        // 根据配置的相机名称去遍历存在的相机句柄
        public static bool SetCurrentCameraHanderByName(string cameraName)
        {
            bool Vaule = false;

            foreach (NetProtocol.CAMERAINFO data in ServerCameraList)
            {
                if (data.cName.ToString() == cameraName)
                {
                    Vaule = true;
                    hCurOperateCamera = data.hNode;

                    break;
                }
            }
            return Vaule;
        }

        //打开当前摄像头
        public static void openCamera(IntPtr OperateCamera, string CameraName)
        {
            Stream str;
            str = new MemoryStream();
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_OPENCAMERA_MSG, (Int16)NetProtocol.Protocol.CTOS_CAMERAINFO_LEN);

            byte[] MSG = new byte[NetProtocol.Protocol.CTOS_CAMERAINFO_LEN];
            object objMsg = NetTools.BytesToStuct(MSG, typeof(NetProtocol.CTOS_CAMERAINFO));
            NetProtocol.CTOS_CAMERAINFO pMsg = (NetProtocol.CTOS_CAMERAINFO)objMsg;

            pMsg.camerHander = OperateCamera;
            pMsg.PlayerIndex = 1001;
            pMsg.cName = NetTools.CodeBytes(CameraName, 64);

            byte[] ReadMsgbuff = NetTools.GetBytes(pMsg);
      
            byte[] PacketBuff = new byte[NetProtocol.Protocol.CTOS_CAMERAINFO_LEN + headBuff.Length];
            str.Write(headBuff, 0, headBuff.Length);
            str.Write(ReadMsgbuff, 0, ReadMsgbuff.Length);
            str.Position = 0;
            int r = str.Read(PacketBuff, 0, PacketBuff.Length);

            if (r > 0)
            {
                clientSocket.Send(PacketBuff, PacketBuff.Length, SocketFlags.None);
            }
        }

        // 关闭当前摄像头
        public static void closeCamera()
        {
            Stream str;
            str = new MemoryStream();
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_CLOSECAMERA_MSG, (Int16)NetProtocol.Protocol.CTOS_CAMERAINFO_LEN);

            byte[] MSG = new byte[NetProtocol.Protocol.CTOS_CAMERAINFO_LEN];
            object objMsg = NetTools.BytesToStuct(MSG, typeof(NetProtocol.CTOS_CAMERAINFO));
            NetProtocol.CTOS_CAMERAINFO pMsg = (NetProtocol.CTOS_CAMERAINFO)objMsg;

            pMsg.camerHander = hCurOperateCamera;
            pMsg.PlayerIndex = 1001;
            pMsg.cName = CurCameraName;
              //  NetTools.CodeBytes(CurCameraName, 64);
            byte[] ReadMsgbuff = NetTools.GetBytes(pMsg);

            byte[] PacketBuff = new byte[NetProtocol.Protocol.CTOS_CAMERAINFO_LEN + headBuff.Length];
            str.Write(headBuff, 0, headBuff.Length);
            str.Write(ReadMsgbuff, 0, ReadMsgbuff.Length);
            str.Position = 0;
            int r = str.Read(PacketBuff, 0, PacketBuff.Length);

            if (r > 0)
            {
                clientSocket.Send(PacketBuff, PacketBuff.Length, SocketFlags.None);
            }
        }


        // 设置预设位
        public static void SetPrePosition(string name,int pos)
        {

            Stream str;
            str = new MemoryStream();
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_SETPREPOSITION_MSG, (Int16)NetProtocol.Protocol.PREPOSITIONINFO_LEN);

            byte[] MSG = new byte[NetProtocol.Protocol.PREPOSITIONINFO_LEN];
            object objMsg = NetTools.BytesToStuct(MSG, typeof(NetProtocol.PREPOSITIONINFO));
            NetProtocol.PREPOSITIONINFO pMsg = (NetProtocol.PREPOSITIONINFO)objMsg;
            pMsg.camerHander = hCurOperateCamera;
            pMsg.cName = NetTools.CodeBytes(name, 64);
            pMsg.ret = pos;
            byte[] ReadMsgbuff = NetTools.GetBytes(pMsg);

            byte[] PacketBuff = new byte[NetProtocol.Protocol.PREPOSITIONINFO_LEN + headBuff.Length];
            str.Write(headBuff, 0, headBuff.Length);
            str.Write(ReadMsgbuff, 0, ReadMsgbuff.Length);
            str.Position = 0;
            int r = str.Read(PacketBuff, 0, PacketBuff.Length);

            if (r > 0)
            {
                clientSocket.Send(PacketBuff, PacketBuff.Length, SocketFlags.None);
            }
            
        }

        // 打开预设位
        public static void GoToPrePosition(string name, int pos)
        {

            Stream str;
            str = new MemoryStream();
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_GOTOPREPOSITION_MSG, (Int16)NetProtocol.Protocol.PREPOSITIONINFO_LEN);

            byte[] MSG = new byte[NetProtocol.Protocol.PREPOSITIONINFO_LEN];
            object objMsg = NetTools.BytesToStuct(MSG, typeof(NetProtocol.PREPOSITIONINFO));
            NetProtocol.PREPOSITIONINFO pMsg = (NetProtocol.PREPOSITIONINFO)objMsg;
            pMsg.camerHander = hCurOperateCamera;
            pMsg.cName = NetTools.CodeBytes(name, 64);
            pMsg.ret = pos;
            byte[] ReadMsgbuff = NetTools.GetBytes(pMsg);

            byte[] PacketBuff = new byte[NetProtocol.Protocol.PREPOSITIONINFO_LEN + headBuff.Length];
            str.Write(headBuff, 0, headBuff.Length);
            str.Write(ReadMsgbuff, 0, ReadMsgbuff.Length);
            str.Position = 0;
            int r = str.Read(PacketBuff, 0, PacketBuff.Length);

            if (r > 0)
            {
                clientSocket.Send(PacketBuff, PacketBuff.Length, SocketFlags.None);
            }

        }
        public static void SetVideoCallBackFunc(IntPtr OperateCamera,byte[]CameraName)
        {
            Stream str;
            str = new MemoryStream();
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_GETCALLBACKDATA_MSG, (Int16)NetProtocol.Protocol.CTOS_CAMERAINFO_LEN);

            byte[] MSG = new byte[NetProtocol.Protocol.CTOS_CAMERAINFO_LEN];
            object objMsg = NetTools.BytesToStuct(MSG, typeof(NetProtocol.CTOS_CAMERAINFO));
            NetProtocol.CTOS_CAMERAINFO pMsg = (NetProtocol.CTOS_CAMERAINFO)objMsg;

            pMsg.camerHander = OperateCamera;
            pMsg.PlayerIndex = 1001;
            pMsg.cName = CameraName;
            byte[] ReadMsgbuff = NetTools.GetBytes(pMsg);

            byte[] PacketBuff = new byte[NetProtocol.Protocol.CTOS_CAMERAINFO_LEN + headBuff.Length];
            str.Write(headBuff, 0, headBuff.Length);
            str.Write(ReadMsgbuff, 0, ReadMsgbuff.Length);
            str.Position = 0;
            int r = str.Read(PacketBuff, 0, PacketBuff.Length);

            if (r > 0)
            {
                clientSocket.Send(PacketBuff, PacketBuff.Length, SocketFlags.None);
            }
        }

        public static void BeginAceptCurrentData()
        {
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_BEGINSENDDATA_MSG, 0);
            clientSocket.Send(headBuff, headBuff.Length, SocketFlags.None);
        }

        public static void EndAceptCurrentData()
        {
            byte[] headBuff = GetSocketHeadBuff(NetProtocol.NET_GM_PROTPCOL.CTOS_ENDSENDDATA_MSG, 0);
            clientSocket.Send(headBuff, headBuff.Length, SocketFlags.None);
        }
 
}
