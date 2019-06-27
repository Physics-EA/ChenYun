using UnityEngine;
using System.Collections;
/// <summary>
/// 用来保存需要显示的用户的相关信息
/// </summary>
public struct UserInfoRecord
{
	/// <summary>
	/// 显示用的序号
	/// </summary>
	public string no;
	/// <summary>
	/// 保存用户基础信息
	/// </summary>
	public UserBasicInfo UBInfo;
	/// <summary>
	/// 保存用户所在组的信息
	/// </summary>
	public GroupInfo GInfo;
}
/// <summary>
/// 操作用户信息记录
/// </summary>
public class UserRecordItem : MonoBehaviour {
	/// <summary>
	/// 显示序号的标签
	/// </summary>
	public UILabel LblNo;
	/// <summary>
	/// 显示账号信息的标签
	/// </summary>
	public UILabel LblAccount;
	/// <summary>
	/// 显示真实姓名的标签
	/// </summary>
	public UILabel LblRealName;
	/// <summary>
	/// 显示所在组的标签
	/// </summary>
	public UILabel LblGroup;
	/// <summary>
	/// 显示创建时间的标签
	/// </summary>
	public UILabel LblCreateTime;
	/// <summary>
	/// 显示当前用户状态的标签
	/// </summary>
	public UIToggle LblStatus;
	/// <summary>
	/// 保存所显示用户的记录信息
	/// </summary>
	private UserInfoRecord userRecord;
    /// <summary>
    /// 高亮背景
    /// </summary>
    public GameObject backGround;
    /// <summary>
    /// 选中背景
    /// </summary>
    public GameObject selectGround;

	/// <summary>
	/// 把指定的值显示在相关的组件上
	/// </summary>
	/// <param name="userRecord">User record.</param>
	public void SetValue(UserInfoRecord userRecord,bool forbidden = false)
	{
		Logger.Instance.WriteLog("初始化用户记录项目");
		UIEventListener.Get (gameObject).onClick = ShowUserModifyWindow;
		this.userRecord = userRecord;
		LblNo.text = userRecord.no;
		LblAccount.text = userRecord.UBInfo.UserName;
		LblRealName.text = userRecord.UBInfo.RealName;
		LblGroup.text = userRecord.GInfo.Name;
		LblCreateTime.text = System.DateTime.Parse(userRecord.UBInfo.CreateTime).ToString("yyyy年MM月dd日");
		
        if(userRecord.UBInfo.Status == "正常")
        {
            LblStatus.value = true;
        }
        else 
        {
            LblStatus.value = false;
        }
		if(forbidden)
		{
//			LblNo.color = Color.gray;
//			LblAccount.color = Color.gray;
//			LblRealName.color = Color.gray;
//			LblGroup.color = Color.gray;
//			LblCreateTime.color = Color.gray;
		}
	}
	/// <summary>
	/// 用来打开显示详细信息的界面
	/// </summary>
	/// <param name="go">Go.</param>
    //GameObject Window;
	public void ShowUserModifyWindow(GameObject go)
	{
		UserRecordManage.Instance.ShowUserInfo(userRecord);
        
	}

	void OnDisable()
	{
        //Destroy (Window);
	}

    public void HoverOver()
    {
        backGround.SetActive(true);
    }

    public void HoverOut()
    {
        backGround.SetActive(false);
    }

    public void Click()
    {
        UserRecordManage.Instance.ClearSelect();
        selectGround.SetActive(true);
    }

	public UserInfoRecord GetUserInfo()
	{
		return userRecord;
	}
}
