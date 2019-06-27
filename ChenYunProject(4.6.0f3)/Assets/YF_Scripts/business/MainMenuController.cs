using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 控制主菜单中按钮相关事件
/// </summary>
public class MainMenuController : MonoBehaviour {
	/// <summary>
	/// 视频巡航按钮图片精灵
	/// </summary>
	public UISprite Btn_VideoCruise;
	/// <summary>
	/// 账户管理按钮图片精灵
	/// </summary>
	public UISprite Btn_AccountManage;
	/// <summary>
	/// 个人管理按钮图片精灵
	/// </summary>
	public UISprite Btn_PersonalManage;
	/// <summary>
	/// 系统管理按钮图片精灵
	/// </summary>
	public UISprite Btn_SystemManage;
	/// <summary>
	/// 视频巡航面板
	/// </summary>
	public GameObject Panel_VideoCruise;
	/// <summary>
	/// 账户管理面板
	/// </summary>
	public GameObject Panel_AccountManage;
	/// <summary>
	/// 个人管理面板
	/// </summary>
	public GameObject Panel_PersonalManage;
	/// <summary>
	/// 系统管理面板
	/// </summary>
	public GameObject Panel_SystemManage;
	/// <summary>
	/// 存放所有主菜单按钮图片精灵对象
	/// </summary>
	private List<UISprite> BtnSprites;
	/// <summary>
	/// 存放所有操作面板
	/// </summary>
	private List<GameObject> Panels;
	/// <summary>
	/// 存放所有操作面板
	/// </summary>
	private Dictionary<UISprite,GameObject> DicBtnShade;
	/// <summary>
	/// 控制功能列表切换
	/// </summary>
	public static bool canNotOpen = false;

	void Start()
	{
		BtnSprites = new List<UISprite> ();
		BtnSprites.Add (Btn_VideoCruise);
		BtnSprites.Add (Btn_PersonalManage);
		BtnSprites.Add (Btn_SystemManage);
		BtnSprites.Add (Btn_AccountManage);

		DicBtnShade = new Dictionary<UISprite, GameObject>();
		DicBtnShade.Add(Btn_VideoCruise,GetBtnShadeSprite(Btn_VideoCruise));
		DicBtnShade.Add(Btn_PersonalManage,GetBtnShadeSprite(Btn_PersonalManage));
		DicBtnShade.Add(Btn_SystemManage,GetBtnShadeSprite(Btn_SystemManage));
		DicBtnShade.Add(Btn_AccountManage,GetBtnShadeSprite(Btn_AccountManage));

		Panels = new List<GameObject> ();
		Panels.Add (Panel_VideoCruise);
		Panels.Add (Panel_PersonalManage);
		Panels.Add (Panel_SystemManage);
		Panels.Add (Panel_AccountManage);
		if(AuthorityHelper.NotAuthority(AuthorityHelper.DEVICE_MANAGEMENT)
		   &&AuthorityHelper.NotAuthority(AuthorityHelper.PATROL_PLAN_MANAGEMENT)
		   &&AuthorityHelper.NotAuthority(AuthorityHelper.EVACUATE_AREA_MANAGEMENT)
		   &&AuthorityHelper.NotAuthority(AuthorityHelper.EVACUATE_PLAN_MANAGEMENT))
		{
			Btn_SystemManage.enabled = false;
		}
		if(AuthorityHelper.NotAuthority(AuthorityHelper.USER_INFO_BROWSE)
		   && AuthorityHelper.NotAuthority(AuthorityHelper.USER_GROUP_INFO_BROWSE)
		   && AuthorityHelper.NotAuthority(AuthorityHelper.HISTORY_MANAGEMENT))
		{
			Btn_AccountManage.enabled = false;
		}
	}

	private GameObject GetBtnShadeSprite(UISprite btnSprite)
	{
		GameObject go = btnSprite.GetComponentsInChildren<UISprite>()[1].gameObject;
		go.SetActive(false);
		return go;
	}

