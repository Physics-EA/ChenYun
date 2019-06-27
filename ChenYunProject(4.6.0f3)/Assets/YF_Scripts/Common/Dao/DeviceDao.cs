using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;


/// <summary>
/// 设备信息结构
/// </summary>
public struct DeviceInfo
{
    /// <summary>
    /// 设备ID
    /// </summary>
    public string Id;
    /// <summary>
    /// 设备GUID.
    /// </summary>
    public string Guid;
    /// <summary>
    /// 设备名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 设备描述
    /// </summary>
    public string Description;
    /// <summary>
    /// 设备状态
    /// </summary>
    public string Status;
    /// <summary>
    /// 设备在场景中的世界坐标X轴的值
    /// </summary>
    public string PosX;
    /// <summary>
    /// 设备在场景中的世界坐标Y轴的值
    /// </summary>
    public string PosY;
    /// <summary>
    /// 设备在场景中的世界坐标Z轴的值
    /// </summary>
    public string PosZ;
    /// <summary>
    /// 场景中摄像机模型的标示
    /// </summary>
    public string CameraTag;
    /// <summary>
    ///  自定义的辅助点用来可控制camera的位置及朝向，聚焦到摄像机模型时的x坐标
    /// </summary>
    public string RotatePointPosX;
    /// <summary>
    ///  自定义的辅助点用来可控制camera的位置及朝向，聚焦到摄像机模型时的y坐标
    /// </summary>
    public string RotatePointPosY;
    /// <summary>
    ///  自定义的辅助点用来可控制camera的位置及朝向，聚焦到摄像机模型时的z坐标
    /// </summary>
    public string RotatePointPosZ;
    /// <summary>
    /// unity Camera 聚焦到摄像机模型时的x坐标
    /// </summary>
    public string CameraPosX;
    /// <summary>
    /// unity Camera 聚焦到摄像机模型时的y坐标
    /// </summary>
    public string CameraPosY;
    /// <summary>
    /// unity Camera 聚焦到摄像机模型时的z坐标
    /// </summary>
    public string CameraPosZ;
    public string CameraRotatX;
    public string CameraRotatY;
    public string CameraRotatZ;

    public string MonitorRadio;
    public string MonitorScope;
    public string MonitorOffset;

    /// <summary>
    /// The RTSP URL.
    /// </summary>
    public string RTSPUrl;

    /// <summary>
    /// 是否使用rtsp打开视频
    /// </summary>
    public bool UseRTSP;


    /// <summary>
    /// 用来存客流Url
    /// </summary>
    public string PassengerFlowUrl;

}

/// <summary>
/// 对设备表进行相关的操作
/// </summary>
/// 
public class DeviceDao
{

    /// <summary>
    /// 检索数据时的返回值
    /// </summary>
    public List<DeviceInfo> Result;


    public DeviceDao()
    {
        Result = new List<DeviceInfo>();
    }


