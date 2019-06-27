using UnityEngine;
using System.Collections;

public class AuthorityItemControl : MonoBehaviour {

    /// <summary>
    /// lable数组
    /// </summary>
    public UILabel[] authorityLable;
    /// <summary>
    /// 对应lable选中状态
    /// </summary>
    public UIToggle[] authorityToggle;
    /// <summary>
    /// lable数组使用数量
    /// </summary>
    private int lableCount;

	public GroupInfoModify modify;

    void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        //初始化使用数量为0，并清空lable内字符和默认为false的toggle选中状态

        lableCount = 0;
        if (authorityLable.Length != authorityToggle.Length)
        {
            Debug.LogError("lable对象数量与toggle对象数量不匹配；lable：" + authorityLable.Length + " toggle：" + authorityToggle.Length);
        }

        for (int i = 0; i < authorityToggle.Length; i++)
        {
            authorityToggle[i].value = false;
        }
        for (int i = 0; i < authorityLable.Length; i++)
        {
            authorityLable[i].text = string.Empty;
            authorityLable[i].gameObject.SetActive(false);
        }
     
    }

    /// <summary>
    /// 添加lable信息
    /// </summary>
    /// <param name="text">lable显示内容</param>
    /// <param name="value">lable选定状态</param>
    /// <returns></returns>
    public UIToggle AddLable(AuthorityInfo info)
    {
        if (lableCount < 4)
        {
            authorityLable[lableCount].text = info.Description;
            authorityLable[lableCount].gameObject.SetActive(true);
            lableCount++;
            return authorityToggle[lableCount - 1];
            
        }
        else
        {
            //已超出lable数组范围
            return null;
        }        
    }

    /// <summary>
    /// 设置当前lable选中状态
    /// </summary>
    public void LableState(bool b)
    {
        authorityToggle[lableCount - 1].value = b;
    }

	public void Click()
	{
		modify.Comfirm ();
	}

    /// <summary>
    /// 检测当前是否还有空余可用lable
    /// </summary>
    /// <returns></returns>
    public bool isUnoccupied()
    {
        return lableCount < 4;
    }
}
