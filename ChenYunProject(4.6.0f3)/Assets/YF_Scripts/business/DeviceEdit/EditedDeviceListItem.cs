using UnityEngine;
using System.Collections.Generic;
using System.Text;
public class EditedDeviceListItem : MonoBehaviour {

	public GameObject Description;
	public GameObject BtnBind;
	public GameObject BtnGoto;
	public GameObject BtnSetScope;
	public GameObject BtnSetRTSP;
	public GameObject SelectedBg;
	public GameObject lingxingPrefab;
	public GameObject MonitorScopePrefab;

	private GameObject monitor;
	private DeviceInfo info;
	private bool Existing = false;
	private DelBind delBind;
	private GameObject phyDevicePanel;
	private string CameraTag;
	private GameObject lingxing;
	private GameObject monitorScope;
	private GameObject setMonitorScopePanel;
	private GameObject setRTSPPanel;

	public void SetValue(GameObject monitor,DelBind delBind,GameObject phyDevicePanel,GameObject SMSPanel,GameObject RTSPPanel)
	{
		Logger.Instance.WriteLog("初始化场景摄像机列表项目");
		this.delBind = delBind;
		this.monitor = monitor;
		this.phyDevicePanel = phyDevicePanel;
		setMonitorScopePanel = SMSPanel;
		setRTSPPanel = RTSPPanel;
		BtnSetScope.SetActive(false);
		BtnSetRTSP.SetActive(false);
		CameraTag = monitor.transform.GetChild (0).name;
		Logger.Instance.WriteLog("从数据加载此列表项相关信息");
		DeviceDao dDao = new DeviceDao ();
		dDao.Select002 (CameraTag);
		if(dDao.Result.Count > 0)
		{
			Logger.Instance.WriteLog("数据加载成功");
			info = dDao.Result[0];
			Description.GetComponent<UIInput>().value = info.Description;
			Description.GetComponent<BoxCollider>().enabled = true;
			Existing = true;
			monitorScope = Instantiate(MonitorScopePrefab,
			                           new Vector3(monitor.transform.position.x,0.2f,monitor.transform.position.z),
			                           Quaternion.Euler(0,0,0)) as GameObject;
			DrawSector ds = monitorScope.GetComponent<DrawSector>();
			ds.Scope = int.Parse(info.MonitorScope);
			ds.Radio = int.Parse(info.MonitorRadio);
			ds.Offset = int.Parse(info.MonitorOffset);
			monitorScope.SetActive(false);
			monitor.GetComponent<MonitorInfoData>().Data = dDao.Result[0];
		}
		else
		{
			Logger.Instance.WriteLog("数据加载失败");
			Description.GetComponent<UIInput>().value = CameraTag;
			Description.GetComponent<BoxCollider>().enabled = false;
			Existing = false;
		}



		initDescription = Description.GetComponent<UIInput>().value;

//		lingxing = Instantiate(lingxingPrefab) as GameObject;
//		lingxing.transform.parent = monitor.transform;
//		lingxing.transform.localPosition = Vector3.zero;
		//lingxing.SetActive(false);
	}

	void Binding(CAMARE_INFO info)
	{
		List<byte> des = new List<byte>();
		foreach(byte b in info.describe)
		{
			if(b == 0)
			{
				break;
			}
			des.Add(b);
		}
		List<byte> name = new List<byte>();
		foreach(byte b in info.name)
		{
			if(b == 0)
			{
				break;
			}
			name.Add(b);
		}

		initDescription = System.Text.Encoding.Default.GetString (des.ToArray());
		if(string.IsNullOrEmpty(Description.GetComponent<UIInput>().value))
		{
			Description.GetComponent<UIInput>().value = initDescription;
		}
		//Description.GetComponent<UIInput>().value = initDescription; 
		DeviceDao dDao = new DeviceDao ();
		dDao.Select002 (CameraTag);
		if(dDao.Result.Count <= 0)
		{
			dDao.Insert001(CMSManage.GUIDToString(info.camareGuid),Encoding.Default.GetString (name.ToArray()),Description.GetComponent<UIInput>().value,
			               monitor.transform.position.x.ToString(),monitor.transform.position.y.ToString(),monitor.transform.position.z.ToString(),CameraTag);
			//Encoding.Default.GetString (des.ToArray())
		}
		else
		{
			dDao.Update002(CMSManage.GUIDToString(info.camareGuid),Encoding.Default.GetString (name.ToArray()),Description.GetComponent<UIInput>().value,dDao.Result[0].Id);
		}
		phyDevicePanel.SetActive (false);
		Configure.IsOperating = false;
		EditCameraManager.instance.CloseCamera();
		delBind.bind = null;

		Description.GetComponent<BoxCollider>().enabled = true;

		monitorScope = Instantiate(MonitorScopePrefab,
		                           new Vector3(monitor.transform.position.x,0.2f,monitor.transform.position.z),
		                           Quaternion.Euler(0,0,0)) as GameObject;
		monitorScope.SetActive(false);
	}

