using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public delegate void DelVideoPatrolPlanNameChanged(string newName);
public delegate void DelAction(GameObject go);
public class VideoPatrolPlanEdit : MonoBehaviour {

	public UIInput PlanName;
	public UISprite PlanNameInputBox;

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
	/// 存放所有摄像头的详细信息
	/// </summary>
	private Dictionary<string,DeviceInfo> DicMornitorInfos;

	private DelVideoPatrolPlanNameChanged VideoPatrolPlanNameChanged;
	private VideoPatrolPlanInfo VPPlaninfo;
	public void Init(string _PlanName,DelVideoPatrolPlanNameChanged _VideoPatrolPlanNameChanged)
	{
		Logger.Instance.WriteLog("加载巡逻方案的数据");
		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		vppDao.Select004(_PlanName);
		if(vppDao.Result.Count > 0)
		{
			VPPlaninfo = vppDao.Result[0];
		}
		PlanName.value = VPPlaninfo.Name;
		VideoPatrolPlanNameChanged = _VideoPatrolPlanNameChanged;

		if(DicMornitorInfos == null)
		{
			LoadDeviceInfoRecord();
		}
		ShowDetailList();
	}

	/// <summary>
	/// 从数据库加载摄像头的信息
	/// </summary>
	/// <returns>The device info record.</returns>
	private void LoadDeviceInfoRecord()
	{
		Logger.Instance.WriteLog("加载摄像头的信息");
		DicMornitorInfos = new Dictionary<string, DeviceInfo>();
		DeviceDao dDao = new DeviceDao ();
		dDao.Select001 ();
		if(dDao.Result.Count <= 0)
		{
			Logger.Instance.WriteLog("加载摄像头的信息失败");
			return;
		}
		for(int i = 0; i < dDao.Result.Count; i++)
		{
			DicMornitorInfos.Add(dDao.Result[i].Id,dDao.Result[i]);
			GameObject go = Instantiate(SelectMornitorItemPrefab) as GameObject;
			SelectMornitorList.AddChild(go.transform);
			go.transform.localScale = new Vector3(1,1,1);
			SelecteMornitorItem item = go.GetComponent<SelecteMornitorItem>();
			item.Init(dDao.Result[i]);
			item.BindAction(AddItemToSelectedMornitorList);
		}
	}

	/// <summary>
	/// 在显示面板中显示指定方案的详细信息
	/// </summary>
	private List<GameObject> SelectedMornitorItemLst;
	void ShowDetailList()
	{
		Logger.Instance.WriteLog("显示指定方案的详细信息");
		SelectedMornitorList.transform.DestroyChildren ();
		SelectedMornitorItemLst= new List<GameObject>();
		string[] MornitorIdList = VPPlaninfo.MonitorList.Split ('|');
		string[] TimeList = VPPlaninfo.PlayTimeList.Split ('|');

		for(int i = 0; i < MornitorIdList.Length; i++)
		{
			if(DicMornitorInfos.ContainsKey(MornitorIdList[i]))
			{
				SelectedMornitorItemLst.Add(AddToSelectedMornitorList(DicMornitorInfos[MornitorIdList[i]],TimeList[i]));
			}
		}
		SelectedMornitorList.Reposition ();
	}

