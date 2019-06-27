using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 存储系统运行时当前登录用户的相关信息
/// </summary>
public class DataStore : MonoBehaviour {
	/// <summary>
	/// 存储登陆用户信息
	/// </summary>
	public static UserBasicInfo UserInfo;
	/// <summary>
	/// 存储登陆用户所在组信息
	/// </summary>
	public static GroupInfo GPInfo;
	/// <summary>
	/// 存储登陆用户的权限信息
	/// </summary>
	public static List<AuthorityInfo> AuthorityInfos = new List<AuthorityInfo>();
	/// <summary>
	/// 存储当前操作的功能
	/// </summary>
	public static Operation CurrentOperate;
}
