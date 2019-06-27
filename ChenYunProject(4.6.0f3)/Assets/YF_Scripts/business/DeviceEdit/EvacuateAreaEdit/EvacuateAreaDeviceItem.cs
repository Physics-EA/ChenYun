using UnityEngine;
using System.Collections;

public class EvacuateAreaDeviceItem : MonoBehaviour
{
	/// <summary>
	/// 编号
	/// </summary>
	public UILabel No;
	/// <summary>
	/// 设备名称
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
	[HideInInspector]
	public string CameraId
	{
		get {return cameraId;}
	}
	/// <summary>
	/// 绑定状态
	/// </summary>
	private bool bindStatu;
	private string cameraId;
	private DeviceInfo deviceInfo;

	private string normalSprite;
	public void Init(DeviceInfo _deviceInfo,bool _selected)
	{
		Logger.Instance.WriteLog("初始化疏散区域设备列表项目1");
		deviceInfo = _deviceInfo;
		cameraId = deviceInfo.Id;
		No.text = deviceInfo.Id;
		Name.text = deviceInfo.Description;
		bindStatu = _selected;
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
		Logger.Instance.WriteLog("初始化疏散区域设备列表项目2");
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
			Logger.Instance.WriteLog("解除绑定设备[" + deviceInfo.Description + "]");
		}
		else
		{
			Logger.Instance.WriteLog("绑定设备[" + deviceInfo.Description + "]");
		}
		Init(!bindStatu);
		SaveData();
	}

	public void GotoPosition()
	{
		Camera.main.GetComponent<CameraController> ().GotoPosition(new Vector3(float.Parse(deviceInfo.PosX),0,float.Parse(deviceInfo.PosZ)),2);
	}

	public void SaveData()
	{
		GetComponentInParent<EvacuateAreaDeviceEdit>().SaveData();
	}
}
