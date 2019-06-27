using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 报警处理日志信息
/// </summary>
public struct AlertPrecessLogInfo
{
	public string id;
	public string alertTime;
	public string alertType;
	public string person;
	public string startTime;
	public string endTime;
	public string memo;
}
/// <summary>
/// 报警处理日志数据管理类
/// </summary>
public class AlertPrecessLogDao
{
	public List<AlertPrecessLogInfo> Result;
	public string currentId;
	/// <summary>
	/// 向数据库插入一条新记录
	/// </summary>
	/// <param name="alertTime">Alert time.</param>
	/// <param name="alertType">Alert type.</param>
	/// <param name="person">Person.</param>
	/// <param name="startTime">Start time.</param>
	/// <param name="endTime">End time.</param>
	/// <param name="memo">Memo.</param>
	public void Insert001(string alertTime,string alertType,string person,string startTime,string endTime, string memo)
	{
		GetNextID ();
		string sql = "insert into CYGJ_ALERT_PROCESS_LOG(ID,ALERTTIME,ALERTTYPE,PERSON,STARTTIME,ENDTIME,MEMO) values(" +
			"'" + currentId + "','" + alertTime + "','" + alertType + "','" + person + "','" + startTime + "','" + endTime + "','" + memo + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_ALERT_PROCESS_LOG(ALERTTIME,ALERTTYPE,PERSON,STARTTIME,ENDTIME,MEMO) values(" +
				"'" + alertTime + "','" + alertType + "','" + person + "','" + startTime + "','" + endTime + "','" + memo + "')";
			GetNextID ();
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("向数据库插入" + ret + "条报警处理日志");
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			GetNextID ();
		}
	}
	/// <summary>
	/// 根据记录ID更新开始时间，处理人
	/// </summary>
	/// <param name="startTime">Start time.</param>
	/// <param name="person">Person.</param>
	/// <param name="id">Identifier.</param>
	public void Update001(string startTime, string person, string id)
	{
		string sql = "update CYGJ_ALERT_PROCESS_LOG set STARTTIME = '" + startTime + "', PERSON = '" + person +  "' where ID = '" + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据记录ID更新开始时间，处理人:更新" + ret + "条报警处理日志");
	}
	/// <summary>
	/// 根据记录ID更新结束时间，处理人以及备注
	/// </summary>
	/// <param name="endTime">End time.</param>
	/// <param name="memo">Memo.</param>
	/// <param name="id">Identifier.</param>
	public void Update002(string endTime,string memo,string id)
	{
		string sql = "update CYGJ_ALERT_PROCESS_LOG set ENDTIME = '" + endTime + "', MEMO = '" + memo +  "' where ID = '" + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据记录ID更新结束时间，处理人以及备注人:更新" + ret + "条报警处理日志");
	}
	/// <summary>
	/// 根据记录ID更新开始时间，结束时间，处理人以及备注
	/// </summary>
	/// <param name="time">Time.</param>
	/// <param name="person">Person.</param>
	/// <param name="memo">Memo.</param>
	/// <param name="id">Identifier.</param>
	public void Update003(string time,string person,string memo,string id)
	{
		string sql = "update CYGJ_ALERT_PROCESS_LOG set STARTTIME = '" + time + "', ENDTIME = '" + time +  "', PERSON = '" + person + "', MEMO = '" + memo +  "' where ID = '" + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据记录ID更新开始时间，结束时间，处理人以及备注:更新" + ret + "条报警处理日志");
	}

	public void Select001(string startTime,string endTime,string person)
	{
		Result = new List<AlertPrecessLogInfo> ();
		string sql = "select ID,ALERTTIME,ALERTTYPE,PERSON,STARTTIME,ENDTIME,MEMO from CYGJ_ALERT_PROCESS_LOG where ";
		string where1 = "STARTTIME >= '" + startTime + "'";
		string where2 = "ENDTIME <= '" + endTime + " 23:59:59'";
		string where3 = "PERSON = '" + person + "'";
		string where4 = "ALERTTIME <> " + "' '";
		if(startTime.Trim() != "" || endTime.Trim() != "" || person.Trim() != "")
		{
			if(startTime.Trim() != "") sql += where1 + " and ";
			if(endTime.Trim() != "") sql += where2 + " and ";
			if(person.Trim() != "") sql += where3 + " and ";

			sql += where4 + " order by ALERTTIME asc";
		}
		else
		{
			sql += where4 + " order by ALERTTIME desc";
		}

		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_ALERT_PROCESS_LOG");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				AlertPrecessLogInfo info = new AlertPrecessLogInfo();
				info.id = dr["ID"].ToString();
				info.alertTime = dr["ALERTTIME"].ToString();
				info.alertType = dr["ALERTTYPE"].ToString();
				info.person = dr["PERSON"].ToString();
				info.startTime = dr["STARTTIME"].ToString();
				info.endTime = dr["ENDTIME"].ToString();
				info.memo = dr["MEMO"].ToString();

				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("根据记录开始时间，结束时间，处理人,查找报警处理日志");
	}

	private void GetNextID()
	{
		string sql = "select SEQ_CYGJ_ALERT_PROCESS_LOG_ID.nextval as ID from dual";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "select last_insert_id() as ID from dual";
		}
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_ALERT_PROCESS_LOG");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			currentId = dt.Rows[0]["ID"].ToString();
		}
	}
	
}
