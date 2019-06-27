using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DeviceRecordManage : MonoBehaviour {

	/// <summary>
	/// 设备记录预制体
	/// </summary>
	public GameObject RecordItemPrefab;
	/// <summary>
	/// 挂载设备记录的对象
	/// </summary>
	public UIGrid Records;
	/// <summary>
	/// 预设位设置窗口
	/// </summary>
	public GameObject PresetPositionWindow;
	public void Init()
	{
		LoadDeviceRecord ();
	}
	/// <summary>
	/// 加载设备信息
	/// </summary>
	public void LoadDeviceRecord()
	{
		Logger.Instance.WriteLog("加载设备信息");
		List<DeviceInfo> dInfo;
		DeviceRecordInfo record;
		DeviceDao dDao = new DeviceDao ();
		dDao.Select001 ();
		dInfo = dDao.Result;
		bool forbidden = false;
		Records.transform.DestroyChildren ();
		for(int i = 0; i < dInfo.Count; i++)
		{
			record = new DeviceRecordInfo();
			record.No = "" + (i + 1);
			record.DInfo = dInfo[i];
			forbidden = false;
			if(!CMSManage.Instance.HasPTZCtl(record.DInfo.Guid))
			{
				forbidden = true;
			}
			GameObject go = Instantiate(RecordItemPrefab) as GameObject;
			go.GetComponent<DeviceRecordItem>().Init(record,PresetPositionWindow,forbidden);
			Records.AddChild(go.transform);
			go.transform.localScale = new Vector3(1,1,1);
		}		
	}

	void OnDisable()
	{
		Records.transform.DestroyChildren();
	}
}
