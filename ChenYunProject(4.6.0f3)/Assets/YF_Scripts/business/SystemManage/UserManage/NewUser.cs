using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 增加新的用户
/// </summary>
public class NewUser : MonoBehaviour {
	/// <summary>
	/// 用户账号名输入框
	/// </summary>
	public UIInput IPTAccount;
	/// <summary>
	/// 密码输入框
	/// </summary>
	public UIInput IPTPassword;
	/// <summary>
	/// 用户地址输入框
	/// </summary>
	public UIInput IPTAddress;
	/// <summary>
	/// 用户真实姓名输入框
	/// </summary>
	public UIInput IPTRealName;
	/// <summary>
	/// 用户电话号码输入框
	/// </summary>
	public UIInput IPTTelphone;
	/// <summary>
	/// 备注输入框
	/// </summary>
	public UIInput IPTMemo;
	/// <summary>
	/// 用户组下拉列表
	/// </summary>
	public UIPopupList PPLGroup;
	/// <summary>
	/// 存储可用组的信息
	/// </summary>
	private List<GroupInfo> Groups;
	void Start()
	{
		IPTPassword.value = "1234567";
		GroupDao gpDao = new GroupDao ();
		gpDao.Select003 ();
		Groups = gpDao.Result;
		foreach(GroupInfo info in Groups)
		{
			PPLGroup.AddItem(info.Name);
		}
	}
	/// <summary>
	/// 确定创建用户
	/// </summary>
	public void Comfirm()
	{
		Logger.Instance.WriteLog("保存新创建的用户信息");
		UserBasicDao ubdao = new UserBasicDao ();
		ubdao.Insert001 (IPTAccount.value, IPTPassword.value, IPTRealName.value,
		                IPTTelphone.value, System.DateTime.Now.ToString ("yyyy年MM月dd日 HH:mm:ss"),
		                IPTAddress.value, "正常", IPTMemo.value);

		ubdao.Select003 (IPTAccount.value);
		UserBasicInfo ubInfo = ubdao.Result [0];

		string goupId = "1";
		foreach(GroupInfo info in Groups)
		{
			if(PPLGroup.value == info.Name)
			{
				goupId = info.Id;
				break;
			}
		}

		UserGroupDao ugpDao = new UserGroupDao ();
		ugpDao.Insert001 (ubInfo.ID, goupId);

		UserRecordManage.Instance.GetComponent<UserRecordManage> ().ReLoadUserRecord ();
		Destroy (gameObject);
	}
	/// <summary>
	/// 关闭创建窗口
	/// </summary>
	public void Close()
	{
		Destroy (gameObject);
	}
}
