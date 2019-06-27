using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 声明一个委托
/// </summary>
/// <param name="success"></param>
public delegate void VoidDelegate(bool success);


/// <summary>
/// 服务器客流信息...?
/// </summary>
public class CMSManage : MonoBehaviour
{
    public static CMSManage Instance;

    /// <summary>
    /// 定义一个client端
    /// </summary>
    private NetworkClient client;

    /// <summary>
    /// 标记登入成功
    /// </summary>
    private bool loginSuccess;

    /// <summary>
    /// 保存所有已经打开的摄像机
    /// <摄像机ID，已经打开的个数>
    /// </summary>
    private Dictionary<int, int> OpenCameraList;

    /// <summary>
    /// 保存所有请求打开摄像机请求方的回调函数
    /// <请求打开摄像机的ID，所有请求方的回调函数列表>
    /// </summary>
    private Dictionary<int, List<VoidDelegate>> OpenCameraRequesters;


    // Use this for initialization
    void Start()
    {
        Instance = this;
        client = NetworkClient.Instance;
        OpenCameraList = new Dictionary<int, int>();
        OpenCameraRequesters = new Dictionary<int, List<VoidDelegate>>();
        StartCoroutine("ConnectService");
    }

    // Update is called once per frame
    void Update()
    {
        if (client.isConnect)
        {
            client.NetUpdate();
        }

        if (client.reconnect)
        {
            client.reconnect = false;
            client.LogoffCMS();
            bool ret = client.loginToCMS(sys_info.CMS_loginName, sys_info.CMS_loginPassword, sys_info.CMS_ip, sys_info.CMS_port, sys_info.CMS_Protocol, LoginCallback);
            if (ret)
            {
                Logger.Instance.WriteLog("重新请求登陆监控服务器成功");
            }
            else
            {
                Logger.Instance.WriteLog("重新请求登陆监控服务器失败");
            }
        }
    }

    void OnDisable()
    {
        client.LogoffCMS();
        client.CloseConnect();
    }

    IEnumerator ConnectService()
    {

        bool isConnect = false;
        client.Connect();
        isConnect = client.isConnect;
        if (isConnect)
        {
            Logger.Instance.WriteLog("请求登陆监控服务器");
            bool ret = client.loginToCMS(sys_info.CMS_loginName, sys_info.CMS_loginPassword, sys_info.CMS_ip, sys_info.CMS_port, sys_info.CMS_Protocol, LoginCallback);
            if (ret)
            {
                Logger.Instance.WriteLog("请求登陆监控服务器成功");
            }
            else
            {
                Logger.Instance.WriteLog("请求登陆监控服务器失败");
            }
        }
        yield return null;
    }

    void LoginCallback(int camaerId, CMS_GUID guid, bool success)
    {
        if (success)
        {
            Logger.Instance.WriteLog("登陆监控服务器成功");
            Logger.Instance.WriteLog("获取物理摄像机列表");
            client.GetCameraList();
        }
        else
        {
            Logger.Instance.WriteLog("登陆监控服务器失败");
        }
    }

    public void OpenCamera(int cameraId, CMS_GUID guid, VoidDelegate callback)
    {
        Logger.Instance.WriteLog("请求打开摄像头");
        if (OpenCameraList.ContainsKey(cameraId))
        {
            OpenCameraList[cameraId]++;
            callback(true);
            Logger.Instance.WriteLog("相同摄像机已经打开，不用重新打开摄像机。CameraId:" + cameraId);
        }
        else
        {
            if (OpenCameraRequesters.ContainsKey(cameraId))
            {
                OpenCameraRequesters[cameraId].Add(callback);
            }
            else
            {
                List<VoidDelegate> value = new List<VoidDelegate>();
                value.Add(callback);
                OpenCameraRequesters.Add(cameraId, value);
            }
            bool ret = client.OpenCamera(cameraId, guid, OpenCameraCallback);
            if (ret)
            {
                Logger.Instance.WriteLog("请求打开摄像头成功");
            }
            else
            {
                Logger.Instance.WriteLog("请求打开摄像头失败, CameraId:" + cameraId);
            }
        }
    }

    void OpenCameraCallback(int camaerId, CMS_GUID guid, bool success)
    {
        foreach (VoidDelegate function in OpenCameraRequesters[camaerId])
        {
            function(success);
        }

        OpenCameraRequesters.Remove(camaerId);

        if (success)
        {
            OpenCameraList.Add(camaerId, 1);
            Logger.Instance.WriteLog("打开摄像头成功");
        }
        else
        {
            Logger.Instance.WriteLog("打开摄像头失败, CameraId:" + camaerId);
        }
    }

    public void CloseCamera(int cameraId, CMS_GUID guid)
    {
        Logger.Instance.WriteLog("请求关闭摄像头");
        if (OpenCameraList.ContainsKey(cameraId) && OpenCameraList[cameraId] > 1)
        {
            OpenCameraList[cameraId]--;
        }
        else
        {
            bool ret = client.CloseCamera(cameraId, guid, CloseCameraCallback);
            if (ret)
            {
                Logger.Instance.WriteLog("请求关闭摄像头成功");
            }
            else
            {
                Logger.Instance.WriteLog("请求关闭摄像头失败。CameraId:" + cameraId);
            }
        }
    }

