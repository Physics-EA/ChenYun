using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 加载客流统计区域信息
/// 创建新的客流统计区域信息
/// 删除客流统计区域信息
/// </summary>
public class PassengerFlowAreaManage : MonoBehaviour
{

    /// <summary>
    /// 用来绘制区域预制体
    /// </summary>
    public GameObject PassengerFlowAreaPrefab;

    /// <summary>
    /// 客流统计区域信息列表
    /// </summary>
    public UIGrid PassengerFlowAreaLst;

    /// <summary>
    /// 用来显示或设置绑定设备窗口
    /// </summary>
    public GameObject DeviceListPanel;

    /// <summary>
    /// 用来显示或设置报警人数阈值的窗口
    /// </summary>
    public GameObject WarnLevelPanel;

    /// <summary>
    /// 用来显示客流信息的窗口预制体
    /// </summary>
    public GameObject PassengerFlowAreaUIPrefab;

    public GameObject PassengerFlowAreaUIRoot;

    /// <summary>
    /// 存放所有摄像头的详细信息
    /// </summary>
    private List<DeviceInfo> MornitorInfos;

    /// <summary>
    /// 用来保存所有区域的名称
    /// </summary>
    private List<string> PassnegerFlowAreaNameList;

    /// <summary>
    /// 用来保存所有区的信息
    /// </summary>
    private List<PassengerFlowAreaInfo> PassengerFlowAreaInfos;

    /// <summary>
    /// 用来保存被选中的项目
    /// </summary>
    /// 

    [HideInInspector]
    public GameObject SelectedItem;

    void Awake()
    {
        PassnegerFlowAreaNameList = new List<string>();
    }

    bool isLoaded = false;

    void OnEnable()
    {
        if (isLoaded) return;
        isLoaded = true;
        StartCoroutine("LoadDeviceInfoRecord");
        StartCoroutine("LoadPassnegerFlowAreaRecord");
    }


    /// <summary>
    /// 从数据库加载摄像头的信息
    /// </summary>
    /// <returns>The device info record.</returns>
    IEnumerator LoadDeviceInfoRecord()
    {
        Logger.Instance.WriteLog("从数据库加载摄像头的信息");
        DeviceDao dDao = new DeviceDao();
        dDao.Select001();
        MornitorInfos = dDao.Result;
        yield return null;
    }



