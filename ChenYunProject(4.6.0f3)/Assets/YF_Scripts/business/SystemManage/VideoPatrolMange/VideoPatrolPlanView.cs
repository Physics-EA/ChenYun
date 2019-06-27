using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VideoPatrolPlanView : MonoBehaviour 
{
	public UIGrid VideoPatrolPlanGrid;
	public GameObject RecordItemPrefab;
	public GameObject MapPanel;
	public GameObject VideoPatrolPlanEditPanel;
	public int MapMoveDistance;
	public int PlanEditPanelMoveDistance;
	private List<VideoPatrolPlanInfo> VideoPatrolPlanInfos;
	private string CurrentSelectedId = "";

	public static VideoPatrolPlanItem SelectedItem;
	// Use this for initialization
	public void Init () 
	{
		StartCoroutine ("LoadRecord");
	}

	/// <summary>
	/// Loads the record.
	/// </summary>
	/// <returns>The record.</returns>
	IEnumerator LoadRecord()
	{
		Logger.Instance.WriteLog("加载巡逻方案的数据");
		if(VideoPatrolPlanInfos == null)
		{
			VideoPatrolPlanInfos = new List<VideoPatrolPlanInfo> ();
		}
		VideoPatrolPlanGrid.transform.DestroyChildren ();
		VideoPatrolPlanInfos.Clear ();

		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		vppDao.Select001 ();
		VideoPatrolPlanInfos = vppDao.Result;

		for(int i = 0; i < VideoPatrolPlanInfos.Count;i++)
		{
			AddItemToVideoPatrolPlanGrid(VideoPatrolPlanInfos[i]);
		}

		if(VideoPatrolPlanInfos.Count > 0)
		{
			ShowPlanDetail(VideoPatrolPlanGrid.GetChild(0).gameObject);
		}
		if(VideoPatrolPlanInfos.Count == 0)
		{
			VideoPatrolPlanEditPanel.SetActive(false);
		}
		yield return null;
	}

	private void AddItemToVideoPatrolPlanGrid(VideoPatrolPlanInfo info)
	{
		GameObject RecordItem = Instantiate (RecordItemPrefab) as GameObject;
		RecordItem.GetComponent<VideoPatrolPlanItem>().Init(info);
		VideoPatrolPlanGrid.AddChild(RecordItem.transform);
		RecordItem.transform.localScale = new Vector3(1,1,1);
		UIEventListener.Get(RecordItem).onClick = ShowPlanDetail;
	}
	/// <summary>
	/// 打开指定记录的详细面板
	/// </summary>
	/// <param name="go">Go.</param>
	void ShowPlanDetail(GameObject go)
	{
		if(SelectedItem == go.GetComponent<VideoPatrolPlanItem> ())return;
		Logger.Instance.WriteLog("打开指定记录的详细面板");
		if(SelectedItem)SelectedItem.Deselected();
		SelectedItem = go.GetComponent<VideoPatrolPlanItem> ();
		SelectedItem.Selected();
		VideoPatrolPlanEditPanel.SetActive(true);
		VideoPatrolPlanEditPanel.GetComponent<VideoPatrolPlanEdit>().Init(SelectedItem.PlanName.text,PlanNameChanged);
	}

	private void PlanNameChanged(string newName)
	{
		SelectedItem.UpdateName(newName);
	}

	/// <summary>
	/// 删除选定的巡逻方案
	/// </summary>
	public void DeleteVideoPatrolPlan()
	{
		Logger.Instance.WriteLog("删除选定的巡逻方案");
		if(VideoPatrolPlanInfos.Count <= 0)return;

		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		int ret = vppDao.Delete001 (SelectedItem.info.Id);
		if(ret == 0)
		{
			Logger.Instance.WriteLog("删除选定的巡逻方案失败");
			return;
		}

		UIScrollView uiScrollView = SelectedItem.GetComponentInParent<UIScrollView> ();
		UIScrollBar scrollBar = (UIScrollBar)uiScrollView.verticalScrollBar;
		float offset = scrollBar.value;
		uiScrollView.ResetPosition ();
		uiScrollView.verticalScrollBar.value = offset;
		VideoPatrolPlanGrid.RemoveChild(SelectedItem.transform);
		Destroy(SelectedItem.gameObject);
		
		if(VideoPatrolPlanGrid.transform.childCount > 1)
		{
			if(VideoPatrolPlanGrid.GetChild(0).transform == SelectedItem.transform)
			{
				SelectedItem = null;
				ShowPlanDetail(VideoPatrolPlanGrid.GetChild(1).gameObject);
			}
			else
			{
				SelectedItem = null;
				ShowPlanDetail(VideoPatrolPlanGrid.GetChild(0).gameObject);
			}
		}
		else
		{
			VideoPatrolPlanEditPanel.SetActive(false);
		}
	}

	/// <summary>
	/// 创建新的巡逻方案
	/// </summary>
	public void AddVideoPatrolPlan()
	{
		Logger.Instance.WriteLog("创建新的巡逻方案");
		VideoPatrolPlanInfo info = new VideoPatrolPlanInfo();
		int i = 1;
		VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao ();
		while(true)
		{
			vppDao.Select004("新建方案" + i);
			if(vppDao.Result.Count <= 0)break;
			i++;
		}
		info.Name = "新建方案" + i;
		info.MonitorList = "";
		info.PlayTimeList = "";

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
		AddItemToVideoPatrolPlanGrid(info);
		ShowPlanDetail(VideoPatrolPlanGrid.GetChild(VideoPatrolPlanGrid.transform.childCount - 1).gameObject);
	}

	void OnDisable()
	{
		VideoPatrolPlanGrid.transform.DestroyChildren ();
		if(VideoPatrolPlanInfos != null)VideoPatrolPlanInfos.Clear ();
	}
}
