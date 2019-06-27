using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class LogManage : MonoBehaviour {

	public UIInput DateFrom;
	public UIInput DateTo;
	public UIInput selectChr;
//	public UIPopupList Person;
//	public UIPopupList LogType;

	public UIGrid AlertLogGrid;
	public UIGrid VideoPatrolLogGrid;
	public UIGrid LableLogGrid;

	public GameObject AlertLogItemPrefab;
	public GameObject VideoPatrolItemPrefab;
	public GameObject LableItemPrefab;

	public GameObject ShowPictruePanel;

	public UILabel ErrorHit;
	private string ErrorMsg;

	public string[] lablesName;
	public GameObject[] lablesPanel;

	private DateTime formT;
	private DateTime toT;
	private List<VideoPatrolLogInfo> tempVideoList;

	void Start()
	{
		tempVideoList = null;
		CreateLable ();
	}

	private void CreateLable()
	{
		for(int i = 0; i < lablesName.Length; i++)
		{
			GameObject g = Instantiate(LableItemPrefab) as GameObject;
			g.GetComponent<LogLableItem>().SetValue(lablesName[i], lablesPanel[i]);
			LableLogGrid.AddChild(g.transform);
			g.transform.localScale = Vector3.one;
		}
	}

	void OnEnable()
	{
		SearchVideoPatrolLog ();
	}

//	private EventDelegate.Callback SearchLog;

	public void Search()
	{
		Logger.Instance.WriteLog("检索日志信息");
//		ErrorHit.text = "";
		if(CheckError())
		{
//			ErrorHit.text = ErrorMsg;
			return;
		}
		SearchVideoPatrolLog ();
//		SearchLog ();
	}

	private void CreatrVideoItem(VideoPatrolLogInfo info, int i)
	{
		GameObject go = Instantiate(VideoPatrolItemPrefab) as GameObject;
		VideoPatrolLogGrid.AddChild(go.transform);
		go.transform.localScale = new Vector3(1,1,1);
		go.GetComponent<VideoPatrolLogItem>().SetValue("" + (i + 1),info,null,ShowPictruePanel);//VideoPatrolDetailLogPanel,ShowPictruePanel);
	}


	/// <summary>
	/// 检查检索的日期是否正确
	/// </summary>
	/// <returns><c>true</c>, if error was checked, <c>false</c> otherwise.</returns>
	private bool CheckError()
	{
		string dateFrom = DateFrom.value.Trim();
		formT = new DateTime();
		if(dateFrom != "")
		{
			if(dateFrom.Length != 8)
			{
				ErrorMsg = "开始日期必须为8位";
				return true;
			}
			string tmpDt = dateFrom.Substring(0,4) + "/" + dateFrom.Substring(4,2) + "/" + dateFrom.Substring(6,2) + " 00:00:00";
			if(DateTime.TryParse(tmpDt,out formT) == false)
			{
				int month = int.Parse(dateFrom.Substring(4,2));
				if(month > 12 || month == 0)
				{
					ErrorMsg = "开始日期" + dateFrom.Substring(0,4) + "[ " + dateFrom.Substring(4,2) + " ]" + dateFrom.Substring(6,2) + "月份 不能大于 12 或 等于 0";
					return true;
				}
				int days = DateTime.DaysInMonth(int.Parse(dateFrom.Substring(0,4)),int.Parse(dateFrom.Substring(4,2)));
				int day = int.Parse(dateFrom.Substring(6,2));
				if( day == 0 || day > days)
				{
					ErrorMsg = "开始日期" + dateFrom.Substring(0,4) + "[ " + dateFrom.Substring(4,2) + " ][ " + dateFrom.Substring(6,2) + " ] " +  month +  "月份的天数不能大于" + days + "或等于0";
					return true;
				}
				
			}
		}

		
		string dateTo = DateTo.value.Trim();
		toT = new DateTime ();
		if(dateTo != "")
		{
			if(dateTo.Length != 8)
			{
				ErrorMsg = "结束日期必须为8位";
				return true;
			}
			string tmpDt = dateTo.Substring(0,4) + "/" + dateTo.Substring(4,2) + "/" + dateTo.Substring(6,2) + " 23:59:59";
			if(DateTime.TryParse(tmpDt,out toT) == false)
			{
				int month = int.Parse(dateTo.Substring(4,2));
				if(month > 12 || month == 0)
				{
					ErrorMsg = "结束日期"  + dateTo.Substring(0,4) + "[ " + dateTo.Substring(4,2) + " ]" + dateTo.Substring(6,2) + " 月份不能大于 12 或 等于 0";
					return true;
				}
				int days = DateTime.DaysInMonth(int.Parse(dateTo.Substring(0,4)),int.Parse(dateTo.Substring(4,2)));
				int day = int.Parse(dateTo.Substring(6,2));
				if( day == 0 || day > days)
				{
					ErrorMsg = "结束日期" + dateTo.Substring(0,4) + "[ " + dateTo.Substring(4,2) + " ][ " + dateTo.Substring(6,2) + " ] " + month +  "月份的天数不能大于" + days + "或等于0";
					return true;
				}
				
			}
		}


		if(formT.CompareTo (toT) == 1)
		{
			ErrorMsg = "开始日期不能大于结束日期";
			return true;
		}

		return false;
	}

	private void SearchAlertLog()
	{
		Logger.Instance.WriteLog("检索报警日志");
		AlertLogGrid.transform.DestroyChildren ();
		AlertPrecessLogDao aplDao = new AlertPrecessLogDao ();
		string dateFrom = "";
		string dateTo = "";
		if(DateFrom.value.Trim() != "")
		{
			string date = DateFrom.value.Trim();
			dateFrom = date.Substring(0,4) + "/" + date.Substring(4,2) + "/" + date.Substring(6,2);
		}
		if(DateTo.value.Trim() != "")
		{
			string date = DateTo.value.Trim();
			dateTo = date.Substring(0,4) + "/" + date.Substring(4,2) + "/" + date.Substring(6,2);
		}
		
		aplDao.Select001 (dateFrom,dateTo,"");
		AlertPrecessLogInfo info;

		for(int i = 0; i < aplDao.Result.Count; i++)
		{
			info = aplDao.Result[i];
			GameObject go = Instantiate(AlertLogItemPrefab) as GameObject;
			AlertLogGrid.AddChild(go.transform);
			go.transform.localScale = new Vector3(1,1,1);
			go.GetComponent<AlertLogItem>().SetValue("" + (i + 1),info);
		}


//		if(Person.items.Count <= 0)
//		{
//			List<string> name = new List<string> ();
//			name.Add ("");
//			foreach(AlertPrecessLogInfo aplinfo in aplDao.Result)
//			{
//				if(!name.Contains(aplinfo.person))
//				{
//					name.Add(aplinfo.person);
//				}
//			}
//			Person.items.AddRange(name);
//		}
	}

	IEnumerator SearcjLog()
	{
		Logger.Instance.WriteLog("检索视频巡航日志");
		VideoPatrolLogGrid.transform.DestroyChildren ();
		VideoPatrolLogDao vplDao = new VideoPatrolLogDao ();
		string dateFrom = "";
		string dateTo = "";
		string selece = selectChr.value.Trim ();
		if(DateFrom.value.Trim() != "")
		{
			string date = DateFrom.value.Trim();
			dateFrom = date.Substring(0,4) + "/" + date.Substring(4,2) + "/" + date.Substring(6,2);
		}
		if(DateTo.value.Trim() != "")
		{
			string date = DateTo.value.Trim();
			dateTo = date.Substring(0,4) + "/" + date.Substring(4,2) + "/" + date.Substring(6,2);
		}
		
		vplDao.Select001 ("","",selece);
		VideoPatrolLogInfo info;
		for(int i = 0; i <  vplDao.Result.Count; i++)
		{
			info = vplDao.Result[i];
			GameObject go = Instantiate(VideoPatrolItemPrefab) as GameObject;
			VideoPatrolLogGrid.AddChild(go.transform);
			go.transform.localScale = new Vector3(1,1,1);
			go.GetComponent<VideoPatrolLogItem>().SetValue("" + (i + 1),info,null,ShowPictruePanel);
			yield return null;
		}
	}

	private void SearchVideoPatrolLog()
	{
		StopCoroutine ("SearcjLog");
		StartCoroutine ("SearcjLog");
	}

	private string CurrentLogType = "";
	public void OnLogTypeChanged()
	{
		ErrorHit.text = "";
		DateFrom.value = "";
		DateTo.value = "";
//		Person.value = "";
//		Person.items.Clear ();

//		if(LogType.value != CurrentLogType)
//		{
//			CurrentLogType = LogType.value;
//			if(CurrentLogType == "报警处理")
//			{
//				SearchLog = SearchAlertLog;
//				AlertLogPanel.SetActive(true);
//				VideoPatrolLogPanel.SetActive(false);
//				VideoPatrolDetailLogPanel.SetActive(false);
//				SearchLog();
//				return;
//			}
//
//			if(CurrentLogType == "视频巡逻")
//			{
//				SearchLog = SearchVideoPatrolLog;
//				AlertLogPanel.SetActive(false);
//				VideoPatrolLogPanel.SetActive(true);
//				VideoPatrolDetailLogPanel.SetActive(true);
//				SearchLog();
//			}
//		}
	}
}
