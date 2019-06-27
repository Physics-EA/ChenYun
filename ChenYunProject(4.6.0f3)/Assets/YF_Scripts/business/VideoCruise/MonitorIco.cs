using UnityEngine;
using System.Collections;



/// <summary>
/// 对摄像机图标相关事件的操作
/// </summary>
public class MonitorIco : MonoBehaviour
{
    /// <summary>
    /// 摄像机列表项目的引用，用来进行关联操作
    /// </summary>
    private GameObject MonitorListItemRef;

    /// <summary>
    /// 用来判定视频播放窗口是否已经显示，
    /// 初始化为false
    /// </summary>
    private bool isDisplayWindowShowing;

    /// <summary>
    /// 用来控制播放摄像机录像
    /// </summary>
    private GameObject MonitorVideoCtr;

    /// <summary>
    /// 默认颜色，
    /// 摄像机Main Color
    /// </summary>
    private Color DefaultColor;

    /// <summary>
    /// 世界坐标
    /// </summary>
    private Vector3 WorldPos = Vector3.zero;

    void Start()
    {
        //MonitorVideoCtr = GameObject.FindObjectOfType<MonitorVideoDisplayController>().gameObject;

        isDisplayWindowShowing = false;

        //将摄像机的材质赋值给DefaultColor
        DefaultColor = gameObject.renderer.material.color;

        //如果MonitorVideoCtr为空，则将将 MonitorVideoDisplayController 这个类型的对象赋值给 MonitorVideoCtr
        if (MonitorVideoCtr == null)
        {
            MonitorVideoCtr = GameObject.FindObjectOfType<MonitorVideoDisplayController>().gameObject;
        }
    }


    /// <summary>
    /// 当鼠标在图标上按下时调用，使图标闪烁
    /// </summary>
    /// <param name="go">Go.</param>
    /// <param name="onHover">If set to <c>true</c> on hover.</param>
    void Flicker(GameObject go, bool onPress)
    {
        if (onPress && isDisplayWindowShowing)
        {
            MonitorVideoCtr.SendMessage("Flicker", gameObject);
            StartCoroutine("StartFlicker");
        }
    }


    /// <summary>
    /// 其它程序调用，使图标闪烁
    /// </summary>
    void FlickerNoParam()
    {
        StartCoroutine("StartFlicker");
    }


    /// <summary>
    /// 实现图标闪烁
    /// </summary>
    /// <returns>The flicker.</returns>
    IEnumerator StartFlicker()
    {
        //		Color color = GetComponent<UISprite> ().color;
        //		for(int i = 0; i < 2;i++)
        //		{
        //			GetComponent<UISprite> ().color = Color.black;
        //			yield return new WaitForSeconds(1.0f / 8);
        //			GetComponent<UISprite> ().color = color;
        yield return new WaitForSeconds(1.0f / 8);
        //		}
    }


    /// <summary>
    /// 绑定摄像机图标对象
    /// </summary>
    /// <param name="monitorIcon">Monitor icon.</param>
    public void BindMonitorListItem(GameObject ListItem)
    {
        MonitorListItemRef = ListItem;
    }


    void OnMouseUpAsButton()
    {
        SwitchStatus(gameObject);
    }


    /// <summary>
    /// 播放监控录像
    /// </summary>
    /// <param name="go">Go.</param>
    void SwitchStatus(GameObject go)
    {
        Logger.Instance.WriteLog("视频监控状态切换");
        if (DataStore.CurrentOperate != Operation.VIDEO_CRUISE) return;
        StopCoroutine("StartFlicker");
        if (MonitorListItemRef != null && MonitorListItemRef != go)
        {
            MonitorListItemRef.SendMessage("SwitchStatus", gameObject);
        }
        if (isDisplayWindowShowing)
        {
            Logger.Instance.WriteLog("请求关闭摄像监控");
            MonitorVideoCtr.SendMessage("StopVideo", gameObject);
        }
        else
        {
            Logger.Instance.WriteLog("请求打开摄像监控");
            MonitorVideoCtr.SendMessage("PlayVideo", gameObject);
        }
    }


    /// <summary>
    /// 当播放窗口关闭时将调用次方法
    /// </summary>
    void DisplayWindowClosed()
    {
        Logger.Instance.WriteLog("播放窗口被关闭");
        isDisplayWindowShowing = false;
        SetColor(DefaultColor);
        MonitorListItemRef.SendMessage("DisplayWindowClosed");
    }


    /// <summary>
    /// 当播放窗口打开时将调用次方法
    /// </summary>
    void DisplayWindowOpend()
    {
        Logger.Instance.WriteLog("播放窗口被打开");
        isDisplayWindowShowing = true;
        MonitorListItemRef.SendMessage("DisplayWindowOpend");
    }


    /// <summary>
    /// 设置图标颜色
    /// </summary>
    /// <param name="color">Color.</param>
    private Color newColor;


    void SetColor(Color color)
    {
        newColor = color;
        gameObject.renderer.material.color = color;
    }


    void UseNewColor()
    {
        gameObject.renderer.material.color = newColor;
    }


    void UseDefaultColor()
    {
        gameObject.renderer.material.color = DefaultColor;
    }


    void StopVideo()
    {
        Logger.Instance.WriteLog("关闭摄像监控");
        if (isDisplayWindowShowing)
        {
            SwitchStatus(gameObject);
        }
    }


    void PlayVideo()
    {
        Logger.Instance.WriteLog("打开摄像监控");
        if (!isDisplayWindowShowing)
        {
            SwitchStatus(gameObject);
        }
    }
}
