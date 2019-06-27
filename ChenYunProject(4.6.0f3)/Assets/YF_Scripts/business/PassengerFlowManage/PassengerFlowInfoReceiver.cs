using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// PassengerFlowData是一个结构体，用来存放摄像机上面的信息：通道，总人数，进入人数，出去人数，IP地址
/// </summary>
public struct PassengerFlowData
{
    /// <summary>
    /// 摄像机的编号
    /// </summary>
    public int channel;

    /// <summary>
    /// 总人数
    /// </summary>
    public int SumCount;

    /// <summary>
    /// 进入的人数
    /// </summary>
    public int EnterCount;

    /// <summary>
    /// 出去的人数
    /// </summary>
    public int ExitCount;

    /// <summary>
    /// 服务器IP
    /// </summary>
    public string ipFrom;
};


/// <summary>
/// 处理客流统计信息相关的问题
/// </summary>
public class PassengerFlowInfoReceiver : MonoBehaviour
{

    //public GameObject PassengerFlowAreaPrefab;

    /// <summary>
    /// 用来存储 passengerAreaUIitem
    /// </summary>
    public GameObject PassengerFlowInfoShow;

    /// <summary>
    /// 用来存储areaUI
    /// </summary>
    public Transform PassengerFlowInfoShowParent;

    /// <summary>
    /// 用来存储人流信息panel控制面板
    /// </summary>
    public GameObject PassengerInfo;

    /// <summary>
    /// 用来存储PassengerFlowAreaInfo类型的集合，
    /// 表示有哪几块区域
    /// </summary>
    private List<PassengerFlowAreaInfo> infoLst;

    /// <summary>
    /// 用来标记是否显示UI信息
    /// </summary>
    public static bool showInfo = false;

    /// <summary>
    /// 用来存放绿色材质
    /// </summary>
    public Material mat_green;

    /// <summary>
    /// 用来存放黄色材质
    /// </summary>
    public Material mat_yellow;

    /// <summary>
    /// 用来存放橘色材质
    /// </summary>
    public Material mat_orange;

    /// <summary>
    /// 用来存放红色材质
    /// </summary>
    public Material mat_red;


    void Start()
    {
        StartCoroutine("LoadPassengerFlowArea");
        StartCoroutine("BindPassengerFlowInfoToCamera");
        StartCoroutine("GetPassengerFlow");
        StartCoroutine("ClearData");
    }


