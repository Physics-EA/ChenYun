using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 用来管理组记录的相关操作
/// </summary>
public class GroupRecordManage : MonoBehaviour {
	public static GroupRecordManage Instance;
	/// <summary>
	/// 新建组按钮
	/// </summary>
	public GameObject BtnNewGroup;
    /// <summary>
    /// 账户组UI界面管理脚本
    /// </summary>
    private GroupManageUIPanelControl groupUI;

	void Start () 
	{
		if(AuthorityHelper.NotAuthority(AuthorityHelper.USER_GROUP_MANAGEMENT))
		{
			BtnNewGroup.SetActive(false);
		}
		Instance = this;
        groupUI = gameObject.GetComponent<GroupManageUIPanelControl>();
        if (groupUI == null)
        {
            Debug.LogError("当前对象缺失 GroupManageUIPanelControl 脚本；对象名：" + gameObject.name);
        }
		LoadGroupRecord ();
	}

	void OnEnable()
	{

	}

	/// <summary>
	/// 加载组信息
	/// </summary>
	public void LoadGroupRecord()
	{
		Logger.Instance.WriteLog("加载用户组信息");
		List<GroupInfo> groups;
		GroupDao gpDao = new GroupDao ();
		gpDao.Select003 ();
		groups = gpDao.Result;
		GroupInfoRecord record;
		for(int i = 0; i < groups.Count; i++)
		{
			record = new GroupInfoRecord();
			record.No = groups[i].Id;
			record.GInfo = groups[i];
            //新UI添加账户组信息
            groupUI.AddAccount(record, i);
		}
		groupUI.accountParent.GetComponent<UIGrid> ().repositionNow = true;
		groupUI.accountParent.GetComponent<UIWidget>().UpdateAnchors(); 
	}
	/// <summary>
	/// 重新加载组信息
	/// </summary>
	public void ReloadGroupRecord()
	{
		Logger.Instance.WriteLog("重新加载用户组信息");
        groupUI.ClearAccount();
		LoadGroupRecord ();
	}

	/// <summary>
	/// 打开创建新组的界面
	/// </summary>
	public void CreateNewGroup()
	{
        
	}
}
