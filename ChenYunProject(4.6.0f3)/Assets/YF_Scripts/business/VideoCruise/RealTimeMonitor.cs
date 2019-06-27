using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public delegate void DelWriteLog(string camera, string startTime, string endTime, string memo);
public delegate void DelSavePicture(string name, byte[] image, bool rotateFlip);


/// <summary>
/// 实时监控
/// </summary>
public class RealTimeMonitor : MonoBehaviour
{
    public static RealTimeMonitor Instance;

    /// <summary>
    /// 摄像机列表项目预制体
    /// </summary>
    public GameObject MonitorListItemPrefab;

    /// <summary>
    /// 存放摄像机对象列表项目的组件
    /// </summary>
    public UIGrid MonitorList;

    /// <summary>
    /// 存储摄像机图标,已经对应的摄像机信息
    /// </summary>
    private Dictionary<GameObject, DeviceInfo> Monitors;

    /// <summary>
    /// 巡逻方案选择列表
    /// </summary>
    public GameObject PatrolPlanPopuplist;

    /// <summary>
    /// 启动巡逻方案的按钮
    /// </summary>
    public GameObject BtnStartPlan;

    /// <summary>
    /// 显示巡逻方案中指定视频的窗口
    /// </summary>
    public GameObject VideoPatrolWindow;

    /// <summary>
    /// 小地图面板
    /// </summary>
    public GameObject MapPanel;

    /// <summary>
    /// 摄像机监控范围预制体
    /// </summary>
    public GameObject MonitorScopePrefab;

    /// <summary>
    /// 场景摄像机父物体
    /// </summary>
    public Transform SceneCameras;

    /// <summary>
    /// 控制摄像机3D面板显示按钮文本
    /// </summary>
    //public UILabel CameraShowButton;

    /// <summary>
    /// 总客流面板
    /// </summary>
    public GameObject PassengerInfo;

    /// <summary>
    /// 总客流按钮
    /// </summary>
    public UIButton PassengerInfoButton;

    /// <summary>
    /// 总客流箭头图标
    /// </summary>
    public Transform PassengerInfoArrow;

    /// <summary>
    /// 显示所有摄像头面板
    /// </summary>
    public GameObject ShiPinSheBeiPanel;

    /// <summary>
    /// 视频设备按钮
    /// </summary>
    public UIButton ShiPinSheBeiBtn;

    /// <summary>
    /// 视频设备箭头图标
    /// </summary>
    public Transform ShiPinSheBeiArrow;

    /// <summary>
    /// 存放所有摄像头的详细信息
    /// </summary>
    private List<DeviceInfo> MornitorInfos;

    /// <summary>
    /// 存放视频巡逻方案信息
    /// </summary>
    private List<VideoPatrolPlanInfo> VideoPatrolPlanInfos;

    /// <summary>
    /// 读取摄像头信息，并创建对应的摄像机图标以及摄像头列表
    /// </summary>
    void Start()
    {
        Instance = this;
        StartCoroutine("LoadMonitorRecord");
        StartCoroutine("LoadVideoPatrolPlanRecord");
        ShiPinSheBeiPanel.GetComponent<UIPanel>().alpha = 0;
    }
    /// <summary>
    /// 加载摄像头的信息
    /// </summary>
    /// <returns>The monitor record.</returns>
    IEnumerator LoadMonitorRecord()
    {
        Logger.Instance.WriteLog("加载摄像头的信息");
        Monitors = new Dictionary<GameObject, DeviceInfo>();
        DeviceDao dDao = new DeviceDao();
        dDao.Select001();
        List<DeviceInfo> dInfos = dDao.Result;
        MornitorInfos = dDao.Result;

        GameObject MonitorListItem;
        GameObject[] MonitorObjs = GetGameObjectChild(SceneCameras);
        Dictionary<string, GameObject> DicMonitor = new Dictionary<string, GameObject>();
        foreach (GameObject go in MonitorObjs)
        {
            DicMonitor.Add(go.transform.GetChild(0).name, go);
        }
        GameObject MonitorIcon;
        for (int i = 0; i < dInfos.Count; i++)
        {
            //创建新的摄像头列表项目
            MonitorListItem = Instantiate(MonitorListItemPrefab) as GameObject;
            MonitorList.AddChild(MonitorListItem.transform);
            MonitorListItem.transform.localScale = new Vector3(1, 1, 1);

            MonitorListItem.GetComponent<MonitorListItem>().Init(dInfos[i].Id, dInfos[i].Description, dInfos[i]);

            MonitorIcon = DicMonitor[dInfos[i].CameraTag];
            Monitors.Add(MonitorIcon, dInfos[i]);
            MonitorIcon.GetComponent<MonitorInfoData>().Data = dInfos[i];
            MonitorIcon.GetComponent<MonitorIco>().BindMonitorListItem(MonitorListItem);
            MonitorListItem.GetComponent<MonitorListItem>().BindMonitorIcon(MonitorIcon);
        }
        yield return null;
    }

