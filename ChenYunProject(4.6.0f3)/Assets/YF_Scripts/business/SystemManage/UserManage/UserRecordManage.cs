using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 对用户信息记录，进行相关的操作
/// </summary>
public class UserRecordManage : MonoBehaviour {
	/// <summary>
	/// 对这个类的对象的一个全局引用
	/// </summary>
	public static UserRecordManage Instance;
	/// <summary>
	/// 用来显示用户相关记录的预制体
	/// </summary>
	public GameObject RecordItemPrefab;
	/// <summary>
	/// 挂载用户记录的对象
	/// </summary>
	public UIGrid Records;
    /// <summary>
    /// 账户名称输入框
    /// </summary>
    public UIInput accountName;
    /// <summary>
    /// 真实姓名输入框
    /// </summary>
    public UIInput userName;
    /// <summary>
    /// 备注信息输入框
    /// </summary>
    public UIInput remark;
    /// <summary>
    /// 家庭地址输入框
    /// </summary>
    public UIInput userAdd;
    /// <summary>
    /// 账户密码输入框
    /// </summary>
    public UIInput accountPsw;
    /// <summary>
    /// 电话号码输入框
    /// </summary>
    public UIInput userMobile;
    /// <summary>
    /// 创建时间文本框
    /// </summary>
    public UILabel createTime;
    /// <summary>
    /// 用户组
    /// </summary>
    public UILabel[] groups;

	public GameObject newBtn;

	public GameObject updateBtn;
    /// <summary>
    /// 当前显示的用户信息
    /// </summary>
    private UserInfoRecord UIRecord;

	void Start () 
	{
		if(AuthorityHelper.NotAuthority(AuthorityHelper.USER_INFO_MANAGEMENT))
		{
			newBtn.SetActive(false);
		}
		Instance = this;
		LoadUserRecord (1000,1);

	}
	/// <summary>
	/// 重新加载用户信息
	/// </summary>
	public void ReLoadUserRecord()
	{
		Logger.Instance.WriteLog("重新加载用户信息");
		Records.transform.DestroyChildren ();
		LoadUserRecord (1000,1);
	}

	/// <summary>
	/// 加载用户信息
	/// </summary>
	/// <param name="pageSize">Page size.</param>
	/// <param name="pageIndex">Page index.</param>
	public void LoadUserRecord(int pageSize, int pageIndex)
	{
		Logger.Instance.WriteLog("加载用户信息");
		List<UserBasicInfo> ubInfo = new List<UserBasicInfo>();
		UserInfoRecord record;
		UserBasicDao ubDao = new UserBasicDao (pageSize, pageIndex);        
        int ubInfoSize = ubDao.getUserInfoFormDB(ref ubInfo);
        if (ubInfoSize == -1)
        {
            Logger.Instance.error("获取用户组信息失败");
        }
		bool forbidden = false;
		for(int i = 0; i < ubInfo.Count; i++)
		{
			forbidden = false;
			record = new UserInfoRecord();
			record.no = ubInfo[i].ID;
			record.UBInfo = ubInfo[i];
			//根据用户ID获取相关组的信息
			GroupDao gDao = new GroupDao();
			List<GroupInfo> groups=new List<GroupInfo>();
            int groupsSize= gDao.SelectUserGroup(ubInfo[i].ID,ref groups);
            if (groupsSize < 0)
            {
                Logger.Instance.error("通过用户id检索用户组信息失败!!");
				return;
            }

			if(groups.Count <= 0)
			{
				continue;
			}
			record.GInfo = groups[0];

			if(record.UBInfo.Status == "禁用")
			{
				forbidden = true;
			}
			GameObject go = Instantiate(RecordItemPrefab) as GameObject;
			go.GetComponent<UserRecordItem>().SetValue(record,forbidden);
			if(i == 0)
			{
				go.GetComponent<UserRecordItem>().Click();
				ShowUserInfo(record);
			}
			Records.AddChild(go.transform);
			go.transform.localScale = new Vector3(1,1,1);
		}

	}
	/// <summary>
	/// 打开创建新用户的界面
	/// </summary>
    //GameObject Window;
	public void CreateNewUser()
	{
        //Logger.Instance.WriteLog("打开创建新用户的界面");
        //Window = Instantiate (NewUserWindowPrefab) as GameObject;
        //Window.transform.localPosition = NewUserWindowPos;
        //Window.transform.localScale = new Vector3 (1,1,1);

	}

