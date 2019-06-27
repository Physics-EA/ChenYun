using UnityEngine;
using System.Collections;
using System.Data;
public class DevicEditLogin : MonoBehaviour {
	/// <summary>
	/// 加载界面
	/// </summary>
	public GameObject LoadingUI;
	public UIInput Account;
	public UIInput Password;

	void Update()
	{
		if(Input.GetKey(KeyCode.Tab))
		{
			if(Account.isSelected)
			{
				Password.isSelected = true;
			}
		}
	}

	public void Login()
	{
		Logger.Instance.WriteLog("登陆编辑场景");
		string sql = "select * from all_users where username = '" + Account.value.ToUpper() + "'";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "select * from user where user = '" + Account.value.Trim() + "'";
		}
		DataSet ds =  OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql,"allusers");
		if(ds.Tables[0].Rows.Count >= 1)
		{
			int ret = OdbcDataManager.Instance.Reconnection(Account.value,Password.value);
			if(ret != 0)
			{
				return;
			}
			LoadingUI.SetActive (true);
			LoadingUI.SendMessage ("LoadLevel","DeviceEditScene");
		}
		else
		{
			Logger.Instance.WriteLog("登陆编辑场景失败");
		}
	}

	public void Exit()
	{
		Logger.Instance.WriteLog("退出应用程序");
		Application.Quit();
	}
}