    private GameObject[] GetGameObjectChild(Transform tf)
    {
        GameObject[] child = new GameObject[tf.childCount];
        for (int i = 0; i < child.Length; i++)
        {
            child[i] = tf.GetChild(i).gameObject;
        }
        return child;
    }

    public void ReLoadVideoPatrolPlanRecord()
    {
        StartCoroutine("LoadVideoPatrolPlanRecord");
    }
    /// <summary>
    /// 从数据库加载巡逻方案的数据
    /// </summary>
    /// <returns>The video patrol plan record.</returns>
    IEnumerator LoadVideoPatrolPlanRecord()
    {
        Logger.Instance.WriteLog("从数据库加载巡逻方案的数据");
        VideoPatrolPlanDao vppDao = new VideoPatrolPlanDao();
        vppDao.Select001();
        VideoPatrolPlanInfos = vppDao.Result;
        UIPopupList PopuList = PatrolPlanPopuplist.GetComponent<UIPopupList>();
        PopuList.Clear();
        foreach (VideoPatrolPlanInfo info in VideoPatrolPlanInfos)
        {
            PopuList.AddItem(info.Name);
        }
        if (VideoPatrolPlanInfos.Count > 0)
        {
            PopuList.value = VideoPatrolPlanInfos[0].Name;
        }
        yield return null;
    }

    /// <summary>
    /// 存放当前需要启动巡逻方案的所有摄像头ID
    /// </summary>
    private string[] MonitorIdList;
    /// <summary>
    /// 存放当前需要启动巡逻方案的所有摄像头停留时间
    /// </summary>
    private string[] PlayTimeList;
    private string PatrolLogID;
    private string dirctoryPath;

    /// <summary>
    /// 开发使用自动跳过下一个按键的XML读取配置函数
    /// </summary>
    /// <param name="plan"></param>
    private void ConstomNext(PlayVideoPatrolPlan plan)
    {
        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        doc.Load(Application.dataPath + "/YF_Config/PatrolConfig.xml");
        System.Xml.XmlElement el = (System.Xml.XmlElement)doc.SelectSingleNode("config");
        string value = el.GetAttribute("next");
        if (value == "0")
        {
            plan.constomNext = false;
        }
        else
        {
            plan.constomNext = true;
        }
    }

