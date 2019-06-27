using UnityEngine;
using System.Collections;

public class AccountManageControl : MonoBehaviour {
    /// <summary>
    /// 组管理按钮
    /// </summary>
    public GameObject groupBtn;
    /// <summary>
    /// 选中背景
    /// </summary>
    public GameObject groupSelect;
    /// <summary>
    /// 组管理面板
    /// </summary>
    public GameObject groupPanel;
    /// <summary>
    /// 账户管理按钮
    /// </summary>
    public GameObject accountBtn;
    /// <summary>
    /// 选中背景
    /// </summary>
    public GameObject accountSelect;
    /// <summary>
    /// 账户管理面板
    /// </summary>
    public GameObject accountPanel;
    /// <summary>
    /// 日志管理按钮
    /// </summary>
    public GameObject logBtn;
    /// <summary>
    /// 选中背景
    /// </summary>
    public GameObject logSelect;
    /// <summary>
    /// 日志管理面板
    /// </summary>
    public GameObject logPanel;


	public void Initialize()
	{
		ClearSelect();
		GroupSelect(false, false);
		AccountSelect(false, false);
		LogSelect(false, false);
		
		if(AuthorityHelper.HasAuthority(AuthorityHelper.HISTORY_MANAGEMENT))
		{
			Click(logBtn);
			logBtn.GetComponent<BoxCollider>().enabled = true;
			logBtn.GetComponent<UIButton>().enabled = true;
		}
		if(AuthorityHelper.HasAuthority(AuthorityHelper.USER_INFO_BROWSE))
		{
			Click(accountBtn);
			accountBtn.GetComponent<BoxCollider>().enabled = true;
			accountBtn.GetComponent<UIButton>().enabled = true;
		}
		if(AuthorityHelper.HasAuthority(AuthorityHelper.USER_GROUP_INFO_BROWSE))
		{
			Click(groupBtn);
			groupBtn.GetComponent<BoxCollider>().enabled = true;
			groupBtn.GetComponent<UIButton>().enabled = true;
		}
	}

    private void GroupSelect(bool b, bool s)
    {
        groupSelect.SetActive(b);
        groupPanel.SetActive(b);
		groupBtn.GetComponent<UIButton> ().enabled = s;
		groupBtn.GetComponent<BoxCollider> ().enabled = s;
    }

    private void AccountSelect(bool b, bool s)
    {
        accountSelect.SetActive(b);
        accountPanel.SetActive(b);
		accountBtn.GetComponent<UIButton> ().enabled = s;
		accountBtn.GetComponent<BoxCollider> ().enabled = s;
    }

    private void LogSelect(bool b, bool s)
    {
        logSelect.SetActive(b);
        logPanel.SetActive(b);
		logBtn.GetComponent<UIButton> ().enabled = s;
		logBtn.GetComponent<BoxCollider> ().enabled = s;
    }

    private void ClearSelect()
    {
        groupSelect.SetActive(false);
        accountSelect.SetActive(false);
        logSelect.SetActive(false);
    }

    public void Click(GameObject g)
    {
        ClearSelect();
        if (g == groupBtn)
        {
            GroupSelect(true, groupBtn.GetComponent<UIButton>().enabled);
            AccountSelect(false, accountBtn.GetComponent<UIButton>().enabled);
            LogSelect(false, logBtn.GetComponent<UIButton>().enabled);
        }
        else if (g == accountBtn)
        {
			GroupSelect(false, groupBtn.GetComponent<UIButton>().enabled);
			AccountSelect(true, accountBtn.GetComponent<UIButton>().enabled);
			LogSelect(false, logBtn.GetComponent<UIButton>().enabled);
        }
        else if (g == logBtn)
        {
			GroupSelect(false, groupBtn.GetComponent<UIButton>().enabled);
			AccountSelect(false, accountBtn.GetComponent<UIButton>().enabled);
			LogSelect(true, logBtn.GetComponent<UIButton>().enabled);
        }
    }
}
