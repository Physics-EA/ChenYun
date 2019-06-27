using UnityEngine;
using System.Collections;
/// <summary>
/// 权限帮助类
/// </summary>
public static class AuthorityHelper
{
	/// <summary>
	/// 用户信息查看权限
	/// </summary>
	public const string USER_INFO_BROWSE = "UserInfoBrowse";
	/// <summary>
	/// 用户修改权限
	/// </summary>
	public const string USER_INFO_MANAGEMENT = "UserInfoManagement";
	/// <summary>
	/// 用户组信息查看权限
	/// </summary>
	public const string USER_GROUP_INFO_BROWSE = "UserGroupInfoBrowse";
	/// <summary>
	/// 用户组修改权限
	/// </summary>
	public const string USER_GROUP_MANAGEMENT = "UserGroupManagement";
	/// <summary>
	/// 巡航预案管理权限
	/// </summary>
	public const string PATROL_PLAN_MANAGEMENT = "PatrolPlanManagement";
	/// <summary>
	/// 设备管理权限
	/// </summary>
	public const string DEVICE_MANAGEMENT = "DeviceManagement";
	/// <summary>
	/// 日志管理
	/// </summary>
	public const string HISTORY_MANAGEMENT = "HistoryManagement";
	/// <summary>
	/// 疏散区域管理
	/// </summary>
	public const string EVACUATE_AREA_MANAGEMENT = "EvacuateAreaManagement";
	/// <summary>
	/// 疏散预案管理
	/// </summary>
	public const string EVACUATE_PLAN_MANAGEMENT = "EvacuatePlanManagement";

	/// <summary>
	/// 有指定权限时返回true
	/// </summary>
	public static bool HasAuthority(string AuthorityName)
	{
		foreach(AuthorityInfo info in DataStore.AuthorityInfos)
		{
			if(info.Authority == AuthorityName)
			{
				return true;
			}
		}
		return false;
	}
	/// <summary>
	/// 没有指定权限时返回true
	/// </summary>
	public static bool NotAuthority(string AuthorityName)
	{
		return !HasAuthority (AuthorityName);
	}
}
