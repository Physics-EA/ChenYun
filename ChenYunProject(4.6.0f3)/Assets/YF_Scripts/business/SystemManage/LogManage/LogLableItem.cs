using UnityEngine;
using System.Collections;

public class LogLableItem : MonoBehaviour {
	/// <summary>
	/// 高亮背景
	/// </summary>
	public GameObject backGround;
	/// <summary>
	/// lable显示文字
	/// </summary>
	public UILabel text;
	/// <summary>
	/// 管理面板ID
	/// </summary>
	private GameObject logPanel;
	/// <summary>
	/// 自身toggle组件
	/// </summary>
	private UIToggle uiTG;

	void Awake()
	{
		uiTG = gameObject.GetComponent<UIToggle> ();
	}

	public void SetValue(string name, GameObject panel)
	{
		text.text = name;
		logPanel = panel;
	}

	public void HoverOver()
	{
		backGround.SetActive (true);
	}

	public void HoverOut()
	{
		backGround.SetActive (false);
	}

	public void SelectClick()
	{
		logPanel.SetActive (uiTG.value);
	}
}
