using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;



enum GAMESERVER_ERROR
{
    ERR_GAMESERVER_SUCCESS = 0x0000,
    ERR_GAMESERVER_GENERIC = 0x0001,
    ERR_GAMESERVER_TIME_LIMIT = 0x0002,
    ERR_GAMESERVER_THROW_PACK = 0x0003,

    ERR_LOGIN_TO_CMS_FAILD = 0x1004,
}

/**@brief PTZ 运动类型*/
public enum PTZOperation
{
	PTZ_UP = 0,     /** 上 */
	PTZ_DOWN =1,    /** 下 */
	PTZ_RIGHT = 2,	/** 右 */
	PTZ_LEFT = 3,	/** 左*/
	PTZ_IRIS_OPEN = 4,  /** 光圈放大*/
	PTZ_IRIS_CLOSE = 5,  /** 光圈缩小*/
	PTZ_ZOOM_WIDE = 6,  /** 拉远*/
	PTZ_ZOOM_TELE = 7,  /** 拉近*/
	PTZ_FOCUS_NEAR,  /** 近焦*/
	PTZ_FOCUS_FAR,  /** 远焦*/
	PTZ_STOP,  /** 停止*/
	PTZ_SET_PREPOSITION,  /** 不可用*/
	PTZ_CALL_PREPOSITION,  /** 不可用*/
	PTZ_START_AUTOSCAN,  /** */
	PTZ_STOP_AUTOSCAN,  /** */
	PTZ_AUTO_SKIP,  /** 平跳*/
	PTZ_FIRST_POSITION,  /** 首位*/
	PTZ_SET_SUB_DEVICE,  /** */
	PTZ_CLEAR_SUB_DEVICE,  /** */
	PTZ_SET_HOLD_POSITION,  /** */
	PTZ_OPEN_HOLD_POSITION,  /** */
	PTZ_HOLD_POSINTION_TIME,  /** */
	PTZ_CANCEL_HOLD_POSITION,  /** */
	PTZ_RAINCLEAN_START,  /** */
	PTZ_RAINCLEAN_STOP,  /** */
	PTZ_LIGHT_OPEN,  /** */
	PTZ_LIGHT_CLOSE,  /** */
	PTZ_BRUSH_START,  /** */
	PTZ_BRUSH_STOP,  /** */
	PTZ_AUTO_FOCUS = 31,/** */
	PTZ_MANUAL_FOCUS,  /** */
	PTZ_IRIS_PRIORITY,  /** */
	PTZ_IRIS_FULLAUTO,  /** */
	
	PTZ_STOP_DH_SD = 45,/** */
	PTZ_STOP_IRIS,  /** */
	PTZ_STOP_FOCUS,  /** */
	PTZ_STOP_ZOOM,  /** */
	PTZ_STOP_UPDOWN,  /** */
	PTZ_STOP_LEFTRIGHT,  /** */
	//仅限帕尔高D和帕尔高P使用
	PTZ_UP_LEFT,
	PTZ_UP_RIGHT,
	PTZ_DOWN_LEFT,
	PTZ_DOWN_RIGHT,
	
};

public enum CmdNum 
{
    //客户端请求登录
    CTOS_CLIENTLOGIN_MSG = 1,
    STOC_CLIENTLOGIN_MSG,
    //服务器客户端 心跳包
    STOC_KEEPALIVE_MSG,
    CTOS_KEEPALIVE_MSG,
    //客户端请求登出 和 断开连接效果一样
    CTOS_LOGINOUT_MSG,
    STOC_LOGINOUT_MSG,

    //获取视频组的数据
    CTOS_GETVIDEODATA_MSG,
    STOC_GETVIDEODATA_MSG,

    //打开指定的视频
    CTOS_OPENCAMERA_MSG,
    STOC_OPENCAMERA_MSG,

    //关闭指定的视频
    CTOS_CLOSECAMERA_MSG,
    STOC_CLOSECAMERA_MSG,

	//设置预设位
	CTOS_SETPREPOSITION_MSG,
	STOC_SETPREPOSITION_MSG,
	
	//打开预设位
	CTOS_GOTOPREPOSITION_MSG,
	STOC_GOTOPREPOSITION_MSG,
	
	//操作云台控制
	CTOS_SETPTZCTRL_MSG,
	STOC_SETPTZCTRL_MSG,

