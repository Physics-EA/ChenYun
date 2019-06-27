using UnityEngine;
using System.Collections;
/// <summary>
/// 用来操作权限列表单个项目
/// </summary>
public class AuthorityItem : MonoBehaviour {
	/// <summary>
	/// 项目标签
	/// </summary>
	public UILabel Label;
	/// <summary>
	/// 判断当前项目是否被选择
	/// </summary>
	public bool isSelected;
	/// <summary>
	/// 权限信息
	/// </summary>
	public AuthorityInfo AuthInfo;
	/// <summary>
	/// 默认精灵
	/// </summary>
	private string DefaultSprite;

	void Awake()
	{
		DefaultSprite = GetComponent<UISprite> ().spriteName;
	}
	/// <summary>
	/// 将给定的数据进行设置
	/// </summary>
	/// <param name="AuthInfo">Auth info.</param>
	public void SetValue(AuthorityInfo AuthInfo)
	{
		this.AuthInfo = AuthInfo;
		Label.text = AuthInfo.Description;
	}
	/// <summary>
	/// 权限列表项目被单击是调用
	/// </summary>
	public void Change()
	{
		if(isSelected)
		{
			Deselect();
		}
		else
		{
			Select();
		}
	}
	/// <summary>
	/// 选中当前值
	/// </summary>
	public void Select()
	{
		isSelected = true;
		GetComponent<UIButton>().normalSprite = GetComponent<UIButton>().hoverSprite;
	}
	/// <summary>
	/// 取消选中状态
	/// </summary>
	public void Deselect()
	{

		isSelected = false;
		GetComponent<UIButton>().normalSprite = DefaultSprite;

	}
	/// <summary>
	/// 设置成可用状态
	/// </summary>
	public void Enable()
	{
		GetComponent<BoxCollider> ().enabled = true;
		GetComponent<UIButton> ().enabled = true;
	}
	/// <summary>
	/// 设置成不可以状态
	/// </summary>
	public void Disable()
	{
		GetComponent<BoxCollider> ().enabled = false;
		GetComponent<UIButton> ().enabled = false;
	}
}
