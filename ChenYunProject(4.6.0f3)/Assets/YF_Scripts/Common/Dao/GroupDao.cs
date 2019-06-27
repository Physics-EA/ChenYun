using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 存储组的信息
/// </summary>
public struct GroupInfo
{
	/// <summary>
	/// 组的ID
	/// </summary>
	public string Id;
	/// <summary>
	/// 组的名称
	/// </summary>
	public string Name;
	/// <summary>
	/// 组的状态
	/// </summary>
	public string Status;
}
/// <summary>
/// 操作跟组相关的数据库记录
/// </summary>
public class GroupDao
{
	public List<GroupInfo> Result;

	public GroupDao()
	{
		Result = new List<GroupInfo> ();
	}
	/// <summary>
	/// 根据组ID检索组信息
	/// </summary>
	/// <param name="groupId">组ID</param>
	public void Select001(string groupId)
	{
		Result.Clear ();
		string sql = "select ID , NAME,STATUS from CYGJ_GROUP where ID = '" + groupId + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_GROUP");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			if(dt.Rows.Count > 0)
			{
				GroupInfo info = new GroupInfo();
				DataRow dr = dt.Rows[0];
				info.Id = dr["ID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.Status = dr["STATUS"].ToString();

				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("根据用户组ID检索用户组信息。检索件数：" + Result.Count + ",检索条件：ID = " + groupId);
	}

    public int SelectUserGroup(string userId, ref List<GroupInfo> groups)
    {
        try
        {
            UserGroupDao ugdao = new UserGroupDao();
            string uid = "", groupId = "";
            int err = ugdao.getGroupId(userId, ref uid, ref groupId);
            if(err<1)
            {
                if(err==0)
                    Logger.Instance.WriteLog("指定用户的用户组不存在！。用户ID：" + userId);
                else
                    Logger.Instance.WriteLog("查询指定用户的用户组失败！。用户ID：" + userId);
                return 0;
            }
            string sql = "select ID , NAME,STATUS from CYGJ_GROUP where ID = '" + groupId + "'";
            DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, "CYGJ_GROUP");
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    GroupInfo info = new GroupInfo();
                    DataRow dr = dt.Rows[0];
                    info.Id = dr["ID"].ToString();
                    info.Name = dr["NAME"].ToString();
                    info.Status = dr["STATUS"].ToString();

                    groups.Add(info);
                }
            }
            Logger.Instance.WriteLog("根据用户ID检索用户组信息。检索件数：" + groups.Count + ",检索条件：ID = " + userId);
        }
        catch (System.Exception e)
        {
            Logger.Instance.error(e.Message);
            groups.Clear();
            return -1;
        }
        return groups.Count;
    }


	/// <summary>
	/// 根据用户ID检索组信息
	/// </summary>
	/// <param name="groupId">用户ID</param>
	public void Select002(string userId)
	{
		Result.Clear ();
		UserGroupDao ugdao = new UserGroupDao();
		ugdao.Select001(userId);
		if(ugdao.Result.Id == "-1")
		{
			Logger.Instance.WriteLog("指定用户不存在。用户ID：" + userId);
			return;
		}
		UserGroupInfo ugInfo = ugdao.Result;
		string sql = "select ID , NAME,STATUS from CYGJ_GROUP where ID = '" + ugInfo.GroupId + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_GROUP");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			if(dt.Rows.Count > 0)
			{
				GroupInfo info = new GroupInfo();
				DataRow dr = dt.Rows[0];
				info.Id = dr["ID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.Status = dr["STATUS"].ToString();
				
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("根据用户ID检索用户组信息。检索件数：" + Result.Count + ",检索条件：ID = " + userId);
	}

	/// <summary>
	/// 检索所有组信息
	/// </summary>
	public void Select003()
	{
		Result.Clear ();
		string sql = "select ID , NAME,STATUS from CYGJ_GROUP where NAME <> '超级管理员'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_GROUP");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				GroupInfo info = new GroupInfo();
				info.Id = dr["ID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.Status = dr["STATUS"].ToString();
				
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("检索所有不是超级管理员的用户组信息。检索件数：" + Result.Count);
	}
	/// <summary>
	/// 根据组名称检索组信息
	/// </summary>
	/// <param name="groupName">组名称</param>
	public void Select004(string groupName)
	{
		Result.Clear ();
		string sql = "select ID , NAME,STATUS from CYGJ_GROUP where NAME = '" + groupName + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_GROUP");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			if(dt.Rows.Count > 0)
			{
				GroupInfo info = new GroupInfo();
				DataRow dr = dt.Rows[0];
				info.Id = dr["ID"].ToString();
				info.Name = dr["NAME"].ToString();
				info.Status = dr["STATUS"].ToString();
				
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("据组名称检索用户组信息。检索件数：" + Result.Count + ",检索条件：NAME:" + groupName);
	}
	/// <summary>
	/// 将指定名称的组插入到数据库
	/// </summary>
	/// <param name="gourpName">Gourp name.</param>
	public void Insert001(string gourpName)
	{
		string sql = "insert into CYGJ_GROUP(ID,NAME,STATUS) values(SEQ_CYGJ_GROUP_ID.nextval,'" + gourpName + "','正常')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_GROUP(NAME,STATUS) values('" + gourpName + "','正常')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
		Logger.Instance.WriteLog("将指定名称的组插入到数据库。插入件数：" + ret);

	}
	/// <summary>
	/// 删除指定组Id的记录
	/// </summary>
	/// <param name="groupId">Group identifier.</param>
	public void Delete001(string groupId)
	{
		string sql = "delete from CYGJ_GROUP where ID = '" + groupId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
		Logger.Instance.WriteLog("删除指定组Id的记录。删除件数：" + ret + ",删除条件：ID = " + groupId);
	}
	/// <summary>
	/// 删除指定组名称的记录
	/// </summary>
	/// <param name="groupId">Group identifier.</param>
	public void Delete002(string gourpName)
	{
		string sql = "delete from CYGJ_GROUP where NAME = '" + gourpName + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
		Logger.Instance.WriteLog("删除指定组名称的记录。删除件数：" + ret + ",删除条件：ID = " + gourpName);
	}
	/// <summary>
	/// 更新指定组的状态
	/// </summary>
	/// <param name="groupId">Group identifier.</param>
	/// <param name="newStatus">New status.</param>
	public void Update001(string groupId,string newStatus)
	{
		string sql = "update CYGJ_GROUP set STATUS = '" + newStatus + "' where ID = '" + groupId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
		Logger.Instance.WriteLog("更新指定组的状态。更新件数：" + ret + ",更新条件：ID = " + groupId);
	}
}
