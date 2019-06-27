using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

public class NetProtocol : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //  class Protocol;
    public  struct ALLST_GENERIC
    {// 协议头信息
        public Int32 dwTimeStamp;				// 时间戳
        public UInt16 wType;						// 信息类型
        public Int32 iPlayerID;					// 玩家ID
        public Int32 iMessageLen;				// 信息长度
        public Int16 nError;						// 错误代码，0表示成功
    };


    public  struct ConstVar
    {
        public const int ZZ_NAME_MAX = 36;//名称基本长度

        public const int RECIVEBUFFSIZE_MAX = 201 * 1280;// 接收包的最大长度
    }
    // 客户端登录信息
    public struct CTOS_CLIENTLOGIN
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = ConstVar.ZZ_NAME_MAX + 3)]
        public byte[] PlayerName;
        public UInt32 playerUID;
    };

    // 协议
    public enum NET_GM_PROTPCOL
    {
        CTOS_CLIENTLOGIN_MSG = 0x0000,
        STOC_CLIENTLOGIN_MSG = 0x0001,
        STOC_REGISTER_MSG_TYPE = 0x0002,
        STOC_LOGINOUT_MSG = 0x0003,

        CTOS_INITVIDEO_MSG,//初始化视频信息
        STOC_INITVIDEO_MSG,

        CTOS_GETVIDEODATA_MSG,//获取视频组的数据
        STOC_GETVIDEODATA_MSG,

        CTOS_OPENCAMERA_MSG,//打开当前视频
        STOC_OPENCAMERA_MSG,

        CTOS_CLOSECAMERA_MSG,//关闭当前视频
        STOC_CLOSECAMERA_MSG,

        CTOS_SETPREPOSITION_MSG,//设置预设位
        STOC_SETPREPOSITION_MSG,

        CTOS_GOTOPREPOSITION_MSG,//打开预设位
        STOC_GOTOPREPOSITION_MSG,

        CTOS_GETCALLBACKDATA_MSG,//请求回调数据
        STOC_GETCALLBACKDATA_MSG,

        CTOS_BEGINSENDDATA_MSG,//传输当前一帧数据
        CTOS_ENDSENDDATA_MSG,

        CTOS_SETPTZCTRL_MSG,//操作云台控制
        STOC_SETPTZCTRL_MSG,

        CTOS_LOGINOFFSERVER_MSG,//退出服务器消息
        STOC_LOGINOFFSERVER_MSG,
    };

    //错误信息
    public enum GAMESERVER_ERROR
    {
        ERR_GAMESERVER_SUCCESS = 0x0000,
        ERR_GAMESERVER_GENERIC = 0x0001,
        ERR_GAMESERVER_TIME_LIMIT = 0x0002,
        ERR_GAMESERVER_THROW_PACK = 0x0003
    };

//    /**@brief PTZ 运动类型*/
//    public enum PTZOperation
//    {
//        PTZ_UP = 0,     /** 上 */
//        PTZ_DOWN =1,    /** 下 */
//        PTZ_RIGHT = 2,	/** 右 */
//        PTZ_LEFT = 3,	/** 左*/
//        PTZ_IRIS_OPEN = 4,  /** 光圈放大*/
//        PTZ_IRIS_CLOSE = 5,  /** 光圈缩小*/
//        PTZ_ZOOM_WIDE = 6,  /** 拉远*/
//        PTZ_ZOOM_TELE = 7,  /** 拉近*/
//        PTZ_FOCUS_NEAR,  /** 近焦*/
//        PTZ_FOCUS_FAR,  /** 远焦*/
//        PTZ_STOP,  /** 停止*/
//        PTZ_SET_PREPOSITION,  /** 不可用*/
//        PTZ_CALL_PREPOSITION,  /** 不可用*/
//        PTZ_START_AUTOSCAN,  /** */
//        PTZ_STOP_AUTOSCAN,  /** */
//        PTZ_AUTO_SKIP,  /** 平跳*/
//        PTZ_FIRST_POSITION,  /** 首位*/
//        PTZ_SET_SUB_DEVICE,  /** */
//        PTZ_CLEAR_SUB_DEVICE,  /** */
//        PTZ_SET_HOLD_POSITION,  /** */
//        PTZ_OPEN_HOLD_POSITION,  /** */
//        PTZ_HOLD_POSINTION_TIME,  /** */
//        PTZ_CANCEL_HOLD_POSITION,  /** */
//        PTZ_RAINCLEAN_START,  /** */
//        PTZ_RAINCLEAN_STOP,  /** */
//        PTZ_LIGHT_OPEN,  /** */
//        PTZ_LIGHT_CLOSE,  /** */
//        PTZ_BRUSH_START,  /** */
//        PTZ_BRUSH_STOP,  /** */
//        PTZ_AUTO_FOCUS = 31,/** */
//        PTZ_MANUAL_FOCUS,  /** */
//        PTZ_IRIS_PRIORITY,  /** */
//        PTZ_IRIS_FULLAUTO,  /** */
//
//        PTZ_STOP_DH_SD = 45,/** */
//        PTZ_STOP_IRIS,  /** */
//        PTZ_STOP_FOCUS,  /** */
//        PTZ_STOP_ZOOM,  /** */
//        PTZ_STOP_UPDOWN,  /** */
//        PTZ_STOP_LEFTRIGHT,  /** */
//        //仅限帕尔高D和帕尔高P使用
//        PTZ_UP_LEFT,
//        PTZ_UP_RIGHT,
//        PTZ_DOWN_LEFT,
//        PTZ_DOWN_RIGHT,
//
//    };
    public struct STOC_CLIENTLOGIN
    {
        public UInt32 PlayerIndex;
    };

    public struct STOC_INITVIDEO
    {
        public UInt32 Ret;
    };

    public struct CMS_LogonInfo
    {
        public UInt32 nStructVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] srvIP;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] User;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Passwd;
        public int nProtocol;
        public int nRelayMode;
    }