	void OnDisable()
	{
        //Destroy (Window);
	}

    /// <summary>
    /// 取消选中状态
    /// </summary>
    public void ClearSelect()
    {
        foreach (Transform tf in Records.transform)
        {
            tf.GetComponent<UserRecordItem>().selectGround.SetActive(false);
        }
    }

    private void ForbiddenModify(bool b)
    {
        accountName.GetComponent<BoxCollider>().enabled = false;
        userName.GetComponent<BoxCollider>().enabled = b;
        userAdd.GetComponent<BoxCollider>().enabled = b;
        remark.GetComponent<BoxCollider>().enabled = b;
        userMobile.GetComponent<BoxCollider>().enabled = b;

		foreach(UILabel lb in groups)
		{
			lb.transform.parent.GetComponent<BoxCollider>().enabled = b;
		}

    }

    public void ShowUserInfo(UserInfoRecord record)
    {
		if(AuthorityHelper.NotAuthority(AuthorityHelper.USER_INFO_BROWSE))
		{
			Debug.Log("无操作权限");
			return;
		}

        Logger.Instance.WriteLog("显示用户相关信息");
        UIRecord = record;
        if (UIRecord.GInfo.Name == "超级管理员" || UIRecord.UBInfo.UserName == DataStore.UserInfo.UserName)
        {
            ForbiddenModify(false);
        }
        else
        {
			if(AuthorityHelper.NotAuthority(AuthorityHelper.USER_INFO_MANAGEMENT))
			{
				ForbiddenModify(false);
			}
			else
			{
            	ForbiddenModify(true);
			}
        }

        GroupDao gpDao = new GroupDao();
        gpDao.Select003();
        List<GroupInfo> gpInfos = gpDao.Result;
        foreach (UILabel lb in groups)
        {
            lb.text = string.Empty;
			lb.transform.parent.GetComponent<userGroupItem>().selectToggle.value = false;
        }

        for (int i = 0; i < gpInfos.Count; i++)
        {
            if (i < groups.Length)
            {
                groups[i].text = gpInfos[i].Name;
				groups[i].transform.parent.GetComponent<userGroupItem>().info = gpInfos[i];
                groups[i].transform.parent.GetComponent<userGroupItem>().backGround.SetActive(false);
                groups[i].transform.parent.GetComponent<userGroupItem>().selectGround.SetActive(false);
                if (record.GInfo.Name == gpInfos[i].Name)
                {
                    groups[i].transform.parent.GetComponent<userGroupItem>().selectToggle.value = true;
                }
                else
                {
                    groups[i].transform.parent.GetComponent<userGroupItem>().selectToggle.value = false;
                }
            }
            else
            {
                Logger.Instance.WriteLog("组长度超出目前UI限定数量");
                break;
            }
        }

        accountName.value = UIRecord.UBInfo.UserName;
        createTime.text = System.DateTime.Parse(UIRecord.UBInfo.CreateTime).ToString("yyyy年MM月dd日");
        accountPsw.value = "**********";
        userName.value = UIRecord.UBInfo.RealName;
        userAdd.value = UIRecord.UBInfo.Address;
        remark.value = UIRecord.UBInfo.Memo;
        userMobile.value = UIRecord.UBInfo.Telphone;
    }

    /// <summary>
    /// 取消用户组选中状态
    /// </summary>
    public void ClearGroupSelect()
    {
        foreach (UILabel lb in groups)
        {
            lb.transform.parent.GetComponent<userGroupItem>().selectGround.SetActive(false);
        }
    }