	//退出服务器消息
	CTOS_LOGINOFFSERVER_MSG,
	STOC_LOGINOFFSERVER_MSG,

	//客流统计消息
	STOC_PASSENGER_FLOW_MSG,
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class CTOS_CLIENTLOGIN 
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)sys_info.MAX_NAME_SIZE + 1)]
    public byte[] UserName;	//

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)sys_info.MAX_NAME_SIZE + 1)]
    public byte[] PassWD;	//

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)sys_info.MAX_IPADDRESS_SIZE + 1)]
    public byte[] IPadress;	//

    public UInt16 port;

	public UInt16 Protocol;

	public CTOS_CLIENTLOGIN(string name, string passwd, string ip, UInt16 _port,UInt16 _Protocol)
    {
        UserName = new byte[(int)sys_info.MAX_NAME_SIZE + 1];
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(name);
        if (bytes.Length > UserName.Length)
        {
            Debug.LogError("CTOS_CLIENTLOGIN : UserName is too long");
        }
        else
        {
            Buffer.BlockCopy(bytes, 0, UserName, 0, bytes.Length);
        }

        PassWD = new byte[(int)sys_info.MAX_NAME_SIZE + 1];
        byte[] bytePassWD = System.Text.Encoding.UTF8.GetBytes(passwd);
        if (bytePassWD.Length > PassWD.Length)
        {
            Debug.LogError("CTOS_CLIENTLOGIN : PassWD is too long");
        }
        else
        {
            Buffer.BlockCopy(bytePassWD, 0, PassWD, 0, bytePassWD.Length);
        }

        IPadress = new byte[(int)sys_info.MAX_NAME_SIZE + 1];
        byte[] byteIPadress = System.Text.Encoding.UTF8.GetBytes(ip);
        if (byteIPadress.Length > IPadress.Length)
        {
            Debug.LogError("CTOS_CLIENTLOGIN : PassWD is too long");
        }
        else
        {
            Buffer.BlockCopy(byteIPadress, 0, IPadress, 0, byteIPadress.Length);
        }

        port = _port;

		Protocol = _Protocol;
    }
}

public struct CMS_GUID
{
	public UInt32 Data1;
	public ushort Data2;
	public ushort Data3;
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	public byte[] Data4;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CAMARE_INFO
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)sys_info.MAX_CAMARENAME_SIZE + 1)]
	public byte[]		name;
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)sys_info.MAX_CAMARE_DESC_SZIE + 1)]
	public byte[]		describe;
	public CMS_GUID	camareGuid;	//摄像头的全局唯一 id
	public UInt16		bPTZ;		//是否是云台 0 没有云台 1有云台	
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTOS_PTZ_INFO
{
	public int cameraId;
	public CMS_GUID camareGuid;
	public UInt16 type;
	public UInt16 speedX;
	public UInt16 speedY;
	public UInt16 speedZ;
	public int nPreset;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTOS_SETPREPOSITION_INFO
{
	public int cameraId;
	public CMS_GUID camareGuid;
	public UInt16 index;
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
	public byte[] name;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CTOS_GTOPREPOSITION_INFO
{
	public int cameraId;
	public CMS_GUID camareGuid;
	public UInt16 index;
}

//返回打开摄像机 是否 成功
public struct STOC_OPENCAMERA
{
	public int cameraId;
	public CMS_GUID camareGuid;
}

//返回关闭摄像机 是否成功
public struct STOC_CLOSECAMERA
{
	public int cameraId;
	public CMS_GUID camareGuid;	
}

//返回云台控制 是否成功
public struct STOC_SETPTZCTRL
{
	public int cameraId;
	public CMS_GUID camareGuid;
}

//返回设置预设位 是否成功
public struct STOC_SETPREPOSITION
{
	public int cameraId;
	public CMS_GUID camareGuid;
}

//返回前往预置位 是否成功
public struct STOC_GOTOPREPOSITION
{
	public int cameraId;
	public CMS_GUID camareGuid;
}

//客流的 消息
struct STOC_PASSENGER_FLOW
{
	public int channel;     //视频通道
	public int RuleId;		//0 是 客流进入  1 客人离开
	public int count;		//变化的人数
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public byte[] ipFrom;
}
