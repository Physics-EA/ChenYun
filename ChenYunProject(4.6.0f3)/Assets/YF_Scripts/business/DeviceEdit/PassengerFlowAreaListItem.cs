using UnityEngine;
using System.Collections;
/// <summary>
/// 客流统计区域具体项目
/// 变更区域名称
/// 设置绑定设备
/// 设置报警级别人数
/// </summary>
public class PassengerFlowAreaListItem : MonoBehaviour {
	/// <summary>
	/// 区域名称
	/// </summary>
	public UIInput InputName;
	/// <summary>
	/// 被选择是的背景色
	/// </summary>
	public GameObject SelectedBg;
	/// <summary>
	/// 区域按钮
	/// </summary>
	public GameObject AreaBt;
	/// <summary>
	/// 定位按钮
	/// </summary>
	public GameObject GotoBt;
	/// <summary>
	/// 删除按钮
	/// </summary>
	public GameObject DeleteBt;
	/// <summary>
	/// 绘制区域的预制体
	/// </summary>
	public GameObject PassengerFlowAreaPrefab;
	/// <summary>
	/// 设定区域的按钮标签
	/// </summary>
	public UILabel ULScope;
	/// <summary>
	/// 客流区域条目信息
	/// </summary>
	[HideInInspector]
	public PassengerFlowAreaInfo info;
	/// <summary>
	/// 客流信息窗口根节点
	/// </summary>
	[HideInInspector]
	public GameObject PassengerFlowAreaUIRoot;
	/// <summary>
	/// 保存绘制的区域
	/// </summary>
	private GameObject area;
	/// <summary>
	/// 保存客流区域管理的引用
	/// </summary>
	private PassengerFlowAreaManage PFAMagage;
	/// <summary>
	/// 保存显示或设置绑定的设备列表窗口
	/// </summary>
	private GameObject DeviceListPanel;
	/// <summary>
	/// 保存显示或设置报警人数阈值的窗口
	/// </summary>
	private GameObject WarnLevelPanel;
	/// <summary>
	/// 面片中心位置
	/// </summary>
	private Vector3 centerPos = Vector3.zero;
	/// <summary>
	/// 显示客流信息窗口
	/// </summary>
	private GameObject PassengerFlowAreaUI;

	/// <summary>
	/// 当鼠标放到描述输入框时调用
	/// 如果有其它操作禁用输入功能
	/// </summary>
	public void OnDescriptionLabelHoverOver()
	{
		if(Configure.IsOperating)
		{
			InputName.enabled = false;
			InputName.isSelected = false;
		}
		else
		{
			InputName.enabled = true;
			Selected();
		}
	}
	
	/// <summary>
	/// 自动加载初始化调用
	/// </summary>
	/// <param name="_info">_info.</param>
	/// <param name="_PFAMagage">_ PFA magage.</param>
	/// <param name="_DeviceListPanel">_ device list panel.</param>
	/// <param name="_WarnLevelPanel">_ warn level panel.</param>
	public void SetValue(PassengerFlowAreaInfo _info,PassengerFlowAreaManage _PFAMagage,GameObject _DeviceListPanel,GameObject _WarnLevelPanel,GameObject _PassengerFlowAreaUI)
	{
		Logger.Instance.WriteLog("初始化客流统计区域列表项目");
		info = _info;
		InputName.value = info.Name;
		PFAMagage = _PFAMagage;
		DeviceListPanel = _DeviceListPanel;
		WarnLevelPanel = _WarnLevelPanel;
		if(info.Name == "主客流")
		{
			AreaBt.SetActive(false);
			GotoBt.SetActive(false);
			DeleteBt.SetActive(false);
			area = null;
		}
		else 
		{
			Logger.Instance.WriteLog("创建客流区域显示对象");
			string[] point = info.Points.Split('|');
			Vector3[] pts = new Vector3[point.Length / 3];
			
			for(int i = 0; i < pts.Length; i++)
			{
				pts[i] = new Vector3(float.Parse(point[i * 3]), float.Parse(point[i * 3 + 1]), float.Parse(point[i * 3 + 2]));
				centerPos += pts[i];
			}
			centerPos /= pts.Length;
			
			int[] triangles = new int[pts.Length];
			for(int i = 0; i < triangles.Length;i++)
			{
				triangles[i] = i;
			}
			area = new GameObject();
			area.layer = LayerMask.NameToLayer("PassengerFlowArea");
			area.AddComponent<MeshCollider> ();
			MeshRenderer meshrend = area.AddComponent<MeshRenderer>();
			meshrend.material.shader = Shader.Find("Particles/Alpha Blended");
			meshrend.material.SetColor("_TintColor",new Color(0,1,0,0.2f));
			MeshFilter meshFilter = area.AddComponent<MeshFilter>();
			meshFilter.mesh.vertices = pts;
			meshFilter.mesh.triangles = triangles;
			meshFilter.mesh.RecalculateNormals();
			area.transform.position = new Vector3(0,0.2f,0);
			area.layer = LayerMask.NameToLayer("PassengerFlowArea");

			Logger.Instance.WriteLog("绑定客流统计信息显示面板");
			PassengerFlowAreaUI = _PassengerFlowAreaUI;
			PassengerFlowAreaUI.GetComponent<PassengerAreaUI>().Bind(centerPos,info.Name);
			PassengerFlowAreaUI.transform.parent = PassengerFlowAreaUIRoot.transform;
			PassengerFlowAreaUI.transform.localScale = new Vector3(1,1,1);
			PassengerFlowAreaUI.AddComponent<EditedPassengerFlowInfoShow>().pAreaUI = PassengerFlowAreaUI.GetComponent<PassengerAreaUI>();
			PassengerFlowAreaUI.GetComponent<EditedPassengerFlowInfoShow>().Init(info);
			EditedPassengerFlowInfoReceiver.PFArea += PassengerFlowAreaUI.GetComponent<EditedPassengerFlowInfoShow>().UpdateData;
		}

	}
	/// <summary>
	/// 手动添加初始化调用
	/// </summary>
	/// <param name="_info">_info.</param>
	/// <param name="_PFAMagage">_ PFA magage.</param>
	/// <param name="_area">_area.</param>
	/// <param name="_DeviceListPanel">_ device list panel.</param>
	/// <param name="_WarnLevelPanel">_ warn level panel.</param>
	public void SetValue(PassengerFlowAreaInfo _info,PassengerFlowAreaManage _PFAMagage,GameObject _area,GameObject _DeviceListPanel,GameObject _WarnLevelPanel,GameObject _PassengerFlowAreaUI)
	{
		Logger.Instance.WriteLog("初始化客流统计区域列表项目");
		info = _info;
		InputName.value = info.Name;
		PFAMagage = _PFAMagage;
		DeviceListPanel = _DeviceListPanel;
		WarnLevelPanel = _WarnLevelPanel;
		area = _area;

		Logger.Instance.WriteLog("绑定客流统计信息显示面板");
		PassengerFlowAreaUI = _PassengerFlowAreaUI;
		PassengerFlowAreaUI.AddComponent<EditedPassengerFlowInfoShow>().pAreaUI = PassengerFlowAreaUI.GetComponent<PassengerAreaUI>();
		PassengerFlowAreaUI.GetComponent<EditedPassengerFlowInfoShow>().Init(info);
		EditedPassengerFlowInfoReceiver.PFArea += PassengerFlowAreaUI.GetComponent<EditedPassengerFlowInfoShow>().UpdateData;
		PassengerFlowAreaUI.SetActive(false);
	}

