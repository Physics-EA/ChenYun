using UnityEngine;
using System.Collections;
/// <summary>
/// 摄像机列表项目组件，对摄像机列表项目进行相关操作
/// </summary>
public class MonitorListItem : MonoBehaviour {
	/// <summary>
	/// 摄像机ID
	/// </summary>
	public UILabel Id;
	/// <summary>
	/// 摄像机名称
	/// </summary>
	public UILabel Name;
	/// <summary>
	/// 开关按钮
	/// </summary>
	public UIButton BtnSwitch;
	/// <summary>
	/// 高亮背景
	/// </summary>
	public GameObject backGround;
	/// <summary>
	/// 选中背景
	/// </summary>
	public GameObject selectGround;
	/// <summary>
	/// 摄像机图标的引用，用来进行关联操作
	/// </summary>
	private GameObject MonitorIconRef;
	private string CheckMarkSprite;
	/// <summary>
	/// 判断是否被选中
	/// </summary>
	private bool Checked;
	/// <summary>
	/// 设备的数据库信息
	/// </summary>
	private DeviceInfo dinfo;

	void Awake()
	{
		Checked = false;
		CheckMarkSprite = BtnSwitch.normalSprite;
	}

	public void Init(string id,string _name, DeviceInfo info)
	{
		Id.text = id;
		Name.text = _name;
		dinfo = info;
	}

	/// <summary>
	/// 定位摄像机位置
	/// </summary>
	public void MoveBtnClick()
	{
		Camera.main.GetComponent<CameraController> ().GotoPosition (new Vector3(float.Parse(dinfo.RotatePointPosX),0,float.Parse(dinfo.RotatePointPosZ))
		                                                            ,new Vector3(float.Parse(dinfo.CameraPosX),float.Parse(dinfo.CameraPosY),float.Parse(dinfo.CameraPosZ))
		                                                            ,new Vector3(float.Parse(dinfo.CameraRotatX),float.Parse(dinfo.CameraRotatY),float.Parse(dinfo.CameraRotatZ)),null,true);
	}

	/// <summary>
	/// 绑定摄像机图标对象
	/// </summary>
	/// <param name="monitorIcon">Monitor icon.</param>
	public void BindMonitorIcon(GameObject monitorIcon)
	{
		MonitorIconRef = monitorIcon;
	}
	/// <summary>
	/// 播放监控录像
	/// </summary>
	/// <param name="go">Go.</param>
	public void SwitchStatus(GameObject go)
	{
		if(MonitorIconRef != null && go != MonitorIconRef)
		{
			MonitorIconRef.SendMessage ("SwitchStatus",gameObject);
		}
	}
	/// <summary>
	/// 当播放窗口关闭时将调用次方法
	/// </summary>
	public void DisplayWindowClosed()
	{
		Logger.Instance.WriteLog("摄像机[" + Name.text +  "]播放窗口被关闭，切换图标状态");
		Checked = false;
		BtnSwitch.normalSprite = CheckMarkSprite;
		BtnSwitch.GetComponentInChildren<UILabel>().text = "打开";
	}
	/// <summary>
	/// 当播放窗口打开时将调用次方法
	/// </summary>
	public void DisplayWindowOpend()
	{
		Logger.Instance.WriteLog("摄像机[" + Name.text +  "]播放窗口被打开，切换图标状态");
		BtnSwitch.normalSprite = BtnSwitch.pressedSprite;
		BtnSwitch.GetComponentInChildren<UILabel>().text = "关闭";
	}

	public void HoverOver()
	{
		backGround.SetActive (true);
	}

	public void HoverOut()
	{
		backGround.SetActive (false);
	}
}