    /// <summary>
    /// 检索所有的数据
    /// </summary>
    public void Select001()
    {
        Result.Clear();
        string sql = "select ID,GUID,NAME,DESCRIPTION,STATUS,POSX,POSY,POSZ,CAMERATAG,ROTATEPOINTPOSX,ROTATEPOINTPOSY," +
            "ROTATEPOINTPOSZ,CAMERAPOSX,CAMERAPOSY,CAMERAPOSZ,CAMERAROTATEX,CAMERAROTATEY,CAMERAROTATEZ,MONITORRADIO,MONITORSCOPE,OFFSETANGLE,RTSPURL,USERTSP,PASSENGERFLOWURL from CYGJ_DEVICE";
        DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, "CYGJ_DEVICE");
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            DeviceInfo info;
            foreach (DataRow dr in dt.Rows)
            {
                info = new DeviceInfo();
                info.Id = dr["ID"].ToString();
                info.Guid = dr["GUID"].ToString();
                info.Name = dr["NAME"].ToString();
                info.Description = dr["DESCRIPTION"].ToString();
                info.Status = dr["STATUS"].ToString();
                info.PosX = dr["POSX"].ToString();
                info.PosY = dr["POSY"].ToString();
                info.PosZ = dr["POSZ"].ToString();
                info.CameraTag = dr["CAMERATAG"].ToString();
                info.RotatePointPosX = dr["ROTATEPOINTPOSX"].ToString();
                info.RotatePointPosY = dr["ROTATEPOINTPOSY"].ToString();
                info.RotatePointPosZ = dr["ROTATEPOINTPOSZ"].ToString();
                info.CameraPosX = dr["CAMERAPOSX"].ToString();
                info.CameraPosY = dr["CAMERAPOSY"].ToString();
                info.CameraPosZ = dr["CAMERAPOSZ"].ToString();
                info.CameraRotatX = dr["CAMERAROTATEX"].ToString();
                info.CameraRotatY = dr["CAMERAROTATEY"].ToString();
                info.CameraRotatZ = dr["CAMERAROTATEZ"].ToString();
                info.MonitorRadio = dr["MONITORRADIO"].ToString();
                info.MonitorScope = dr["MONITORSCOPE"].ToString();
                info.MonitorOffset = dr["OFFSETANGLE"].ToString();
                info.RTSPUrl = dr["RTSPURL"].ToString();
                info.UseRTSP = dr["USERTSP"].ToString() == "1" ? true : false;
                info.PassengerFlowUrl = dr["PASSENGERFLOWURL"].ToString();
                Result.Add(info);
            }
        }
        Logger.Instance.WriteLog("向数据库检索所有设备数据");
    }


    /// <summary>
    /// 检索跟参数匹配的数据，此参数在数据库中是唯一的。
    /// </summary>
    /// <param name="CameraTag">Camera tag.</param>
    public void Select002(string CameraTag)
    {
        Result.Clear();
        string sql = "select ID,GUID,NAME,DESCRIPTION,STATUS,POSX,POSY,POSZ,CAMERATAG,ROTATEPOINTPOSX,ROTATEPOINTPOSY," +
            "ROTATEPOINTPOSZ,CAMERAPOSX,CAMERAPOSY,CAMERAPOSZ,CAMERAROTATEX,CAMERAROTATEY,CAMERAROTATEZ,MONITORRADIO,MONITORSCOPE,OFFSETANGLE,RTSPURL,USERTSP,PASSENGERFLOWURL from CYGJ_DEVICE where CAMERATAG = '" + CameraTag + "'";
        DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, "CYGJ_DEVICE");
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            DeviceInfo info;
            foreach (DataRow dr in dt.Rows)
            {
                info = new DeviceInfo();
                info.Id = dr["ID"].ToString();
                info.Guid = dr["GUID"].ToString();
                info.Name = dr["NAME"].ToString();
                info.Description = dr["DESCRIPTION"].ToString();
                info.Status = dr["STATUS"].ToString();
                info.PosX = dr["POSX"].ToString();
                info.PosY = dr["POSY"].ToString();
                info.PosZ = dr["POSZ"].ToString();
                info.CameraTag = dr["CAMERATAG"].ToString();
                info.RotatePointPosX = dr["ROTATEPOINTPOSX"].ToString();
                info.RotatePointPosY = dr["ROTATEPOINTPOSY"].ToString();
                info.RotatePointPosZ = dr["ROTATEPOINTPOSZ"].ToString();
                info.CameraPosX = dr["CAMERAPOSX"].ToString();
                info.CameraPosY = dr["CAMERAPOSY"].ToString();
                info.CameraPosZ = dr["CAMERAPOSZ"].ToString();
                info.CameraRotatX = dr["CAMERAROTATEX"].ToString();
                info.CameraRotatY = dr["CAMERAROTATEY"].ToString();
                info.CameraRotatZ = dr["CAMERAROTATEZ"].ToString();
                info.MonitorRadio = dr["MONITORRADIO"].ToString();
                info.MonitorScope = dr["MONITORSCOPE"].ToString();
                info.MonitorOffset = dr["OFFSETANGLE"].ToString();
                info.RTSPUrl = dr["RTSPURL"].ToString();
                info.UseRTSP = dr["USERTSP"].ToString() == "1" ? true : false;
                info.PassengerFlowUrl = dr["PASSENGERFLOWURL"].ToString();
                Result.Add(info);
            }
        }
        Logger.Instance.WriteLog("向数据库检索指定设备信息，检索条件：CAMERATAG = " + CameraTag);
    }
    /// <summary>
    /// 检索跟参数匹配的数据，此参数在数据库中是唯一的。
    /// </summary>
    /// <param name="CameraId">Camera identifier.</param>
    public void Select003(string CameraId)
    {
        Result.Clear();
        string sql = "select ID,GUID,NAME,DESCRIPTION,STATUS,POSX,POSY,POSZ,CAMERATAG,ROTATEPOINTPOSX,ROTATEPOINTPOSY," +
            "ROTATEPOINTPOSZ,CAMERAPOSX,CAMERAPOSY,CAMERAPOSZ,CAMERAROTATEX,CAMERAROTATEY,CAMERAROTATEZ,MONITORRADIO,MONITORSCOPE,OFFSETANGLE,RTSPURL,USERTSP,PASSENGERFLOWURL from CYGJ_DEVICE where ID = '" + CameraId + "'";
        DataSet ds = OdbcDataManager.Instance.odbcOra.ReturnDataSet(sql, "CYGJ_DEVICE");
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            DeviceInfo info;
            foreach (DataRow dr in dt.Rows)
            {
                info = new DeviceInfo();
                info.Id = dr["ID"].ToString();
                info.Guid = dr["GUID"].ToString();
                info.Name = dr["NAME"].ToString();
                info.Description = dr["DESCRIPTION"].ToString();
                info.Status = dr["STATUS"].ToString();
                info.PosX = dr["POSX"].ToString();
                info.PosY = dr["POSY"].ToString();
                info.PosZ = dr["POSZ"].ToString();
                info.CameraTag = dr["CAMERATAG"].ToString();
                info.RotatePointPosX = dr["ROTATEPOINTPOSX"].ToString();
                info.RotatePointPosY = dr["ROTATEPOINTPOSY"].ToString();
                info.RotatePointPosZ = dr["ROTATEPOINTPOSZ"].ToString();
                info.CameraPosX = dr["CAMERAPOSX"].ToString();
                info.CameraPosY = dr["CAMERAPOSY"].ToString();
                info.CameraPosZ = dr["CAMERAPOSZ"].ToString();
                info.CameraRotatX = dr["CAMERAROTATEX"].ToString();
                info.CameraRotatY = dr["CAMERAROTATEY"].ToString();
                info.CameraRotatZ = dr["CAMERAROTATEZ"].ToString();
                info.MonitorRadio = dr["MONITORRADIO"].ToString();
                info.MonitorScope = dr["MONITORSCOPE"].ToString();
                info.MonitorOffset = dr["OFFSETANGLE"].ToString();
                info.RTSPUrl = dr["RTSPURL"].ToString();
                info.UseRTSP = dr["USERTSP"].ToString() == "1" ? true : false;
                info.PassengerFlowUrl = dr["PASSENGERFLOWURL"].ToString();
                Result.Add(info);
            }
        }
        Logger.Instance.WriteLog("向数据库检索指定设备信息，检索条件：ID = " + CameraId);
    }
    /// <summary>
    /// 插入基本摄像机信息数据
    /// </summary>
    /// <param name="guid">GUID.</param>
    /// <param name="name">Name.</param>
    /// <param name="descrition">Descrition.</param>
    /// <param name="posx">Posx.</param>
    /// <param name="posy">Posy.</param>
    /// <param name="posz">Posz.</param>
    /// <param name="cameraTag">Camera tag.</param>
    public void Insert001(string guid, string name, string descrition, string posx, string posy, string posz, string cameraTag)
    {
        string sql = "insert into CYGJ_DEVICE(ID,GUID,NAME,DESCRIPTION,STATUS,POSX,POSY,POSZ,CAMERATAG,ROTATEPOINTPOSX,ROTATEPOINTPOSY,ROTATEPOINTPOSZ,CAMERAPOSX,CAMERAPOSY,CAMERAPOSZ,CAMERAROTATEX,CAMERAROTATEY,CAMERAROTATEZ,MONITORRADIO,MONITORSCOPE,OFFSETANGLE) values " +
            "(SEQ_CYGJ_DEVICE_ID.nextval,'" + guid + "','" + name + "','" + descrition + "','正常','" + posx + "','" + posy + "','" + posz + "','" + cameraTag + "',' ',' ',' ',' ',' ',' ',' ',' ',' ','0','0','0')";
        if (OdbcDataManager.Instance.DataBaseType == "MySQL")
        {
            sql = "insert into CYGJ_DEVICE(GUID,NAME,DESCRIPTION,STATUS,POSX,POSY,POSZ,CAMERATAG,ROTATEPOINTPOSX,ROTATEPOINTPOSY,ROTATEPOINTPOSZ,CAMERAPOSX,CAMERAPOSY,CAMERAPOSZ,CAMERAROTATEX,CAMERAROTATEY,CAMERAROTATEZ,MONITORRADIO,MONITORSCOPE,OFFSETANGLE) values " +
                "('" + guid + "','" + name + "','" + descrition + "','正常','" + posx + "','" + posy + "','" + posz + "','" + cameraTag + "',' ',' ',' ',' ',' ',' ',' ',' ',' ','0','0','0')";

        }
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("向设备表插入" + ret + "条记录");
    }
    /// <summary>
    /// 更新额外的摄像机信息数据，id为检索条件
    /// </summary>
    /// <param name="RotatePointPosX">Rotate point position x.</param>
    /// <param name="RotatePointPosY">Rotate point position y.</param>
    /// <param name="RotatePointPosZ">Rotate point position z.</param>
    /// <param name="CameraPosX">Camera position x.</param>
    /// <param name="CameraPosY">Camera position y.</param>
    /// <param name="CameraPosZ">Camera position z.</param>
    /// <param name="CameraRotatX">Camera rotat x.</param>
    /// <param name="CameraRotatY">Camera rotat y.</param>
    /// <param name="CameraRotatZ">Camera rotat z.</param>
    /// <param name="id">Identifier.</param>
    public void Update001(float RotatePointPosX, float RotatePointPosY, float RotatePointPosZ, float CameraPosX, float CameraPosY,
                          float CameraPosZ, float CameraRotatX, float CameraRotatY, float CameraRotatZ, string id)
    {
        string sql = "update CYGJ_DEVICE set ROTATEPOINTPOSX = '" + RotatePointPosX + "',"
            + "ROTATEPOINTPOSY = '" + RotatePointPosY + "',"
                + "ROTATEPOINTPOSZ = '" + RotatePointPosZ + "',"
                + "CAMERAPOSX = '" + CameraPosX + "',"
                + "CAMERAPOSY = '" + CameraPosY + "',"
                + "CAMERAPOSZ = '" + CameraPosZ + "',"
                + "CAMERAROTATEX = '" + CameraRotatX + "',"
                + "CAMERAROTATEY = '" + CameraRotatY + "',"
                + "CAMERAROTATEZ = '" + CameraRotatZ + "'"
                + " where ID = '" + id + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("根据传入的ID更新额外的摄像机信息数据。向设备表更新" + ret + "条记录,更新条件：ID = " + id);
    }
    /// <summary>
    /// 根据传入的ID修改摄像机的GUID，名称，描述信息
    /// </summary>
    /// <param name="guid">GUID.</param>
    /// <param name="name">Name.</param>
    /// <param name="descrition">Descrition.</param>
    /// <param name="id">Identifier.</param>
    public void Update002(string guid, string name, string description, string id)
    {
        string sql = "update CYGJ_DEVICE set GUID = '" + guid + "',"
                + "NAME = '" + name + "',"
                + "DESCRIPTION = '" + description + "'"
                + " where ID = '" + id + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("根据传入的ID修改摄像机的GUID，名称，描述信息。向设备表更新" + ret + "条记录,更新条件：ID = " + id);
    }
    /// <summary>
    /// 根据传入的id修改摄像机描述信息，并将摄像机名称修改为描述信息
    /// </summary>
    /// <param name="description">Description.</param>
    /// <param name="id">Identifier.</param>
    public void Update003(string description, string id)
    {
        string sql = "update CYGJ_DEVICE set NAME = '" + description + "',"
                + "DESCRIPTION = '" + description + "'"
                + " where ID = '" + id + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("根据传入的id修改摄像机描述信息，并将摄像机名称修改为描述信息。向设备表更新" + ret + "条记录,更新条件：ID = " + id);
    }
    /// <summary>
    /// 更新指定ID记录的监控半径，监控范围，偏移角度
    /// </summary>
    /// <param name="radio">Radio.</param>
    /// <param name="scope">Scope.</param>
    /// <param name="angle">offset.</param>
    /// <param name="id">Identifier.</param>
    public void Update004(string radio, string scope, string offset, string id)
    {
        string sql = "update CYGJ_DEVICE set MONITORRADIO = '" + radio + "',"
            + "MONITORSCOPE = '" + scope + "',"
                + "OFFSETANGLE = '" + offset + "'"
                + " where ID = '" + id + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("更新指定ID记录的监控半径，监控范围，偏移角度。向设备表更新" + ret + "条记录,更新条件：ID = " + id);
    }

    /// <summary>
    /// 更新指定ID设备的RTSP地址，并制定是否启用
    /// </summary>
    /// <param name="url">url.</param>
    /// <param name="useRtsp">useRtsp.</param>
    /// <param name="id">Identifier.</param>
    public void Update005(string url, string useRtsp, string pfUrl, string id)
    {
        string sql = "update CYGJ_DEVICE set RTSPURL = '" + url + "',"
            + "USERTSP = '" + useRtsp + "',"
            + "PASSENGERFLOWURL = '" + pfUrl + "'"
            + " where ID = '" + id + "'";
        int ret = OdbcDataManager.Instance.odbcOra.ExecuteNonQuery(sql);
        Logger.Instance.WriteLog("更新指定ID设备的RTSP地址，并制定是否启用。向设备表更新" + ret + "条记录,更新条件：ID = " + id);
    }
}