	private float FLOAT(string str)
	{
		return float.Parse(str);
	}

	private string STRING(float val)
	{
		return val.ToString();
	}
	/// <summary>
	/// 移动到统计区域位置
	/// </summary>
	public void GotoAreaPos()
	{	
		Logger.Instance.WriteLog("移动到统计区域位置");
		if(Configure.IsOperating)
		{
			Logger.Instance.WriteLog("正在执行其它操作");
			return;
		}

		Selected();

		if(info.Points.Trim() != "")Camera.main.GetComponent<CameraController> ().GotoPosition(centerPos,2);
	}
	/// <summary>
	/// 当区域名称改变时调用，用来判断名称是否合法
	/// </summary>
	public void OnNameChanged()
	{
		Logger.Instance.WriteLog("调用客流统计区域名称变更");
		if(InputName.value == info.Name)return;
		if(info.Name == "主客流")
		{
			InputName.value = info.Name;
			return;
		}
		int index = PFAMagage.FindPassengerFlowAreaInfoIndex(InputName.value);
		if(index < 0)
		{
			Logger.Instance.WriteLog("保存变更后客流统计区域名称");
			PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao ();
			pfaDao.Update002(InputName.value,info.Id);
			PFAMagage.UpdateAreaName(info.Name,InputName.value);
			info.Name = InputName.value;
			PassengerFlowAreaUI.GetComponent<PassengerAreaUI>().name.text = info.Name;
			return;
		}
		else
		{
			InputName.value = info.Name;
			WarnWindow.Instance.Show(WarnWindow.WarnType.SameName);
		}
		Logger.Instance.WriteLog("调用客流统计区域名称变更失败");
	}

	private bool IsDrawingArea = false;
	/// <summary>
	/// 单击绘制按钮时调用
	/// 第一次单击 是绘制
	/// 第二次单击是撤销
	/// </summary>
	public void ModifyScope()
	{
		//如果正在绘制区域则取消绘制操作
		if(IsDrawingArea)
		{
			Logger.Instance.WriteLog("取消客流统计区域绘制");
			ULScope.text = "绘制";
			ULScope.color = Color.white;
			PFAMagage.FinishedUpdatePassengerFlowArea();
			if(area)area.SetActive(true);
			if(area && PassengerFlowAreaUI)PassengerFlowAreaUI.SetActive(true);
			IsDrawingArea = false;
			Configure.IsOperating = false;
			return;
		}
		Logger.Instance.WriteLog("客流统计区域绘制");
		if(Configure.IsOperating)
		{
			Logger.Instance.WriteLog("正在执行其它操作");
			return;
		}
		IsDrawingArea = true;
		Selected();
		Configure.IsOperating = true;
		ULScope.text = "取消";
		ULScope.color = Color.black;
		if(area)area.SetActive(false);
		if(PassengerFlowAreaUI)PassengerFlowAreaUI.SetActive(false);
		PFAMagage.UpdatePassengerFlowArea(this);
	}

