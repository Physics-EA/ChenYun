using UnityEngine;
using System.Collections;
/// <summary>
/// 实现修改密码相关功能
/// </summary>
public class PasswordModify : MonoBehaviour {
	public GameObject InputArea;
	public GameObject HitArea;
	/// <summary>
	/// 旧密码输入框
	/// </summary>
	public UIInput OldPassword;
	/// <summary>
	/// 新密码输入框
	/// </summary>
	public UIInput NewPassword;
	/// <summary>
	/// 新密码提示图标
	/// </summary>
	public UISprite NewPwdHitIcon;
	/// <summary>
	/// 新密码提示文本
	/// </summary>
	public UILabel NewPwdHitText;
	/// <summary>
	/// 确认密码输入框
	/// </summary>
	public UIInput ConfirmPassword;
	/// <summary>
	/// 确认密码提示图标
	/// </summary>
	public UISprite ConfirmPwdHitIcon;
	/// <summary>
	/// 确认密码提示文本
	/// </summary>
	public UILabel ConfirmPwdHitText;
	/// <summary>
	/// 显示账号名称的标签
	/// </summary>
	public UILabel Account;
	/// <summary>
	/// 保存输入的旧密码
	/// </summary>
	private string oldPasswd;
	/// <summary>
	/// 保存输入的新密码
	/// </summary>
	private string newPasswd;
	/// <summary>
	/// 保存输入的确认密码
	/// </summary>
	private string cfmPasswd;
	/// <summary>
	/// 判定输入的旧密码是否正确
	/// </summary>
	private bool isOldPasswdOk;
	/// <summary>
	/// 判定输入的新密码是否正确
	/// </summary>
	private bool isNewPasswdOk;
	/// <summary>
	/// 判定输入的确认密码是否正确
	/// </summary>
	private bool isCfmPasswdOk;
	/// <summary>
	/// 允许输入的合法字符
	/// </summary>
	private string LegalSign = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_abcdefghijklmnopqrstuvwxyz";
	
	/// <summary>
	/// 判断输入的旧密码是否符合规则
	/// </summary>
	/// <param name="input">Input.</param>
	/// <param name="HitIcon">Hit icon.</param>
	/// <param name="hitInfo">Hit info.</param>
	public void OnOldPasswdInputChange(UIInput input,UISprite HitIcon, UILabel hitInfo)
	{
		HitArea.SetActive(false);
		Check (input, HitIcon, hitInfo, out oldPasswd, out isOldPasswdOk);
	}
	/// <summary>
	/// 检查输入的旧密码是否正确
	/// </summary>
	/// <param name="input">Input.</param>
	/// <param name="HitIcon">Hit icon.</param>
	/// <param name="hitInfo">Hit info.</param>
	public void OnOldPasswdInputDeselect(UIInput input,UISprite HitIcon, UILabel hitInfo)
	{
		if(oldPasswd != DataStore.UserInfo.Password && isOldPasswdOk)
		{
			HitIcon.enabled = false;
			hitInfo.text = "密码输入错误";
			isOldPasswdOk = false;
			return;
		}

		if(oldPasswd == DataStore.UserInfo.Password )
		{
			HitIcon.enabled = true;
		}
	}
	/// <summary>
	/// 判断输入的新密码是否符合规则
	/// </summary>

	public void OnNewPasswdInputChange()
	{
		HitArea.SetActive(false);
		Check (NewPassword, NewPwdHitIcon, NewPwdHitText, out newPasswd, out isNewPasswdOk);
		if(isNewPasswdOk)NewPwdHitIcon.enabled = true;
	}
	/// <summary>
	/// 判断输入的新密码是否符合规则
	/// </summary>
	
	public void OnNewPasswdInputDeselect()
	{
		HitArea.SetActive(false);
		OnCfmPasswdInputDeselect();
	}
	/// <summary>
	/// 判断输入的新密码确认密码是否符合规则
	/// </summary>