	bool InitFinished = false;
	void LateUpdate()
	{
		if(!InitFinished)
		{
			InitFinished = true;
			SwitchToVideoCruisePanel();
		}
	}
	/// <summary>
	/// 切换到视频巡航面板
	/// </summary>
	public void SwitchToVideoCruisePanel()
	{
		if(canNotOpen)
		{
			return;
		}
		DataStore.CurrentOperate = Operation.VIDEO_CRUISE;
		Execute (Panel_VideoCruise,Btn_VideoCruise);
		Panel_VideoCruise.GetComponent<EvacuationPlanManage>().ReLoadEvacuationArea();
		Panel_VideoCruise.GetComponent<RealTimeMonitor>().ReLoadVideoPatrolPlanRecord();
		Panel_VideoCruise.SetActive(true);
		GameObject[] gos = GameObject.FindGameObjectsWithTag("VideoWindow");
		foreach(GameObject go in gos)
		{
			go.SendMessage("Show");
		}
	}
	/// <summary>
	/// 切换到账户面板
	/// </summary>
	public void SwitchToAccountManagePanel()
	{
		if(canNotOpen)
		{
			return;
		}
		DataStore.CurrentOperate = Operation.PERSONAL_MANAGE;
		Execute (Panel_AccountManage,Btn_AccountManage);
		Panel_AccountManage.GetComponent<AccountManageControl> ().Initialize ();
	}
	/// <summary>
	/// 切换到个人管理面板
	/// </summary>
	public void SwitchToPersonalManagePanel()
	{
		if(canNotOpen)
		{
			return;
		}
		DataStore.CurrentOperate = Operation.PERSONAL_MANAGE;
		Panel_PersonalManage.GetComponent<PersonalManage>().Init();
		Execute (Panel_PersonalManage,Btn_PersonalManage);
	}
	/// <summary>
	/// 切换到系统管理面板
	/// </summary>
	public void SwitchToSystemManagePanel()
	{
		if(canNotOpen)
		{
			return;
		}
		DataStore.CurrentOperate = Operation.SYSTEM_MANAGE;
		Panel_SystemManage.GetComponent<SystemManageController>().Init();
		Execute (Panel_SystemManage,Btn_SystemManage);
		StartCoroutine("ShowMonitor");
		StartCoroutine("CheckMouseHover");
		GameObject[] gos = GameObject.FindGameObjectsWithTag("VideoWindow");
		foreach(GameObject go in gos)
		{
			go.SendMessage("Hidden");
		}
	}

	private string DefaultSpriteName;
	private UISprite SelectedSprite;
	private Color NewColor = new Color(0,0.24f,0.45f);
	/// <summary>
	/// 执行切换窗口操作
	/// </summary>
	/// <param name="SubMenu">面板</param>
	/// <param name="MainMenuBtn">主菜单按钮图片精灵</param>
	private void Execute(GameObject Panel,UISprite MainMenuBtn)
	{
		Panel.GetComponent<UIPanel>().alpha = 1;
		ResetOtherButtonSpriteExcept (MainMenuBtn);
		CloseOtherSubMenuExcept (Panel);
		SelectedSprite = MainMenuBtn;
		DefaultSpriteName = MainMenuBtn.GetComponent<UIButton>().normalSprite;
		MainMenuBtn.spriteName = MainMenuBtn.GetComponent<UIButton>().pressedSprite;
		MainMenuBtn.GetComponentInChildren<UILabel>().color = NewColor;
		MainMenuBtn.GetComponent<BoxCollider>().enabled = false;
		DicBtnShade[MainMenuBtn].SetActive(true);
	}
	/// <summary>
	/// 将其他按钮图片精灵设为默认的图片精灵
	/// </summary>
	/// <param name="ExceptSprite">除外项目</param>
	private void ResetOtherButtonSpriteExcept(UISprite ExceptSprite)
	{
		foreach(UISprite BtnSprite in BtnSprites)
		{
			if(BtnSprite && BtnSprite.GetInstanceID() != ExceptSprite.GetInstanceID())
			{
				if(SelectedSprite && BtnSprite.GetInstanceID() == SelectedSprite.GetInstanceID())
				{
					BtnSprite.GetComponent<BoxCollider>().enabled = true;
					BtnSprite.spriteName = DefaultSpriteName;
					BtnSprite.GetComponentInChildren<UILabel>().color = Color.white;
					DicBtnShade[BtnSprite].SetActive(false);
					break;
				}
			}

		}
	}
	/// <summary>
	/// 关闭其他面板
	/// </summary>
	/// <param name="ExceptSelf">除外项目</param>
	private void CloseOtherSubMenuExcept(GameObject ExceptPanel)
	{
		foreach(GameObject SubMenu in Panels)
		{
			if(SubMenu.GetInstanceID() != ExceptPanel.GetInstanceID())
			{
				SubMenu.GetComponent<UIPanel>().alpha = 0;
				if(SubMenu.GetComponent<SystemManageController>())
				{
					SubMenu.GetComponent<SystemManageController>().Init();
					StartCoroutine("HideMonitor");
					StopCoroutine("CheckMouseHover");
				}
			}
		}
	}

