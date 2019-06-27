using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 视频巡逻记录结构
/// </summary>
public struct VideoPatrolPlanInfo
{
	/// <summary>
	/// 记录ID
	/// </summary>
	public string Id;
	/// <summary>
	/// 巡逻方案名称
	/// </summary>
	public string Name;
	/// <summary>
	/// 巡逻摄像头列表
	/// 格式(1|2|3|4)
	/// 数字为摄像头在数据库中ID
	/// </summary>
	public string MonitorList;
	/// <summary>
	/// 每个摄像头播放时间
	/// 格式(10|20|30|40)
	/// 单位位秒，每个数字对应MonitorList的每个摄像头
	/// 例如：摄像头 3 的播放时间为 30 秒
	/// </summary>
	public string PlayTimeList;
	/// <summary>
	/// 创建数据的账号
	/// </summary>
	public string AccountName;
	/// <summary>
	/// 创建时间
	/// </summary>
	public string CreateTime;
}
/// <summary>
/// 巡逻方案数据库管理类
/// </summary>
public class VideoPatrolPlanDao
{
	public List<VideoPatrolPlanInfo> Result;
	/// <summary>
	/// 插入巡逻方案记录
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="MonitorList">Monitor list.</param>
	/// <param name="PlayTimeList">Play time list.</param>
	/// <param name="account">Account.</param>
	public void Insert001(string name ,string MonitorList,string PlayTimeList,string account)
	{
		string sql = "insert into CYGJ_VIDEO_PATROL_PLAN(ID,NAME,DEVICEIDLIST,PLAYTIMELIST,ACCOUNTNAME,CREATETIME) values " +
			"(SEQ_CYGJ_VIDEO_PATROL_PLAN_ID.nextval,'" + name + "','" + MonitorList + "','" + PlayTimeList + "','" + account + "',to_char(sysdate,'yyyy-mm-dd hh24:mi:ss'))";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_VIDEO_PATROL_PLAN(NAME,DEVICEIDLIST,PLAYTIMELIST,ACCOUNTNAME,CREATETIME) values " +
				"('" + name + "','" + MonitorList + "','" + PlayTimeList + "','" + account + "',sysdate())";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("插入巡逻方案记录。插入件数：" + ret);
	}
	/// <summary>
	/// 根据用指定ID更新视频巡航方案的设备列表，播放时间
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="MonitorList">Monitor list.</param>
	/// <param name="PlayTimeList">Play time list.</param>
	public void Update001(string id,string MonitorList,string PlayTimeList)
	{
		string sql = "update CYGJ_VIDEO_PATROL_PLAN set  DEVICEIDLIST = '" + MonitorList + "', PLAYTIMELIST = '" + PlayTimeList + "' where ID = '" + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据用指定ID更新视频巡航方案的设备列表，播放时间。更新件数：" + ret + ",更新条件：ID = " + id);
	}
	/// <summary>
	/// 根据用指定ID更新视频巡航方案的名称
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="name">Name.</param>
	public int Update002(string id,string name)
	{
		string sql = "update CYGJ_VIDEO_PATROL_PLAN set  NAME = '" + name + "' where ID = '" + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据用指定ID更新视频巡航方案的名称。更新件数：" + ret + ",更新条件：ID = " + id);
		return ret;
	}
	/// <summary>
	/// 删除用指定ID视频巡航方案记录
	/// </summary>
	/// <param name="id">Identifier.</param>
	public int Delete001(string id)
	{
		string sql = "delete from CYGJ_VIDEO_PATROL_PLAN  where ID = '" + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("删除用指定ID视频巡航方案记录。删除件数：" + ret + ",删除条件：ID = " + id);
		return ret;
	}
	/// <summary>
	/// 按ID升序检索所有视频巡航方案记录
	/// </summary>
	public void Select001()
	{
		Result = new List<VideoPatrolPlanInfo> ();

		string sql = "select ID,NAME,DEVICEIDLIST,PLAYTIMELIST,ACCOUNTNAME,CREATETIME from CYGJ_VIDEO_PATROL_PLAN order by ID asc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_VIDEO_PATROL_PLAN");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				VideoPatrolPlanInfo info = new VideoPatrolPlanInfo();
				info.Id = dr["ID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.MonitorList = dr["DEVICEIDLIST"].ToString();
				info.PlayTimeList = dr["PLAYTIMELIST"].ToString();
				info.AccountName = dr["ACCOUNTNAME"].ToString();
				info.CreateTime = dr["CREATETIME"].ToString();
				Result.Add(info);
			}			
		}
		Logger.Instance.WriteLog("按ID升序检索所有视频巡航方案记录。检索件数：" + Result.Count);
	}
	/// <summary>
	/// 按ID降序检索所有视频巡航方案记录
	/// </summary>
	public void Select002()
	{
		Result = new List<VideoPatrolPlanInfo> ();
		
		string sql = "select ID,NAME,DEVICEIDLIST,PLAYTIMELIST,ACCOUNTNAME,CREATETIME from CYGJ_VIDEO_PATROL_PLAN order by ID desc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_VIDEO_PATROL_PLAN");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				VideoPatrolPlanInfo info = new VideoPatrolPlanInfo();
				info.Id = dr["ID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.MonitorList = dr["DEVICEIDLIST"].ToString();
				info.PlayTimeList = dr["PLAYTIMELIST"].ToString();
				info.AccountName = dr["ACCOUNTNAME"].ToString();
				info.CreateTime = dr["CREATETIME"].ToString();

				Result.Add(info);
			}			
		}
		Logger.Instance.WriteLog("按ID降序检索所有视频巡航方案记录。检索件数：" + Result.Count);
	}

	/// <summary>
	/// 根据ID检索视频巡航方案记录
	/// </summary>
	public void Select003(string id)
	{
		Result = new List<VideoPatrolPlanInfo> ();
		
		string sql = "select ID,NAME,DEVICEIDLIST,PLAYTIMELIST,ACCOUNTNAME,CREATETIME from CYGJ_VIDEO_PATROL_PLAN where ID = '" + id + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_VIDEO_PATROL_PLAN");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				VideoPatrolPlanInfo info = new VideoPatrolPlanInfo();
				info.Id = dr["ID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.MonitorList = dr["DEVICEIDLIST"].ToString();
				info.PlayTimeList = dr["PLAYTIMELIST"].ToString();
				info.AccountName = dr["ACCOUNTNAME"].ToString();
				info.CreateTime = dr["CREATETIME"].ToString();
				
				Result.Add(info);
			}			
		}
		Logger.Instance.WriteLog("根据ID检索视频巡航方案记录。检索件数：" + Result.Count);
	}

	/// <summary>
	/// 根据name检索视频巡航方案记录
	/// </summary>
	public void Select004(string name)
	{
		Result = new List<VideoPatrolPlanInfo> ();
		
		string sql = "select ID,NAME,DEVICEIDLIST,PLAYTIMELIST,ACCOUNTNAME,CREATETIME from CYGJ_VIDEO_PATROL_PLAN where NAME = '" + name + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_VIDEO_PATROL_PLAN");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				VideoPatrolPlanInfo info = new VideoPatrolPlanInfo();
				info.Id = dr["ID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.MonitorList = dr["DEVICEIDLIST"].ToString();
				info.PlayTimeList = dr["PLAYTIMELIST"].ToString();
				info.AccountName = dr["ACCOUNTNAME"].ToString();
				info.CreateTime = dr["CREATETIME"].ToString();
				
				Result.Add(info);
			}			
		}
		Logger.Instance.WriteLog("根据name检索视频巡航方案记录。检索件数：" + Result.Count);
	}
}