    /// <summary>
    /// 从数据库加载巡逻方案的数据
    /// </summary>
    /// <returns>The video patrol plan record.</returns>
    IEnumerator LoadPassnegerFlowAreaRecord()
    {
        yield return new WaitForEndOfFrame();
        Logger.Instance.WriteLog("从数据库加载巡逻方案的数据");
        PassengerFlowAreaLst.gameObject.GetComponent<UIWidget>().UpdateAnchors();
        PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao();
        pfaDao.Select001();
        PassengerFlowAreaInfos = pfaDao.Result;
        PassnegerFlowAreaNameList.Clear();
        foreach (PassengerFlowAreaInfo info in PassengerFlowAreaInfos)
        {
            Logger.Instance.WriteLog("创建客流统计区域列表项目");
            PassnegerFlowAreaNameList.Add(info.Name);
            GameObject go = Instantiate(PassengerFlowAreaPrefab) as GameObject;
            PassengerFlowAreaLst.AddChild(go.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.GetComponent<PassengerFlowAreaListItem>().PassengerFlowAreaUIRoot = PassengerFlowAreaUIRoot;
            go.GetComponent<PassengerFlowAreaListItem>().SetValue(info, this, DeviceListPanel, WarnLevelPanel, Instantiate(PassengerFlowAreaUIPrefab) as GameObject);
        }
        yield return null;
    }


    /// <summary>
    /// 删除选定的客流统计区域
    /// </summary>
    public void DeletePassnegerFlowArea()
    {
        Logger.Instance.WriteLog("删除选定的客流统计区域");
        if (Configure.IsOperating)
        {
            Logger.Instance.WriteLog("正在执行其它操作");
            return;
        }
        if (SelectedItem == null) return;
        if (PassengerFlowAreaInfos.Count <= 0) return;
        string AreaName = SelectedItem.GetComponentInChildren<UIInput>().value;
        int AreafosIndex = FindPassengerFlowAreaInfoIndex(AreaName);
        PassengerFlowAreaInfo info = PassengerFlowAreaInfos[AreafosIndex];
        PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao();
        pfaDao.Delete001(info.Id);
        PassengerFlowAreaInfos.RemoveAt(AreafosIndex);
        PassnegerFlowAreaNameList.Remove(AreaName);
        PassengerFlowAreaLst.RemoveChild(SelectedItem.transform);
        Destroy(SelectedItem);
        SelectedItem = null;
        //更新列表
        PassengerFlowAreaLst.GetComponent<UIWidget>().enabled = false;
        PassengerFlowAreaLst.GetComponent<UIWidget>().enabled = true;
    }


    /// <summary>
    /// 创建新的客流统计区域
    /// </summary>
    private bool isDrawingArea = false;

    public void UpdatePassengerFlowArea(PassengerFlowAreaListItem item)
    {
        if (isDrawingArea) return;
        Logger.Instance.WriteLog("开始绘制客流统计区域");
        isDrawingArea = true;
        CameraController.CanMoveable = false;
        gameObject.GetComponent<MeasurementTools>().pfaItem = item;
        gameObject.GetComponent<MeasurementTools>().enabled = true;
    }

    public void FinishedUpdatePassengerFlowArea()
    {
        Logger.Instance.WriteLog("绘制完成客流统计区域");
        isDrawingArea = false;
        CameraController.CanMoveable = true;
        gameObject.GetComponent<MeasurementTools>().enabled = false;
        gameObject.GetComponent<MeasurementTools>().pfaItem = null;
    }

    public void AddPassnegerFlowArea()
    {
        Logger.Instance.WriteLog("增加客流统计区域");
        if (Configure.IsOperating)
        {
            Logger.Instance.WriteLog("真正执行其它操作");
            return;
        }
        _AddPassnegerFlowArea(" ", null);
    }
    /// <summary>
    /// 保存新创建的客流区域
    /// </summary>
    public void _AddPassnegerFlowArea(string points, GameObject area)
    {
        PassengerFlowAreaInfo info = new PassengerFlowAreaInfo();
        int i = 1;
        while (PassnegerFlowAreaNameList.Contains("新建客流统计区域" + i))
        {
            i++;
        }
        info.Name = "新建客流统计区域" + i;
        info.CameraIdLst = "";
        PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao();
        Logger.Instance.WriteLog("保存新建客流统计区域信息");
        pfaDao.Insert001(info.Name, info.CameraIdLst, points);
        Logger.Instance.WriteLog("检索最新的客流统计区域信息");
        pfaDao.Select002();
        if (pfaDao.Result.Count <= 0 || pfaDao.Result[0].Name != info.Name) return;
        info = pfaDao.Result[0];
        PassengerFlowAreaInfos.Add(info);
        PassnegerFlowAreaNameList.Add(info.Name);
        GameObject go = Instantiate(PassengerFlowAreaPrefab) as GameObject;
        PassengerFlowAreaLst.AddChild(go.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.GetComponent<PassengerFlowAreaListItem>().PassengerFlowAreaUIRoot = PassengerFlowAreaUIRoot;
        go.GetComponent<PassengerFlowAreaListItem>().SetValue(info, this, area, DeviceListPanel, WarnLevelPanel, Instantiate(PassengerFlowAreaUIPrefab) as GameObject);
    }
    private string STRING(float obj)
    {
        return obj.ToString();
    }
    /// <summary>
    /// 根据区域名称查找详细的信息
    /// </summary>
    /// <returns>The Passenger Flow Area info.</returns>
    /// <param name="planName">Plan name.</param>
    private PassengerFlowAreaInfo FindPassengerFlowAreaInfo(string planName)
    {
        return PassengerFlowAreaInfos[FindPassengerFlowAreaInfoIndex(planName)];
    }
    /// <summary>
    /// 根据区域名称查找所在保存列表中的索引
    /// </summary>
    /// <returns>The video patrol plan info index.</returns>
    /// <param name="planName">Plan name.</param>
    public int FindPassengerFlowAreaInfoIndex(string planName)
    {
        int index = -1; ;
        for (int i = 0; i < PassengerFlowAreaInfos.Count; i++)
        {
            if (planName == PassengerFlowAreaInfos[i].Name)
            {
                index = i;
                break;
            }
        }
        return index;
    }
    /// <summary>
    /// 将名为oldName的区域的名称改成newName
    /// </summary>
    /// <param name="oldName">OldName.</param>
    /// <param name="newName">NewName.</param>
    public void UpdateAreaName(string oldName, string newName)
    {
        int index = FindPassengerFlowAreaInfoIndex(oldName);
        PassengerFlowAreaInfo info = PassengerFlowAreaInfos[index];
        info.Name = newName;
        PassengerFlowAreaInfos[index] = info;
    }
}
