using UnityEngine;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// （结构体）客流统计区域信息
/// </summary>
public struct PassengerFlowAreaInfo
{
    public string Id;
    public string Name;
    public string CameraIdLst;
    public string Points;
    public string WarnLevel1;
    public string WarnLevel2;
    public string WarnLevel3;
};


/// <summary>
/// （结构体）客流统计信息log结构体
/// </summary>
public struct PassengerFlowInfoLog
{
    public string Id;
    public string Date;
    public string PassengerFlowUrl;
    public string SumCount;
    public string EnterCount;
    public string ExitCount;
}


/// <summary>
/// 处理数据库信息
/// </summary>
public class PassengerFlowAreaDao
{

    /// <summary>
    /// 定义一个 PassengerFlowAreaInfo类型的结构体变量 Result，
    /// 里面存放的时哪几块区域
    /// </summary>
    public List<PassengerFlowAreaInfo> Result;


    /// <summary>
    /// 网客流统计区域信息插入数据
    /// </summary>
    /// <param name="name"></param>
    /// <param name="MonitorList"></param>
    /// <param name="points">地点</param>
    public void Insert001(string name, string MonitorList, string points)
    {
        string sql = "insert into CYGJ_PASSENGERFLOW_AREA(ID,NAME,CAMERAIDLST,POINTS) values " +
            "(SEQ_CYGJ_PASSENGERFLOW_AREA_ID.nextval,'" + name + "','" + MonitorList + "','" + points + "')";
        if (OdbcDataManager.Instance.DataBaseType == "MySQL")
        {
            sql = "insert into CYGJ_PASSENGERFLOW_AREA(NAME,CAMERAIDLST,POINTS) values " +
                "('" + name + "','" + MonitorList + "','" + points + "')";
        }
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("往客流统计区域表中插入数据。插入件数：" + ret);
    }


    /// <summary>
    /// 往客流统计信息log表中插入数据
    /// </summary>
    /// <param name="Date">Date.</param>
    /// <param name="PassengerFlowUrl">Passenger flow URL.</param>
    /// <param name="SumCount">Sum count.</param>
    /// <param name="EnterCount">Enter count.</param>
    /// <param name="EixtCount">Eixt count.</param>
    public void Insert002(string Date, string PassengerFlowUrl, string SumCount, string EnterCount, string EixtCount)
    {
        string sql = "insert into CYGJ_PASSENGERFLOW_LOG(ID,DATETIME,PASSENGERFLOWURL,SUMCOUNT,ENTERCOUNT,EXITCOUNT) values " +
            "(SEQ_CYGJ_PASSENGERFLOW_LOG.nextval,'" + Date + "','" + PassengerFlowUrl + "','" + SumCount + "','" + EnterCount + "','" + EixtCount + "')";
        if (OdbcDataManager.Instance.DataBaseType == "MySQL")
        {
            sql = "insert into CYGJ_PASSENGERFLOW_LOG(DATETIME,PASSENGERFLOWURL,SUMCOUNT,ENTERCOUNT,EXITCOUNT) values " +
                "('" + Date + "','" + PassengerFlowUrl + "','" + SumCount + "','" + EnterCount + "','" + EixtCount + "')";
        }
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("往客流统计信息log表中插入数据。插入件数：" + ret);
    }


    /// <summary>
    /// 客流统计区域ID删减数据
    /// </summary>
    /// <param name="userId">User identifier.</param>
    public void Delete001(string areaId)
    {
        string sql = "delete from CYGJ_PASSENGERFLOW_AREA where ID = '" + areaId + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("根据客流统计区域ID删除数据。删除件数：" + ret + ",删除条件：ID = " + areaId);
    }


    /// <summary>
    /// 检索所有数据按升序排列
    /// </summary>
    public void Select001()
    {

        //Result为空，在此之前还没有赋值
        Result = new List<PassengerFlowAreaInfo>();

        //清空Result中的所有元素
        Result.Clear();

        //在表CYGJ_PASSENGERFLOW_AREA中通过ID选择
        string sql = "select ID,NAME,CAMERAIDLST,POINTS,WARNLEVEL1,WARNLEVEL2,WARNLEVEL3 from CYGJ_PASSENGERFLOW_AREA order by ID asc";


        DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, "CYGJ_PASSENGERFLOW_AREA");


        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                PassengerFlowAreaInfo info = new PassengerFlowAreaInfo();
                info.Id = dr["ID"].ToString();
                info.Name = dr["NAME"].ToString();
                info.CameraIdLst = dr["CAMERAIDLST"].ToString();
                info.Points = dr["POINTS"].ToString();
                info.WarnLevel1 = dr["WARNLEVEL1"].ToString();
                info.WarnLevel2 = dr["WARNLEVEL2"].ToString();
                info.WarnLevel3 = dr["WARNLEVEL3"].ToString();