//    public struct CMS_GUID
//    {
//        public UInt32 Data1;
//        public UInt16 Data2;
//        public UInt16 Data3;
//        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
//        public byte[] Data4;
//    }

    public struct CMS_NodeData
    {
        public IntPtr hNode;			/**  节点HANDLE */
        public int nNodeID;		/**	 节点ID		*/
        public int nNodeType;		/**  节点类型	*/
        public int nStatus;		/**  节点状态 0 :禁用; 1: 可用*/
        public int bPTZ;			/**  是否云台	*/
        public int nPTZType;		/**  云台类型	*/
        public int nPTZAddr;		/**  云台地址	*/
        CMS_GUID devGUID;		/**  镜头GUID	*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] cName;/**  节点名称	*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
        public byte[] cDescribe;	/**  节点描述	*/
    }

    public struct STOC_VIDEOINFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public CMS_NodeData[] VideoData;/**  视频数据	*/
    };


    public struct CAMERAINFO
    {
        public int nStatus;		/**  节点状态 0 :禁用; 1: 可用*/
        public int bPTZ;			/**  是否云台	*/
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] cName;/**  节点名称	*/
        public IntPtr hNode;			/**  节点HANDLE */
    }



    enum CMS_VIDEO_FRAME_TYPE
    {
        VFT_I,	//I Frame
        VFT_P,	//P Frame
        VFT_B,	//B Frame
        VFT_SI,
        VFT_SP
    };

    enum CMS_VIDEO_FRAME_FORMAT
    {
        VFF_NONE,//类型有误
        VFF_YUV420,//YUV420格式
        VFF_RGB32
    };//RGB32格式

    public struct CMS_VIDEO_FRAME
    {
        CMS_VIDEO_FRAME_FORMAT enumDataFormat;//数据类型

        public IntPtr pY;//YUV之Y指针，如果enumDataFormat为YUV420则此指针有效
        public IntPtr pU;//YUV之U指针，如果enumDataFormat为YUV420则此指针有效
        public IntPtr pV;//YUV之V指针，如果enumDataFormat为YUV420则此指针有效
        public IntPtr pRGB;//RGB指针，如果enumDataFormat为RGB32则此指针有效
        public int iWidth;//帧宽
        public int iHeight;//帧高
        public UInt64 timestamp;//时间戳
        CMS_VIDEO_FRAME_TYPE enumFrameType;//帧类型
        public UInt32 uiSliceNum;//切片编号,切片导出时适用
        public UInt32 uiSliceMode;//切片模式，切片导出时适用
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public UInt32[] uiReserveData;
    }

    public struct CTOS_CAMERAINFO
    {
        public IntPtr camerHander;
        public UInt32 PlayerIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] cName;/**  名称	*/
    };

    public struct STOC_CAMERANAME
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] pName;
        public int size;//有效长度
    };

    public struct PREPOSITIONINFO
    {
        public IntPtr camerHander;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] cName;/**  名称	*/
        public int ret;
    };

    public struct SETPTZOPERATIONINFO
    {
        public IntPtr camerHander;//摄像头句柄
        public int operationType;//操作类型
        public int SpeedX;//速度
        public int SpeedY;
        public int SpeedZ;
        public int nPreset;//参数预留
    };

    public class Protocol
    {
        public static int ALLST_GENERIC_LEN = Marshal.SizeOf(typeof(ALLST_GENERIC));//packet 长度

        public static int CTOS_CLIENTLOGIN_LEN = Marshal.SizeOf(typeof(CTOS_CLIENTLOGIN));//用户登录信息 长度

        public static int STOC_CLIENTLOGIN_LEN = Marshal.SizeOf(typeof(STOC_CLIENTLOGIN));

        public static int CTOS_INITVIDEO_LEN = Marshal.SizeOf(typeof(CMS_LogonInfo));

        public static int STOC_INITVIDEO_LEN = Marshal.SizeOf(typeof(STOC_INITVIDEO));

        public static int STOC_VIDEODATA_LEN = Marshal.SizeOf(typeof(STOC_VIDEOINFO));

        public static int CTOS_CAMERAINFO_LEN = Marshal.SizeOf(typeof(CTOS_CAMERAINFO));

        public static int STOC_CAMERANAME_LEN = Marshal.SizeOf(typeof(STOC_CAMERANAME));


        public static int PREPOSITIONINFO_LEN = Marshal.SizeOf(typeof(PREPOSITIONINFO));

        public static int SETPTZOPERATIONINFO_LEN = Marshal.SizeOf(typeof(SETPTZOPERATIONINFO));

    }



}
