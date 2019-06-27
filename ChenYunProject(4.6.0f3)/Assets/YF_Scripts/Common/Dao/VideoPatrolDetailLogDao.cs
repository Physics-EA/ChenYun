using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 巡逻方案详细日志信息
/// </summary>
public struct VideoPatrolDetailLogInfo
{
	/// <summary>
	/// 数据库ID
	/// </summary>
	public string id;
	/// <summary>
	/// 查看的摄像机
	/// </summary>
	public string camera;
	/// <summary>
	/// 开始执行时间
	/// </summary>
	public string startTime;
	/// <summary>
	/// 结束时间
	/// </summary>
	public string endTime;
	/// <summary>
	/// 备注
	/// </summary>
	public string memo;
	/// <summary>
	/// 巡逻方案日志id
	/// </summary>
	public string patrolLogId;
}

public class VideoPatrolDetailLogDao
{
	public List<VideoPatrolDetailLogInfo> Result;
	/// <summary>
	/// 根据传入的视频巡逻日志id检索详细的日志信息
	/// </summary>
	/// <param name="patrolLogId">Patrol log identifier.</param>
	public void Select001(string patrolLogId)
	{
		Result = new List<VideoPatrolDetailLogInfo> ();
		string sql = "select ID,CAMERA,STARTTIME,ENDTIME,MEMO,PATROLLOGID from CYGJ_VIDEO_PATROL_DETAIL_LOG where PATROLLOGID = '" + patrolLogId + "' order by STARTTIME asc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_VIDEO_PATROL_DETAIL_LOG");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				VideoPatrolDetailLogInfo info = new VideoPatrolDetailLogInfo();
				info.id = dr["ID"].ToString();
				info.camera = dr["CAMERA"].ToString();
				info.startTime = dr["STARTTIME"].ToString();
				info.endTime = dr["ENDTIME"].ToString();
				info.memo = dr["MEMO"].ToString();
				info.patrolLogId = dr["PATROLLOGID"].ToString();
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("根据指定的视频巡逻日志id检索详细的日志信息。检索件数：" + Result.Count + ",检索条件：PATROLLOGID = " + patrolLogId);
	}

	/// <summary>
	/// 向数据库中插入新的数据
	/// </summary>
	/// <param name="camera">Camera.</param>
	/// <param name="startTime">Start time.</param>
	/// <param name="endTime">End time.</param>
	/// <param name="memo">Memo.</param>
	/// <param name="patrolLogId">Patrol log identifier.</param>
	public void Insert001(string camera,string startTime,string endTime,string memo,string patrolLogId)
	{
		string sql = "insert into CYGJ_VIDEO_PATROL_DETAIL_LOG(ID,CAMERA,STARTTIME,ENDTIME,MEMO,PATROLLOGID) values(SEQ_CYGJ_PATROL_DETAIL_LOG_ID.nextval," +
			"'" + camera + "','" + startTime + "','" + endTime + "','" + memo + "','" + patrolLogId + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_VIDEO_PATROL_DETAIL_LOG(CAMERA,STARTTIME,ENDTIME,MEMO,PATROLLOGID) values(" +
				"'" + camera + "','" + startTime + "','" + endTime + "','" + memo + "','" + patrolLogId + "')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("插入视频巡逻详细log数据。插入件数：" + ret);
	}
}
