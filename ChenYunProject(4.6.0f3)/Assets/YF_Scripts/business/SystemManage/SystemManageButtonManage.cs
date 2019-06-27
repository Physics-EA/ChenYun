using UnityEngine;
using System.Collections;
/// <summary>
/// 管理系统管理功能的相关按钮
/// </summary>
public class SystemManageButtonManage : MonoBehaviour {
	/// <summary>
	/// 用户管理按钮
	/// </summary>
	public GameObject BtnUserManage;
	/// <summary>
	/// 用户组管理按钮
	/// </summary>
	public GameObject BtnGroupManage;
	/// <summary>
	/// 巡航预案管理按钮
	/// </summary>
	public GameObject BtnCruiseManage;
	/// <summary>
	/// 设备管理按钮
	/// </summary>
	public GameObject BtnDeviceMange;
	/// <summary>
	/// 日志管理按钮
	/// </summary>
	public GameObject BtnLogManage;
	/// <summary>
	/// 判断用户是否有相应的操作权限，如果没有则将相应的按钮禁用掉
	/// </summary>
	void OnEnable()
	{

		if(AuthorityHelper.NotAuthority (AuthorityHelper.USER_INFO_BROWSE)
		   &&AuthorityHelper.NotAuthority (AuthorityHelper.USER_INFO_MANAGEMENT))
		{
			BtnUserManage.SetActive (false);
		}

		if(AuthorityHelper.NotAuthority (AuthorityHelper.USER_GROUP_INFO_BROWSE)
		   &&AuthorityHelper.NotAuthority (AuthorityHelper.USER_GROUP_MANAGEMENT))
		{
			BtnGroupManage.SetActive (false);
		}
		
		if(AuthorityHelper.NotAuthority (AuthorityHelper.EVACUATE_PLAN_MANAGEMENT))
		{
			BtnCruiseManage.SetActive (false);
		}

		if(AuthorityHelper.NotAuthority (AuthorityHelper.DEVICE_MANAGEMENT))
		{
			BtnDeviceMange.SetActive (false);
		}

		
		if(AuthorityHelper.NotAuthority (AuthorityHelper.HISTORY_MANAGEMENT))
		{
			BtnLogManage.SetActive (false);
		}
	}
}