	public void AreaModify(GameObject go, string points)
	{
		Logger.Instance.WriteLog("修改客流统计区域");
		if(area != null)
		{
			Destroy(area);
		}
		PFAMagage.FinishedUpdatePassengerFlowArea();
		IsDrawingArea = false;
		ULScope.text = "绘制";
		ULScope.color = Color.white;
		Configure.IsOperating = false;
		area = go;
		Logger.Instance.WriteLog("更新客流统计区域信息");
		PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao ();
		pfaDao.Update001 (points, info.Id);
		info.Points = points;
		string[] point = points.Split('|');
		Vector3[] pts = new Vector3[point.Length / 3];
		Vector3 vec = Vector3.zero;
		for(int i = 0; i < pts.Length; i++)
		{
			pts[i] = new Vector3(float.Parse(point[i * 3]), float.Parse(point[i * 3 + 1]), float.Parse(point[i * 3 + 2]));
			vec += pts[i];
		}
		vec /= pts.Length;
		centerPos = vec;
		if(area && PassengerFlowAreaUI)
		{
			Logger.Instance.WriteLog("绑定客流统计信息显示面板");
			PassengerFlowAreaUI.SetActive(true);
			PassengerFlowAreaUI.GetComponent<PassengerAreaUI>().Bind(centerPos,info.Name);
		}
	}

	/// <summary>
	/// 点击绑定按钮时调用
	/// 打开设备绑定窗口
	/// </summary>
	public void BindDevice()
	{
		Logger.Instance.WriteLog("打开客流统计区域绑定设备面板");
		if(Configure.IsOperating)
		{
			Logger.Instance.WriteLog("正在执行其它操作");
			return;
		}
		Selected();
		Configure.IsOperating = true;
		if(DeviceListPanel.activeSelf)return;
		DeviceListPanel.SetActive(true);
		DeviceListPanel.GetComponent<PassengerFlowAreaDeviceEdit>().ShowDetial(this);
	}
	/// <summary>
	/// 点击阈值按钮时调用
	/// 打开区域报警级别阈值窗口
	/// </summary>
	public void SetWarnLevel()
	{
		Logger.Instance.WriteLog("打开客流统计区域报警级别阈值设置面板");
		if(Configure.IsOperating)
		{
			Logger.Instance.WriteLog("正在执行其它操作");
			return;
		}
		Selected();
		Configure.IsOperating = true;
		if(WarnLevelPanel.activeSelf)return;
		WarnLevelPanel.SetActive(true);
		WarnLevelPanel.GetComponent<PassengerFlowAreaWarnLevelEdit>().SetValue(this);
	}
	/// <summary>
	/// 更新区域报警级别阈值信息
	/// </summary>
	public void UpdateWarnLevel()
	{
		Logger.Instance.WriteLog("更新客流统计区域报警级别阈值信息");
		PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao ();
		pfaDao.Update004(info.WarnLevel1,info.WarnLevel2,info.WarnLevel3,info.Id);
	}
	/// <summary>
	/// 更新绑定的设备信息
	/// </summary>
	/// <param name="idList">Identifier list.</param>
	public void UpdateDeviceIdLst(string idList)
	{
		Logger.Instance.WriteLog("更新客流统计区域绑定的设备信息");
		info.CameraIdLst = idList;
		PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao ();
		pfaDao.Update003(info.CameraIdLst,info.Id);
		PassengerFlowAreaUI.GetComponent<EditedPassengerFlowInfoShow>().Init(info);
	}

	public void DeleteItem()
	{
		PFAMagage.SelectedItem = gameObject;
		PFAMagage.DeletePassnegerFlowArea();
	}

	public void Selected()
	{
		if(Configure.IsOperating)return;
		transform.parent.BroadcastMessage("CancelSelected",gameObject);
		SelectedBg.SetActive(true);
	}

	void CancelSelected(GameObject except)
	{
		if(except == gameObject) return;
		SelectedBg.SetActive (false);
	}

	void OnDisable()
	{
		if(area)area.SetActive(false);
		SelectedBg.SetActive (false);
		if(PassengerFlowAreaUI)PassengerFlowAreaUI.SetActive(false);
		if(Camera.main)Camera.main.GetComponent<CameraController> ().StopMove();
	}
	void OnEnable()
	{
		if(area)area.SetActive(true);
		if(area && PassengerFlowAreaUI)PassengerFlowAreaUI.SetActive(true);
	}
	void OnDestroy()
	{
		if(PassengerFlowAreaUI)EditedPassengerFlowInfoReceiver.PFArea -= PassengerFlowAreaUI.GetComponent<EditedPassengerFlowInfoShow>().UpdateData;
		if(area)Destroy(area);
		if(PassengerFlowAreaUI)Destroy(PassengerFlowAreaUI);
		if(Camera.main)Camera.main.GetComponent<CameraController> ().StopMove();
	}
}
