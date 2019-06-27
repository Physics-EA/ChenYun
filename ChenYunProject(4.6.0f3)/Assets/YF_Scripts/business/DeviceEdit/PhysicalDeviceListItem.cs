using UnityEngine;
using System.Collections.Generic;

public class PhysicalDeviceListItem : MonoBehaviour {
	public UILabel Description;
	private CAMARE_INFO info;
	DelBind delBind;
	private int index;
	public GameObject selectbg;
	public GameObject bindedFlag;
	public void SetValue(int _index,CAMARE_INFO info,DelBind delBind)
	{
		this.delBind = delBind;
		this.info = info;
		this.index = _index;
		List<byte> des = new List<byte>();
		foreach(byte b in info.name)
		{
			if(b == 0)
			{
				break;
			}
			des.Add(b);
		}
		Description.text = System.Text.Encoding.Default.GetString (des.ToArray()); 

	}

	public void OnEnable()
	{
		selectbg.SetActive(false);
		bindedFlag.SetActive(false);
		if(delBind != null)
		{
			if(delBind.deviceInfo.Guid != null && delBind.deviceInfo.Guid.Trim() != "" 
			   && delBind.deviceInfo.Guid == CMSManage.GUIDToString(info.camareGuid))
			{
				bindedFlag.SetActive(true);
			}
		}
	}

	void OnDisable()
	{
		bindedFlag.SetActive(false);
	}

	public void Bind()
	{
		delBind.bind (info);

	}

	public void OpenCamera()
	{
		transform.parent.BroadcastMessage ("ResetSelect");
		selectbg.SetActive(true);
		EditCameraManager.instance.OpenCamera(index,info);	
	}

	public void ResetSelect()
	{
		selectbg.SetActive(false);
	}

}
