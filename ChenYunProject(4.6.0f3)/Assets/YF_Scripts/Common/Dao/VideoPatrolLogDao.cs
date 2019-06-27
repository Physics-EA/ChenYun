using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 视频巡逻日志信息
/// </summary>
public struct VideoPatrolLogInfo
{
	/// <summary>
	/// 数据库id
	/// </summary>
	public string id;
	/// <summary>
	/// 执行人员
	/// </summary>
	public string person;
	/// <summary>
	/// 开始运行时间
	/// </summary>
	public string startDate;
	/// <summary>
	/// 执行的方案名称
	/// </summary>
	public string planName;
	/// <summary>
	/// 是否异常
	/// </summary>
	public string exception;
}
/// <summary>
/// 操作巡逻方案执行的日志记录
/// </summary>
public class VideoPatrolLogDao
{
	public List<VideoPatrolLogInfo> Result;
	public string currentId;
	/// <summary>
	/// 检索指定条件的数据
	/// </summary>
	/// <param name="dateFrom">开始日期</param>
	/// <param name="dateTo">结束日期</param>
	/// <param name="person">执行人</param>
	public void Select001(string dateFrom,string dateTo,string select)
	{
		Result = new List<VideoPatrolLogInfo> ();
		string sql = "select ID,PERSON,STARTDATE,PLANNAME,EXCEPTION from CYGJ_VIDEO_PATROL_LOG where ";

		string where1 = "STARTDATE >= '" + dateFrom + "'";
		string where2 = "STARTDATE <= '" + dateTo + " 23:59:59'";
		string where3 = "PERSON LIKE '%" + select + "%'" + " or ID LIKE '%" + select + "%'" + " or PLANNAME LIKE '%" + select + "%'";
		string where4 = "PLANNAME <> " + "' '";
		if(dateFrom.Trim() != "" || dateTo.Trim() != "" || select.Trim() != "")
		{
			if(dateFrom.Trim() != "") sql += where1 + " and ";
			if(dateTo.Trim() != "") sql += where2 + " and ";
			if(select.Trim() != "") sql += where3 + " and ";
			
			sql += where4 + " order by STARTDATE asc";
		}
		else
		{
			sql += where4 + " order by STARTDATE desc";
		}

		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_VIDEO_PATROL_LOG");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				VideoPatrolLogInfo info = new VideoPatrolLogInfo();
				info.id = dr["ID"].ToString();
				info.person = dr["PERSON"].ToString();
				info.startDate = dr["STARTDATE"].ToString();
				info.planName = dr["PLANNAME"].ToString();
				info.exception = dr["EXCEPTION"].ToString();
				
				Result.Add(info);
			}
		}

		Logger.Instance.WriteLog("检索指定条件的视频巡逻方案执行日志。检索件数：" + Result.Count);
	}
	/// <summary>
	/// 向数据库插入一条新视频巡逻方案执行日志
	/// </summary>
	/// <param name="person">执行人</param>
	/// <param name="startDate">开始时间</param>
	/// <param name="planName">Plan name.</param>
	/// <param name="exception">Exception.</param>
	public void Insert001(string person,string startDate,string planName,string exception = "无")
	{
		GetNextId ();
		string sql = "insert into CYGJ_VIDEO_PATROL_LOG(ID,PERSON,STARTDATE,PLANNAME,EXCEPTION) values (" +
			"'" + currentId + "','" + person + "','" + startDate + "','" + planName + "','" + exception +"')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_VIDEO_PATROL_LOG(PERSON,STARTDATE,PLANNAME,EXCEPTION) values (" +
				"'" + person + "','" + startDate + "','" + planName + "','" + exception +"')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("插入视频巡逻方案执行日志。插入件数：" + ret);
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			GetNextId ();
		}
	}
	/// <summary>
	/// 获取下个可以的id
	/// </summary>
	private void GetNextId()
	{
		string sql = "select SEQ_CYGJ_VIDEO_PATROL_LOG_ID.nextval as ID from dual";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "select last_insert_id() as ID from dual";
		}
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_VIDEO_PATROL_LOG");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			currentId = dt.Rows[0]["ID"].ToString();
		}
	}
}
