using UnityEngine;
using System.Collections;

public class ChangeAccount : MonoBehaviour {

	public UILabel CurrentAccount;
	public UIInput CurrentPassword;

	public UIInput NewAccount;
	public UIInput NewPassword;

	public GameObject LoginInfo;

	public GameObject MainMenuPanel;

	public GameObject Junction;
	private bool HasJunction = true;
	private string DefaultSprite;
	void OnEnable()
	{
		DefaultSprite = Junction.GetComponent<UIButton> ().normalSprite;
		CurrentAccount.text = DataStore.UserInfo.UserName;
		CurrentPassword.value = "";
		NewAccount.value = "";
		NewPassword.value = "";
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			if(CurrentPassword.isSelected)
			{
				NewAccount.isSelected = true;
				return;
			}
			if(NewAccount.isSelected)
			{
				NewPassword.isSelected = true;
			}
		}
	}

	public void Login()
	{
		Logger.Instance.WriteLog("用户登录");
		UserBasicDao ubdDao = new UserBasicDao ();

		if(CurrentPassword.value.Trim() == DataStore.UserInfo.Password || HasJunction == false)
		{
			ubdDao.Select001 (NewAccount.value.Trim(),NewPassword.value.Trim());
			if(ubdDao.Result.Count == 1)
			{
				GroupDao gpDao = new GroupDao();
				gpDao.Select002(ubdDao.Result[0].ID);
				if(gpDao.Result.Count == 1)
				{
					DataStore.UserInfo = ubdDao.Result[0];
					DataStore.GPInfo = gpDao.Result[0];

					//检索用户所属组的权限列表
					GroupAuthorityDao gaDao = new GroupAuthorityDao();
					gaDao.Select001(DataStore.GPInfo.Id);
					//检索权限信息
					AuthorityDao aDao = new AuthorityDao();
					aDao.Select001();


					DataStore.AuthorityInfos.Clear();
					//将用户的权限详细信息保存下来
					foreach(GroupAuthorityInfo gaInfo in gaDao.Result)
					{
						foreach(AuthorityInfo aInfo in aDao.Result)
						{
							if(gaInfo.AuthorityId == aInfo.Id)
							{
								DataStore.AuthorityInfos.Add(aInfo);
								break;
							}
						}
					}

					Junction.GetComponent<UIButton>().normalSprite = DefaultSprite;
					HasJunction = true;
					LoginInfo.GetComponent<LoginInfo>().UpdateInfo();
					MainMenuPanel.GetComponent<MainMenuController>().UpdateInfo();
					gameObject.SetActive(false);
				}
			}
		}
	}


	public void ChangeStatus()
	{
		if(HasJunction)
		{
			Junction.GetComponent<UIButton>().normalSprite = Junction.GetComponent<UIButton>().hoverSprite;
			HasJunction = false;
		}
		else
		{
			Junction.GetComponent<UIButton>().normalSprite = DefaultSprite;
			HasJunction = true;
		}
	}

	public void Cancel()
	{
		Junction.GetComponent<UIButton>().normalSprite = DefaultSprite;
		HasJunction = true;
		gameObject.SetActive(false);
	}
}