    /// <summary>
    /// 运行当前选择的巡逻方案，巡逻方案从下拉列表中读取
    /// </summary>
    private string BtnStartPlanNormalSprite;
    public void RunPlan()
    {
        Logger.Instance.WriteLog("运行当前选择的巡逻方案");
        MainMenuController.canNotOpen = true;
        UIPopupList PopuList = PatrolPlanPopuplist.GetComponent<UIPopupList>();
        string planName = PopuList.value;
        VideoPatrolPlanInfo info = FindPlanInfo(planName);
        MonitorIdList = info.MonitorList.Split('|');
        PlayTimeList = info.PlayTimeList.Split('|');
        //显示播放窗口，并设置相关的回调函数
        VideoPatrolWindow.SetActive(true);
        PlayVideoPatrolPlan pvpPlan = VideoPatrolWindow.GetComponent<PlayVideoPatrolPlan>();
        pvpPlan.NextVideo = PlayNextMonitroVideo;
        pvpPlan.StopVideo = StopPlan;
        pvpPlan.WriteLog = WriteLog;
        pvpPlan.SavePicture = SavePicture;
        ConstomNext(pvpPlan);
        MapPanel.SetActive(true);
        MapPanel.GetComponent<DrawMap>().Draw(planName, MonitorIdList, true);
        nextVideoIndex = 0;
        PlayNextMonitroVideo();
        //巡逻方案开始后，禁用启动按钮和选择列表
        PatrolPlanPopuplist.GetComponent<BoxCollider>().enabled = false;
        BtnStartPlanNormalSprite = BtnStartPlan.GetComponent<UIButton>().normalSprite;
        BtnStartPlan.GetComponent<UIButton>().normalSprite = BtnStartPlan.GetComponent<UIButton>().pressedSprite;
        BtnStartPlan.GetComponent<BoxCollider>().enabled = false;

        VideoPatrolLogDao vplDao = new VideoPatrolLogDao();
        string time = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        vplDao.Insert001(DataStore.UserInfo.RealName, time, planName);
        PatrolLogID = vplDao.currentId;
        dirctoryPath = Application.dataPath + "/SaveImage/" + planName + System.DateTime.Parse(time).ToString("yyyyMMddHHmmss");
        Directory.CreateDirectory(dirctoryPath);
    }
    /// <summary>
    /// 把操作记录写到数据库中
    /// </summary>
    /// <param name="camera">Camera.</param>
    /// <param name="startTime">Start time.</param>
    /// <param name="endTime">End time.</param>
    /// <param name="memo">Memo.</param>
    public void WriteLog(string camera, string startTime, string endTime, string memo)
    {
        Logger.Instance.WriteLog("把视频巡逻方案操作记录写到数据库中");
        VideoPatrolDetailLogDao vpdlDao = new VideoPatrolDetailLogDao();
        vpdlDao.Insert001(camera, startTime, endTime, memo, PatrolLogID);

    }
    /// <summary>
    /// 将指定的图片保存到本地文件中
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="image">Image.</param>
    public void SavePicture(string name, byte[] image, bool rotateFlip = true)
    {
        Logger.Instance.WriteLog("保存视频巡逻图片");
        if (image == null || image.Length == 0)
        {
            Debug.LogError("image is null");
        }
        FileStream fs = File.Open(dirctoryPath + "/" + name + ".jpg", FileMode.OpenOrCreate);
        fs.Write(image, 0, image.Length);
        fs.Close();

        if (rotateFlip)
        {
            System.Drawing.Bitmap b = new System.Drawing.Bitmap(dirctoryPath + "/" + name + ".jpg");
            b.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
            b.Save(dirctoryPath + "/" + name + ".jpg");
        }


    }

