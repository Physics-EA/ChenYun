using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 疏散区域
/// </summary>
public struct EvacuateArea
{
	public string id;
	public string name;
	public string fontSize;
	public string points;
	public string cameraList;
}
/// <summary>
/// 疏散预案关联区域
/// </summary>
public struct EvacuateAreaOfPlan
{
	public string id;
	public string evacuatePlanId;
	public string evacuateAreaId;
}
/// <summary>
/// 疏散预案
/// </summary>
public struct EvacuatePlan
{
	public string id;
	public string name;
}

public class EvacuationPlanDao : MonoBehaviour 
{
	/// <summary>
	/// 检索所有的疏散区域
	/// </summary>
	public List<EvacuateArea> Select001()
	{
		List<EvacuateArea> Result = new List<EvacuateArea>();
		string sql = "select ID,NAME,FONTSIZE,POINTS,CAMERALIST from CYGJ_EVACUATE_AREA order by ID asc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_EVACUATE_AREA");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				EvacuateArea info = new EvacuateArea();
				info.id = dr["ID"].ToString();
				info.name = dr["NAME"].ToString();
				info.fontSize = dr["FONTSIZE"].ToString();
				info.points = dr["POINTS"].ToString();
				info.cameraList = dr["CAMERALIST"].ToString();
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("向数据库检索所有的疏散区域");
		return Result;
	}

	/// <summary>
	/// 检索疏散预案对应的疏散区域
	/// </summary>
	public List<EvacuateAreaOfPlan> Select002(string evacuatePlanId)
	{
		List<EvacuateAreaOfPlan> Result = new List<EvacuateAreaOfPlan>();
		string sql = "select ID,EVACUATEPLANID,EVACUATEAREAID from CYGJ_EVACUATE_AREAOFPLAN where EVACUATEPLANID = '" + evacuatePlanId + "' order by ID asc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_EVACUATE_AREAOFPLAN");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				EvacuateAreaOfPlan info = new EvacuateAreaOfPlan();
				info.id = dr["ID"].ToString();
				info.evacuatePlanId = dr["EVACUATEPLANID"].ToString();
				info.evacuateAreaId = dr["EVACUATEAREAID"].ToString();
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("向数据库检索疏散预案对应的疏散区域。检索条件：EVACUATEPLANID = " + evacuatePlanId);
		return Result;
	}

	/// <summary>
	/// 检索所有的疏散预案
	/// </summary>
	public List<EvacuatePlan> Select003()
	{
		List<EvacuatePlan> Result = new List<EvacuatePlan>();
		string sql = "select ID,NAME from CYGJ_EVACUATE_PLAN order by ID asc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_EVACUATE_PLAN");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				EvacuatePlan info = new EvacuatePlan();
				info.id = dr["ID"].ToString();
				info.name = dr["NAME"].ToString();
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("向数据库检索所有的疏散预案");
		return Result;
	}

	/// <summary>
	/// 检索指定名称的疏散预案
	/// </summary>
	public List<EvacuatePlan> Select004(string name)
	{
		List<EvacuatePlan> Result = new List<EvacuatePlan>();
		string sql = "select ID,NAME from CYGJ_EVACUATE_PLAN where NAME = '" + name + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_EVACUATE_PLAN");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				EvacuatePlan info = new EvacuatePlan();
				info.id = dr["ID"].ToString();
				info.name = dr["NAME"].ToString();
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("向数据库检索指定名称的疏散预案。检索条件：NAME = " + name);
		return Result;
	}

	
	/// <summary>
	/// 检索指定名称的疏散区域
	/// </summary>
	public List<EvacuateArea> Select005(string name)
	{
		List<EvacuateArea> Result = new List<EvacuateArea>();
		string sql = "select ID,NAME,FONTSIZE,POINTS,CAMERALIST from CYGJ_EVACUATE_AREA where NAME = '" + name + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_EVACUATE_AREA");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				EvacuateArea info = new EvacuateArea();
				info.id = dr["ID"].ToString();
				info.name = dr["NAME"].ToString();
				info.fontSize = dr["FONTSIZE"].ToString();
				info.points = dr["POINTS"].ToString();
				info.cameraList = dr["CAMERALIST"].ToString();
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("向数据库检索指定名称的疏散区域。检索条件：NAME = " + name);
		return Result;
	}

	/// <summary>
	/// 检索指定ID的疏散区域
	/// </summary>
	public List<EvacuateArea> Select006(string areaId)
	{
		List<EvacuateArea> Result = new List<EvacuateArea>();
		string sql = "select ID,NAME,FONTSIZE,POINTS,CAMERALIST from CYGJ_EVACUATE_AREA where ID = '" + areaId + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_EVACUATE_AREA");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				EvacuateArea info = new EvacuateArea();
				info.id = dr["ID"].ToString();
				info.name = dr["NAME"].ToString();
				info.fontSize = dr["FONTSIZE"].ToString();
				info.points = dr["POINTS"].ToString();
				info.cameraList = dr["CAMERALIST"].ToString();
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("向数据库检索指定ID的疏散区域。检索条件：ID = " + areaId);
		return Result;
	}

	/// <summary>
	/// 删除指定的疏散区域
	/// </summary>
	/// <param name="evacuateAreaId">Evacuate area identifier.</param>
	public void Delete001(string evacuateAreaId)
	{
		string sql = "delete from CYGJ_EVACUATE_AREA where ID = '" + evacuateAreaId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("向数据库删除指定的疏散区域。删除记录数：" + ret + ",删除条件：ID = " + evacuateAreaId);

	}

	/// <summary>
	/// 从区域预案关联表中删除指定区域id的记录
	/// </summary>
	/// <param name="evacuateAreaId">Evacuate area identifier.</param>
	public void Delete002(string evacuateAreaId)
	{
		string sql = "delete from CYGJ_EVACUATE_AREAOFPLAN where EVACUATEAREAID = '" + evacuateAreaId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("从区域预案关联表中删除指定区域id的记录。删除记录数：" + ret + ",删除条件：EVACUATEAREAID = " + evacuateAreaId);
	}

	/// <summary>
	/// 从区域预案关联表中删除指定疏散预案id记录
	/// </summary>
	/// <param name="evacuateAreaId">Evacuate area identifier.</param>
	public void Delete003(string evacuatePlanId)
	{
		string sql = "delete from CYGJ_EVACUATE_AREAOFPLAN where EVACUATEPLANID = '" + evacuatePlanId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("从区域预案关联表中删除指定疏散预案id记录。删除记录数：" + ret + ",删除条件：EVACUATEPLANID = " + evacuatePlanId);
		
	}

	/// <summary>
	/// 删除指定疏散预案
	/// </summary>
	/// <param name="evacuateAreaId">Evacuate area identifier.</param>
	public void Delete004(string evacuatePlanId)
	{
		string sql = "delete from CYGJ_EVACUATE_PLAN where ID = '" + evacuatePlanId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("删除指定疏散预案。删除记录数：" + ret + ",删除条件：ID = " + evacuatePlanId);
		
	}
	/// <summary>
	/// 插入一条疏散区域记录
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="fontSize">Font size.</param>
	/// <param name="points">Points.</param>
	public void Insert001(string name,string fontSize,string points)
	{
		string sql = "insert into CYGJ_EVACUATE_AREA(ID,NAME,FONTSIZE,POINTS) values(CYGJ_EVACUATE_AREA_ID.nextval,'" + name + "','" + fontSize + "','" + points + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_EVACUATE_AREA(NAME,FONTSIZE,POINTS) values('" + name + "','" + fontSize + "','" + points + "')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("插入疏散区域记录。插入记录数：" + ret);
	}
	/// <summary>
	/// 插入一条疏散预案对应的疏散区域记录
	/// </summary>
	/// <param name="evacuatePlanId">Evacuate plan identifier.</param>
	/// <param name="evacuateAreaId">Evacuate area identifier.</param>
	public void Insert002(string evacuatePlanId,string evacuateAreaId)
	{
		string sql = "insert into CYGJ_EVACUATE_AREAOFPLAN(ID,EVACUATEPLANID,EVACUATEAREAID) values(CYGJ_EVACUATE_AREAOFPLAN_ID.nextval,'" + evacuatePlanId + "','" + evacuateAreaId + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_EVACUATE_AREAOFPLAN(EVACUATEPLANID,EVACUATEAREAID) values('" + evacuatePlanId + "','" + evacuateAreaId + "')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("插入条疏散预案对应的疏散区域记录。插入记录数：" + ret);
	}
	/// <summary>
	/// 插入一条疏散预案记录
	/// </summary>
	/// <param name="name">Name.</param>
	public void Insert003(string name)
	{
		string sql = "insert into CYGJ_EVACUATE_PLAN(ID,NAME) values(CYGJ_EVACUATE_PLAN_ID.nextval,'" + name + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_EVACUATE_PLAN(NAME) values('" + name + "')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("插入疏散预案记录。插入记录数：" + ret);
	}

	/// <summary>
	/// 更新指定疏散预案名称
	/// </summary>
	/// <param name="name">Name.</param>
	public void Update001(string id,string newName)
	{
		string sql = "update CYGJ_EVACUATE_PLAN set NAME = '" + newName + "' where ID = '"  + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("更新指定疏散预案名称。更新记录数：" + ret + "更新条件：ID = " + id);
	}

	/// <summary>
	/// 更新指定疏散区域名称
	/// </summary>
	/// <param name="name">Name.</param>
	public void Update002(string id,string newName)
	{
		string sql = "update CYGJ_EVACUATE_AREA set NAME = '" + newName + "' where ID = '"  + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("更新指定疏散区域名称。更新记录数：" + ret + ",更新条件：ID = " + id);
	}

	/// <summary>
	/// 更新指定疏散区域顶点信息
	/// </summary>
	/// <param name="name">Name.</param>
	public void Update003(string id,string newPoints)
	{
		string sql = "update CYGJ_EVACUATE_AREA set POINTS = '" + newPoints + "' where ID = '"  + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("更新指定疏散区域顶点信息。更新记录数：" + ret + ",更新条件：ID = " + id);
	}

	/// <summary>
	/// 更新指定疏散区域名称字体
	/// </summary>
	/// <param name="name">Name.</param>
	public void Update004(string id,string newFontSize)
	{
		string sql = "update CYGJ_EVACUATE_AREA set FONTSIZE = '" + newFontSize + "' where ID = '"  + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("更新指定疏散区域名称字体。更新记录数：" + ret + ",更新条件：ID = " + id);
	}

	
	/// <summary>
	/// 更新指定疏散区域绑定的摄像机
	/// </summary>
	/// <param name="name">Name.</param>
	public void Update005(string id,string newCameraList)
	{
		string sql = "update CYGJ_EVACUATE_AREA set CAMERALIST = '" + newCameraList + "' where ID = '"  + id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("更新指定疏散区域绑定的摄像机。更新记录数：" + ret + ",更新条件：ID = " + id);
	}
}