    /// <summary>
    /// 到凌晨时清除数据
    /// </summary>
    /// <returns>The data.</returns>
    IEnumerator ClearData()
    {
        //用来存放当前时间
        DateTime now;

        //用来标记数据是否需要被清除
        bool dataCleaned = true;

        //获取当前日期
        DateTime today = DateTime.Today;

        while (true)
        {
            //将当前本机时间覆盖
            now = DateTime.Now;

            //如果当前本机日期不等与today中存放的日期
            if (now.Month != today.Month && now.Day != today.Day)
            {
                //将当前日期赋值给today
                today = DateTime.Today;

                //标记为false，以便清除信息
                dataCleaned = false;
            }

            //如果当前时间大于0:08分
            if (now.Hour == 0 && now.Minute == 8 && now.Second >= 0 && !dataCleaned)
            {
                Logger.Instance.WriteLog("到凌晨时清除数据客流统计信息");

                //将信息清除 （将DicPassengerFlowData中的元素删除）
                DicPassengerFlowData.Clear();

                if (PFArea != null)

                    PFArea.Invoke(DicPassengerFlowData);

                dataCleaned = true;
            }

            //等1秒钟后执行下一步
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// 定义一个代理 DelPassengerFlowArea 返回值为 void ，方法签名为 Dictionary
    /// </summary>
    /// <param name="data"></param>
    delegate void DelPassengerFlowArea(Dictionary<string, PassengerFlowData> data);

    /// <summary>
    /// 声明一个DelPassengerFlowArea类型的委托 PFArea
    /// </summary>
    DelPassengerFlowArea PFArea;

    /// <summary>
    /// 加载客流统计区域，及加载当天最新统计信息，
    /// 画客流统计区域
    /// </summary>
    /// <returns>The passenger flow area.</returns>
    IEnumerator LoadPassengerFlowArea()
    {
        Logger.Instance.WriteLog("加载客流统计区域，及加载当天最新统计信息");
        Logger.Instance.WriteLog("加载客流统计区域");


        //实例化一个PassengerFlowAreaDao类
        PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao();

        //调用PassengerFlowAreaDao中的Select001函数
        pfaDao.Select001();

        //将pfa.Result中的结构体赋值给infoLst
        infoLst = pfaDao.Result;

        //遍历infoLst集合
        foreach (var info in infoLst)
        {
            //如果PassengerFlowAreaInfo中的Name为主客流
            if (info.Name == "主客流")
            {
                //定义一个对象
                GameObject g = new GameObject();

                //将这个对象的位置初始化为原点
                g.transform.position = Vector3.zero;

                //给这个对象上挂PassengerFlowInfoDetail脚本，并用p来接收这个脚本
                PassengerFlowInfoDetail p = g.AddComponent<PassengerFlowInfoDetail>();

                //给这四个材质变量赋初值
                p.mat_red = mat_red;
                p.mat_green = mat_green;
                p.mat_yellow = mat_yellow;
                p.mat_orange = mat_orange;

                //得到人流控制面板UI
                p.PFInfoUI = PassengerInfo.GetComponent<PassengerFlowInfoUI>();
                //初始化对象p结构体PassengerFlowAreaInfo的信息
                p.Init(info, null);

                PFArea += p.UpdateData;

                continue;
            }

            //定义一个string类型的数组，并用"|"分割
            string[] point = info.Points.Split('|');

            //定义一个V3类型及的数组，长度为...?
            Vector3[] pts = new Vector3[point.Length / 3];

            //定义一个V3类型的变量，并初始化为原点
            Vector3 vec = Vector3.zero;

            //遍历客流统计区域顶点的集合
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i] = new Vector3(float.Parse(point[i * 3]), float.Parse(point[i * 3 + 1]), float.Parse(point[i * 3 + 2]));
                vec += pts[i];
            }

            vec /= pts.Length;

            int[] triangles = new int[pts.Length];

            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = i;
            }

            //定义一个对象
            GameObject go = new GameObject();

            //给这个对象设置层级
            go.layer = LayerMask.NameToLayer("PassengerFlowArea");

            //给go对象添加MeshRender组件
            MeshRenderer meshrend = go.AddComponent<MeshRenderer>();

            //meshrend.material.shader = Shader.Find("Particles/Alpha Blended");
            //meshrend.material.SetColor("_TintColor",new Color(0,1,0,0.2f));

            //给go对象赋值绿色材质
            meshrend.material = mat_green;

            //给go对象添加MeshFilter组件
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();

            //给网格定点赋上pts三角形面片
            meshFilter.mesh.vertices = pts;

            //网格角度为triangles
            meshFilter.mesh.triangles = triangles;

            //重新计算网格的发现
            meshFilter.mesh.RecalculateNormals();

            //给go对象添加meshCollider组件
            MeshCollider mc = go.AddComponent<MeshCollider>();

            //将meshFilter的mesh赋值给MeshCollider的sharedMesh，作为共享网格
            mc.sharedMesh = meshFilter.mesh;

            //给go对象重新赋值一个坐标
            go.transform.position = new Vector3(0, 0.2f, 0);


            GameObject infoUI = Instantiate(PassengerFlowInfoShow) as GameObject;


            infoUI.transform.SetParent(PassengerFlowInfoShowParent);


            infoUI.transform.localScale = Vector3.one;

            infoUI.GetComponent<PassengerAreaUI>().Bind(vec, info.Name);

            //GameObject go = Instantiate(PassengerFlowAreaPrefab) as GameObject;


            PassengerFlowInfoDetail pfid = go.AddComponent<PassengerFlowInfoDetail>();

            pfid.mat_red = mat_red;
            pfid.mat_green = mat_green;
            pfid.mat_yellow = mat_yellow;
            pfid.mat_orange = mat_orange;

            pfid.Init(info, infoUI.GetComponent<PassengerAreaUI>());
            PFArea += pfid.UpdateData;
        }


        //从数据库加载当天最新的客流统计信息
        Logger.Instance.WriteLog("从数据库加载当天最新的客流统计信息");

        List<PassengerFlowInfoLog> InfoLogLst = pfaDao.Select003(DateTime.Now.ToString("yyyyMMdd") + "000000");

        PassengerFlowData _pfData = new PassengerFlowData();

        foreach (var logInfo in InfoLogLst)
        {
            _pfData.SumCount = int.Parse(logInfo.SumCount);
            _pfData.EnterCount = int.Parse(logInfo.EnterCount);
            _pfData.ExitCount = int.Parse(logInfo.ExitCount);
            DicPassengerFlowData[logInfo.PassengerFlowUrl] = _pfData;
        }