	/// <summary>
	/// 将摄像头加入到，已选择列表中
	/// </summary>
	private GameObject AddToSelectedMornitorList(DeviceInfo _DeviceInfo,string time = "5")
	{
		Logger.Instance.WriteLog("将摄像头加入到，已选择列表中");
		GameObject newGo = Instantiate (SelectedMornitorItemPrefab) as GameObject;
		SelectedMornitorList.AddChild (newGo.transform);
		newGo.transform.localScale = new Vector3 (1,1,1);
		SelectedMornitorItem SelectedItem = newGo.GetComponent<SelectedMornitorItem>();
		SelectedItem.Init(_DeviceInfo,time);
		SelectedItem.BindAction(SelectedItemMoveUp,SelectedItemMoveDown,SelectedItemDelete,SelectedItemTimeChanged);
		return newGo;
	}
	/// <summary>
	/// 刷新已选择的摄像机列表
	/// </summary>
	private void ResetSelectedMornitorList()
	{
		SelectedMornitorList.GetChildList().Clear();
		foreach(GameObject go in SelectedMornitorItemLst)
		{
			SelectedMornitorList.AddChild(go.transform);
		}
	}
	/// <summary>
	/// 将已选择的摄像头列表中指定的项目向上移动
	/// （此函数用作回调用）
	/// </summary>
	/// <param name="go">Go.</param>
	private void SelectedItemMoveUp(GameObject go)
	{
		int MoveUpItemIndex = SelectedMornitorItemLst.IndexOf(go);
		if(MoveUpItemIndex <= 0)return;
		GameObject MoveUpItem = SelectedMornitorItemLst[MoveUpItemIndex];
		SelectedMornitorItemLst[MoveUpItemIndex] = SelectedMornitorItemLst[MoveUpItemIndex - 1];
		SelectedMornitorItemLst[MoveUpItemIndex - 1] = MoveUpItem;
		ResetSelectedMornitorList();
		SaveVideoPatrolPlan();
	}
	/// <summary>
	/// 将已选择的摄像头列表中指定的项目向下移动
	/// （此函数用作回调用）
	/// </summary>
	/// <param name="go">Go.</param>
	private void SelectedItemMoveDown(GameObject go)
	{
		int MoveDownItemIndex = SelectedMornitorItemLst.IndexOf(go);
		if(MoveDownItemIndex >= SelectedMornitorItemLst.Count - 1)return;
		GameObject MoveDownItem = SelectedMornitorItemLst[MoveDownItemIndex];
		SelectedMornitorItemLst[MoveDownItemIndex] = SelectedMornitorItemLst[MoveDownItemIndex + 1];
		SelectedMornitorItemLst[MoveDownItemIndex + 1] = MoveDownItem;
		ResetSelectedMornitorList();
		SaveVideoPatrolPlan();
	}
	/// <summary>
	/// 将已选择的摄像头列表中指定的项目时间改变时调用
	/// （此函数用作回调用）
	/// </summary>
	/// <param name="go">Go.</param>
	private void SelectedItemTimeChanged(GameObject go)
	{
		SaveVideoPatrolPlan();
	}
	/// <summary>
	/// 删除已选择的摄像头列表中指定的项目
	/// （此函数用作回调用）
	/// </summary>
	/// <param name="go">Go.</param>
	private void SelectedItemDelete(GameObject go)
	{
		SelectedMornitorItemLst.Remove(go);
		SelectedMornitorList.RemoveChild(go.transform);
		Destroy(go);

		SelectedMornitorList.repositionNow = true;
		SaveVideoPatrolPlan();
	}


	/// <summary>
	/// 将指定的摄像头加入到，已选择列表中
	/// （此函数用作回调用）
	/// </summary>
	private void AddItemToSelectedMornitorList(GameObject go)
	{
		SelectedMornitorItemLst.Add(AddToSelectedMornitorList(go.GetComponent<SelecteMornitorItem>().info));
		SelectedMornitorList.Reposition ();
		SaveVideoPatrolPlan();
	}

	/// <summary>
	/// 疏散预案名称被选中时调用
	/// </summary>
	public void PLanNameSelected()
	{
		PlanNameInputBox.enabled = true;
	}
	/// <summary>
	/// 当项目名称改变时调用
	/// </summary>
	public void PlanNameChanged()
	{
		Logger.Instance.WriteLog("视频巡航方案名称被改变");
		PlanNameInputBox.enabled = false;
		if(PlanName.value.Trim() == "")
		{
			PlanName.value = VPPlaninfo.Name;
			Logger.Instance.WriteLog("更新视频巡航方案名称失败,名称为空");
			return;
		}
		if(PlanName.value == VPPlaninfo.Name)
		{
			return;
		}
		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		vppDao.Select004(PlanName.value);
		if(vppDao.Result.Count > 0)
		{
			PlanName.value = VPPlaninfo.Name;
			Logger.Instance.WriteLog("更新视频巡航方案名称失败,名称已存在");
			return;
		}

		Logger.Instance.WriteLog("更新视频巡航方案名称");
		int ret = vppDao.Update002 (VPPlaninfo.Id,PlanName.value);
		if(ret > 0)
		{
			VPPlaninfo.Name = PlanName.value;
			if(VideoPatrolPlanNameChanged != null)VideoPatrolPlanNameChanged.Invoke(VPPlaninfo.Name);
			Logger.Instance.WriteLog("更新视频巡航方案名称成功");
		}
		else
		{
			PlanName.value = VPPlaninfo.Name;
			Logger.Instance.WriteLog("更新视频巡航方案名称失败,数据库更新失败");
		}
	}

	/// <summary>
	/// 保存巡航方案修改的信息
	/// </summary>
	public void SaveVideoPatrolPlan()
	{
		Logger.Instance.WriteLog("保存巡航方案修改的信息");
		string idList = "";
		string timeList = "";
		foreach(GameObject go in SelectedMornitorItemLst)
		{
			idList += go.GetComponent<SelectedMornitorItem>().MornitorInfo.Id + "|";
			timeList += go.GetComponent<SelectedMornitorItem>().PatrolTimeInput.value + "|";
		}
		int idListLength = idList.Length;
		int timeListLength = timeList.Length;
		if(idList.Length > 0)
		{
			timeList = timeList.Remove (timeListLength - 1);
			idList = idList.Remove (idListLength - 1);
		}

		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		vppDao.Update001 (VPPlaninfo.Id,idList,timeList);
	}

	void OnDisable()
	{
		SelectedMornitorList.transform.DestroyChildren ();
		SelectMornitorList.transform.DestroyChildren ();
		if(DicMornitorInfos != null)DicMornitorInfos.Clear();
		DicMornitorInfos = null;
	}
}