	public void OnCfmPasswdInputChange()
	{
		HitArea.SetActive(false);
		Check (ConfirmPassword, ConfirmPwdHitIcon, ConfirmPwdHitText, out cfmPasswd, out isCfmPasswdOk);
		if(isCfmPasswdOk)ConfirmPwdHitIcon.enabled = true;
	}

	/// <summary>
	/// 判断输入的新密码和确认密码是相同
	/// </summary>
	public void OnCfmPasswdInputDeselect()
	{
		Check (ConfirmPassword, ConfirmPwdHitIcon, ConfirmPwdHitText, out cfmPasswd, out isCfmPasswdOk);
		if(isCfmPasswdOk)
		{
			if(ConfirmPassword.value != newPasswd )
			{
				ConfirmPwdHitText.text = "密码不一致";
				ConfirmPwdHitIcon.enabled = false;
				isCfmPasswdOk = false;
				return;
			}

			ConfirmPwdHitIcon.enabled = true;
		}
	}
	/// <summary>
	/// 提交密码修改请求
	/// </summary>
	public void OnConfirm()
	{
		Logger.Instance.WriteLog("提交用户密码修改");
		if(isOldPasswdOk && isNewPasswdOk && isCfmPasswdOk)
		{
			Logger.Instance.WriteLog("保存用户密码修改");
			UserBasicDao ubdao = new UserBasicDao();
			ubdao.Update001(newPasswd,DataStore.UserInfo.ID);
			DataStore.UserInfo.Password = newPasswd;
			UIInput[] inputs = InputArea.GetComponentsInChildren<UIInput> ();
			foreach(UIInput ipt in inputs)
			{
				ipt.value = "";
				UILabel label = ipt.GetComponentInChildren<UILabel> ();
				label.text = "";
			}
			HitArea.SetActive(true);
			//OldPassword.gameObject.SetActive(false);
			return;
		}
		Logger.Instance.WriteLog("用户密码修改失败");
		return;
	}
	private void ClearInputText()
	{

	}
	/// <summary>
	/// 取消密码修改
	/// </summary>
	public void OnCancel()
	{
		Logger.Instance.WriteLog("取消用户密码修改");
		gameObject.SetActive(false);
	}
	/// <summary>
	/// 判断输入的密码是否符合规则
	/// </summary>
	/// <param name="input">Input.</param>
	/// <param name="HitIcon">Hit icon.</param>
	/// <param name="hitInfo">Hit info.</param>
	private void Check(UIInput input,UISprite HitIcon, UILabel hitInfo,out string outText,out bool isPasswdOk)
	{
		isPasswdOk = false;
		outText = input.value;
		HitIcon.enabled = false;
		if(NotContainIllegalCharacter(outText))
		{
			hitInfo.text = "请输入合法字符";
			return;
		}
		if(outText.Length < 6)
		{
			hitInfo.text = "请输入6-20个字符";
			return;
		}
		hitInfo.text = "";
		isPasswdOk = true;
	}
	/// <summary>
	/// 如果给定字符串包含违法字符，则返回true否则返回true。
	/// </summary>
	/// <returns><c>true</c>, if contain illegal character was noted, <c>false</c> otherwise.</returns>
	/// <param name="str">String.</param>
	private bool  NotContainIllegalCharacter(string str)
	{
		for(int i = 0; i < str.Length; i++)
		{
			if(LegalSign.Contains(str.Substring(i,1)))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	void OnEnable()
	{
		Account.text = DataStore.UserInfo.UserName;
		HitArea.SetActive(false);
		OldPassword.gameObject.SetActive(true);
		UIInput[] inputs = InputArea.GetComponentsInChildren<UIInput> ();
		foreach(UIInput ipt in inputs)
		{
			ipt.value = "";
			UILabel label = ipt.GetComponentInChildren<UILabel> ();
			label.text = "";
		}

	}


}