    public void Confirm()
    {
        Logger.Instance.WriteLog("保存修改后的用户信息");  
		UserRecordItem tempItem = null;
		if(UIRecord.UBInfo.Status == "新建")
		{
			return;
		}
		else 
		{
			UserBasicDao ubdao = new UserBasicDao();
			ubdao.Update003(userName.value, UIRecord.UBInfo.Password, userMobile.value,
			                userAdd.value, remark.value, UIRecord.UBInfo.ID);
		}        

		for(int i  = 0; i < Records.transform.childCount; i++)
		{
			tempItem = Records.transform.GetChild(i).GetComponent<UserRecordItem>();
			if(tempItem.LblAccount.text == UIRecord.UBInfo.UserName)
			{
				UIRecord.UBInfo.Telphone = userMobile.value;
				UIRecord.UBInfo.Address = userAdd.value;
				UIRecord.UBInfo.RealName = userName.value;
				UIRecord.UBInfo.Memo = remark.value;
				break;
			}
		}

        string gstr = string.Empty;
        foreach (UILabel lab in groups)
        {
            if (lab.transform.parent.GetComponent<userGroupItem>().selectToggle.value)
            {
                gstr = lab.text;
				break;
            }
        }
		UIRecord.GInfo.Name = gstr;

        GroupDao gpDao = new GroupDao();
        gpDao.Select003();
        List<GroupInfo> gpInfos = gpDao.Result;
        foreach (GroupInfo info in gpInfos)
        {
            if (gstr == info.Name)
            {
				UserGroupDao ugDao = new UserGroupDao();
				if(UIRecord.UBInfo.Status == "新建")
				{
					ugDao.Insert001(UIRecord.UBInfo.ID, info.Id);
					UIRecord.UBInfo.Status = "正常";
				}
				else 
				{
					ugDao.Update001(info.Id, UIRecord.UBInfo.ID);
					break;
				}				
			}
        }

		if(tempItem != null)
		{
			tempItem.SetValue(UIRecord);
		}
        //ReLoadUserRecord();
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
	}

	public void NewUser()
	{
		UserInfoRecord record = new UserInfoRecord ();
		record.no = "new";
		record.GInfo = new GroupInfo ();

		record.UBInfo = new UserBasicInfo ();
		record.UBInfo.ID = "new";
		record.UBInfo.UserName = "新建用户";
		record.UBInfo.Password = "123456";
		record.UBInfo.Address = string.Empty;
		record.UBInfo.CreateTime = System.DateTime.Now.ToString ("yyyy年MM月dd日 HH:mm:ss");
		record.UBInfo.Memo = string.Empty;
		record.UBInfo.RealName = string.Empty;
		record.UBInfo.Status = "新建";
		record.UBInfo.Telphone = string.Empty;

		GameObject go = Instantiate(RecordItemPrefab) as GameObject;
		go.GetComponent<UserRecordItem>().SetValue(record,true);
		Records.AddChild(go.transform);
		go.transform.localScale = new Vector3(1,1,1);
		go.GetComponent<UserRecordItem>().Click();
		ShowUserInfo(record);
		ForbiddenModify(true);
		newBtn.SetActive (false);
		updateBtn.SetActive (true);
	}


	public void AddNewUser()
	{

		Logger.Instance.WriteLog("保存新创建的用户信息");
		UserBasicDao ubdao = new UserBasicDao ();
		ubdao.Insert001 (accountName.value, UIRecord.UBInfo.Password, userName.value,
		                 userMobile.value, System.DateTime.Now.ToString ("yyyy年MM月dd日 HH:mm:ss"),
		                 userAdd.value, "正常", remark.value);
		
		ubdao.Select003 (accountName.value);
		UserBasicInfo ubInfo = ubdao.Result [0];
		
		string goupId = "1";
		foreach(UILabel lb in groups)
		{
			if(lb.transform.parent.GetComponent<userGroupItem>().selectToggle.value)
			{
				goupId = lb.transform.parent.GetComponent<userGroupItem>().info.Id;
			}
		}
		
		UserGroupDao ugpDao = new UserGroupDao ();
		ugpDao.Insert001 (ubInfo.ID, goupId);
		
		ReLoadUserRecord ();
		newBtn.SetActive (true);
		updateBtn.SetActive (false);

	}
}