	public void Change()
	{
		Logger.Instance.WriteLog("打开绑定真实摄像机信息面板");
//		if(Configure.IsOperating)
//		{
//			Logger.Instance.WriteLog("正在执行其它操作");
//			return;
//		}
		Configure.IsOperating = true;
		//if(delBind.bind != null) return;
		transform.parent.BroadcastMessage("CancelSelected",gameObject);
		//lingxing.SetActive(true);

		delBind.bind = Binding;
		delBind.deviceInfo = info;
		phyDevicePanel.SetActive (false);
		phyDevicePanel.SetActive (true);
		SelectedBg.SetActive (true);
	}

	/// <summary>
	/// 移动到摄像机模型位置
	/// </summary>
	public void GotoMonitorPos()
	{
		Logger.Instance.WriteLog("移动到摄像机模型位置");
		if(Configure.IsOperating)
		{
			Logger.Instance.WriteLog("正在执行其它操作");
			return;
		}
		if(delBind.bind != null) return;

		SelectedBg.SetActive (true);
		transform.parent.BroadcastMessage("CancelSelected",gameObject);
		//lingxing.SetActive(true);
		if(monitorScope != null)monitorScope.SetActive(true);
		if(monitorScope != null)BtnSetScope.SetActive(true);
		if(monitorScope != null)BtnSetRTSP.SetActive(true);
		Camera.main.GetComponent<CameraController> ().GotoPosition(monitor.transform.position,2);
	}
	/// <summary>
	/// 当描述文字被修改时调用
	/// 标记当前描述文字被修改
	/// </summary>
	bool inputChanged = false;
	string initDescription = "";
	public void OnInputChanged()
	{
		inputChanged = false;
		if(initDescription != Description.GetComponent<UIInput>().value)
		{
			initDescription = Description.GetComponent<UIInput>().value;
			inputChanged = true;
		}
	}
	/// <summary>
	/// 当输入框失去焦点时调用,保存修改内容到数据库
	/// </summary>
	public void SaveDescription()
	{
		DeviceDao dDao = new DeviceDao ();
		dDao.Update003(Description.GetComponent<UIInput>().value,info.Id);
	}

	public void SetScope()
	{
		Logger.Instance.WriteLog("打开摄像机显示区域设置面板");
		if(Configure.IsOperating)
		{
			Logger.Instance.WriteLog("正在执行其它操作");
			return;
		}
		Configure.IsOperating = true;
		setMonitorScopePanel.SetActive(true);
		ReloadData();
		setMonitorScopePanel.GetComponent<MonitorScopeSetter>().SetValue(monitorScope,info.Id);
	}
	/// <summary>
	/// 设置RTSP信息
	/// </summary>
	public void SetRTSP()
	{
		Logger.Instance.WriteLog("打开摄像机RTSP信息设置面板");
		if(Configure.IsOperating)
		{
			Logger.Instance.WriteLog("正在执行其它操作");
			return;
		}
		Configure.IsOperating = true;
		setRTSPPanel.SetActive(true);
		ReloadData();
		setRTSPPanel.GetComponent<RTSPSetter>().SetValue(info.RTSPUrl,info.UseRTSP,info.PassengerFlowUrl,info.Id);
	}
	//从数据库中重新加载数据
	void ReloadData()
	{
		Logger.Instance.WriteLog("从数据库中重新加载此列表项相关信息");
		DeviceDao dDao = new DeviceDao ();
		dDao.Select002 (CameraTag);
		if(dDao.Result.Count > 0)
		{
			info = dDao.Result[0];
			DrawSector ds = monitorScope.GetComponent<DrawSector>();
			ds.Scope = int.Parse(info.MonitorScope);
			ds.Radio = int.Parse(info.MonitorRadio);
			ds.Offset = int.Parse(info.MonitorOffset);
			Description.GetComponent<UIInput>().value = info.Description;
		}
	}
	void CancelSelected(GameObject except)
	{
		if(except == gameObject) return;
		//lingxing.SetActive(false);
		SelectedBg.SetActive (false);
		BtnSetScope.SetActive(false);
		BtnSetRTSP.SetActive(false);
		if(monitorScope != null)monitorScope.SetActive(false);
		setMonitorScopePanel.SetActive(false);
		setRTSPPanel.SetActive(false);
	}

	/// <summary>
	/// 当鼠标放到名称或字体输入框时调用
	/// 如果有其它操作禁用输入功能
	/// </summary>
	public void OnDescriptionLabelHoverOver()
	{
		if(Configure.IsOperating)
		{
			Description.GetComponent<UIInput>().enabled = false;
			Description.GetComponent<UIInput>().isSelected = false;
		}
		else
		{
			Description.GetComponent<UIInput>().enabled = true;
			SelectedBg.SetActive (true);
			transform.parent.BroadcastMessage("CancelSelected",gameObject);
		}
	}
}
