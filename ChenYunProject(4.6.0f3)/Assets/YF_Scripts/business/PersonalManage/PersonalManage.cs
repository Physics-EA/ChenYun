using UnityEngine;
using System.Collections;

public class PersonalManage : MonoBehaviour {
	/// <summary>
	/// 用来显示个人信息的对象
	/// </summary>
	public GameObject PersonalInfo;
	/// <summary>
	/// 用来修改密码的对象
	/// </summary>
	public GameObject PasswordModify;
	/// <summary>
	/// 显示个人信息的按钮
	/// </summary>
	public UIButton PersonalInfoBtn;
	/// <summary>
	/// 修改密码的按钮
	/// </summary>
	public UIButton PasswordModifyBtn;


	private string BtnNormalSprite;

	void Start()
	{
		BtnNormalSprite = PersonalInfoBtn.normalSprite;
	}

	public void Init()
	{
		ShowPersonalInfo();
	}

	/// <summary>
	/// 显示个人信息
	/// </summary>
	public void ShowPersonalInfo()
	{
		Logger.Instance.WriteLog("显示个人信息面板");
		PersonalInfo.SetActive (true);
		PersonalInfo.GetComponent<PersonalDetailInfoShow>().Init();
		PasswordModify.SetActive (false);

		PersonalInfoBtn.normalSprite = PersonalInfoBtn.pressedSprite;
		PersonalInfoBtn.GetComponent<BoxCollider>().enabled = false;

		PasswordModifyBtn.GetComponent<BoxCollider>().enabled = true;
		PasswordModifyBtn.normalSprite = BtnNormalSprite;
	}
	/// <summary>
	/// 显示修改密码
	/// </summary>
	public void ShowPasswordModify()
	{
		Logger.Instance.WriteLog("显示修改密码面板");
		PersonalInfo.SetActive (false);
		PasswordModify.SetActive (true);

		PasswordModifyBtn.normalSprite = PasswordModifyBtn.pressedSprite;
		PasswordModifyBtn.GetComponent<BoxCollider>().enabled = false;

		PersonalInfoBtn.GetComponent<BoxCollider>().enabled = true;
		PersonalInfoBtn.normalSprite = BtnNormalSprite;

	}
}