                Result.Add(info);
            }
        }

        Logger.Instance.WriteLog("按ID升序检索所有的客流区域数据。检索件数：" + Result.Count);
    }


    /// <summary>
    /// 检索所有数据按降序排列
    /// </summary>
    public void Select002()
    {
        Result = new List<PassengerFlowAreaInfo>();
        Result.Clear();
        string sql = "select ID,NAME,CAMERAIDLST,POINTS,WARNLEVEL1,WARNLEVEL2,WARNLEVEL3 from CYGJ_PASSENGERFLOW_AREA order by ID desc";
        DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, "CYGJ_PASSENGERFLOW_AREA");
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                PassengerFlowAreaInfo info = new PassengerFlowAreaInfo();
                info.Id = dr["ID"].ToString();
                info.Name = dr["NAME"].ToString();
                info.CameraIdLst = dr["CAMERAIDLST"].ToString();
                info.Points = dr["POINTS"].ToString();
                info.WarnLevel1 = dr["WARNLEVEL1"].ToString();
                info.WarnLevel2 = dr["WARNLEVEL2"].ToString();
                info.WarnLevel3 = dr["WARNLEVEL3"].ToString();
                Result.Add(info);
            }
        }
        Logger.Instance.WriteLog("按ID降序检索所有的客流区域数据。检索件数：" + Result.Count);
    }


    /// <summary>
    /// 从客流信息log表中查找大于或等于 datetime 的最新数据
    /// </summary>
    /// <param name="datetime">Datetime.</param>
    public List<PassengerFlowInfoLog> Select003(string datetime)
    {
        List<PassengerFlowInfoLog> _Result = new List<PassengerFlowInfoLog>();
        string sql = "select ID,DATETIME,PASSENGERFLOWURL,SUMCOUNT,ENTERCOUNT,EXITCOUNT from CYGJ_PASSENGERFLOW_LOG where DATETIME in (select max(DATETIME) from CYGJ_PASSENGERFLOW_LOG) and DATETIME >= '" + datetime + "'";
        DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, "CYGJ_PASSENGERFLOW_LOG");
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                PassengerFlowInfoLog info = new PassengerFlowInfoLog();
                info.Id = dr["ID"].ToString();
                info.Date = dr["DATETIME"].ToString();
                info.PassengerFlowUrl = dr["PASSENGERFLOWURL"].ToString();
                info.SumCount = dr["SUMCOUNT"].ToString();
                info.EnterCount = dr["ENTERCOUNT"].ToString();
                info.ExitCount = dr["EXITCOUNT"].ToString();
                _Result.Add(info);
            }
        }
        Logger.Instance.WriteLog("从客流信息log表中查找大于或等于 datetime 的最新数据。检索件数：" + _Result.Count + ",检索条件：DATETIME = " + datetime);
        return _Result;
    }


    /// <summary>
    /// 更新客流统计区域的坐标，缩放比例
    /// </summary>
    /// <param name="PosX">Position x.</param>
    /// <param name="PosY">Position y.</param>
    /// <param name="PosZ">Position z.</param>
    /// <param name="ScalseX">Scalse x.</param>
    /// <param name="ScaleY">Scale y.</param>
    /// <param name="ScaleZ">Scale z.</param>
    /// <param name="ID">I.</param>
    public void Update001(string Points, string ID)
    {
        string sql = "update CYGJ_PASSENGERFLOW_AREA set " +
                "POINTS = '" + Points +
                "' where ID = '" + ID + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("更新指定ID客流统计区域的坐标，缩放比例。更新件数：" + ret + ",更新条件：ID = " + ID);
    }


    /// <summary>
    /// 更新客流统计区域的名称
    /// </summary>
    /// <param name="Name">Name.</param>
    /// <param name="ID">I.</param>
    public void Update002(string Name, string ID)
    {
        string sql = "update CYGJ_PASSENGERFLOW_AREA set " +
            "NAME = '" + Name + "' where ID = '" + ID + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("更新指定ID客流统计区域的名称。更新件数：" + ret + ",更新条件：ID = " + ID);

    }


    /// <summary>
    /// 更新绑定的摄像机列表
    /// </summary>
    /// <param name="CameraIdLst">Camera identifier lst.</param>
    /// <param name="ID">I.</param>
    public void Update003(string CameraIdLst, string ID)
    {
        string sql = "update CYGJ_PASSENGERFLOW_AREA set " +
            "CAMERAIDLST = '" + CameraIdLst + "' where ID = '" + ID + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("更新指定ID客流统计区域的绑定的摄像机列表。更新件数：" + ret + ",更新条件：ID = " + ID);
    }


    /// <summary>
    /// 更新指定ID客流统计区域的个报警级别人数
    /// </summary>
    /// <param name="Level1">Level1.</param>
    /// <param name="Level2">Level2.</param>
    /// <param name="Level3">Level3.</param>
    /// <param name="ID">I.</param>
    public void Update004(string Level1, string Level2, string Level3, string ID)
    {
        string sql = "update CYGJ_PASSENGERFLOW_AREA set " +
                "WARNLEVEL1 = '" + Level1 +
                "',WARNLEVEL2 = '" + Level2 +
                "',WARNLEVEL3 = '" + Level3 +
                "' where ID = '" + ID + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("更新指定ID客流统计区域的个报警级别人数。更新件数：" + ret + ",更新条件：ID = " + ID);
    }

}
