using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 保存组与权限的关联表的信息
/// </summary>
public struct GroupAuthorityInfo
{
	/// <summary>
	/// 数据记录ID
	/// </summary>
	public string Id;
	/// <summary>
	/// 组ID
	/// </summary>
	public string GroupId;
	/// <summary>
	/// 权限ID
	/// </summary>
	public string AuthorityId;
};
/// <summary>
/// 对组权限关联表进行操作
/// 组与权限是一对多的关系
/// </summary>
public class GroupAuthorityDao
{
	/// <summary>
	/// 检索数据是的返回值
	/// </summary>
	public List<GroupAuthorityInfo> Result;
	/// <summary>
	/// 根据组Id检索数据库记录
	/// </summary>
	/// <param name="GroupId">组Id</param>
	public void Select001(string GroupId)
	{
		Result = new List<GroupAuthorityInfo> ();
		string sql = "select ID,GROUPID,AUTHORITYID from CYGJ_GROUP_AUTHORITY where GROUPID = '" + GroupId + "' order by AUTHORITYID asc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_GROUP_AUTHORITY");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			GroupAuthorityInfo info;
			foreach(DataRow dr in dt.Rows)
			{
				info = new GroupAuthorityInfo();
				info.Id = dr["ID"].ToString();
				info.GroupId = dr["GROUPID"].ToString();
				info.AuthorityId = dr["AUTHORITYID"].ToString();

				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("根据组Id检索组与权限的关联表记录。检索条件：GROUPID:" + GroupId);
	}

	/// <summary>
	/// 插入新的数据
	/// </summary>
	/// <param name="userId">组ID</param>
	/// <param name="groupId">权限ID</param>
	public void Insert001(string groupId, string authorityId)
	{
		string sql = "insert into CYGJ_GROUP_AUTHORITY(ID,GROUPID,AUTHORITYID) values(SEQ_CYGJ_GROUP_AUTHORITY_ID.nextval,'" + groupId + "','" + authorityId + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_GROUP_AUTHORITY(GROUPID,AUTHORITYID) values('" + groupId + "','" + authorityId + "')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("向组与权限的关联表记录。插入件数：" + ret);
	}

	/// <summary>
	/// 插入新组
	/// </summary>
	/// <param name="info">Info.</param>
	public void InsertNewGroup(GroupInfo info)
	{
		string sql = "insert into CYGJ_GROUP(ID,NAME,STATUS) values('" + info.Id + "','" + info.Name + "','" + info.Status + "')";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("向组表记录。插入件数：" + ret);
	}

	/// <summary>
	/// 根据传入的Id删除数据
	/// </summary>
	/// <param name="Id">组权限关联表ID</param>
	public void Delete001(string Id)
	{
		string sql = "delete from CYGJ_GROUP_AUTHORITY where ID = '" + Id + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("删除指定Id的组权限关联表记录。删除记录数：" + ret + ",删除条件：ID = " + Id);
	}
	/// <summary>
	/// 删除指定组id的记录
	/// </summary>
	/// <param name="groupId">Group identifier.</param>
	public void Delete002(string groupId)
	{
		string sql = "delete from CYGJ_GROUP_AUTHORITY where GROUPID = '" + groupId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("删除指定组id的组权限关联表记录。删除记录数：" + ret + ",删除条件：GROUPID = " + groupId);
	}
	/// <summary>
	/// 删除指定权限id的记录
	/// </summary>
	/// <param name="authId">Auth identifier.</param>
	public void Delete003(string authId)
	{
		string sql = "delete from CYGJ_GROUP_AUTHORITY where AUTHORITYID = '" + authId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("删除指定权限id的组权限关联表记录。删除记录数：" + ret + ",删除条件：AUTHORITYID = " + authId);
	}
}