    //下个需要播放的摄像头的索引
    int nextVideoIndex = 0;
    GameObject monitorScope;
    /// <summary>
    /// 播放下一个摄像头
    /// </summary>
    void PlayNextMonitroVideo()
    {
        Logger.Instance.WriteLog("播放下一个摄像监控");
        if (nextVideoIndex > 0)
        {
            GameObject lingxing = FindLingXing(FindDeviceInfo(MonitorIdList[nextVideoIndex - 1]).CameraTag);
            if (lingxing != null) lingxing.SetActive(false);
            MapPanel.GetComponent<DrawMap>().SetMapIconColor(FindDeviceInfo(MonitorIdList[nextVideoIndex - 1]).Id, Color.green);
        }

        if (monitorScope != null) Destroy(monitorScope);

        DeviceInfo dInfo = FindDeviceInfo(MonitorIdList[nextVideoIndex]);
        GameObject lingxing2 = FindLingXing(dInfo.CameraTag);
        if (lingxing2 != null) lingxing2.SetActive(true);
        MapPanel.GetComponent<DrawMap>().SetMapIconColor(dInfo.Id, Color.red, true);

        Transform trans = FindMonitor(dInfo.CameraTag).transform;
        monitorScope = Instantiate(MonitorScopePrefab,
                                   new Vector3(trans.position.x, 0.2f, trans.position.z),
                                   Quaternion.Euler(0, 0, 0)) as GameObject;
        DrawSector ds = monitorScope.GetComponent<DrawSector>();
        ds.Scope = int.Parse(dInfo.MonitorScope);
        ds.Radio = int.Parse(dInfo.MonitorRadio);
        ds.Offset = int.Parse(dInfo.MonitorOffset);
        ds.ReDrawSector();

        //判断是否已经是最后一个摄像头
        bool isLast = false;
        if (nextVideoIndex == MonitorIdList.Length - 1)
        {
            isLast = true;
        }
        //判断指定的摄像头是否存在，如果存在着播放此摄像头的视频
        if (MonitorIdList.Length > nextVideoIndex)
        {
            PlayMonitorVideo(MonitorIdList[nextVideoIndex], PlayTimeList[nextVideoIndex], isLast);
            nextVideoIndex++;
        }
        else
        {
            nextVideoIndex = 0;
            VideoPatrolWindow.SetActive(false);
        }
    }
    /// <summary>
    /// 播放指定的摄像头
    /// </summary>
    /// <param name="monitorId">摄像头Id</param>
    /// <param name="playTime">最少停留时间</param>
    /// <param name="isLast">If set to <c>true</c> is last.</param>
    private void PlayMonitorVideo(string monitorId, string playTime, bool isLast = false)
    {
        DeviceInfo info = FindDeviceInfo(monitorId);
        VideoPatrolWindow.GetComponent<PlayVideoPatrolPlan>().Play(info, playTime, isLast);
    }
    /// <summary>
    /// 回调函数，使启动按钮及巡逻方案选择下拉列表可用
    /// </summary>
    void StopPlan()
    {
        Logger.Instance.WriteLog("停止视频巡逻方案");
        PatrolPlanPopuplist.GetComponent<BoxCollider>().enabled = true;
        BtnStartPlan.GetComponent<BoxCollider>().enabled = true;
        BtnStartPlan.GetComponent<UIButton>().normalSprite = BtnStartPlanNormalSprite;
        if (monitorScope != null) Destroy(monitorScope);
        MapPanel.SetActive(false);

        if (nextVideoIndex > 0)
        {
            GameObject lingxing = FindLingXing(FindDeviceInfo(MonitorIdList[nextVideoIndex - 1]).CameraTag);
            if (lingxing != null) lingxing.SetActive(false);
        }
    }
    /// <summary>
    /// 查找指定的摄像机
    /// </summary>
    /// <returns>The monitor.</returns>
    /// <param name="cameraTag">Camera tag.</param>
    private GameObject FindMonitor(string cameraTag)
    {
        foreach (GameObject key in Monitors.Keys)
        {
            if (Monitors[key].CameraTag == cameraTag)
            {
                Transform tran = key.transform.FindChild(cameraTag);
                return tran != null ? tran.gameObject : null;
            }
        }
        return null;
    }
    /// <summary>
    /// 查找指定摄像机摄像机对应的菱形对象
    /// </summary>
    /// <returns>The ling xing.</returns>
    /// <param name="cameraTag">Camera tag.</param>
    private GameObject FindLingXing(string cameraTag)
    {
        foreach (GameObject key in Monitors.Keys)
        {
            if (Monitors[key].CameraTag == cameraTag)
            {
                Transform tran = key.transform.FindChild("lingxing");
                return tran != null ? tran.gameObject : null;
            }
        }
        return null;
    }
    /// <summary>
    /// 根据给定的摄像头ID，查找相对应的详细信息.
    /// </summary>
    /// <returns>相应的设备信息</returns>
    /// <param name="monitorId">摄像头ID</param>
    private DeviceInfo FindDeviceInfo(string monitorId)
    {
        DeviceInfo retval = new DeviceInfo();
        foreach (DeviceInfo info in MornitorInfos)
        {
            if (info.Id == monitorId)
            {
                retval = info;
                break;
            }
        }
        return retval;
    }
    /// <summary>
    /// 查找指定名称的巡逻方案的详细信息
    /// </summary>
    /// <returns>相应的巡逻方案信息/returns>
    /// <param name="planName">Plan name.</param>
    private VideoPatrolPlanInfo FindPlanInfo(string planName)
    {
        VideoPatrolPlanInfo retVal = new VideoPatrolPlanInfo();
        foreach (VideoPatrolPlanInfo info in VideoPatrolPlanInfos)
        {
            if (info.Name == planName)
            {
                retVal = info;
                break;
            }
        }

        return retVal;
    }

