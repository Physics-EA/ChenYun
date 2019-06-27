using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VideoPatrolMange : MonoBehaviour {
	/// <summary>
	/// 存放已经被选择摄像头对象的组件
	/// </summary>
	public UIGrid SelectedMornitorList;
	/// <summary>
	/// 存放所有需要选中的摄像头的组件
	/// </summary>
	public UIGrid SelectMornitorList;
	/// <summary>
	/// 被选择摄像头列表项目预制体
	/// </summary>
	public GameObject SelectedMornitorItemPrefab;
	/// <summary>
	/// 需要选择摄像头列表项目预制体
	/// </summary>
	public GameObject SelectMornitorItemPrefab;
	/// <summary>
	/// 方案编辑相关联的小地图
	/// </summary>
	public GameObject VideoPatrolPlanEditMapPanel;
	/// <summary>
	/// 方案选择下拉列表
	/// </summary>
	public GameObject PlanName;
	/// <summary>
	/// 方案预览的显示面板
	/// </summary>
	public GameObject VideoPatrolPlanViewPanel;
	/// <summary>
	/// 向方案中添加摄像头时的默认停留时间
	/// </summary>
	public UIInput InitTime;
	/// <summary>
	/// 存放所有摄像头的详细信息
	/// </summary>
	private List<DeviceInfo> MornitorInfos;
	/// <summary>
	/// 存放已经被选择摄像机头对应的摄像头信息在MornitorInfos的位置索引
	/// <已经被选择摄像机头对象引用,MornitorInfos的位置索引>
	/// </summary>
	private Dictionary<GameObject,int> SelectedMornitorDictionary;
	/// <summary>
	/// 存放需要选择摄像机头对应的摄像头信息在MornitorInfos的位置索引
	/// <需要选择摄像机头对象引用,MornitorInfos的位置索引>
	/// </summary>
	private Dictionary<GameObject,int> SelectMornitorListDictionary;
	/// <summary>
	/// 存放所有的巡逻方案信息
	/// </summary>
	private List<VideoPatrolPlanInfo> VideoPatrolPlanInfos;
	/// <summary>
	/// 当前选择的方案在VideoPatrolPlanInfos的索引
	/// </summary>
	private int selectPlanInfosIndex;
	private List<string> PlanNameList;
	/// <summary>
	/// 初始化相关的数据
	/// </summary>
	void Awake()
	{
		Logger.Instance.WriteLog("初始化视频巡航相关的数据");
		PlanNameList = new List<string> ();
		SelectedMornitorDictionary = new Dictionary<GameObject, int> ();
		SelectMornitorListDictionary = new Dictionary<GameObject, int> ();

//		UIEventListener.Get(Popuplist).onClick = CatchDropList;

		InitTime.GetComponent<UIEventTrigger> ().onDeselect.Add (new EventDelegate (InitTimeChanged));
		PlanName.GetComponentInChildren<UIEventTrigger> ().onDeselect.Add (new EventDelegate (PlanNameChanged));
//		PlanName.GetComponentInChildren<UIEventTrigger> ().onSelect.Add (new EventDelegate (SetInitPlanName));
	}
	void OnEnable()
	{
		StartCoroutine ("LoadDeviceInfoRecord");
		StartCoroutine ("LoadVideoPatrolPlanRecord");
	}
	/// <summary>
	/// 将下拉列表选择框放到指定的位置
	/// 点击选择列表是调用
	/// </summary>
	/// <param name="go">Go.</param>
//	void CatchDropList(GameObject go)
//	{
//		Transform dropDown = transform.FindChild ("Drop-down List");
//		if(dropDown)
//		{
//			dropDown.parent = Popuplist.transform.FindChild ("Anchor");
//			dropDown.localPosition = Vector3.zero;
//		}
//	}
	/// <summary>
	/// 从数据库加载摄像头的信息
	/// </summary>
	/// <returns>The device info record.</returns>
	IEnumerator LoadDeviceInfoRecord()
	{
		Logger.Instance.WriteLog("加载摄像头的信息");
		DeviceDao dDao = new DeviceDao ();
		dDao.Select001 ();
		MornitorInfos = dDao.Result;
		SelectMornitorList.transform.DestroyChildren ();
		for(int i = 0; i < MornitorInfos.Count; i++)
		{
			GameObject go = Instantiate(SelectMornitorItemPrefab) as GameObject;
			SelectMornitorList.AddChild(go.transform);
			go.transform.localScale = new Vector3(1,1,1);
			go.transform.FindChild ("CameraDescription").GetComponentInChildren<UILabel>().text = MornitorInfos[i].Description;
			SelectMornitorListDictionary.Add(go,i);
			go.transform.FindChild ("Sort").GetComponent<UILabel> ().text = (i +1) + "";
			UIEventListener.Get(go.transform.FindChild ("CameraDescription").gameObject).onDoubleClick = AddToSelectedMornitorList;
			UIEventListener.Get(go.transform.FindChild ("GotoIcon").gameObject).onDoubleClick = GotoMornitorPosition;
		}
		yield return null;
	}
	/// <summary>
	/// 从数据库加载巡逻方案的数据
	/// </summary>
	/// <returns>The video patrol plan record.</returns>
	IEnumerator LoadVideoPatrolPlanRecord()
	{
		Logger.Instance.WriteLog("加载巡逻方案的数据");
		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		vppDao.Select001 ();
		VideoPatrolPlanInfos = vppDao.Result;
//		UIPopupList PopuList = Popuplist.GetComponent<UIPopupList> ();
		PlanNameList.Clear ();
		foreach(VideoPatrolPlanInfo info in VideoPatrolPlanInfos)
		{
			PlanNameList.Add(info.Name);
		}
		if(VideoPatrolPlanInfos.Count <= 0)
		{
			AddVideoPatrolPlan();
		}
		if(VideoPatrolPlanInfos.Count > 0)
		{
			ShowDetailList(VideoPatrolPlanInfos [0].Name);
		}
		yield return null;
	}
	/// <summary>
	/// 当方案选择列表变更选项时调用
	/// </summary>
//	public void PopuListValueChange()
//	{
//		UIPopupList PopuList = Popuplist.GetComponent<UIPopupList> ();
//		ShowDetailList (PopuList.value);
//	}
	/// <summary>
	/// 在显示面板中显示指定方案的详细信息
	/// </summary>
	/// <param name="planName">Plan name.</param>
	void ShowDetailList(string planName)
	{
		Logger.Instance.WriteLog("显示指定方案的详细信息");
		PlanName.GetComponentInChildren<UIInput> ().value = planName;
		SelectedMornitorList.transform.DestroyChildren ();
		SelectedMornitorDictionary.Clear ();
		if(planName.Trim() == "") 
		{
			VideoPatrolPlanEditMapPanel.GetComponent<DrawMap> ().Clear();
			selectPlanInfosIndex = -1;
			return;
		}
		VideoPatrolPlanInfo info = FindVideoPatrolPlanInfo (planName);
		selectPlanInfosIndex = FindVideoPatrolPlanInfoIndex (planName);
		string[] MornitorIdList = info.MonitorList.Split ('|');
		string[] TimeList = info.PlayTimeList.Split ('|');
		int index = -1;
		for(int i = 0; i < MornitorIdList.Length; i++)
		{
			index = findMornitorInfosIndex(MornitorIdList[i]);
			if(index < 0)
			{
				continue;
			}
			AddToSelectedMornitorList(i + 1,MornitorInfos[index].Description,index,TimeList[i]);
		}
		SelectedMornitorList.Reposition ();
		VideoPatrolPlanEditMapPanel.GetComponent<DrawMap> ().Draw (planName,MornitorIdList);
	}
	/// <summary>
	/// 根据方案名称查找详细的信息
	/// </summary>
	/// <returns>The video patrol plan info.</returns>
	/// <param name="planName">Plan name.</param>
	private VideoPatrolPlanInfo FindVideoPatrolPlanInfo(string planName)
	{
		return VideoPatrolPlanInfos[FindVideoPatrolPlanInfoIndex(planName)];
	}
	/// <summary>
	/// 根据方案名称查找所在保存列表中的索引
	/// </summary>
	/// <returns>The video patrol plan info index.</returns>
	/// <param name="planName">Plan name.</param>
	private int FindVideoPatrolPlanInfoIndex(string planName)
	{
		int index = -1;;
		for(int i = 0; i < VideoPatrolPlanInfos.Count; i++)
		{
			if(planName == VideoPatrolPlanInfos[i].Name)
			{
				index = i;
				break;
			}
		}
		return index;
	}
	/// <summary>
	/// 根据摄像头Id查找所在保存列表中的索引
	/// </summary>
	/// <returns>The mornitor infos index.</returns>
	/// <param name="id">摄像头Id</param>
	private int findMornitorInfosIndex(string id)
	{
		int index = -1;
		for(int i = 0; i < MornitorInfos.Count; i++)
		{
			if(MornitorInfos[i].Id == id)
			{
				index = i;
				break;
			}
		}
		return index;
	}

	/// <summary>
	/// 将指定的摄像头加入到，已选择列表中
	/// （此函数用作回调用）
	/// </summary>
	/// <param name="go">Go.</param>
	void AddToSelectedMornitorList(GameObject go)
	{
		Logger.Instance.WriteLog("将指定的摄像头加入到，已选择列表中");
		int no = SelectedMornitorList.transform.childCount;
		string description = go.GetComponent<UILabel> ().text;
		int index = SelectMornitorListDictionary [go.transform.parent.gameObject];
		AddToSelectedMornitorList (no,description,index,InitTime.value);
		SelectedMornitorList.Reposition ();
		SaveVideoPatrolPlan ();
	}
	/// <summary>
	/// 将摄像头加入到，已选择列表中
	/// </summary>
	/// <param name="no">显示用的序号</param>
	/// <param name="description">摄像头名称</param>
	/// <param name="index">MornitorInfos的位置索引</param>
	/// <param name="time">巡逻停留时间</param>
	private void AddToSelectedMornitorList(int no, string description,int index,string time = "5")
	{
		Logger.Instance.WriteLog("将摄像头加入到，已选择列表中");
		GameObject newGo = Instantiate (SelectedMornitorItemPrefab) as GameObject;
		SelectedMornitorList.AddChild (newGo.transform);
		newGo.transform.localScale = new Vector3 (1,1,1);
		SetBackground (no,newGo);
		newGo.transform.FindChild ("Sort").GetComponent<UILabel> ().text = SelectedMornitorList.transform.childCount.ToString();
		newGo.transform.FindChild ("Description").GetComponent<UILabel> ().text = description;
		UIInput input = newGo.transform.FindChild ("Time").GetComponent<UIInput> ();
		input.value = time;
		UIEventListener.Get(newGo.transform.FindChild("Description").gameObject).onDoubleClick = DeleteFromSelectedMornitorList;
		UIEventListener.Get(newGo.transform.FindChild("GotoIcon").gameObject).onDoubleClick = GotoMornitorPosition;
		UIEventListener.Get(newGo.transform.FindChild("Up").gameObject).onClick = MoveToUp;
		UIEventListener.Get(newGo.transform.FindChild("Down").gameObject).onClick = MoveToDown;
		SelectedMornitorDictionary.Add (newGo, index);
	}
	/// <summary>
	/// 从已选择的摄像头列表中删除指定的项目
	/// </summary>
	/// <param name="go">Go.</param>
	void DeleteFromSelectedMornitorList(GameObject go)
	{
		Logger.Instance.WriteLog("从已选择的摄像头列表中删除指定的项目");
		SelectedMornitorList.RemoveChild(go.transform.parent);
		SelectedMornitorDictionary.Remove (go.transform.parent.gameObject);
		Destroy (go.transform.parent.gameObject);
		int i = 1;
		foreach(GameObject Mornitor in SelectedMornitorDictionary.Keys)
		{
			Mornitor.transform.FindChild ("Sort").GetComponent<UILabel> ().text = "" + i;
			SetBackground (i,Mornitor);
			i++;
		}
		UIScrollBar scrollBar = (UIScrollBar)SelectedMornitorList.GetComponentInParent<UIScrollView> ().verticalScrollBar;
		float offset = scrollBar.value;
		SelectedMornitorList.GetComponentInParent<UIScrollView> ().ResetPosition ();
		SelectedMornitorList.GetComponentInParent<UIScrollView> ().verticalScrollBar.value = offset;

		SaveVideoPatrolPlan ();

	}

	public void ScrollBarValueChanged(UIScrollBar sb)
	{
		if(sb.barSize + 0.001f >= 1)
		{
			SelectedMornitorList.GetComponentInParent<UIScrollView> ().ResetPosition ();
		}
	}
//	void SetInitPlanName()
//	{
//		if(VideoPatrolPlanInfos.Count <= 0) return;
//		VideoPatrolPlanInfo info = VideoPatrolPlanInfos [selectPlanInfosIndex];
//		Popuplist.GetComponentInChildren<UIInput> ().value = info.Name;
//	}
	/// <summary>
	/// 当项目名称改变时调用
	/// </summary>
	void PlanNameChanged()
	{
		Logger.Instance.WriteLog("视频巡航方案名称被改变");
		//UIPopupList PopuList = Popuplist.GetComponent<UIPopupList> ();
		VideoPatrolPlanInfo info = VideoPatrolPlanInfos [selectPlanInfosIndex];
		string currentName = info.Name;
		string newName = PlanName.GetComponentInChildren<UIInput> ().value;
		if(newName.Trim() == "")
		{
			PlanName.GetComponentInChildren<UIInput> ().value = currentName;
			return;
		}
		if(currentName == newName)
		{
			return;
		}

		if(PlanNameList.Contains(newName))
		{
			PlanName.GetComponentInChildren<UIInput> ().value = currentName;
			return;
		}
		PlanNameList [selectPlanInfosIndex] = newName;
		info.Name = newName;
		VideoPatrolPlanInfos [selectPlanInfosIndex] = info;
		PlanName.GetComponentInChildren<UIInput> ().value = newName;
		Logger.Instance.WriteLog("更新视频巡航方案名称");
		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		vppDao.Update002 (info.Id,info.Name);

		VideoPatrolPlanEditMapPanel.GetComponent<DrawMap> ().Title.text = info.Name;
		VideoPatrolPlanViewPanel.SendMessage ("ReloadRecord");
	}
	/// <summary>
	/// 默认初始时间改变时调用此函数
	/// </summary>
	void InitTimeChanged()
	{
		if(InitTime.value.Trim() == "" || int.Parse(InitTime.value) < 5)
		{
			InitTime.value = 5 + "";
			return;
		}
		if(int.Parse(InitTime.value) < 10 && InitTime.value.Length == 2)
		{
			InitTime.value = InitTime.value.Substring(1);
		}
	}

	public void TimeChanged()
	{
		SaveVideoPatrolPlan ();
	}
	/// <summary>
	/// 将已选择的摄像头列表中指定的项目向上移动
	/// </summary>
	/// <param name="go">Go.</param>
	void MoveToUp(GameObject go)
	{
		Logger.Instance.WriteLog("将已选择的摄像头列表中指定的项目向上移动");
		GameObject[] keys = new GameObject[SelectedMornitorDictionary.Keys.Count];
		int[] values = new int[SelectedMornitorDictionary.Values.Count];
		SelectedMornitorDictionary.Keys.CopyTo (keys,0);
		SelectedMornitorDictionary.Values.CopyTo (values,0);
		int index = int.Parse( go.transform.parent.FindChild ("Sort").GetComponent<UILabel> ().text) - 1;
		if(index > 0)
		{
			GameObject tmpkey = keys[index - 1];
			keys[index - 1] = keys[index];
			keys[index] = tmpkey;

			int tmpvalue = values[index - 1];
			values[index - 1] = values[index];
			values[index] = tmpvalue;
		}

		SelectedMornitorDictionary.Clear ();
		SelectedMornitorList.GetChildList().Clear();
		for(int i = 0; i < keys.Length; i++)
		{
			SelectedMornitorDictionary.Add(keys[i],values[i]);
			SelectedMornitorList.AddChild(keys[i].transform);
			keys[i].transform.localScale = new Vector3(1,1,1);
			keys[i].transform.FindChild ("Sort").GetComponent<UILabel> ().text = "" + (i + 1);
			SetBackground (i + 1,keys[i]);
		}

		SaveVideoPatrolPlan ();
	}
	/// <summary>
	/// 将已选择的摄像头列表中指定的项目向下移动
	/// </summary>
	/// <param name="go">Go.</param>
	void MoveToDown(GameObject go)
	{
		Logger.Instance.WriteLog("将已选择的摄像头列表中指定的项目向下移动");
		GameObject[] keys = new GameObject[SelectedMornitorDictionary.Keys.Count];
		int[] values = new int[SelectedMornitorDictionary.Values.Count];
		SelectedMornitorDictionary.Keys.CopyTo (keys,0);
		SelectedMornitorDictionary.Values.CopyTo (values,0);
		int index = int.Parse( go.transform.parent.FindChild ("Sort").GetComponent<UILabel> ().text) - 1;
		if(index < keys.Length - 1)
		{
			GameObject tmpkey = keys[index + 1];
			keys[index + 1] = keys[index];
			keys[index] = tmpkey;
			
			int tmpvalue = values[index + 1];
			values[index + 1] = values[index];
			values[index] = tmpvalue;
		}
		
		SelectedMornitorDictionary.Clear ();
		SelectedMornitorList.GetChildList().Clear();
		for(int i = 0; i < keys.Length; i++)
		{
			SelectedMornitorDictionary.Add(keys[i],values[i]);
			SelectedMornitorList.AddChild(keys[i].transform);
			keys[i].transform.localScale = new Vector3(1,1,1);
			keys[i].transform.FindChild ("Sort").GetComponent<UILabel> ().text = "" + (i + 1);
			SetBackground (i + 1,keys[i]);
		}

		SaveVideoPatrolPlan ();
	}
	/// <summary>
	/// 根据数据的序号设置不同的背景图片
	/// </summary>
	/// <param name="no">No.</param>
	/// <param name="item">Item.</param>
	private void SetBackground(int no,GameObject item)
	{
		if(no % 2 == 0)
		{
			item.transform.FindChild ("Background1").gameObject.SetActive(false);
			item.transform.FindChild ("Background2").gameObject.SetActive(true);
		}
		else
		{
			item.transform.FindChild ("Background1").gameObject.SetActive(true);
			item.transform.FindChild ("Background2").gameObject.SetActive(false);
		}

	}
	/// <summary>
	/// 将场景中的镜头移动指定设备所在位置
	/// </summary>
	/// <param name="go">Go.</param>
	void GotoMornitorPosition(GameObject go)
	{
		Logger.Instance.WriteLog("将场景中的镜头移动指定设备所在位置");
		int index = SelectMornitorListDictionary.ContainsKey (go.transform.parent.gameObject) ? SelectMornitorListDictionary[go.transform.parent.gameObject] : SelectedMornitorDictionary[go.transform.parent.gameObject];
		DeviceInfo info = MornitorInfos[index];
		Camera.main.GetComponent<CameraController> ().GotoPosition (new Vector3(float.Parse(info.RotatePointPosX),0,float.Parse(info.RotatePointPosZ))
		                                                            ,new Vector3(float.Parse(info.CameraPosX),float.Parse(info.CameraPosY),float.Parse(info.CameraPosZ))
		                                                            ,new Vector3(float.Parse(info.CameraRotatX),float.Parse(info.CameraRotatY),float.Parse(info.CameraRotatZ)));
	}
	/// <summary>
	/// 删除选定的巡逻方案
	/// </summary>
	public void DeleteVideoPatrolPlan()
	{
		Logger.Instance.WriteLog("删除选定的巡逻方案");
		if(VideoPatrolPlanInfos.Count <= 0)return;
		string planName = PlanName.GetComponentInChildren<UIInput> ().value;
		int PlaninfosIndex = FindVideoPatrolPlanInfoIndex (planName);
		VideoPatrolPlanInfo info = VideoPatrolPlanInfos[PlaninfosIndex];
		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		vppDao.Delete001 (info.Id);
		VideoPatrolPlanInfos.RemoveAt (PlaninfosIndex);
		PlanNameList.Remove (planName);
		if(VideoPatrolPlanInfos.Count > 0)
		{
			PlanName.GetComponentInChildren<UIInput> ().value = VideoPatrolPlanInfos[0].Name;
		}
		else
		{
			PlanName.GetComponentInChildren<UIInput> ().value = "";
		}
		if(VideoPatrolPlanInfos.Count <= 0)
		{
			AddVideoPatrolPlan();
		}
		ShowDetailList (PlanName.GetComponentInChildren<UIInput> ().value);
		VideoPatrolPlanViewPanel.SendMessage ("ReloadRecord");
	}
	/// <summary>
	/// 创建新的巡逻方案
	/// </summary>
	public void AddVideoPatrolPlan()
	{
		Logger.Instance.WriteLog("创建新的巡逻方案");
		VideoPatrolPlanInfo info = new VideoPatrolPlanInfo();
		int i = 1;
		while(PlanNameList.Contains("新建方案" + i))
		{
			i++;
		}
		info.Name = "新建方案" + i;
		info.MonitorList = "";
		info.PlayTimeList = "";
		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		Logger.Instance.WriteLog("保存新建巡逻方案");
		vppDao.Insert001 (info.Name,info.MonitorList,info.PlayTimeList,DataStore.UserInfo.UserName);
		Logger.Instance.WriteLog("加载新建巡逻方案");
		vppDao.Select002 ();
		if(vppDao.Result.Count <= 0 || vppDao.Result[0].Name != info.Name)
		{
			Logger.Instance.WriteLog("新建巡逻方案失败");
			return;
		}
		info = vppDao.Result [0];
		VideoPatrolPlanInfos.Add (info);
		PlanNameList.Add (info.Name);
		PlanName.GetComponentInChildren<UIInput> ().value = info.Name;
		ShowDetailList (PlanName.GetComponentInChildren<UIInput> ().value);

		VideoPatrolPlanViewPanel.SendMessage ("ReloadRecord");
	}
	/// <summary>
	/// 保存巡航方案修改的信息
	/// </summary>
	public void SaveVideoPatrolPlan()
	{
		Logger.Instance.WriteLog("保存巡航方案修改的信息");
		string idList = "";
		string timeList = "";
		foreach(GameObject go in SelectedMornitorDictionary.Keys)
		{
			idList += MornitorInfos[SelectedMornitorDictionary[go]].Id + "|";
			timeList += go.transform.FindChild("Time").GetComponent<UIInput>().value + "|";
		}
		int idListLength = idList.Length;
		int timeListLength = timeList.Length;
		if(idList.Length > 0)
		{
			timeList = timeList.Remove (timeListLength - 1);
			idList = idList.Remove (idListLength - 1);
		}
		string planName = PlanName.GetComponentInChildren<UIInput> ().value;
		int PlaninfosIndex = FindVideoPatrolPlanInfoIndex (planName);
		VideoPatrolPlanInfo info = VideoPatrolPlanInfos[PlaninfosIndex];
		info.MonitorList = idList;
		info.PlayTimeList = timeList;
		VideoPatrolPlanEditMapPanel.GetComponent<DrawMap> ().Draw (planName,idList.Split('|'));
		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		vppDao.Update001 (info.Id,idList,timeList);
		VideoPatrolPlanInfos[PlaninfosIndex] = info;

		VideoPatrolPlanViewPanel.SendMessage ("ReloadRecord");
	}
}
