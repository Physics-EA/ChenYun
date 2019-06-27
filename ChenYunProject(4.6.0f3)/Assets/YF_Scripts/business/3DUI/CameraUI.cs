using UnityEngine;
using System.Collections;

/// <summary>
/// 挂在3DUI下面的CameraUI（Clone）上
/// </summary>
public class CameraUI : MonoBehaviour
{
    /// <summary>
    /// 绑定的Camera
    /// </summary>
    public GameObject bindCamera;

    /// <summary>
    /// CameraUI面板显示的进入的人数
    /// </summary>
    public UILabel inNum;

    /// <summary>
    /// CameraUI面板显示的滞留的人数
    /// </summary>
    public UILabel standNum;

    /// <summary>
    /// CameraUI面板显示的离开的人数
    /// </summary>
    public UILabel outNum;

    /// <summary>
    /// 用来比较dis与distance的大小
    /// </summary>
    public float distance;

    /// <summary>
    /// 向量
    /// </summary>
    private Vector3 vec;

    /// <summary>
    /// 用来存储vec向量的平方
    /// </summary>
    private float dis;

    /// <summary>
    /// 用来标记CameraUI是否已经打开，初始化为 true
    /// </summary>
    private bool on_off;


    void Start()
    {
        on_off = true;
    }


    /// <summary>
    /// 用来设置CameraUI的位置和朝向
    /// </summary>
    public void Bind()
    {
        //UI摄像机世界坐标转换成屏幕坐标
        Vector3 pos = UICamera.mainCamera.WorldToScreenPoint(bindCamera.transform.position + new Vector3(0, 2.5f, 0));

        //Camera的位置为将从屏幕坐标转换为世界坐标
        transform.position = UICamera.mainCamera.ScreenToWorldPoint(pos);

        //激活CameraUI信息
        ActiveUI(PassengerFlowInfoReceiver.showInfo);
    }

    /// <summary>
    /// 通过PassengerFlowInfoCamera传进来的人流数据信息，赋值给CameraUI信息显示面板
    /// </summary>
    /// <param name="_inNum">进入Camera的人数</param>
    /// <param name="_standNum">停留Camera的人数</param>
    /// <param name="_outNum">离开Camera的人数</param>
    public void SetPassengerNum(int _inNum, int _standNum, int _outNum)
    {
        inNum.text = _inNum.ToString();
        standNum.text = _standNum.ToString();
        outNum.text = _outNum.ToString();
    }



    void Update()
    {
        //主摄像机与Camera之间的向量
        vec = Camera.main.transform.position - transform.position;

        //将vec向量的平方赋值给dis
        dis = vec.sqrMagnitude;

        //如果CameraUI与主摄像机之间的距离小于distance，则显示全部CameraUI信息
        if (dis <= distance)
        {
            ShowAll();
        }
        //如果CameraUI与主摄像机之间的距离大于distance，则关闭全部CameraUI信息
        else
        {
            CloseAll();
        }

        //如果CameraUI已经打开，朝向 Vec向量方向
        if (on_off)
        {
            vec = transform.position - vec;
            transform.LookAt(vec);
        }
    }

    void CloseAll()
    {
        //如果CameraUI是关着的，则直接退出函数
        if (!on_off)
        {
            return;
        }

        //获得CameraUI上面的全部子物体数量
        int count = transform.childCount;

        //遍历CameraUI上面的子物体索引，将子物体隐藏
        for (int i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        //标记CameraUI为关闭状态
        on_off = false;
    }


    /// <summary>
    /// 显示CameraUI的全部信息
    /// </summary>
    void ShowAll()
    {
        //如果CameraUI为打开状态，则直接退出函数
        if (on_off)
        {
            return;
        }

        //获得CameraUI上面的全部子物体数量
        int count = transform.childCount;

        //遍历CameraUI上面的子物体索引，将子物体激活
        for (int i = 0; i < count; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        //标记CameraUI为打开状态
        on_off = true;
    }

    /// <summary>
    /// 用来激活CameraUI
    /// </summary>
    /// <param name="b">用来标记是否激活相机</param>
    public void ActiveUI(bool b)
    {
        gameObject.SetActive(b);
    }
}
