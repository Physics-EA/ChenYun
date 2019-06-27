using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 账户组管理UI界面控制
/// </summary>
public class GroupManageUIPanelControl : MonoBehaviour {

    /// <summary>
    /// 账户组item预制体
    /// </summary>
    public GameObject accountPerfab;
    /// <summary>
    /// 权限组item预制体
    /// </summary>
    public GameObject authorityPerfab;
    /// <summary>
    /// 账户组item父节点
    /// </summary>
    public Transform accountParent;
    /// <summary>
    /// 权限组item父节点
    /// </summary>
    public Transform authorityParent;
	/// <summary>
	/// 新增按钮
	/// </summary>
	public GameObject newBtn;
	/// <summary>
	/// 更新按钮
	/// </summary>
	public GameObject updateBtn;
    /// <summary>
    /// 当前权限组item脚本
    /// </summary>
    private AuthorityItemControl authorityItem;
    /// <summary>
    /// 组相关信息脚本
    /// </summary>
    private GroupInfoModify infoModity;
    /// <summary>
    /// 权限toggle列表
    /// </summary>
    public Dictionary<string, UIToggle> tgList;
	/// <summary>
	/// 新增group脚本
	/// </summary>
	private GroupRecordItem newItem;

    void Awake()
    {
        infoModity = gameObject.GetComponent<GroupInfoModify>();
        tgList = new Dictionary<string, UIToggle>();
        if (infoModity == null)
        {
            Debug.LogError("缺失GroupInfoModify脚本 ");
        }

        Initialize();
    }

    /// <summary>
    /// 初始化当前UI功能及状态
    /// </summary>
    public void Initialize()
    {
        //初始化赋空当前权限组item脚本,并清空账户组和权限组下方的所有自对象
        authorityItem = null;
        accountParent.DestroyChildren();
        authorityParent.DestroyChildren();
        if (accountPerfab == null || authorityPerfab == null)
        {
            Debug.LogError("预制体为空;accountPerfab = " + accountPerfab == null ? "null" : accountPerfab.ToString() + " authorityPerfab = " + authorityPerfab == null ? "null" : authorityPerfab.ToString());
        }
        if (accountParent == null || authorityParent == null)
        {
            Debug.LogError("父节点为空;accountParent = " + accountParent == null ? "null" : accountParent.ToString() + " authorityParent = " + authorityParent == null ? "null" : authorityParent.ToString());
        }
    }

    /// <summary>
    /// 添加账户组对象
    /// </summary>
    public void AddAccount(GroupInfoRecord info, int id)
    {
        GameObject g = Instantiate(accountPerfab) as GameObject;
        g.GetComponent<GroupRecordItem>().SetValue(info, this, id);
        g.GetComponent<GroupRecordItem>().infoModify = infoModity;
		g.GetComponent<GroupRecordItem> ().LBLStatus.value = true;
        g.transform.SetParent(accountParent);
        g.transform.localScale = Vector3.one;
		accountParent.GetComponent<UIGrid> ().repositionNow = true;
		if(id == 0 || id == -1)
		{
			g.GetComponent<GroupRecordItem>().ShowGroupModifyWindow(gameObject);
			g.GetComponent<GroupRecordItem>().Click();
			if(id == -1)
			{
				newItem = g.GetComponent<GroupRecordItem>();
			}
		}
    }

    /// <summary>
    /// 添加权限组对象
    /// </summary>
    public void AddAuthority(AuthorityInfo info, bool b)
    {
        Logger.Instance.WriteLog("添加权限组对象");
        if (tgList.ContainsKey(info.Id))
        {
            tgList[info.Id].value = b;
        }
        else
        {
            if (authorityItem == null || !authorityItem.isUnoccupied())
            {
                GameObject g = Instantiate(authorityPerfab) as GameObject;
                authorityItem = g.GetComponent<AuthorityItemControl>();
				authorityItem.modify = infoModity;
                if (authorityItem == null)
                {
                    Debug.LogError("对象缺失AuthorityItemControl脚本，创建对象失败，对象名：" + g.name);
                    return;
                }
                g.transform.SetParent(authorityParent);
                g.transform.localScale = Vector3.one;
                UIToggle tg = authorityItem.AddLable(info);
                if (tg == null)
                {
                    Debug.LogError("有异常判断");
                }
                else
                {
                    tgList[info.Id] = tg;
                    tg.value = b;
                }
            }
            else
            {
                UIToggle tg = authorityItem.AddLable(info);
                if (tg == null)
                {
                    Debug.LogError("有异常判断");
                }
                else
                {
                    tgList[info.Id] = tg;
                    tg.value = b;
                }
            }

            authorityParent.GetComponent<UIWidget>().UpdateAnchors();
            authorityParent.GetComponent<UIGrid>().repositionNow = true;
        }
 
    } 

    /// <summary>
    /// 清除账户组下item
    /// </summary>
    public void ClearAccount()
    {
        accountParent.DestroyChildren();
        accountParent.GetComponent<UIGrid>().enabled = true;
    }

    /// <summary>
    /// 清除权限组下item
    /// </summary>
    public void ClearAuthority()
    {
        UIGrid grid = authorityParent.GetComponent<UIGrid>();
        foreach (Transform tf in grid.GetChildList())
        {
            grid.RemoveChild(tf);
            Destroy(tf.gameObject);
        }
    }

    public void ClearAccountSelect()
    {
        foreach (Transform child in accountParent)
        {
            child.GetComponent<GroupRecordItem>().selectGround.SetActive(false);
        }
    }

	public void DeleteSelect()
	{
		foreach (Transform child in accountParent)
		{
			if(child.GetComponent<GroupRecordItem>().selectGround.activeSelf)
			{
				Destroy(child.gameObject);
				break;
			}
		}
	}
	
	public void AddNewGroup()
	{
		string num;
		int number = 1;
		GroupInfoRecord info = new GroupInfoRecord ();
		foreach(Transform tf in accountParent)
		{
			num = tf.GetComponent<GroupRecordItem>().LBLNo.text.Trim();
			int n = System.Convert.ToInt32(num);
			if(number < n)
			{
				number = n + 1;
			}
		}
		info.No = number.ToString ();
		info.GInfo = new GroupInfo ();
		info.GInfo.Id = info.No;
		info.GInfo.Name = "新增用户组" + info.No;
		info.GInfo.Status = "新增";

		AddAccount (info, -1);
		newBtn.SetActive (false);
		updateBtn.SetActive (true);
	}

	public void UpdateNewGroup()
	{
		newItem.UpdateNew ();
		infoModity.Comfirm ();
		newItem = null;
		newBtn.SetActive (true);
		updateBtn.SetActive (false);
	}
}
