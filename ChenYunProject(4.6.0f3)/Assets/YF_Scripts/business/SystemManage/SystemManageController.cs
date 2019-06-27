using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 系统管理相关操作控制
/// </summary>
public class SystemManageController : MonoBehaviour {
	/// <summary>
	/// 设备管理窗口
	/// </summary>
	public GameObject DeviceManageWindow;
	/// <summary>
	/// 巡航方案管理窗口
	/// </summary>
	public GameObject CruiseManageWindow;
	/// <summary>
	/// 疏散区域管理窗口
	/// </summary>
	public GameObject EvacuateAreaManageWindow;
	/// <summary>
	/// 疏散预案管理窗口
	/// </summary>
	public GameObject EvacuatePlanManageWindow;
	/// <summary>
	/// 设备管理按钮
	/// </summary>
	public UIButton DeviceManageButton;
	/// <summary>
	/// 巡航方案管理按钮
	/// </summary>
	public UIButton CruiseManageButton;
	/// <summary>
	/// 疏散区域管理按钮
	/// </summary>
	public UIButton EvacuateAreaManageButton;
	/// <summary>
	/// 疏散预案管理按钮
	/// </summary>
	public UIButton EvacuatePlanManageButton;

	public delegate void InitDelegate();
	/// <summary>
	/// 存储对应功能窗口
	/// </summary>
	private Dictionary<UIButton,GameObject> DicWindows;
	/// <summary>
	/// 存储对应功能初始化调用
	/// </summary>
	private Dictionary<UIButton,InitDelegate> DicInits;
	private string normalSprite;
	private UIButton PreSelectedButton;

	void Start()
	{
		DicWindows = new Dictionary<UIButton,GameObject>();
		DicWindows.Add (DeviceManageButton,DeviceManageWindow);
		DicWindows.Add (CruiseManageButton,CruiseManageWindow);
		DicWindows.Add (EvacuateAreaManageButton,EvacuateAreaManageWindow);
		DicWindows.Add (EvacuatePlanManageButton,EvacuatePlanManageWindow);

		DicInits = new Dictionary<UIButton, InitDelegate>();
		DicInits.Add(DeviceManageButton,DeviceManageWindow.GetComponentInChildren<DeviceRecordManage>().Init);
		DicInits.Add(CruiseManageButton,CruiseManageWindow.GetComponentInChildren<VideoPatrolPlanView>().Init);
		DicInits.Add(EvacuateAreaManageButton,EvacuateAreaManageWindow.GetComponentInChildren<EvacuateAreaEdit>().Init);
		DicInits.Add(EvacuatePlanManageButton,EvacuatePlanManageWindow.GetComponentInChildren<EvacuatePlanEdit>().Init);

		foreach(GameObject window in DicWindows.Values)
		{
			window.SetActive(false);
		}
		//UpdateAuthority();
	}

	public void UpdateAuthority()
	{
		foreach(UIButton button in DicWindows.Keys)
		{
			button.enabled = true;
			button.GetComponent<BoxCollider>().enabled = true;
		}
		if(AuthorityHelper.NotAuthority(AuthorityHelper.DEVICE_MANAGEMENT))
		{
			DeviceManageButton.enabled = false;
		}
		if(AuthorityHelper.NotAuthority(AuthorityHelper.PATROL_PLAN_MANAGEMENT))
		{
			CruiseManageButton.enabled = false;
		}
		if(AuthorityHelper.NotAuthority(AuthorityHelper.EVACUATE_AREA_MANAGEMENT))
		{
			EvacuateAreaManageButton.enabled = false;
		}
		if(AuthorityHelper.NotAuthority(AuthorityHelper.EVACUATE_PLAN_MANAGEMENT))
		{
			EvacuatePlanManageButton.enabled = false;
		}
	}
	/// <summary>
	/// 切换到设备管理界面
	/// </summary>
	public void SwitchToDeviceManage(UIButton requester)
	{
		Logger.Instance.WriteLog("切换到设备管理界面");
		SwitchWindows (requester);
	}
	/// <summary>
	/// 切换到巡航方案管理界面
	/// </summary>
	public void SwitchToCruiseManage(UIButton requester)
	{
		Logger.Instance.WriteLog("切换到巡航方案管理界面");
		SwitchWindows (requester);
	}
	/// <summary>
	/// 切换到疏散区域管理界面
	/// </summary>
	public void SwitchToEvacuateAreaManage(UIButton requester)
	{
		Logger.Instance.WriteLog("切换到疏散区域管理界面");
		SwitchWindows (requester);
	}
	/// <summary>
	/// 切换到疏散预案管理界面
	/// </summary>
	public void SwitchToEvacuatePlanManage(UIButton requester)
	{
		Logger.Instance.WriteLog("切换到疏散预案管理界面");
		SwitchWindows (requester);
	}
	/// <summary>
	/// 打开指定的管理界面，并关闭已经打开的界面
	/// </summary>
	/// <param name="requester">Requester.</param>
	private void SwitchWindows(UIButton requester)
	{
		if(!requester.enabled)
		{
			Logger.Instance.WriteLog("没有权限");
			return;
		}
		if(PreSelectedButton)
		{
			PreSelectedButton.normalSprite = normalSprite;
			PreSelectedButton.GetComponent<BoxCollider>().enabled = true;
			DicWindows[PreSelectedButton].SetActive(false);

		}
		PreSelectedButton = requester;
		PreSelectedButton.GetComponent<BoxCollider>().enabled = false;
		normalSprite = PreSelectedButton.normalSprite;
		PreSelectedButton.normalSprite = PreSelectedButton.pressedSprite;
		DicWindows[PreSelectedButton].SetActive(true);
		if(DicInits.ContainsKey(PreSelectedButton))DicInits[PreSelectedButton].Invoke();
	}
	
	public void Init()
	{
		UpdateAuthority();
		foreach(UIButton button in DicWindows.Keys)
		{
			if(button.enabled)
			{
				SwitchToDeviceManage (DeviceManageButton);
				break;
			}
		}
	}
}