    void CloseCameraCallback(int cameraId, CMS_GUID guid, bool success)
    {
        if (success)
        {
            OpenCameraList.Remove(cameraId);
            Logger.Instance.WriteLog("关闭摄像头成功");
        }
        else
        {
            Logger.Instance.WriteLog("关闭摄像头失败。CameraId:" + cameraId);
        }
    }

    public void SetPresetPosition(int cameraId, CMS_GUID guid, ushort presetPosindex)
    {
        Logger.Instance.WriteLog("请求设置预设位");
        bool ret = client.SetPresetPosition(cameraId, guid, presetPosindex, SetPresetPosCallback);
        if (ret)
        {
            Logger.Instance.WriteLog("请求设置预设位成功");
        }
        else
        {
            Logger.Instance.WriteLog("请求设置预设位失败。CamaeraId:" + cameraId);
        }
    }

    void SetPresetPosCallback(int cameraId, CMS_GUID guid, bool success)
    {
        if (success)
        {
            Logger.Instance.WriteLog("设置预设位成功");
        }
        else
        {
            Logger.Instance.WriteLog("设置预设位失败。CameraId:" + cameraId);
        }
    }

    public void GotoPresetPosition(int cameraId, CMS_GUID guid, ushort presetPosindex)
    {
        Logger.Instance.WriteLog("请求移动到预设位");
        bool ret = client.GotoPresetPosition(cameraId, guid, presetPosindex, GotoPresetPosCallback);
        if (ret)
        {
            Logger.Instance.WriteLog("请求移动到预设位成功");
        }
        else
        {
            Logger.Instance.WriteLog("请求移动到预设位失败。CameraId:" + cameraId);
        }
    }

    void GotoPresetPosCallback(int cameraId, CMS_GUID guid, bool success)
    {
        if (success)
        {
            Logger.Instance.WriteLog("移动到预设位成功");
        }
        else
        {
            Logger.Instance.WriteLog("移动到预设位失败。CameraId:" + cameraId);
        }
    }

    public void PTZCtl(int cameraId, CMS_GUID guid, PTZOperation opt)
    {
        Logger.Instance.WriteLog("请求PTZCtl");
        bool ret = client.SetPTZControl(cameraId, guid, opt, 1, 1, 1, 0, PTZCtlCallback);
        if (ret)
        {
            Logger.Instance.WriteLog("请求PTZCtl成功");
        }
        else
        {
            Logger.Instance.WriteLog("请求PTZCtl失败。CameraId:" + cameraId);
        }

    }

    void PTZCtlCallback(int cameraId, CMS_GUID guid, bool success)
    {
        if (success)
        {
            Logger.Instance.WriteLog("PTZCtl成功");
        }
        else
        {
            Logger.Instance.WriteLog("PTZCtl失败。CameraId:" + cameraId);
        }
    }

    public void GetPassengerFlow(DelPassengerFlow callback)
    {
        client.GetPassengerFlow(callback);
    }

    public bool isConnecting()
    {
        return client.isConnect;
    }

    public List<CAMARE_INFO> GetCameraInfo()
    {
        List<CAMARE_INFO> list = new List<CAMARE_INFO>();
        if (client.cameraList != null)
        {
            CAMARE_INFO[] info;
            info = client.cameraList.ToArray();
            list.AddRange(info);
            return list;
        }
        return null;
    }

    public bool HasPTZCtl(string guid)
    {
        if (client.cameraList != null)
        {
            for (int i = 0; i < client.cameraList.Count; i++)
            {
                if (guid == GUIDToString(client.cameraList[i].camareGuid))
                {
                    if (client.cameraList[i].bPTZ == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return false;
    }

    public static CMS_GUID StringToGUID(string guid)
    {
        string[] strs = guid.Split('-');
        CMS_GUID cmsGuid = new CMS_GUID();
        cmsGuid.Data1 = UInt32.Parse(strs[0]);
        cmsGuid.Data2 = ushort.Parse(strs[1]);
        cmsGuid.Data3 = ushort.Parse(strs[2]);
        cmsGuid.Data4 = new byte[8];
        cmsGuid.Data4[0] = byte.Parse(strs[3]);
        cmsGuid.Data4[1] = byte.Parse(strs[4]);
        cmsGuid.Data4[2] = byte.Parse(strs[5]);
        cmsGuid.Data4[3] = byte.Parse(strs[6]);
        cmsGuid.Data4[4] = byte.Parse(strs[7]);
        cmsGuid.Data4[5] = byte.Parse(strs[8]);
        cmsGuid.Data4[6] = byte.Parse(strs[9]);
        cmsGuid.Data4[7] = byte.Parse(strs[10]);
        return cmsGuid;
    }

    public static string GUIDToString(CMS_GUID guid)
    {
        return guid.Data1 + "-" + guid.Data2 + "-" + guid.Data3 + "-" + guid.Data4[0] +
                "-" + guid.Data4[1] + "-" + guid.Data4[2] + "-" + guid.Data4[3] +
                "-" + guid.Data4[4] + "-" + guid.Data4[5] + "-" + guid.Data4[6] + "-" + guid.Data4[7];
    }
}