        if (PFArea != null)
            PFArea.Invoke(DicPassengerFlowData);
        yield return infoLst;
    }


    /// <summary>
    /// 场景中的摄像机跟相对应的现实中实体摄像机的客流统计信息关联起来
    /// </summary>
    /// <returns>The passenger flow info to camera.</returns>
    IEnumerator BindPassengerFlowInfoToCamera()
    {
        Logger.Instance.WriteLog("场景中的摄像机跟相对应的现实中实体摄像机的客流统计信息关联起来");

        //找到摄像机，并用cameraLst集合存储
        GameObject[] cameraLst = GameObject.FindGameObjectsWithTag("CameraFace");

        //遍历摄像机集合
        foreach (var item in cameraLst)
        {
            //给每台摄像机上添加PassengerFlowInfoCamera脚本
            PassengerFlowInfoCamera pfic = item.AddComponent<PassengerFlowInfoCamera>();

            //初始化客流信息
            pfic.Init(item.GetComponent<MonitorInfoData>().Data);


            PFArea += pfic.UpdateData;
        }

        yield return null;
    }


    /// <summary>
    /// 从服务器接收客流统计信息
    /// </summary>
    /// <returns>The passenger flow.</returns>
    IEnumerator GetPassengerFlow()
    {
        Logger.Instance.WriteLog("从服务器接收客流统计信息");

        while (true)
        {
            //如果服务器不为空，并且已经标记为连接
            if (CMSManage.Instance != null && CMSManage.Instance.isConnecting())
            {
                //得到人流信息
                CMSManage.Instance.GetPassengerFlow(PassengerFlow);

                break;
            }

            //等一秒钟后执行下一步
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }


    #region 未使用的代码

    /// <summary>
    /// （未引用）用来存储人流总量
    /// </summary>
    private int PassengerSumCount = 0;

    /// <summary>
    /// （未引用）用来存储进入的人流总量
    /// </summary>
    private int PassengerEnterCount = 0;

    /// <summary>
    /// （未引用）用来存储离开的人流总量
    /// </summary>
    private int PassengerExitCount = 0;

    #endregion


    /// <summary>
    /// (string, PassengerFlowData) 类型的字典集合，
    /// 用来存放不同区域人流信息
    /// 用来存储摄像机的个数
    /// </summary>
    private Dictionary<string, PassengerFlowData> DicPassengerFlowData = new Dictionary<string, PassengerFlowData>();


    /// <summary>
    /// pdf 是一个类型为 PassengerFlowData 的变量，
    /// PassengerFlowData 是一个结构体，用来存放人流数据
    /// </summary>
    private PassengerFlowData pfd = new PassengerFlowData();


    /// <summary>
    /// 用来存储 DicPassengerFlowData 字典集合的键
    /// </summary>
    string DicPassengerFlowDataKey = "";

    /// <summary>
    /// 在 GetPassengerFlow 中调用，
    /// 用来统计摄像机 PassengerFlowData 中的数据
    /// </summary>
    /// <param name="channel">摄像机的通道</param>
    /// <param name="ruleId">ruleID=0表示进，ruleID=1表示出</param>
    /// <param name="count">用来存放人数</param>
    /// <param name="ipFrom">服务器的IP地址</param>
    void PassengerFlow(int channel, int ruleId, int count, string ipFrom)
    {
        //字典集合DicPassengerFlowDataKey的键
        DicPassengerFlowDataKey = channel + ":" + ipFrom;

        //如果字典集合DicPassengerFlowDataKey中不包含DicPassengerFlowDataKey键（即表示没有这个摄像机）
        //则添加一个摄像机
        if (!DicPassengerFlowData.ContainsKey(DicPassengerFlowDataKey))
        {
            pfd.channel = channel;
            pfd.ipFrom = ipFrom;
            pfd.EnterCount = 0;
            pfd.ExitCount = 0;
            pfd.SumCount = 0;

            DicPassengerFlowData[DicPassengerFlowDataKey] = pfd;
        }

        //字典集合DicPassengerFlowDataKey中包含DicPassengerFlowDataKey键
        //如果包含这个摄像机，则赋值给pdf
        else
        {
            pfd = DicPassengerFlowData[DicPassengerFlowDataKey];
        }

        //表示进入一个人
        if (ruleId == 0)
        {
            pfd.SumCount += (1 + count);
            PassengerEnterCount += (1 + count);
            pfd.EnterCount += (1 + count);
        }

        //表示出去一个人
        else if (ruleId == 1)
        {
            pfd.SumCount -= (1 + count);
            PassengerExitCount += (1 + count);
            pfd.ExitCount += (1 + count);

            //如果摄像机上的SumCount小于0
            if (pfd.SumCount < 0)
            {
                pfd.SumCount = 0;
            }
        }

        else
        {
            return;
        }

        //将这个摄像机的人流信息存到 DicPassengerFlowData 这个字典集合中
        DicPassengerFlowData[DicPassengerFlowDataKey] = pfd;

        //判断PFArea这个引用是否为空
        if (PFArea != null)
        {
            PFArea.Invoke(DicPassengerFlowData);
        }

    }

    /// <summary>
    /// 当程序退出时，将客流统计数据存入数据库
    /// </summary>
    void OnApplicationQuit()
    {
        Logger.Instance.WriteLog("将客流统计数据存入数据库");

        //定义一个 PassengerFlowAreaDao类的变量
        PassengerFlowAreaDao pfaDao = new PassengerFlowAreaDao();

        //定义一个PassengerFlowData类的变量
        PassengerFlowData pfData;

        //遍历DicPassengerFlowData.Keys中的键
        foreach (var key in DicPassengerFlowData.Keys)
        {
            //用来选择人流...?
            pfData = DicPassengerFlowData[key];

            pfaDao.Insert002(DateTime.Now.ToString("yyyyMMddHHmmss"), key, pfData.SumCount.ToString(), pfData.EnterCount.ToString(), pfData.ExitCount.ToString());
        }
    }

}
