using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 预设位信息结构
/// </summary>
public struct PrestPositionInfo
{
	public string Id;
	public string DeviceId;
	public string Name;
	public string DESCRIPTION;
	public string IsKeepWatch;
};
/// <summary>
/// 对预设位记录进行相关的操作
/// </summary>
public class PresetPositionDao
{
	public List<PrestPositionInfo> Result;
	public void Insert(string deviceId, string name,string description,string keepWatch = "否")
	{
		string sql = "insert into CYGJ_PRESET_POSITION(ID,DEVICEID,NAME,DESCRIPTION,KEEPWATCH) values(SEQ_CYGJ_PRESET_POSITION_ID.nextval,'" +
			deviceId + "','" + name + "','" + description + "','"+ keepWatch + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_PRESET_POSITION(DEVICEID,NAME,DESCRIPTION,KEEPWATCH) values('" +
				deviceId + "','" + name + "','" + description + "','"+ keepWatch + "')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("往预设位信息表中插入数据。插入件数：" + ret);
	}
	/// <summary>
	/// 根据设备Id检索相关的预设位信息
	/// </summary>
	/// <param name="DeviceId">Device identifier.</param>
	public void Select001(string DeviceId)
	{
		Result = new List<PrestPositionInfo> ();
		string sql = "select ID,DEVICEID,NAME,DESCRIPTION,KEEPWATCH from CYGJ_PRESET_POSITION where DEVICEID = '" + DeviceId + "' order by ID asc";
		DataSet ds =  OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_PRESET_POSITION");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				PrestPositionInfo info = new PrestPositionInfo();
				info.Id = dr["ID"].ToString();
				info.DeviceId = dr["DEVICEID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.DESCRIPTION = dr["DESCRIPTION"].ToString();
				info.IsKeepWatch = dr["KEEPWATCH"].ToString();

				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("根据设备Id检索相关的预设位信息。检索件数：" + Result.Count + ",检索条件：DEVICEID = " + DeviceId);
	}
	/// <summary>
	/// 检索出所有的 name 小于 name + 1，
	/// 而且name + 1 的记录不存在的所有记录，并按升序排列
	/// </summary>
	public void Select002()
	{
		Result = new List<PrestPositionInfo> ();
		string sql = "select t.ID,t.DEVICEID,t.NAME,t.DESCRIPTION,t.KEEPWATCH from CYGJ_PRESET_POSITION t where not exists(select t.* from CYGJ_PRESET_POSITION t2 where t2.name = t.name + 1) order by t.name asc";
		DataSet ds =  OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,"CYGJ_PRESET_POSITION");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				PrestPositionInfo info = new PrestPositionInfo();
				info.Id = dr["ID"].ToString();
				info.DeviceId = dr["DEVICEID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.DESCRIPTION = dr["DESCRIPTION"].ToString();
				info.IsKeepWatch = dr["KEEPWATCH"].ToString();
				
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("检索出所有的 name 小于 name + 1 的预设位信息。检索件数：" + Result.Count);
	}

	/// <summary>
	/// 把指定的记录设定成守望位
	/// </summary>
	/// <param name="Id">Identifier.</param>
	public void Update001(string Id)
	{
		Result = new List<PrestPositionInfo> ();
		string sql = "update CYGJ_PRESET_POSITION set KEEPWATCH = '是' where ID = '" + Id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("把指定的记录设定成守望位。更新件数：" + ret + ",更新条件：ID = " + Id);
	}
	/// <summary>
	/// 把指定设备的所有守望位取消掉
	/// </summary>
	public void Update002(string DeviceId)
	{
		string sql = "update CYGJ_PRESET_POSITION set KEEPWATCH = '否' where DEVICEID = '" + DeviceId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("把指定设备的所有守望位取消掉。更新件数：" + ret + ",更新条件：DEVICEID = " + DeviceId);
	}
	/// <summary>
	/// 把指定的记录取消守望位
	/// </summary>
	/// <param name="Id">Identifier.</param>
	public void Update003(string Id)
	{
		string sql = "update CYGJ_PRESET_POSITION set KEEPWATCH = '否' where ID = '" + Id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("把指定的记录取消守望位。更新件数：" + ret + ",更新条件：ID = " + Id);
	}
	/// <summary>
	/// 更新指定记录的描述
	/// </summary>
	/// <param name="Id">Identifier.</param>
	/// <param name="description">Description.</param>
	public void Update004(string Id,string description)
	{
		string sql = "update CYGJ_PRESET_POSITION set DESCRIPTION = '" + description + "' where ID = '" + Id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("更新指定ID预设位记录的描述。更新件数：" + ret + ",更新条件：ID = " + Id);
	}
	/// <summary>
	/// 删除指定的记录
	/// </summary>
	/// <param name="Id">Identifier.</param>
	public void Delete001(string Id)
	{
		string sql = "delete from CYGJ_PRESET_POSITION where ID = '" + Id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("删除指定的预设位记录。删除件数：" + ret + ",删除条件：ID = " + Id);
	}
	/// <summary>
	/// 删除指定设备的所有记录
	/// </summary>
	/// <param name="DeviceId">Device identifier.</param>
	public void Delete002(string DeviceId)
	{
		string sql = "delete from CYGJ_PRESET_POSITION where DEVICEID = '" + DeviceId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("删除指定设备的所有预设位记录。删除件数：" + ret + ",删除条件：DEVICEID = " + DeviceId);
	}
}
