using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class DeviceEdit : MonoBehaviour {

	/// <summary>
	/// 显示模型摄像机信息的预制体
	/// </summary>
	public GameObject DeviceListItemPrefab;
	/// <summary>
	/// 显示模型摄像机信息列表容器
	/// </summary>
	public UIGrid ItemContainer;

	public GameObject PhysicalDeviceListPanel;

	public GameObject SetMonitorScopePanel;

	public GameObject SetRTSPPanel;
	
	/// <summary>
	/// 显示物理摄像机信息的预制体
	/// </summary>
	public GameObject PhyDeviceListItemPrefab;
	/// <summary>
	/// 显示物理摄像机信息列表容器
	/// </summary>
	public UIGrid PhyDevItemContainer;

	private DelBind bind;
	// Use this for initialization
	void Start () 
	{
		bind = new DelBind ();
		ItemContainer.gameObject.SetActive(true);
		StartCoroutine ("LoadSceneMonitor");
		StartCoroutine ("LoadMonitorInfo");
	}

	/// <summary>
	/// 加载场景中的摄像机模型数据
	/// </summary>
	IEnumerator LoadSceneMonitor()
	{
		Logger.Instance.WriteLog("加载场景中的摄像机模型数据");
		GameObject[] MonitorObjs = GameObject.FindGameObjectsWithTag("CameraFace");
		foreach(GameObject go in MonitorObjs)
		{
			go.GetComponent<BoxCollider>().enabled = false;
			GameObject monitor = Instantiate(DeviceListItemPrefab) as GameObject;
			monitor.GetComponent<EditedDeviceListItem>().SetValue(go,bind,PhysicalDeviceListPanel,SetMonitorScopePanel,SetRTSPPanel);
			ItemContainer.AddChild(monitor.transform);
			monitor.transform.localScale = new Vector3(1,1,1);
		}
		yield return null;
	}
	/// <summary>
	/// 加载真实物理摄像机列表
	/// </summary>
	IEnumerator LoadMonitorInfo()
	{
		Logger.Instance.WriteLog("加载真实物理摄像机列表");
		List<CAMARE_INFO> infos;
		while(true)
		{
			infos = GetCameraInfo();
			if(infos != null)
			{
				for(int i= 0; i<infos.Count;i++)
				{
					GameObject monitor = Instantiate(PhyDeviceListItemPrefab) as GameObject;
					monitor.GetComponent<PhysicalDeviceListItem>().SetValue(i,infos[i],bind);
					PhyDevItemContainer.AddChild(monitor.transform);
					monitor.transform.localScale = new Vector3(1,1,1);
				}
//				foreach(CAMARE_INFO info in infos)
//				{
//					GameObject monitor = Instantiate(PhyDeviceListItemPrefab) as GameObject;
//					monitor.GetComponent<PhysicalDeviceListItem>().SetValue(info,bind);
//					PhyDevItemContainer.AddChild(monitor.transform);
//					monitor.transform.localScale = new Vector3(1,1,1);
//				}
				break;
			}
			yield return new WaitForSeconds(1);
		}
		yield return null;


	}
	/// <summary>
	/// 获取物理摄像机的信息
	/// </summary>
	/// <returns>The camera info.</returns>
	private List<CAMARE_INFO> GetCameraInfo()
	{
		Logger.Instance.WriteLog("获取物理摄像机的信息");
		if(CMSManage.Instance != null && CMSManage.Instance.isConnecting())
		{
			return CMSManage.Instance.GetCameraInfo();
		}
		Logger.Instance.WriteLog("获取物理摄像机的信息失败");
		return null;
	}

	public void ClosePhyDevicePanel()
	{
		Logger.Instance.WriteLog("关闭真实摄像机列表面板");
		Configure.IsOperating = false;
		PhysicalDeviceListPanel.SetActive (false);
		EditCameraManager.instance.CloseCamera();
		bind.bind = null;
	}
}

public delegate void BindCallback(CAMARE_INFO info);
/// <summary>
/// 绑定回调辅助类
/// </summary>
public class DelBind
{
	public BindCallback bind;
	public DeviceInfo deviceInfo;
}
