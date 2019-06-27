using UnityEngine;
using System.Collections;


public enum emUsrState : byte
{
	/// <summary>
	/// 禁用状态
	/// </summary>
	UsrState_BEGIN = 0,
	/// <summary>
	/// 禁用状态
	/// </summary>
	UsrState_FORBIDDEN = 1,
	/// <summary>
	/// 正常状态
	/// </summary>
	UsrState_NORMAL=2,
}

/// <summary>
/// 仅仅用来存放 当前用户的相关信息，和仅仅限于这个用户的操作！！！
/// </summary>
public class user_info {

//输入事件相关定义
	//鼠标左键事件
	public const int MOUSE_LEFT_BUTTON = 0 ;
	//鼠标右键事件
	public const int MOUSE_RIGHT_BUTTON = 0 ;
	//鼠标中键事件
	public const int MOUSE_MIDDLE_BUTTON = 0 ;


//当前使用者的基本信息

	//工号
	public int empNo;
	//员工姓名
	public string empName;
	//用户电话号码
	public string phoneNo;
	//用户所属分组---将关联到分组权限表  
	public int groupId;
	//由管理员输入的备注信息
	public string bz;
	//状态

	public static user_info instance;
	public static user_info Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new user_info();
			}
			return instance;
		}
	}
}
