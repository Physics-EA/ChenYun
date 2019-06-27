using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
/// <summary>
/// 保存权限表记录的信息
/// </summary>
public struct AuthorityInfo
{
	/// <summary>
	/// 数据记录ID
	/// </summary>
	public string Id;
	/// <summary>
	/// 用户ID
	/// </summary>
	public string Authority;
	/// <summary>
	/// 组ID
	/// </summary>
	public string Description;
};
/// <summary>
/// 对权限表进行操作
/// </summary>
public class AuthorityDao 
{
	/// <summary>
	/// 查询数据时的返回结果
	/// </summary>
	public List<AuthorityInfo> Result;
	/// <summary>
	/// 检索所有的数据
	/// </summary>
	public void Select001()
	{
		Result = new List<AuthorityInfo> ();
		string sql = "select ID,AUTHORITY,DESCRIPTION from CYGJ_AUTHORITY order by ID asc";
		DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet (sql, "CYGJ_AUTHORITY");
		if(ds.Tables.Count > 0)
		{
			DataTable dt = ds.Tables[0];
			AuthorityInfo info;
			foreach(DataRow dr in dt.Rows)
			{
				info = new AuthorityInfo();
				info.Id = dr["ID"].ToString();
				info.Authority = dr["AUTHORITY"].ToString();
				info.Description = dr["DESCRIPTION"].ToString();
				
				Result.Add(info);
			}
		}
		Logger.Instance.WriteLog("检索所有权限数据");
	}
}
