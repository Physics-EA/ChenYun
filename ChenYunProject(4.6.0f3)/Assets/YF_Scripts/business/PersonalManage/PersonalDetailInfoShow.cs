using UnityEngine;
using System.Collections;

public class PersonalDetailInfoShow : MonoBehaviour {
	/// <summary>
	/// 账号名称
	/// </summary>
	public UILabel Account;
	/// <summary>
	/// 真实姓名
	/// </summary>
	public UILabel RealName;
	/// <summary>
	/// 用户角色
	/// </summary>
	public UILabel Role;
	/// <summary>
	/// 电话号码
	/// </summary>
	public UILabel Telphone;
	/// <summary>
	/// 家庭住址
	/// </summary>
	public UILabel Address1;
	/// <summary>
	/// 家庭住址
	/// </summary>
	public UILabel Address2;
	/// <summary>
	/// 备注
	/// </summary>
	public UILabel OtherMemo1;
	/// <summary>
	/// 备注
	/// </summary>
	public UILabel OtherMemo2;

	public void Init()
	{
		Account.text = DataStore.UserInfo.UserName;
		RealName.text = DataStore.UserInfo.RealName;
		Role.text = DataStore.GPInfo.Name;
		Telphone.text = DataStore.UserInfo.Telphone;
		if(DataStore.UserInfo.Address.Length > 20)
		{
			Address1.text = DataStore.UserInfo.Address.Substring(0,20);
			Address2.text = DataStore.UserInfo.Address.Substring(20);
		}
		else
		{
			Address1.text = DataStore.UserInfo.Address;
			Address2.text = "";
		}

		if(DataStore.UserInfo.Memo.Length > 20)
		{
			OtherMemo1.text = DataStore.UserInfo.Memo.Substring(0,20);
			OtherMemo2.text = DataStore.UserInfo.Memo.Substring(20);
		}
		else
		{
			OtherMemo1.text = DataStore.UserInfo.Memo;
			OtherMemo2.text = "";
		}
	}
}