	GameObject[] MonitorObjs;
	IEnumerator ShowMonitor()
	{
		if(MonitorObjs == null)MonitorObjs = GameObject.FindGameObjectsWithTag("CameraFace");
		foreach(GameObject go in MonitorObjs)
		{
			go.transform.GetChild (1).gameObject.SetActive(true);
			go.transform.GetChild (1).gameObject.AddComponent<MeshCollider>();
		}
		yield return null;
	}

	IEnumerator HideMonitor()
	{
		if(MonitorObjs != null)
		{
			foreach(GameObject go in MonitorObjs)
			{
				go.transform.GetChild (1).gameObject.SetActive(false);
			}
		}
		yield return null;
	}

	public GameObject HoverObjectInfo;
	IEnumerator CheckMouseHover()
	{
		string CameraName = "";
		DeviceDao dDao = new DeviceDao ();
		while(true)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit,1000,1<<LayerMask.NameToLayer("LingXing")))
			{
				Logger.Instance.WriteLog("显示鼠标悬停摄像机信息");
				CameraName = hit.transform.parent.GetChild (0).name;
				HoverObjectInfo.SetActive(true);
				dDao.Select002 (CameraName);
				if(dDao.Result.Count == 1)
				{
					HoverObjectInfo.GetComponent<HoverOverCameraInfo>().Init(dDao.Result[0].Description,"序号：" + dDao.Result[0].Id);
				}
				else
				{
					HoverObjectInfo.GetComponent<HoverOverCameraInfo>().Init(CameraName,"没有绑定摄像机" );
				}
				HoverObjectInfo.GetComponent<UISprite>().pivot = UIWidget.Pivot.BottomLeft;
				HoverObjectInfo.GetComponentInChildren<UILabel>().alignment = NGUIText.Alignment.Left;
				if(Input.mousePosition.x + HoverObjectInfo.GetComponent<UISprite>().width > Screen.width)
				{
					HoverObjectInfo.GetComponent<UISprite>().pivot = UIWidget.Pivot.BottomRight;
					HoverObjectInfo.GetComponentInChildren<UILabel>().alignment = NGUIText.Alignment.Right;
				}
				HoverObjectInfo.transform.position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
			}
			else
			{
				HoverObjectInfo.SetActive(false);
			}
			yield return new WaitForSeconds(1f);
		}
		
	}

//	public void Enable(bool isEnable)
//	{
//		if(isEnable)
//		{
//			foreach(UISprite sprite in BtnSprites)
//			{
//				sprite.GetComponent<BoxCollider>().enabled = true;
//			}
//
//		}
//		else
//		{
//			foreach(UISprite sprite in BtnSprites)
//			{
//				sprite.GetComponent<BoxCollider>().enabled = false;
//			}
//		}
//	}
//
	public void UpdateInfo()
	{
		SwitchToVideoCruisePanel ();
		Btn_SystemManage.enabled = true;
		Btn_AccountManage.enabled = true;
		if(AuthorityHelper.NotAuthority(AuthorityHelper.DEVICE_MANAGEMENT)
		   &&AuthorityHelper.NotAuthority(AuthorityHelper.PATROL_PLAN_MANAGEMENT)
		   &&AuthorityHelper.NotAuthority(AuthorityHelper.EVACUATE_AREA_MANAGEMENT)
		   &&AuthorityHelper.NotAuthority(AuthorityHelper.EVACUATE_PLAN_MANAGEMENT))
		{
			Btn_SystemManage.enabled = false;
		}

		if(AuthorityHelper.NotAuthority(AuthorityHelper.USER_INFO_BROWSE)
		   && AuthorityHelper.NotAuthority(AuthorityHelper.USER_GROUP_INFO_BROWSE))
		{
			Btn_AccountManage.enabled = false;
		}
	}
}
