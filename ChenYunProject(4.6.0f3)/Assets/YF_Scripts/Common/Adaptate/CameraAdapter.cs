using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ZoomOperate
{
	IN,     //镜头拉近
	OUT     //镜头拉远
}

public enum TurnOperate
{
	LEFT,   //镜头向左旋转
	RIGHT,  //镜头向右旋转
	UP,     //镜头向上旋转
	DOWN    //镜头向下旋转
}

/// <summary>
/// 摄像机的信息
/// </summary>
public struct CameraInfo
{
	string Name;	//相机名称
	bool status;	//当前摄像机的状态
	bool HasPTZ;	//是否可以进行PTZ控制
	
}

public interface CameraAdapter
{
	/// <summary>
	/// 登陆服务器
	/// </summary>
	/// <returns>成功返回true</returns>
	bool LogonServer(params string[] pars);

	/// <summary>
	/// 获取摄像机列表
	/// </summary>
	/// <returns>The camera list.</returns>
	List<CameraInfo> GetCameraList();

	/// <summary>
	/// 打开指定的摄像头
	/// </summary>
	/// <param name="CameraName">摄像机名称</param>
	/// <returns>成功返回true</returns>
	bool Open(string CameraName);
	
	/// <summary>
	/// 关闭指定的摄像头
	/// </summary>
	/// <param name="CameraName">摄像机名称</param>
	void Close(string CameraName);
	
	/// <summary>
	/// 调整指定摄像机镜头的远近
	/// </summary>
	/// <param name="CameraName">摄像机名称</param>
	/// <param name="Operate">操作</param>
	/// <param name="Speed">速度</param>
	void Zoom(string CameraName, ZoomOperate Operate,float Speed);
	/// <summary>
	/// 停止调整指定摄像机镜头的远近
	/// </summary>
	/// <param name="CameraName">Camera name.</param>
	void StopZoom(string CameraName);
	/// <summary>
	/// 旋转摄像头
	/// </summary>
	/// <param name="CameraName">摄像机名称</param>
	/// <param name="Opertate">操作</param>
	/// <param name="Speed">速度</param>
	void Turn(string CameraName, TurnOperate Opertate,float Speed);
	/// <summary>
	/// 停止旋转摄像头
	/// </summary>
	/// <param name="CameraName">Camera name.</param>
	void StopTurn(string CameraName);
	/// <summary>
	/// 捕捉指定摄像机的图像
	/// </summary>
	/// <param name="CameraName">摄像机名称</param>
	/// <returns>图片信息</returns>
	byte[] CaptureImage(string CameraName);

	/// <summary>
	/// 设置预设位置
	/// </summary>
	/// <param name="name">摄像机名称</param>
	/// <returns>成功返回true</returns>
	bool SetPresetPos(string CameraName);

	/// <summary>
	/// 转向预设位置
	/// </summary>
	/// <param name="name">预设位名称</param>
	/// <returns>成功返回true</returns>
	bool GotoPresetPos(string CameraName);

	/// <summary>
	/// 退出服务器
	/// </summary>
	void LogoutService();
}
