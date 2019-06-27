using UnityEngine;
using System.Collections;
/// <summary>
/// 组信息记录结构
/// 用来保存需要显示组的相关信息
/// </summary>
public struct GroupInfoRecord
{
	/// <summary>
	/// 显示用的序号
	/// </summary>
	public string No;
	/// <summary>
	/// 组信息
	/// </summary>
	public GroupInfo GInfo;
};
/// <summary>
/// 操作组信息
/// </summary>
public class GroupRecordItem : MonoBehaviour {
	/// <summary>
	/// 显示序号的标签
	/// </summary>
	public UILabel LBLNo;
	/// <summary>
	/// 显示组名称的标签
	/// </summary>
	public UILabel LBLName;
	/// <summary>
	/// 组名称输入框
	/// </summary>
	public UIInput LBLInput;
	/// <summary>
	/// 显示组状态的标签
	/// </summary>
	public UIToggle LBLStatus;
    /// <summary>
    /// 高亮背景
    /// </summary>
    public GameObject backGround;
    /// <summary>
    /// 选中背景
    /// </summary>
    public GameObject selectGround;
    /// <summary>
    /// 组相关信息脚本
    /// </summary>
    [HideInInspector]
    public GroupInfoModify infoModify;
	/// <summary>
	/// 保存需要显示组信息的记录
	/// </summary>
	private GroupInfoRecord GIRecord;
    /// <summary>
    /// 账户组管理UI
    /// </summary>
    private GroupManageUIPanelControl groupUI;

	/// <summary>
	/// 把指定的值显示在相关的组件上
	/// </summary>
	/// <param name="Record">Record.</param>
	public void SetValue(GroupInfoRecord Record, GroupManageUIPanelControl group, int type)
	{
		Logger.Instance.WriteLog("初始化用户组记录项目");
		UIEventListener.Get (gameObject).onClick = ShowGroupModifyWindow;
		GIRecord = Record;
        groupUI = group;
		LBLNo.text = GIRecord.No;
		LBLName.text = GIRecord.GInfo.Name;
		if(type == -1)
		{
			//新建对象的输入功能打开
			LBLInput.gameObject.GetComponent<BoxCollider>().enabled = true;
			LBLStatus.gameObject.GetComponent<BoxCollider>().enabled = true;
		}
        if (GIRecord.GInfo.Status == "正常")
        {
            LBLStatus.value = true;
        }
        else
        {
            LBLStatus.value = false;
        }
		
	}

	/// <summary>
	/// 用来打开显示详细信息的界面
	/// </summary>
	/// <param name="go">Go.</param>
	GameObject Window;
	public void ShowGroupModifyWindow(GameObject go)
	{
        //groupUI.ClearAuthority();
        infoModify.SetValue(GIRecord.GInfo);        
	}

	void OnDisable()
	{
		Destroy (Window);
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
        groupUI.ClearAccountSelect();
        selectGround.SetActive(true);
    }

	public void UpdateNew()
	{
		LBLInput.gameObject.GetComponent<BoxCollider> ().enabled = false;
		LBLStatus.gameObject.GetComponent<BoxCollider>().enabled = false;
		string str = LBLInput.value.Trim ();
		if(str == "")
		{
			GIRecord.GInfo.Name = LBLName.text.Trim ();
		}
		else 
		{
			GIRecord.GInfo.Name = str;
		}

		GIRecord.GInfo.Status = LBLStatus.value? "正常":"异常";
		GroupAuthorityDao gaDao = new GroupAuthorityDao ();
		gaDao.InsertNewGroup (GIRecord.GInfo);
		infoModify.Confirm (GIRecord.GInfo);
	}
}
