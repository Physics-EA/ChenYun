using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 用来创建新的组
/// </summary>
public class NewGroup : MonoBehaviour {
	/// <summary>
	/// 组名称输入框
	/// </summary>
	public UIInput IPTGroupName;
	/// <summary>
	/// 保存权限列表项目的载体
	/// </summary>
	public UIGrid Authoritys;
	/// <summary>
	/// 权限列表项目预制体
	/// </summary>
	public GameObject AuthorityItemPrefab;
	/// <summary>
	/// 保存从数据库中读取的权限信息
	/// </summary>
	private List<AuthorityInfo> AuthorityInfos;
	/// <summary>
	/// 初始化相关信息
	/// </summary>
	void Start()
	{
		Logger.Instance.WriteLog("加载权限信息");
		AuthorityDao authDao = new AuthorityDao ();
		authDao.Select001 ();
		AuthorityInfos = authDao.Result;
		foreach(AuthorityInfo info in AuthorityInfos)
		{
			GameObject go = Instantiate(AuthorityItemPrefab) as GameObject;
			go.GetComponent<AuthorityItem>().SetValue(info);
			Authoritys.AddChild(go.transform);
			go.transform.localScale = new Vector3(1,1,1);
		}
	}
	/// <summary>
	/// 向数据库插入新的数据
	/// </summary>
	public void Comfirm()
	{
		Logger.Instance.WriteLog("保存用户组信息");
		GroupDao gpDao = new GroupDao ();
		gpDao.Insert001 (IPTGroupName.value);
		if(Authoritys)
		{
			gpDao.Select004 (IPTGroupName.value);
			string groupId = gpDao.Result [0].Id;
			GroupAuthorityDao gaDao = new GroupAuthorityDao ();
			AuthorityItem[] AuthorityItems = Authoritys.GetComponentsInChildren<AuthorityItem>();
			foreach(AuthorityItem item in AuthorityItems)
			{
				if(item.isSelected)
				{
					gaDao.Insert001 (groupId,item.AuthInfo.Id);
				}
			}
		}
		GroupRecordManage.Instance.ReloadGroupRecord ();
		Close ();
	}

	/// <summary>
	/// 关闭创建窗口
	/// </summary>
	public void Close()
	{
		Logger.Instance.WriteLog("关闭用户组创建窗口");
		Destroy (gameObject);
	}
}
