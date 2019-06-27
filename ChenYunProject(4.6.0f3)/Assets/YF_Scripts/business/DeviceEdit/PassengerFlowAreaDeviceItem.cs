using UnityEngine;
using System.Collections;

public class PassengerFlowAreaDeviceItem : MonoBehaviour {

	public UILabel Description;
	public UIButton  button;
	[HideInInspector]
	public string deviceId;
	[HideInInspector]
	public bool isSelected;
	public UIToggle tgIn_in;
	public UIToggle tgIn_out;
	public UIToggle tgOut_in;
	public UIToggle tgOut_out;

	private string normalSprite;
	private string hoverSprite;

	private int in_in;
	private int in_out;
	private int out_in;
	private int out_out;

	void Awake()
	{
		normalSprite = button.normalSprite;
		hoverSprite = button.hoverSprite;
	}

	bool IsInit = false;
	void LateUpdate()
	{
		if(IsInit)return;
		IsInit = true;
		BindFunction();
	}

	public void SetValue(string _deviceId,bool _IsSelected, string description)
	{
		Logger.Instance.WriteLog("初始化客流统计区域设备绑定列表项目");
		deviceId = _deviceId;
		isSelected = _IsSelected;
		Description.text = description;
		if(isSelected)button.normalSprite = hoverSprite;
	}
	
	private void BindFunction()
	{
		tgIn_in.onChange.Add(new EventDelegate(SwitchIn_in));
		tgIn_out.onChange.Add(new EventDelegate(SwitchIn_out));
		tgOut_in.onChange.Add(new EventDelegate(SwitchOut_in));
		tgOut_out.onChange.Add(new EventDelegate(SwitchOut_out));
	}
    public void SetInOut(CameraIdLst lst)
    {
        in_in = int.Parse(lst.inin);
        in_out = int.Parse(lst.inout);
        out_in = int.Parse(lst.outin);
        out_out = int.Parse(lst.outout);

        tgIn_in.value = (in_in == 1);
        tgIn_out.value = (in_out == 1);
        tgOut_in.value = (out_in == 1);
        tgOut_out.value = (out_out == 1);
    }

	public void SwitchStatus()
	{
		if(isSelected)
		{
			isSelected = false;
			button.normalSprite = normalSprite;
		}
		else
		{
			isSelected = true;
			button.normalSprite = hoverSprite;
		}
		SaveData ();
	}

	public void SwitchIn_in()
	{
		if(tgIn_in.value)
		{
			in_in = 1;
			tgIn_out.value = false;
			in_out = 0;
			tgOut_in.value = false;
			out_in = 0;
		}
		else 
		{
			in_in = 0;
		}
		Check();
		SaveData ();
	}

	public void SwitchIn_out()
	{
		if (tgIn_out.value)
		{
			in_out = 1;
			tgIn_in.value = false;
			in_in = 0;
			tgOut_out.value = false;
			out_out = 0;
		}
		else 
		{
			in_out = 0;
		}
		Check();
		SaveData ();
	}

	public void SwitchOut_in()
	{
		if(tgOut_in.value)
		{
			out_in = 1;
			tgOut_out.value = false;
			out_out = 0;
			tgIn_in.value = false;
			in_in = 0;
		}
		else 
		{
			out_in = 0;
		}
		Check();
		SaveData ();
	}

	public void SwitchOut_out()
	{
		if(tgOut_out.value)
		{
			out_out = 1;
			tgOut_in.value = false;
			out_in = 0;
			tgIn_out.value = false;
			in_out = 0;
		}
		else 
		{
			out_out = 0;
		}
		Check();
		SaveData ();
	}

	private void Check()
	{
		int val = in_in + in_out + out_in + out_out;
		if(val > 0 && !isSelected)
		{
			isSelected = true;
			button.normalSprite = hoverSprite;
		}
		if(val <= 0)
		{
			isSelected = false;
			button.normalSprite = normalSprite;
		}
	}

	public string GetToggle()
	{
		string st = ",";
		st += in_in.ToString ();
		st += ",";
		st += in_out.ToString ();
		st += ",";
		st += out_in.ToString ();
		st += ",";
		st += out_out.ToString ();
		return st;
	}

	private void SaveData()
	{
		Debug.Log("SaveData");
		gameObject.GetComponentInParent<PassengerFlowAreaDeviceEdit>().SaveData();
	}
}
