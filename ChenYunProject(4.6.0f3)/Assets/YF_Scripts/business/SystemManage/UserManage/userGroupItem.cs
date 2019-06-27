using UnityEngine;
using System.Collections;

public class userGroupItem : MonoBehaviour {
    /// <summary>
    /// 高亮背景
    /// </summary>
    public GameObject backGround;
    /// <summary>
    /// 选中背景
    /// </summary>
    public GameObject selectGround;
    /// <summary>
    /// 选中标示
    /// </summary>
    public UIToggle selectToggle;

	public GroupInfo info;

	private BoxCollider box;

	void Start()
	{
		box = gameObject.GetComponent<BoxCollider> ();
	}

    public void HoverOver()
    {
		if(!box.enabled)
		{
			return;
		}
        backGround.SetActive(true);
    }

    public void HoverOut()
    {
        backGround.SetActive(false);
    }

    public void Click()
    {
        UserRecordManage.Instance.ClearGroupSelect();
        selectGround.SetActive(true);
        selectToggle.value = !selectToggle.value;
    }
}
