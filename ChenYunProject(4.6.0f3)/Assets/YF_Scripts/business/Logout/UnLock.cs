using UnityEngine;
using System.Collections;

public class UnLock : MonoBehaviour {

	public UIInput Password;

	void OnEnable()
	{
		Password.value = "";
	}

	public void Confirm()
	{
		Logger.Instance.WriteLog("用户解锁");
		if(Password.value.Trim() == DataStore.UserInfo.Password)
		{
			transform.parent.gameObject.SetActive(false);
		}
	}
}
