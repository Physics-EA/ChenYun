using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PassengerFlowAreaDeviceEdit : MonoBehaviour {
	/// <summary>
	/// 设备项目预制体
	/// </summary>
	public GameObject PassengerFlowAreaDevicePrefab;
	/// <summary>
	/// 设备项目信息列表
	/// </summary>
	public UIGrid DeviceList;
	/// <summary>
	/// 显示用标题
	/// </summary>
	public UILabel Title;
	/// <summary>
	/// 保存所有设备信息
	/// </summary>
	private List<DeviceInfo> PFAInfoLst;
	/// <summary>
	/// 保存当前设置的客流区域的条目引用
	/// </summary>
	private PassengerFlowAreaListItem PFALItem;

	private bool isInit;
	/// <summary>
	/// 初始化调用 信息绑定设备列表
	/// </summary>
	/// <param name="_PFALItem">_ PFAL item.</param>
	public void ShowDetial(PassengerFlowAreaListItem _PFALItem)
	{
		Logger.Instance.WriteLog("初始化客流统计区域设备绑定列表");
		PFALItem = _PFALItem;
		Title.text = PFALItem.info.Name;
		foreach(var child in DeviceList.GetChildList())
		{
			DeviceList.RemoveChild(child);
			Destroy(child.gameObject);
		}
		LoadDeviceInfo();
        List<CameraIdLst> deviceIdLst = SplitCameraIdLst(PFALItem.info.CameraIdLst.Split('|'));
		foreach (var info in PFAInfoLst) 
		{
            CreatePassengerFlowAreaDeviceItem(info, deviceIdLst);
		}
		DeviceList.Reposition();
		DeviceList.gameObject.GetComponent<UIWidget>().UpdateAnchors();
	}
	/// <summary>
	/// 保存绑定设备信息
	/// </summary>
	public void SaveData()
	{
		Logger.Instance.WriteLog("保存客流统计区域设备绑定信息");
		string idLst = "";
		foreach(var child in DeviceList.GetChildList())
		{
			if(child.GetComponent<PassengerFlowAreaDeviceItem>().isSelected)
			{
				idLst += child.GetComponent<PassengerFlowAreaDeviceItem>().deviceId + child.GetComponent<PassengerFlowAreaDeviceItem>().GetToggle() + "|";
			}
		}
		if(idLst.Length > 0)
		{
			idLst = idLst.Remove(idLst.Length - 1);
		}
		PFALItem.UpdateDeviceIdLst(idLst);
		//Close();
	}
	/// <summary>
	/// 加载设备信息
	/// </summary>
	private void LoadDeviceInfo()
	{
		Logger.Instance.WriteLog("加载设备信息");
		DeviceDao dDao = new DeviceDao();
		dDao.Select001();
		PFAInfoLst = dDao.Result;
	}

	public void Close()
	{
		Logger.Instance.WriteLog("关闭客流统计区域设备绑定面板");
		foreach(var child in DeviceList.GetChildList())
		{
			Destroy(child.gameObject);
		}
		DeviceList.GetChildList().Clear();
		Configure.IsOperating = false;
		gameObject.SetActive(false);
	}

    private void CreatePassengerFlowAreaDeviceItem(DeviceInfo deviceInfo, List<CameraIdLst> cameraIdLst)
    {
		Logger.Instance.WriteLog("创建客流统计区域设备列表项目");
        GameObject go = Instantiate(PassengerFlowAreaDevicePrefab) as GameObject;
        DeviceList.AddChild(go.transform, true);
        go.transform.localScale = new Vector3(1, 1, 1);
        foreach (var cid in cameraIdLst)
        {
            if (deviceInfo.Id == cid.cameraId)
            {
                go.GetComponent<PassengerFlowAreaDeviceItem>().SetValue(deviceInfo.Id, true, deviceInfo.Description);
                go.GetComponent<PassengerFlowAreaDeviceItem>().SetInOut(cid);
                return;
            }

        }
        go.GetComponent<PassengerFlowAreaDeviceItem>().SetValue(deviceInfo.Id, false, deviceInfo.Description);
    }
    private List<CameraIdLst> SplitCameraIdLst(string[] value)
    {
        List<CameraIdLst> lst = new List<CameraIdLst>();
        string[] vs;
        foreach (string v in value)
        {
            if(string.IsNullOrEmpty(v.Trim()))continue;
            vs = v.Split(',');
            CameraIdLst cameraid = new CameraIdLst();
            cameraid.cameraId = vs[0];
            cameraid.inin = vs[1];
            cameraid.inout = vs[2];
            cameraid.outin = vs[3];
            cameraid.outout = vs[4];
            lst.Add(cameraid);
        }
        return lst;
    }

}

public struct CameraIdLst
{
    public string cameraId;
    public string inin;
    public string inout;
    public string outin;
    public string outout;
}
