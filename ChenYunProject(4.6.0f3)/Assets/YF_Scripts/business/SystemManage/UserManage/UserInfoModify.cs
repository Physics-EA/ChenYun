using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 修改用户信息
/// </summary>
public class UserInfoModify : MonoBehaviour {
	/// <summary>
	/// 用来显示组信息的下拉列表
	/// </summary>
	public UIPopupList PPLRole;
	/// <summary>
	/// 显示账号名称的标签
	/// </summary>
	public UILabel LBLAccount;
	/// <summary>
	/// 用来显示用户创建时间的标签
	/// </summary>
	public UILabel LBLCreateTime;
	/// <summary>
	/// 显示密码的标签
	/// </summary>
	public UILabel LBLPassword;
	/// <summary>
	/// 用户家庭住址的输入框
	/// </summary>
	public UIInput IPTAddress;
	/// <summary>
	/// 显示用户真实姓名的标签
	/// </summary>
	public UILabel IPTRealName;
	/// <summary>
	/// 用户电话号码的输入框
	/// </summary>
	public UIInput IPTTelphone;
	/// <summary>
	/// 备注输入框
	/// </summary>
	public UIInput IPTMemo;
	/// <summary>
	/// 用户信息修改相关的按钮
	/// </summary>
	public GameObject BtnContainer;
	/// <summary>
	/// 保存当前显示的记录信息
	/// </summary>
	UserInfoRecord UIRecord;
	void Start()
	{
		//如果用户没有管理用的权限则把相应的输入框禁用掉
		if(AuthorityHelper.NotAuthority(AuthorityHelper.USER_INFO_MANAGEMENT))
		{
			ForbiddenModify();
		}
	}

	void ForbiddenModify()
	{
		PPLRole.GetComponent<BoxCollider>().enabled = false;
		IPTAddress.GetComponent<BoxCollider>().enabled = false;
		IPTTelphone.GetComponent<BoxCollider>().enabled = false;
		IPTMemo.GetComponent<BoxCollider>().enabled = false;
		LBLPassword.transform.FindChild("ResetPassword").gameObject.SetActive(false);
		PPLRole.transform.FindChild("Background").gameObject.SetActive(false);
		IPTAddress.transform.FindChild("Background").gameObject.SetActive(false);
		IPTTelphone.transform.FindChild("Background").gameObject.SetActive(false);
		IPTMemo.transform.FindChild("Background").gameObject.SetActive(false);
		BtnContainer.SetActive(false);
	}
	/// <summary>
	/// 设置指定的数据进行显示
	/// </summary>
	/// <param name="UIRecord">User interface record.</param>
	public void SetValue(UserInfoRecord UIRecord)
	{
		Logger.Instance.WriteLog("显示用户相关信息");
		this.UIRecord = UIRecord;
		if (UIRecord.GInfo.Name == "超级管理员" || UIRecord.UBInfo.UserName == DataStore.UserInfo.UserName) 
		{
			ForbiddenModify();
		}

		GroupDao gpDao = new GroupDao ();
		gpDao.Select003 ();
		List<GroupInfo> gpInfos = gpDao.Result;
		foreach(GroupInfo info in gpInfos)
		{
			PPLRole.AddItem(info.Name);
		}

		PPLRole.value = UIRecord.GInfo.Name;
		LBLAccount.text = UIRecord.UBInfo.UserName;
		LBLCreateTime.text = System.DateTime.Parse(UIRecord.UBInfo.CreateTime).ToString("yyyy年MM月dd日");
		LBLPassword.text = "**********";
		IPTAddress.value = UIRecord.UBInfo.Address;
		IPTRealName.text = UIRecord.UBInfo.RealName;
		IPTTelphone.value = UIRecord.UBInfo.Telphone;
		IPTMemo.value = UIRecord.UBInfo.Memo;

		if(UIRecord.UBInfo.Status == "禁用")
		{
			BtnContainer.transform.FindChild("BtnForbidden").GetComponentInChildren<UILabel>().text = "启用";
		}
	}
	/// <summary>
	/// 禁用用户
	/// </summary>
	public void Forbidden()
	{
		Logger.Instance.WriteLog("禁用当前用户");
		UserBasicDao ubdao = new UserBasicDao ();
		if(UIRecord.UBInfo.Status == "禁用")
		{
			ubdao.Update002 ("正常", UIRecord.UBInfo.ID);
		}
		else
		{
			ubdao.Update002 ("禁用", UIRecord.UBInfo.ID);
		}
		UserRecordManage.Instance.GetComponent<UserRecordManage> ().ReLoadUserRecord ();
		Destroy (gameObject);
	}
	/// <summary>
	/// 提交数据可修改相关信息
	/// </summary>
	public void Confirm()
	{
		Logger.Instance.WriteLog("保存修改后的用户信息");
		UserBasicDao ubdao = new UserBasicDao ();
		ubdao.Update003 (IPTRealName.text, UIRecord.UBInfo.Password, IPTTelphone.value, 
		                 IPTAddress.value, IPTMemo.value, UIRecord.UBInfo.ID);

		GroupDao gpDao = new GroupDao ();
		gpDao.Select003 ();
		List<GroupInfo> gpInfos = gpDao.Result;
		foreach(GroupInfo info in gpInfos)
		{
			if(PPLRole.value == info.Name)
			{
				UserGroupDao ugDao = new UserGroupDao ();
				ugDao.Update001 (info.Id,UIRecord.UBInfo.ID);
				break;
			}
		}

		UserRecordManage.Instance.GetComponent<UserRecordManage> ().ReLoadUserRecord ();
		Destroy (gameObject);
	}
	/// <summary>
	/// 重置用户密码
	/// </summary>
	public void ResetPassword()
	{
		Logger.Instance.WriteLog("重置用户密码");
		UIRecord.UBInfo.Password = "1234567";
	}
	/// <summary>
	/// 删除当前用户
	/// </summary>
	public void DeleteUser()
	{
		Logger.Instance.WriteLog("删除当前用户");
		UserBasicDao ubdao = new UserBasicDao ();
		ubdao.Delete001 (UIRecord.UBInfo.ID);

		UserGroupDao ugDao = new UserGroupDao ();
		ugDao.Delete001 (UIRecord.UBInfo.ID);

		UserRecordManage.Instance.GetComponent<UserRecordManage> ().ReLoadUserRecord ();
		Destroy (gameObject);

	}
	/// <summary>
	/// 关闭窗口
	/// </summary>
	public void Cancel()
	{
		Destroy (gameObject);
	}
}
