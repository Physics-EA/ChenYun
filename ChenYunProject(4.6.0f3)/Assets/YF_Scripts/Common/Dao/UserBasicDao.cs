using UnityEngine;
using System.Data;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 保存用户基本信息
/// </summary>
public struct UserBasicInfo
{
	/// <summary>
	/// 用户ID
	/// </summary>
	public string ID;
	/// <summary>
	/// 账户名称
	/// </summary>
	public string UserName;
	/// <summary>
	/// 登录密码
	/// </summary>
	public string Password;
	/// <summary>
	/// 用户真实姓名
	/// </summary>
	public string RealName;
	/// <summary>
	/// 用户电话号码
	/// </summary>
	public string Telphone;
	/// <summary>
	/// 用户创建的时间
	/// </summary>
	public string CreateTime;
	/// <summary>
	/// 用户家庭住址
	/// </summary>
	public string Address;
	/// <summary>
	/// 当前用户的状态
	/// </summary>
	public string Status;
	/// <summary>
	/// 备注信息
	/// </summary>
	public string Memo;
};
/// <summary>
/// 操作用户基本信息的数据库记录
/// </summary>
public class UserBasicDao 
{
	/// <summary>
	/// 数据检索结果
	/// </summary>
	public List<UserBasicInfo> Result;
	/// <summary>
	/// 检索数据每页最大件数
	/// </summary>
	private int pageSize;
	/// <summary>
	/// 设置当前要显示第几页的内容
	/// </summary>
	private int CurrentPageIndex;
	/// <summary>
	/// 默认构造函数
	/// </summary>
	public UserBasicDao()
	{
		Result = new List<UserBasicInfo>();
		pageSize = 5000;
		CurrentPageIndex = 1;
	}
	/// <summary>
	/// 根据传入的数据初始化对象
	/// </summary>
	/// <param name="size">每页数据的个数</param>
	/// <param name="index">需要显示的第几页的索引</param>
	public UserBasicDao(int size,int index)
	{
		Result = new List<UserBasicInfo>();
		pageSize = size;
		CurrentPageIndex = index;
	}
	/// <summary>
	/// 根据用户名和密码检索数据
	/// </summary>
	/// <param name="username">用户名</param>
	/// <param name="password">密码</param>
	public void Select001(string username,string password)
	{
		Result.Clear ();
		string sql = "select ID,USERNAME,PASSWORD,REALNAME,TELPHONE,CREATETIME,ADDRESS,STATUS,MEMO from CYGJ_USER_BASIC where " +
			"USERNAME = '" + username + "' and PASSWORD = '" + password + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,pageSize,CurrentPageIndex,"CYGJ_USER_BASIC");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				UserBasicInfo info = new UserBasicInfo();
				info.ID = dr["ID"].ToString();
				info.UserName = dr["USERNAME"].ToString();
				info.Password = dr["PASSWORD"].ToString();
				info.RealName = dr["REALNAME"].ToString();
				info.Telphone = dr["TELPHONE"].ToString();
				info.CreateTime = dr["CREATETIME"].ToString();
				info.Address = dr["ADDRESS"].ToString();
				info.Status = dr["STATUS"].ToString();
				info.Memo = dr["MEMO"].ToString();

				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("根据用户名和密码检索数据。检索件数：" + Result.Count);
	}

    public int getUserInfoFormDB(ref List<UserBasicInfo> users)
    {
        try
        {
            string sql = "select ID,USERNAME,PASSWORD,REALNAME,TELPHONE,CREATETIME,ADDRESS,STATUS,MEMO  from CYGJ_USER_BASIC order by CREATETIME asc";
            DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, pageSize, CurrentPageIndex, "CYGJ_USER_BASIC");
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    UserBasicInfo info = new UserBasicInfo();
                    info.ID = dr["ID"].ToString();
                    info.UserName = dr["USERNAME"].ToString();
                    info.Password = dr["PASSWORD"].ToString();
                    info.RealName = dr["REALNAME"].ToString();
                    info.Telphone = dr["TELPHONE"].ToString();
                    info.CreateTime = dr["CREATETIME"].ToString();
                    info.Address = dr["ADDRESS"].ToString();
                    info.Status = dr["STATUS"].ToString();
                    info.Memo = dr["MEMO"].ToString();
                    users.Add(info);
                }
            }
            Logger.Instance.WriteLog("检索所有用户信息。检索件数：" + users.Count);
        }
        catch (System.Exception e)
        {
            Logger.Instance.error(e.Message);
            users.Clear();
            return -1;
        }
        

        return users.Count;
    }
	/// <summary>
	/// 检索所有的用户信息
    /// 逐步作废！！！！
	/// </summary>
    /// 
	public void Select002()
	{
		Result.Clear ();
		string sql = "select ID,USERNAME,PASSWORD,REALNAME,TELPHONE,CREATETIME,ADDRESS,STATUS,MEMO  from CYGJ_USER_BASIC order by CREATETIME asc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,pageSize,CurrentPageIndex,"CYGJ_USER_BASIC");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				UserBasicInfo info = new UserBasicInfo();
				info.ID = dr["ID"].ToString();
				info.UserName = dr["USERNAME"].ToString();
				info.Password = dr["PASSWORD"].ToString();
				info.RealName = dr["REALNAME"].ToString();
				info.Telphone = dr["TELPHONE"].ToString();
				info.CreateTime = dr["CREATETIME"].ToString();
				info.Address = dr["ADDRESS"].ToString();
				info.Status = dr["STATUS"].ToString();
				info.Memo = dr["MEMO"].ToString();
				
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("检索所有用户信息。检索件数：" + Result.Count);
	}
	/// <summary>
	/// 根据给定的条件检索用户信息
	/// 条件为用户账号 或 真实姓名
	/// </summary>
	/// <param name="condition">Condition.</param>
	public void Select003(string condition)
	{
		Result.Clear ();
		string sql = "select ID,USERNAME,PASSWORD,REALNAME,TELPHONE,CREATETIME,ADDRESS,STATUS,MEMO from CYGJ_USER_BASIC where " +
			"USERNAME = '" + condition + "' or REALNAME = '" + condition + "'";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql,pageSize,CurrentPageIndex,"CYGJ_USER_BASIC");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			foreach(DataRow dr in dt.Rows)
			{
				UserBasicInfo info = new UserBasicInfo();
				info.ID = dr["ID"].ToString();
				info.UserName = dr["USERNAME"].ToString();
				info.Password = dr["PASSWORD"].ToString();
				info.RealName = dr["REALNAME"].ToString();
				info.Telphone = dr["TELPHONE"].ToString();
				info.CreateTime = dr["CREATETIME"].ToString();
				info.Address = dr["ADDRESS"].ToString();
				info.Status = dr["STATUS"].ToString();
				info.Memo = dr["MEMO"].ToString();
				
				Result.Add(info);
			}			
		}
		Logger.Instance.WriteLog("检索用户信息。检索件数：" + Result.Count + ",检索条件：USERNAME = " + condition);
	}
	/// <summary>
	/// 根据用户ID修改密码
	/// </summary>
	/// <param name="newPasswd">新密码</param>
	/// <param name="userId">用户ID</param>
	public void Update001(string newPasswd,string userId)
	{
		string sql = "update CYGJ_USER_BASIC set PASSWORD = '" + newPasswd + "' where ID = '" + userId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据用户ID修改密码。更新件数：" + ret + ",更新条件：ID = " + userId);
	}
	/// <summary>
	/// 根据用户ID修改用户状态
	/// </summary>
	/// <param name="Status">用户状态</param>
	/// <param name="UserID">用户ID</param>
	public void Update002(string Status,string UserId)
	{
		string sql = "update CYGJ_USER_BASIC set STATUS = '" + Status + "' where ID = '" + UserId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据用户ID修改用户状态。更新件数：" + ret + ",更新条件：ID = " + UserId);
	}
	/// <summary>
	/// 根据用户ID修改用户相关数据
	/// </summary>
	/// <param name="Password">密码</param>
	/// <param name="Telphone">电话号码</param>
	/// <param name="Address">家庭住址</param>
	/// <param name="Memo">备注</param>
	/// <param name="UserID">用户ID</param>
	public void Update003(string Username, string Password,string Telphone,string Address,string Memo,string UserID)
	{
		string sql = "update CYGJ_USER_BASIC set " +
				"REALNAME = '" + Username + "'," +
				"PASSWORD = '" + Password + "'," +
				"TELPHONE = '" + Telphone + "'," +
				"ADDRESS = '" + Address + "'," +
				"MEMO = '" + Memo + "' " +
				"where ID = '" + UserID + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("根据用户ID修改用户相关数据。更新件数：" + ret + ",更新条件：ID = " + UserID);
	}
	/// <summary>
	/// 插入新的用户信息
	/// </summary>
	/// <param name="UserName">账号名称</param>
	/// <param name="Password">密码</param>
	/// <param name="RealName">真实姓名</param>
	/// <param name="Telphone">电话号码</param>
	/// <param name="CreateTime">创建时间</param>
	/// <param name="Address">家庭住址</param>
	/// <param name="Status">状态</param>
	/// <param name="Memo">备注</param>
	public void Insert001(string UserName,string Password,string RealName,string Telphone,
	                      string CreateTime,string Address,string Status,string Memo)
	{
		string sql = "insert into CYGJ_USER_BASIC(ID,USERNAME,PASSWORD,REALNAME,TELPHONE,CREATETIME,ADDRESS,STATUS,MEMO) values(" +
			"SEQ_CYGJ_USER_BASIC_ID.nextval," + "'" + UserName + "','" + Password + "','" + RealName + "','" + Telphone + "','" + CreateTime + "','" + Address + "','" + Status + "','" + Memo + "')";
		if(OdbcDataManager.Instance.DataBaseType == "MySQL")
		{
			sql = "insert into CYGJ_USER_BASIC(USERNAME,PASSWORD,REALNAME,TELPHONE,CREATETIME,ADDRESS,STATUS,MEMO) values(" +
				"'" + UserName + "','" + Password + "','" + RealName + "','" + Telphone + "','" + CreateTime + "','" + Address + "','" + Status + "','" + Memo + "')";
		}
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("插入新的用户信息。插入件数：" + ret);
	}
	/// <summary>
	/// 根据用户ID删减数据
	/// </summary>
	/// <param name="userId">User identifier.</param>
	public void Delete001(string userId)
	{
		string sql = "delete from CYGJ_USER_BASIC where ID = '" + userId + "'";
		int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery (sql);
		Logger.Instance.WriteLog("删除指定ID的用户记录。删除件数：" + ret + ",删除条件：ID = " + userId);
	}
}
