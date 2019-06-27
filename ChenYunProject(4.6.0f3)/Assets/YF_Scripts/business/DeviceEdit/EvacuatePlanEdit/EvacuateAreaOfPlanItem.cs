using UnityEngine;
using System.Collections;

public class EvacuateAreaOfPlanItem : MonoBehaviour
{
	/// <summary>
	/// 编号
	/// </summary>
	public UILabel No;
	/// <summary>
	/// 疏散区域名称
	/// </summary>
	public UILabel Name;
	/// <summary>
	/// 绑定按钮文本
	/// </summary>
	public UILabel BindStatuText;
	/// <summary>
	/// 绑定按钮
	/// </summary>
	public UIButton BindStatuBtn;
	[HideInInspector]
	public bool BindStatu
	{
		get {return bindStatu;}
	}
	/// <summary>
	/// 绑定状态
	/// </summary>
	private bool bindStatu;

	[HideInInspector]
	public string AreaId
	{
		get {return areaId;}
	}
	/// <summary>
	/// 疏散区域ID
	/// </summary>
	private string areaId;
	/// <summary>
	/// 疏散区域信息
	/// </summary>
	private EvacuateArea evacuateArea;
	/// <summary>
	/// 疏散区域对象
	/// </summary>
	private GameObject EvacuateAreaGo;
	/// <summary>
	/// 疏散区域位置
	/// </summary>
	private Vector3 EvacuateAreaGoPosition;
	/// <summary>
	/// 正常状态下的按钮图片
	/// </summary>
	private string normalSprite;
	public void Init(EvacuateArea _evacuateArea,GameObject _EvacuateAreaGo,Vector3 _EvacuateAreaGoPosition,bool _selected)
	{
		evacuateArea = _evacuateArea;
		EvacuateAreaGo = _EvacuateAreaGo;
		EvacuateAreaGoPosition = _EvacuateAreaGoPosition;
		areaId = evacuateArea.id;
		No.text = evacuateArea.id;
		Name.text = evacuateArea.name;
		normalSprite = BindStatuBtn.normalSprite;
		if(_selected)
		{
			BindStatuText.text = "已绑定";
			BindStatuBtn.normalSprite = BindStatuBtn.pressedSprite;
		}
		else
		{
			BindStatuText.text = "未绑定";
		}
	}

	public void Init(bool _selected)
	{
		bindStatu = _selected;
		if(_selected)
		{
			BindStatuText.text = "已绑定";
			BindStatuBtn.normalSprite = BindStatuBtn.pressedSprite;
		}
		else
		{
			BindStatuText.text = "未绑定";
			BindStatuBtn.normalSprite = normalSprite;
		}
	}
	
	public void OnBindStatuChanged()
	{
		if(bindStatu)
		{
			Logger.Instance.WriteLog("解除绑定疏散区域[" + evacuateArea.name + "]");
		}
		else
		{
			Logger.Instance.WriteLog("绑定疏散区域[" + evacuateArea.name + "]");
		}
		Init(!bindStatu);
		SaveData();
	}
	/// <summary>
	/// 将视野移动到疏散区域
	/// </summary>
	public void GotoPosition()
	{
		if(EvacuateAreaGo)Camera.main.GetComponent<CameraController> ().GotoPosition(new Vector3(EvacuateAreaGoPosition.x,0,EvacuateAreaGoPosition.z),2);
	}

	public void SaveData()
	{
		GetComponentInParent<EvacuateAreaOfPlanEdit>().SaveData();

	}
}