    /// <summary>
    /// 当脚本激活时显示所有的摄像机图标
    /// </summary>
    void OnEnable()
    {
        StartCoroutine("LoadVideoPatrolPlanRecord");
        if (Monitors == null) return;
        foreach (GameObject go in Monitors.Keys)
        {
            go.SetActive(true);
        }
    }
    /// <summary>
    /// 获取摄像头的名称
    /// </summary>
    /// <returns>The camera name.</returns>
    /// <param name="cameraRef">Camera reference.</param>
    public string GetCameraName(GameObject cameraRef)
    {
        Logger.Instance.WriteLog("获取摄像头的名称");
        return Monitors[cameraRef].Name;
    }

    /// <summary>
    /// 对场景内所有模型摄像机发送控制信息
    /// </summary>
    public void ShowCameraInfo()
    {
        Logger.Instance.WriteLog("对场景内所有模型摄像机发送控制信息");
        if (PassengerFlowInfoReceiver.showInfo)
        {
            PassengerFlowInfoReceiver.showInfo = false;
            //CameraShowButton.text = "显示摄像机客流";
        }
        else
        {
            PassengerFlowInfoReceiver.showInfo = true;
            //CameraShowButton.text = "关闭摄像机客流";
        }

        CameraUICtrl[] cuictrls = SceneCameras.GetComponentsInChildren<CameraUICtrl>();
        for (int i = 0; i < cuictrls.Length; i++)
        {
            cuictrls[i].ShowInfoSet(PassengerFlowInfoReceiver.showInfo);
        }
    }

    private string PInfoBtnNormalSprite;
    public void PassengerInfoBtClick()
    {
        if (PassengerInfo.activeSelf)
        {
            Logger.Instance.WriteLog("关闭客流");
            PassengerInfoButton.normalSprite = PInfoBtnNormalSprite;
            PassengerInfo.SetActive(false);
            PassengerInfoArrow.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Logger.Instance.WriteLog("显示客流");
            PInfoBtnNormalSprite = PassengerInfoButton.normalSprite;
            PassengerInfoButton.normalSprite = PassengerInfoButton.pressedSprite;
            PassengerInfo.SetActive(true);
            PassengerInfoArrow.localRotation = Quaternion.Euler(0, 0, 180);
        }
        ShowCameraInfo();
    }

    private bool CameraListWindowOpend = false;
    private string ShiPinSheBeiBtnNormalSprite;
    /// <summary>
    /// 打开视频设备窗口
    /// </summary>
    public void OpenCameraListWindow()
    {
        if (CameraListWindowOpend)
        {
            //关闭下拉列表
            ShiPinSheBeiPanel.GetComponent<TweenAlpha>().PlayReverse();
            ShiPinSheBeiBtn.normalSprite = ShiPinSheBeiBtnNormalSprite;
            ShiPinSheBeiArrow.localRotation = Quaternion.Euler(0, 0, 0);
            VideoPatrolWindow.GetComponent<PlayVideoPatrolPlan>().AdjustmentPos(false);
        }
        else
        {
            //打开下拉列表
            ShiPinSheBeiPanel.GetComponent<TweenAlpha>().PlayForward();
            ShiPinSheBeiBtnNormalSprite = ShiPinSheBeiBtn.normalSprite;
            ShiPinSheBeiBtn.normalSprite = ShiPinSheBeiBtn.pressedSprite;
            ShiPinSheBeiArrow.localRotation = Quaternion.Euler(0, 0, 180);
            VideoPatrolWindow.GetComponent<PlayVideoPatrolPlan>().AdjustmentPos(true);
        }
        CameraListWindowOpend = !CameraListWindowOpend;
    }
}
