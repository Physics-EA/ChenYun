using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 修改组的相关信息
/// </summary>
public class GroupInfoModify : MonoBehaviour {

	/// <summary>
	/// 保存从数据库中读取的权限信息
	/// </summary>
	private List<AuthorityInfo> AuthorityInfos;
	/// <summary>
	/// 保存组与权限关联表的信息
	/// </summary>
	private List<GroupAuthorityInfo> GroupAuthorityInfos;
	/// <summary>
	/// 保存当前显示的组信息
	/// </summary>
	private GroupInfo groupInfo;
    /// <summary>
    /// 账户组UI界面管理脚本
    /// </summary>
    [HideInInspector]
    public GroupManageUIPanelControl groupUI;


	void Awake()
	{
		groupUI = gameObject.GetComponent<GroupManageUIPanelControl> ();
	}

	/// <summary>
	/// 将给定的数据进行相关设置
	/// </summary>
	/// <param name="AuthInfo">Auth info.</param>
	public void SetValue(GroupInfo groupInfo)
	{
		Logger.Instance.WriteLog("初始化用户组信息");
		bool notGroupManageAuth = AuthorityHelper.NotAuthority (AuthorityHelper.USER_GROUP_MANAGEMENT);
		this.groupInfo = groupInfo;
        //LBLGroupName.text = groupInfo.Name;

		//检索当前组的权限信息
		Logger.Instance.WriteLog("检索当前组的权限信息");
		GroupAuthorityDao gaDao = new GroupAuthorityDao ();
		gaDao.Select001 (groupInfo.Id);
		GroupAuthorityInfos = gaDao.Result;

		//检索所有权限信息
		Logger.Instance.WriteLog("检索所有权限信息");
		AuthorityDao authDao = new AuthorityDao ();
		authDao.Select001 ();
		AuthorityInfos = authDao.Result;

		//生成权限的信息
		foreach(AuthorityInfo aInfo in AuthorityInfos)
		{
            Logger.Instance.WriteLog("生成用户组权限的信息");
            bool b = false;

            ////选中当前组已有的权限
            foreach (GroupAuthorityInfo gaInfo in GroupAuthorityInfos)
            {
                if (aInfo.Id == gaInfo.AuthorityId)
                {
                    b = true;
                    break;
                }
            }

            groupUI.AddAuthority(aInfo, b);

            ////如果当前组已经禁用或者没有管理权限，则禁用权限选中功能
            //if (notGroupManageAuth)
            //{
            //    go.GetComponent<AuthorityItem>().Disable();
            //}
     
		}
	}
	/// <summary>
	/// 处理组权限更改数据
	/// </summary>
	public void Comfirm()
	{
		if(groupInfo.Status.Equals("新增"))
		{
			Logger.Instance.WriteLog("不保存未确认的新增用户组权限信息");
			return;
		}

        Logger.Instance.WriteLog("保存用户组信息");
		string groupId = groupInfo.Id;
		GroupAuthorityDao gaDao = new GroupAuthorityDao ();
		gaDao.Delete002(groupId);

		foreach(string item in groupUI.tgList.Keys)
		{
			if(groupUI.tgList[item].value)
			{
				gaDao.Insert001 (groupId,item);
			}
		}

	}

	/// <summary>
	/// 处理新增数据
	/// </summary>
	/// <param name="ginfo">Ginfo.</param>
	public void Confirm(GroupInfo ginfo)
	{
		Logger.Instance.WriteLog("更新新增用户组信息");
		groupInfo = ginfo;
	}

	/// <summary>
	/// 删除当前组的数据
	/// </summary>
	public void Delete()
	{
		Logger.Instance.WriteLog("删除当前用户组的相关数据");
		string groupId = groupInfo.Id;
		//删除当前组
		GroupDao gDao = new GroupDao ();
		gDao.Delete001 (groupId);

		//删除与当前组相关联的权限信息
		GroupAuthorityDao gaDao = new GroupAuthorityDao ();
		gaDao.Delete002(groupId);
		groupUI.DeleteSelect ();
//		GroupRecordManage.Instance.ReloadGroupRecord ();

	}

	/// <summary>
	/// 如果组已经禁用则启用组，否则禁用组
	/// </summary>
	public void Forbidden()
	{
        //string groupId = groupInfo.Id;
        //GroupDao gDao = new GroupDao ();
        //if(IsForbidden)
        //{
        //    gDao.Update001 (groupId,"正常");
        //}
        //else
        //{
        //    gDao.Update001 (groupId,"禁用");
        //}
        //GroupRecordManage.Instance.ReloadGroupRecord ();
        //Close ();
	}
	/// <summary>
	/// 关闭修改窗口
	/// </summary>
	public void Close()
	{
        //Logger.Instance.WriteLog("关闭用户组信息修改窗口");
        //Destroy (gameObject);
	}
}
