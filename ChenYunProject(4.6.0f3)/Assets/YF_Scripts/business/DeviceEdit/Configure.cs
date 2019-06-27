using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 用来切换功能
/// 设备设置 跟客流统计区域设置直接切换
/// </summary>
public class Configure : MonoBehaviour {

	public GameObject DeviceEdit;
	public GameObject PassengerFlowAreaEdit;
	public GameObject EvacuateAreaEdit;
	public GameObject EvacuatePlanEdit;
	public GameObject HoverObjectInfo;
	public GameObject HidePanelBtn;
	private Dictionary<string,GameObject> BtnMapping;
	private Dictionary<string,string> MsgMapping;
	[HideInInspector]
	public static bool IsOperating
	{
		get
		{
			if(_IsOperation)
			{
				WarnWindow.Instance.Show(WarnWindow.WarnType.IsOperation);
			}
			return _IsOperation;
		}
		set
		{
			_IsOperation = value;
		}
	}
	private static bool _IsOperation = false;
	public void Start()
	{
		HidePanelBtn.SetActive(false);
		BtnMapping = new Dictionary<string, GameObject>();
		BtnMapping.Add("ShowDeviceEdit",DeviceEdit);
		BtnMapping.Add("ShowPassengerFlowAreaEdit",PassengerFlowAreaEdit);
		BtnMapping.Add("ShowEvacuateAreaEdit",EvacuateAreaEdit);
		BtnMapping.Add("ShowEvacuatePlanEdit",EvacuatePlanEdit);
		MsgMapping = new Dictionary<string, string>();
		MsgMapping.Add("ShowDeviceEdit","打开设备编辑面板");
		MsgMapping.Add("ShowPassengerFlowAreaEdit","打开客流区域编辑面板");
		MsgMapping.Add("ShowEvacuateAreaEdit","打开疏散预案区域编辑面板");
		MsgMapping.Add("ShowEvacuatePlanEdit","打开疏散预案编辑面板");
		StartCoroutine("Init");
		StartCoroutine("CheckMouseHover");
	}

	void OnDisable()
	{
		IsOperating = false;
	}

	IEnumerator Init()
	{
		GameObject[] MonitorObjs = GameObject.FindGameObjectsWithTag("CameraFace");
		foreach(GameObject go in MonitorObjs)
		{
			go.transform.GetChild (1).gameObject.SetActive(true);
			go.transform.GetChild (1).gameObject.AddComponent<MeshCollider>();
		}
		yield return null;
	}

	IEnumerator CheckMouseHover()
	{
		string CameraName = "";
		DeviceDao dDao = new DeviceDao ();
		while(true)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit,1000,1<<LayerMask.NameToLayer("LingXing")))
			{
				Logger.Instance.WriteLog("显示鼠标悬停摄像机信息");
				CameraName = hit.transform.parent.GetChild (0).name;
				HoverObjectInfo.SetActive(true);
				dDao.Select002 (CameraName);
				if(dDao.Result != null && dDao.Result.Count == 1)
				{
					HoverObjectInfo.GetComponent<HoverOverCameraInfo>().Init(dDao.Result[0].Description,string.IsNullOrEmpty(dDao.Result[0].PassengerFlowUrl.Trim())?"没有客流统计信息":"有客流统计信息");
				}
				else
				{
					HoverObjectInfo.GetComponent<HoverOverCameraInfo>().Init(CameraName,"没有绑定物理摄像机");
				}
				HoverObjectInfo.GetComponent<UISprite>().pivot = UIWidget.Pivot.BottomLeft;
				HoverObjectInfo.GetComponentInChildren<UILabel>().alignment = NGUIText.Alignment.Left;
				if(Input.mousePosition.x + HoverObjectInfo.GetComponent<UISprite>().width > Screen.width)
				{
					HoverObjectInfo.GetComponent<UISprite>().pivot = UIWidget.Pivot.BottomRight;
					HoverObjectInfo.GetComponentInChildren<UILabel>().alignment = NGUIText.Alignment.Right;
				}
				HoverObjectInfo.transform.position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
			}
			else
			{
				HoverObjectInfo.SetActive(false);
			}
			yield return new WaitForSeconds(1f);
		}

	}
	/// <summary>
	/// 用来判断esc键是否被按下
	/// </summary>
	bool isEscapeDown = false;
	/// <summary>
	/// 用来判断左边的Shift键是否被按下
	/// </summary>
	bool isLeftShiftDown = false;
	void Update()
	{
		if(isEscapeDown && isLeftShiftDown)
		{
			Logger.Instance.WriteLog("切换到登陆界面");
			OdbcDataManager.Instance.ResetConnection();
			Application.LoadLevel("LoginScene");
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			isEscapeDown = true;
		}
		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			isLeftShiftDown = true;
		}
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			isEscapeDown = false;
		}
		if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			isLeftShiftDown = false;
		}
	}
	/// <summary>
	/// 设备配置
	/// </summary>
	public void ShowDeviceEdit()
	{
		Execute("ShowDeviceEdit");
	}
	/// <summary>
	/// 客流区域配置
	/// </summary>
	public void ShowPassengerFlowAreaEdit()
	{
		Execute("ShowPassengerFlowAreaEdit");
	}
	/// <summary>
	/// 疏散区域配置
	/// </summary>
	public void ShowEvacuateAreaEdit()
	{
		Execute("ShowEvacuateAreaEdit");
	}
	/// <summary>
	/// 疏散预案配置
	/// </summary>
	public void ShowEvacuatePlanEdit()
	{
		Execute("ShowEvacuatePlanEdit");
	}

	private GameObject OpendPanel;
	public void CloseOpendPanel()
	{
		if(OpendPanel)
		{
			HidePanelBtn.SetActive(false);
			OpendPanel.SetActive(false);
		}
	}


	private void Execute(string operation)
	{
		if(MsgMapping.ContainsKey(operation))
		{
			Logger.Instance.WriteLog(MsgMapping[operation]);
		}
		if(IsOperating)
		{
			Logger.Instance.WriteLog("正在执行其它操作");
			return;
		}
		HidePanelBtn.SetActive(true);
		foreach (var item in BtnMapping) 
		{
			if(item.Value == null)
			{
				continue;
			}
			if(item.Key == operation)
			{
				item.Value.SetActive(true);
				OpendPanel = item.Value;
			}
			else
			{
				item.Value.SetActive(false);
			}
		}
	}
}
