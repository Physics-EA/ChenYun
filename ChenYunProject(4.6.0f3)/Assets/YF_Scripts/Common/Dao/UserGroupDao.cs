using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 用保存用户记录与组的关联表的信息
/// </summary>
public struct UserGroupInfo
{
	/// <summary>
	/// 数据记录ID
	/// </summary>
	public string Id;
	/// <summary>
	/// 用户ID
	/// </summary>
	public string UserId;
	/// <summary>
	/// 组ID
	/// </summary>
	public string GroupId;
};
/// <summary>
/// 操作用户与组的关联表的记录
/// </summary>
public class UserGroupDao
{
	public UserGroupInfo Result;

    public int getGroupId(string userId, ref string uid, ref string GroupId)
    {
        int result = 0;
        try
        {
            string sql = "select ID , USERID,GROUPID from CYGJ_USER_GROUP where USERID = '" + userId + "'";
            DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, "CYGJ_USER_GROUP");
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    uid = dr["ID"].ToString();
                    GroupId = dr["GROUPID"].ToString();
                    result = 1;
                }
            }
            Logger.Instance.WriteLog("根据用户ID 查询所属组的ID。检索件数：" + (Result.Id == "-1" ? 0 : 1) + ",检索条件：USERID = " + userId);
        }
        catch (System.Exception e)
        {
            Logger.Instance.error(e.Message);
            uid = string.Empty;
            GroupId = string.Empty;
            result = -1;
        }
                
        return result;
    }


	/// <summary>
	/// 根据用户ID 查询所属组的ID
	/// </summary>
	/// <param name="userId">用户ID</param>
	public void Select001(string userId)
	{
		Result = new UserGroupInfo ();
		Result.Id = "-1";
		string sql = "select ID , USERID,GROUPID from CYGJ_USER_GROUP where USERID = '" + userId + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_USER_GROUP");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			if(dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				Result.Id = dr["ID"].ToString();
				Result.UserId = dr["USERID"].ToString();
				Result.GroupId = dr["GROUPID"].ToString();
			}
		}
		Logger.Instance.WriteLog("根据用户ID 查询所属组的ID。检索件数：" + (Result.Id == "-1"? 0: 1) + ",检索条件：USERID = " + userId);
	}
	/// <summary>
	/// 插入新的数据
	/// </summary>
	/// <param name="userId">用户ID</param>
	/// <param name="groupId">组ID</param>
	public void Insert001(string userId, string groupId)
	{
		string sql = "insert into CYGJ_USER_GROUP(ID,USERID,GROUPID) values(SEQ_CYGJ_USER_GROUP_ID.nextval,'" + userId + "','" + groupId + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_USER_GROUP(USERID,GROUPID) values('" + userId + "','" + groupId + "')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("插入新的用户组信息。插入件数：" + ret);
	}
	/// <summary>
	/// 根据用ID修改组ID
	/// </summary>
	/// <param name="groupId">组ID</param>
	/// <param name="userId">用户ID</param>
	public void Update001(string groupId, string userId)
	{
		string sql = "update CYGJ_USER_GROUP set GROUPID = '" + groupId + "' where USERID = '" + userId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据用ID修改组ID。更新件数：" + ret + ",更新条件：USERID = " + userId);
	}
	/// <summary>
	/// 根据组ID修改组ID
	/// </summary>
	/// <param name="newGroupId">新的组ID</param>
	/// <param name="oldGroupId">旧的组ID</param>
	public void Update002(string newGroupId, string oldGroupId)
	{
		string sql = "update CYGJ_USER_GROUP set GROUPID = '" + newGroupId + "' where GROUPID = '" + oldGroupId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据组ID修改组ID。更新件数：" + ret + ",更新条件：USERID = " + oldGroupId);
	}
	/// <summary>
	/// 根据用户ID删除数据
	/// </summary>
	/// <param name="userId">User identifier.</param>
	public void Delete001(string userId)
	{
		string sql = "delete from CYGJ_USER_GROUP where USERID = '" + userId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据用户ID删除数据。删除件数：" + ret + ",删除条件：USERID = " + userId);
	}
}
