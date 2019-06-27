using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class EvacuationPlanManage : MonoBehaviour {

	public GameObject WarningListItemPrefab;
	public UIGrid WarningItemContainer;
	public GameObject AreaNamePrefab;
	//public GameObject CharactorPrefab;
	//private List<GameObject> charactors;
	private GameObject[] Cameras;
	private List<GameObject> WarningList;
	/// <summary>
	/// 保存疏散提示对象以及在数据库存储数据的id
	/// </summary>
	private Dictionary<GameObject,string> DicAlertAndID;
	/// <summary>
	/// 保存疏散提示对象对应的疏散预案
	/// </summary>
	private Dictionary<GameObject,EvacuatePlan> DicAlertAndPlan;
	/// <summary>
	/// 保存疏散区域id对应的疏散区域对象
	/// </summary>
	private Dictionary<string,GameObject> DicAreaIdAndAreaGo;
	/// <summary>
	/// 保存疏散区域id对应的疏散区域名称对象
	/// </summary>
	private Dictionary<string,GameObject> DicAreaIdAndAreaNameGo;
	/// <summary>
	/// 疏散方案选择列表
	/// </summary>
	public GameObject Popuplist;
	/// <summary>
	/// 疏散方案执行按钮
	/// </summary>
	public UIButton EvacuationPlanBtn;
	// Use this for initialization
	void Start () 
	{
		Logger.Instance.WriteLog("初始化疏散预案相关内容");
		Cameras = GameObject.FindGameObjectsWithTag ("CameraFace");
		WarningList = new List<GameObject> ();
		DicAlertAndID = new Dictionary<GameObject, string> ();
		DicAlertAndPlan = new Dictionary<GameObject, EvacuatePlan>();
		DicAreaIdAndAreaGo = new Dictionary<string, GameObject>();
		DicAreaIdAndAreaNameGo = new Dictionary<string, GameObject>();
		Popuplist.GetComponent<UIPopupList>().Clear();
		Areas = new List<GameObject>();
		AreaTexts = new List<GameObject>();
	}

	/// <summary>
	/// 重新加载疏散预案相关信息
	/// </summary>
	public void ReLoadEvacuationArea()
	{
		Popuplist.GetComponent<UIPopupList>().Clear();
		foreach(GameObject area in Areas)
		{
			Destroy(area);
		}
		foreach(GameObject AreaText in AreaTexts)
		{
			Destroy(AreaText);
		}
		Areas.Clear();
		AreaTexts.Clear();

		Logger.Instance.WriteLog("加载疏散预案信息");
		EvacuationPlanDao ePlanDao = new EvacuationPlanDao();
		var ePlan = ePlanDao.Select003();
		
		foreach (var plan in ePlan)
		{
			Popuplist.GetComponent<UIPopupList>().AddItem(plan.name);
		}
		if(ePlan.Count > 0)
		{
			Popuplist.GetComponent<UIPopupList>().value = ePlan[0].name;
			
			StartCoroutine(LoadEvacuationArea());
		}

	}

	List<GameObject> Areas;
	List<GameObject> AreaTexts;
	IEnumerator LoadEvacuationArea()
	{
		Logger.Instance.WriteLog("加载疏散区域信息");
		EvacuationPlanDao ePlanDao = new EvacuationPlanDao();
		List<EvacuateArea> evacuateArea = ePlanDao.Select001();
		GameObject goArea = null;
		GameObject areaText = null;
		foreach(var area in evacuateArea)
		{

			goArea = DrawArea(area);
			if(goArea == null)continue;
			areaText = SetEvacuateAreaText(area);
			AdjustTextAlignment(goArea,areaText);
			DicAreaIdAndAreaGo[area.id] = goArea;
			DicAreaIdAndAreaNameGo[area.id] = areaText;
			TweenColor tc = goArea.AddComponent<TweenColor>();
			tc.style = UITweener.Style.PingPong;
			tc.from = new Color(0,1,0,0.5f);
			tc.to = new Color(1,0,0,0.5f);
			goArea.SetActive(false);
			areaText.SetActive(false);
			Areas.Add(goArea);
			AreaTexts.Add(areaText);
		}

		yield return null;
	}

	/// <summary>
	/// 面片中心位置
	/// </summary>
	private Vector3 centerPos = Vector3.zero;
	//绘制区域
	private GameObject DrawArea(EvacuateArea evacuateArea)
	{
		Logger.Instance.WriteLog("绘制疏散区域");
		if(string.IsNullOrEmpty(evacuateArea.points.Trim()))return null;
		string[] point = evacuateArea.points.Split('|');
		Vector3[] pts = new Vector3[point.Length / 3];
		centerPos = Vector3.zero;
		for(int i = 0; i < pts.Length; i++)
		{
			pts[i] = new Vector3(float.Parse(point[i * 3]), float.Parse(point[i * 3 + 1]), float.Parse(point[i * 3 + 2]));
			centerPos += pts[i];
		}
		centerPos /= pts.Length;
		
		int[] triangles = new int[pts.Length];
		for(int i = 0; i < triangles.Length;i += 3)
		{
			triangles[i] = i;
			triangles[i + 1] = i + 2;
			triangles[i + 2] = i + 1;
		}
	    GameObject GOArea = new GameObject();
		GOArea.AddComponent<MeshCollider> ();
		MeshRenderer meshrend = GOArea.AddComponent<MeshRenderer>();
		meshrend.material.shader = Shader.Find("Transparent/Diffuse");
		meshrend.material.SetColor("_MainColor",new Color(0,1,0,0.2f));
		MeshFilter meshFilter = GOArea.AddComponent<MeshFilter>();
		meshFilter.mesh.vertices = pts;
		meshFilter.mesh.triangles = triangles;
		meshFilter.mesh.RecalculateNormals();
		GOArea.transform.position = new Vector3(0,0.3f,0);
		GOArea.name = evacuateArea.name;
		return GOArea;
	}
	
	//设置疏散区域名称对象
	private GameObject SetEvacuateAreaText(EvacuateArea evacuateArea)
	{
		Logger.Instance.WriteLog("设置疏散区域名称对象");
		GameObject NameText = Instantiate(AreaNamePrefab) as GameObject;
		centerPos.y = 0.2f;
		NameText.transform.position = centerPos;
		NameText.transform.parent = GameObject.Find("SceneUI").transform;
		NameText.transform.localRotation = Quaternion.identity;
		NameText.GetComponent<Text>().text = evacuateArea.name;
		NameText.GetComponent<Text>().fontSize = int.Parse(evacuateArea.fontSize);
		return NameText;
	}
	//设置疏散区域名称文字对齐方式
	private void AdjustTextAlignment(GameObject goArea,GameObject NameText)
	{
		Logger.Instance.WriteLog("设置疏散区域名称文字对齐方式");
		if(!NameText || !goArea) return;
		Vector3 size = goArea.transform.renderer.bounds.size * 4;
		if(NameText.GetComponent<Text>().preferredWidth <= size.x)
		{
			NameText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
		}
		else
		{
			NameText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
		}
		
		NameText.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,size.z);
	}



	/// <summary>
	/// 用来标示是否已经有疏散预案在执行
	/// </summary>
	private bool HasEvacuationPlanExcute = false;

	/// <summary>
	/// 开始执行疏散预案
	/// </summary>
	string EvacuationPlanBtnSprite;
	public void StartEvacuationPlan()
	{
		if(Popuplist.GetComponent<UIPopupList>().items.Count == 0)
		{
			Logger.Instance.WriteLog("执行疏散预案失败，没有可执行的疏散预案");
			return;
		}
		if(Popuplist.GetComponent<UIPopupList>().value.Trim() == "")
		{
			Logger.Instance.WriteLog("执行疏散预案失败，没有可执行的疏散预案");
			return;
		}

		Logger.Instance.WriteLog("开始执行疏散预案");
		if(HasEvacuationPlanExcute)
		{
			Logger.Instance.WriteLog("已经有疏散预案正在执行");
			return;
		}
		MainMenuController.canNotOpen = true;
		Popuplist.GetComponent<BoxCollider>().enabled = false;
		EvacuationPlanBtn.GetComponent<BoxCollider>().enabled = false;
		EvacuationPlanBtnSprite = EvacuationPlanBtn.normalSprite;
		EvacuationPlanBtn.normalSprite = EvacuationPlanBtn.pressedSprite;
		HasEvacuationPlanExcute = true;
		EvacuationPlanDao ePlanDao = new EvacuationPlanDao();
		EvacuatePlan evacuatePlan = ePlanDao.Select004(Popuplist.GetComponent<UIPopupList>().value)[0];
		PrivateStartEvacuationPlan(evacuatePlan);
	}

	void PrivateStartEvacuationPlan (EvacuatePlan evacuatePlan) 
	{
		GameObject go = Instantiate(WarningListItemPrefab) as GameObject;
		WarningItemContainer.AddChild(go.transform);
		go.transform.localScale = new Vector3(1,1,1);
		go.transform.FindChild("Label").GetComponent<UILabel>().text = evacuatePlan.name;

		UIEventListener.Get(go.transform.FindChild("Run").gameObject).onClick = ProcessAlert;
		UIEventListener.Get(go.transform.FindChild("Stop").gameObject).onClick = AlertRelease;
		WarningList.Add(go);
		Logger.Instance.WriteLog("保存预案处理日志");
		AlertPrecessLogDao aplDao = new AlertPrecessLogDao();
		aplDao.Insert001(System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),evacuatePlan.name,"","","","");
		DicAlertAndID[go] = aplDao.currentId;
		DicAlertAndPlan[go] = evacuatePlan;
	}

	/// <summary>
	/// 获取指定疏散预案对应的区域id列表
	/// </summary>
	/// <returns>The correlation area identifier.</returns>
	/// <param name="evacuatePlan">Evacuate plan.</param>
	List<string> GetCorrelationAreaId(EvacuatePlan evacuatePlan)
	{
		Logger.Instance.WriteLog("获取指定疏散预案对应的区域id列表");
		EvacuationPlanDao ePlanDao = new EvacuationPlanDao();
		List<EvacuateAreaOfPlan> eaoPlan = ePlanDao.Select002(evacuatePlan.id);
		List<string> areaIdLst = new List<string>();
		foreach(var eaop in eaoPlan)
		{
			areaIdLst.Add(eaop.evacuateAreaId);
		}
		return areaIdLst;
	}
	/// <summary>
	/// 获取疏散预案相关的摄像机id列表
	/// </summary>
	/// <returns>The correlation camera.</returns>
	/// <param name="AreaIdLst">Area identifier lst.</param>
	List<string> GetCorrelationCamera(List<string> AreaIdLst)
	{
		Logger.Instance.WriteLog("获取疏散预案相关的摄像机id列表");
		EvacuationPlanDao ePlanDao = new EvacuationPlanDao();
		//需要显示的摄像机列表
		List<string> CameraIdLst = new List<string>();
		//检索需要打开的摄像机列表
		foreach(var evacuateAreaId in AreaIdLst)
		{
			string cameraIdLst = ePlanDao.Select006(evacuateAreaId)[0].cameraList;
			if(string.IsNullOrEmpty(cameraIdLst.Trim()))
			{
				continue;
			}
			foreach(var id in cameraIdLst.Split('|'))
			{
				if(!CameraIdLst.Contains(id))
				{
					CameraIdLst.Add(id);
				}
			}
		}
		return CameraIdLst;
	}
	//开始处理报警
	void ProcessAlert(GameObject go)
	{
		Logger.Instance.WriteLog("开始处理报警");
		Color color = Color.white;
		color.a = 0.6f;
		List<string> areaIdLst = GetCorrelationAreaId(DicAlertAndPlan[go.transform.parent.gameObject]);
		List<string> CameraList = GetCorrelationCamera(areaIdLst);
		Logger.Instance.WriteLog("打开相关的摄像监控");
		int cameraOpendCount = 0;
		//最多打开6个摄像头
		if(CameraList != null && CameraList.Count > 0)
		{
			foreach(GameObject camera in Cameras)
			{
				if(CameraList.Contains(camera.GetComponent<MonitorInfoData>().Data.Id))
				{
					camera.BroadcastMessage("PlayEvacuationVideo",SendMessageOptions.DontRequireReceiver);
					cameraOpendCount++;
					if(cameraOpendCount >= 6)
					{
						break;
					}
				}
			}
		}
		if(areaIdLst != null && areaIdLst.Count > 0)
		{
			foreach(string areaId in areaIdLst)
			{
				DicAreaIdAndAreaGo[areaId].SetActive(true);
				DicAreaIdAndAreaNameGo[areaId].SetActive(true);
			}
		}
		Camera.main.GetComponent<CameraController>().MoveToMaxHeight();

		go.transform.parent.FindChild ("Stop").gameObject.SetActive (false);
		go.transform.parent.FindChild ("Background").GetComponent<TweenAlpha> ().enabled = true;
		go.GetComponentInChildren<UILabel> ().text = "处理完成";
		UIEventListener.Get (go).onClick = ProcesFinished;
		Logger.Instance.WriteLog("更新疏散预案处理日志");
		AlertPrecessLogDao aplDao = new AlertPrecessLogDao();
		aplDao.Update001 (System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),DataStore.UserInfo.RealName,DicAlertAndID[go.transform.parent.gameObject]);
	}
	//处理完成
	void ProcesFinished(GameObject go)
	{
		Logger.Instance.WriteLog("疏散预案执行完成");
		Logger.Instance.WriteLog("更新疏散预案处理日志");
		AlertPrecessLogDao aplDao = new AlertPrecessLogDao();
		aplDao.Update002 (System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),"处理完成",DicAlertAndID[go.transform.parent.gameObject]);
		DicAlertAndID.Remove (go.transform.parent.gameObject);
		DicAlertAndPlan.Remove(go.transform.parent.gameObject);
		WarningList.Remove (go.transform.parent.gameObject);
		WarningItemContainer.RemoveChild (go.transform.parent);
		Destroy (go.transform.parent.gameObject);
		Logger.Instance.WriteLog("关闭所有关联摄像监控");
		foreach(GameObject camera in Cameras)
		{
			camera.BroadcastMessage("StopEvacuationVideo",SendMessageOptions.DontRequireReceiver);
		}
		HasEvacuationPlanExcute = false;
		Popuplist.GetComponent<BoxCollider>().enabled = true;
		EvacuationPlanBtn.GetComponent<BoxCollider>().enabled = true;
		EvacuationPlanBtn.normalSprite = EvacuationPlanBtnSprite;
		HideEvacuationArea();
		MainMenuController.canNotOpen = false;
	}
	//解除报警
	void AlertRelease(GameObject go)
	{
		Logger.Instance.WriteLog("解除报警");
		Logger.Instance.WriteLog("更新疏散预案处理日志");
		AlertPrecessLogDao aplDao = new AlertPrecessLogDao();
		aplDao.Update003 (System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),DataStore.UserInfo.RealName,"误报警",DicAlertAndID[go.transform.parent.gameObject]);
		DicAlertAndID.Remove (go.transform.parent.gameObject);
		DicAlertAndPlan.Remove (go.transform.parent.gameObject);
		WarningList.Remove (go.transform.parent.gameObject);
		WarningItemContainer.RemoveChild (go.transform.parent);
		Destroy (go.transform.parent.gameObject);
		HasEvacuationPlanExcute = false;
		Popuplist.GetComponent<BoxCollider>().enabled = true;
		EvacuationPlanBtn.GetComponent<BoxCollider>().enabled = true;
		EvacuationPlanBtn.normalSprite = EvacuationPlanBtnSprite;
		MainMenuController.canNotOpen = false;
	}
	/// <summary>
	/// 隐藏疏散区域
	/// </summary>
	private void HideEvacuationArea()
	{
		Logger.Instance.WriteLog("隐藏疏散区域");
		foreach(var go in DicAreaIdAndAreaGo.Values)
		{
			go.SetActive(false);
		}
		foreach(var go in DicAreaIdAndAreaNameGo.Values)
		{
			go.SetActive(false);
		}

	}
}
