using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvacuateAreaDeviceEdit : MonoBehaviour
{
	public UIInput nameLabel;
	public UISprite nameInputBox;
	public UIGrid EvacuateAreaDeviceItemGrid;
	public GameObject EvacuateAreaDeviceItem;

	public DelEvacuateAreaNameChanged EvacuateAreaNameChanged;
	private string areaId;
	public void Init(string _areaId,DelEvacuateAreaNameChanged _EvacuateAreaNameChanged)
	{
		Logger.Instance.WriteLog("初始化疏散区域设备");
		EvacuateAreaNameChanged = _EvacuateAreaNameChanged;
		areaId = _areaId;
		StartCoroutine(LoadData());
	}

	IEnumerator LoadData()
	{
		Logger.Instance.WriteLog("加载化疏散区域信息");
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		List<EvacuateArea> eAreas = epDao.Select006(areaId);
		if(eAreas.Count <= 0)
		{
			yield return null;
		}
		else
		{
			EvacuateArea area = eAreas[0];
			nameLabel.value = area.name;
			List<string> cameraIdLst = new List<string>();
			if(!string.IsNullOrEmpty(area.cameraList.Trim()))
			{
				cameraIdLst.AddRange(area.cameraList.Split('|'));
			}
			Logger.Instance.WriteLog("加载化设备信息");
			DeviceDao dDao = new DeviceDao();
			dDao.Select001();
			List<DeviceInfo> deviceInfoLst = dDao.Result;
			InitEvacuateAreaDeviceItems(deviceInfoLst,cameraIdLst);
			EvacuateAreaDeviceItemGrid.gameObject.GetComponent<UIWidget>().UpdateAnchors();
		}
		yield return null;
	}
	/// <summary>
	/// 初始化设备列表项目
	/// </summary>
	/// <param name="deviceInfoLst">Device info lst.</param>
	private void InitEvacuateAreaDeviceItems(List<DeviceInfo> deviceInfoLst,List<string> cameraIdLst)
	{
		if(EvacuateAreaDeviceItemGrid.GetChildList().Count > 0)
		{
			foreach(Transform tf in EvacuateAreaDeviceItemGrid.GetChildList())
			{
				EvacuateAreaDeviceItem eadItem = tf.GetComponent<EvacuateAreaDeviceItem>();
				eadItem.Init(cameraIdLst.Contains(eadItem.CameraId));
			}
		}
		else
		{
			foreach(var info in deviceInfoLst)
			{
				GameObject go = Instantiate(EvacuateAreaDeviceItem) as GameObject;
				EvacuateAreaDeviceItemGrid.AddChild(go.transform);
				go.transform.localScale = new Vector3(1,1,1);
				go.GetComponent<EvacuateAreaDeviceItem>().Init(info,cameraIdLst.Contains(info.Id));
			}
		}
	}

	public void NameSelected()
	{
		nameInputBox.enabled = true;
	}

	public void NameDeselected()
	{
		if(EvacuateAreaNameChanged != null)EvacuateAreaNameChanged.Invoke(nameLabel.value);
		nameInputBox.enabled = false;
	}

	public void Close()
	{
		ClearEvacuateAreaDeviceItemGrid();
		gameObject.SetActive(false);
	}

	public void SaveData()
	{
		Logger.Instance.WriteLog("保存疏散预案信息");
		EvacuationPlanDao epDao = new EvacuationPlanDao();

		List<Transform> items = EvacuateAreaDeviceItemGrid.GetChildList();
		string cameraIdLst = "";
		foreach(var item in items)
		{
			if(item.gameObject.GetComponent<EvacuateAreaDeviceItem>().BindStatu)
			{
				cameraIdLst += item.gameObject.GetComponent<EvacuateAreaDeviceItem>().CameraId + "|";
			}
		}
		if(cameraIdLst.Length > 0)
		{
			cameraIdLst = cameraIdLst.Remove(cameraIdLst.Length - 1);
		}
		else
		{
			cameraIdLst = " ";
		}
		epDao.Update005(areaId,cameraIdLst);
	}

	private void ClearEvacuateAreaDeviceItemGrid()
	{
		EvacuateAreaDeviceItemGrid.transform.DestroyChildren();
	}
}
