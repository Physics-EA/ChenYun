using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PresetPositionManage : MonoBehaviour 
{
	/// <summary>
	/// 主窗口，用来显示内容
	/// </summary>
	public GameObject MainWindow;
	/// <summary>
	/// 可以操作摄像机向上移动的区域
	/// </summary>
	public GameObject UpArea;
	/// <summary>
	/// 可以操作摄像机向下移动的区域
	/// </summary>
	public GameObject DownArea;
	/// <summary>
	/// 可以操作摄像机向左移动的区域
	/// </summary>
	public GameObject LeftArea;
	/// <summary>
	/// 可以操作摄像机向右移动的区域
	/// </summary>
	public GameObject RightArea;
	/// <summary>
	/// 显示图片的对象
	/// </summary>
	public UITexture ShowTex;
	/// <summary>
	/// 存放预设位标签的地方
	/// </summary>
	public UIGrid PresetPosItemContainer;
	/// <summary>
	/// 窗口标题
	/// </summary>
	public UILabel Title;
	/// <summary>
	/// 用来显示和修改预设位描述的组件
	/// </summary>
	public UIInput Description;
	/// <summary>
	/// 保存按钮
	/// </summary>
	public GameObject BtnConfirm;

	public GameObject Hit;
	/// <summary>
	/// 保存临时图片信息的对象
	/// </summary>
	private Texture2D tmpText = null;
	/// <summary>
	/// 读取内存数据的对象
	/// </summary>
	private shareMemOnlyRead readMem;
	/// <summary>
	/// 存放从内存中读取的数据
	/// </summary>
	private byte[] buff;
	/// <summary>
	/// 保存图像数据
	/// </summary>
	byte[] vBuff = null;
	/// <summary>
	/// 当前打开的摄像机的ID
	/// </summary>
	int cameraId;
	/// <summary>
	/// 当前打开的摄像机的GUID
	/// </summary>
	CMS_GUID guid;
	/// <summary>
	/// 保存所有预设位的信息
	/// </summary>
	private List<PrestPositionInfo> PresetPosInfos;
	/// <summary>
	/// 存放所有预设位的按钮
	/// </summary>
	private List<GameObject> PresetPosItems;
	/// <summary>
	/// 当前选择的预设位信息
	/// </summary>
	private PrestPositionInfo SelectedPresetPosInfo;
	/// <summary>
	/// 预设位按钮默认图标
	/// </summary>
	private string defaultSprite;
	/// <summary>
	/// 摄像头管理类的引用
	/// </summary>
	private CMSManage CMSManageInstance;
	void Awake()
	{
		CMSManageInstance = CMSManage.Instance;
	}
	/// <summary>
	/// 绑定相关按钮事件
	/// </summary>
	void Start()
	{
		UIEventListener.Get (UpArea).onPress = MoveToUp;
		UIEventListener.Get (DownArea).onPress = MoveToDown;
		UIEventListener.Get (LeftArea).onPress = MoveToLeft;
		UIEventListener.Get (RightArea).onPress = MoveToRight;
		UIEventListener.Get (MainWindow).onHover = Scale;
	}

	public void SetValue(DeviceInfo dInfo)
	{
		cameraId = int.Parse(dInfo.Id);
		guid = CMSManage.StringToGUID (dInfo.Guid);
		CMSManageInstance.OpenCamera (cameraId,guid,StartReadMem);
		Title.text = dInfo.Description;
		LoadPresetPosition ();
	}
	/// <summary>
	/// 加载预设位的信息
	/// </summary>
	void LoadPresetPosition()
	{
		Logger.Instance.WriteLog("加载预设位的信息");
		BtnConfirm.SetActive (false);
		PresetPosItems = new List<GameObject> ();
		PresetPositionDao ppDao = new PresetPositionDao ();
		ppDao.Select001 (cameraId.ToString());
		PresetPosInfos = ppDao.Result;
		if(PresetPosInfos.Count <= 0)
		{
			CreatePresetPosition();

			ppDao.Select001 (cameraId.ToString());
			PresetPosInfos = ppDao.Result;
		}
		for(int i = 0; i < PresetPosInfos.Count; i++)
		{
			GameObject go = PresetPosItemContainer.GetChild(i).gameObject;
			UIEventListener.Get(go).onClick = OnPresetPosItemSelected;
			PresetPosItems.Add(go);
		}
		defaultSprite = PresetPosItems [0].GetComponent<UIButton> ().normalSprite;
		OnPresetPosItemSelected(PresetPosItems[0]);
	}
	/// <summary>
	/// 创建默认预设位信息
	/// </summary>
	private void CreatePresetPosition()
	{
		Logger.Instance.WriteLog("创建默认预设位信息");
		PresetPositionDao ppDao = new PresetPositionDao ();
		for(int i = 0; i < PresetPosItemContainer.GetChildList().Count; i++)
		{
			if(i == 0)
			{
				ppDao.Insert(cameraId.ToString(),(i + 1) + "","请重新设定守望位的信息","是");
				CMSManageInstance.SetPresetPosition(cameraId,guid,(ushort)(i + 1));
			}
			else
			{
				ppDao.Insert(cameraId.ToString(),(i + 1) + "","请重新设定预设位" + i + "的信息");
				CMSManageInstance.SetPresetPosition(cameraId,guid,(ushort)(i + 1));
			}
		}

	}
	/// <summary>
	/// 当预设位的信息有变更时调用
	/// </summary>
	public void PresetInfoChanged()
	{
		BtnConfirm.SetActive (true);
	}
	/// <summary>
	/// 单击预设位标签时调用
	/// 显示选中预设位的信息
	/// </summary>
	/// <param name="go">Go.</param>
	void OnPresetPosItemSelected(GameObject go)
	{
		Logger.Instance.WriteLog("显示选中预设位的信息");
		go.GetComponent<UIButton> ().normalSprite = go.GetComponent<UIButton> ().pressedSprite;
		if (SelectedPresetPosInfo.Id == PresetPosInfos [PresetPosItems.IndexOf (go)].Id)return;
		foreach(GameObject item in PresetPosItems)
		{
			if(item != go)
			{
				item.GetComponent<UIButton> ().normalSprite = defaultSprite;
			}
		}
		SelectedPresetPosInfo = PresetPosInfos [PresetPosItems.IndexOf (go)];

		Description.value = SelectedPresetPosInfo.DESCRIPTION;
		CMSManageInstance.GotoPresetPosition (cameraId,guid,ushort.Parse(SelectedPresetPosInfo.Name));
	}
	/// <summary>
	/// 保存修改后的预设位的信息
	/// </summary>
	public void SavePresetPosition()
	{
		Logger.Instance.WriteLog("保存修改后的预设位的信息");
		PresetPositionDao ppDao = new PresetPositionDao ();
		ppDao.Update004(SelectedPresetPosInfo.Id,Description.value);
		CMSManageInstance.SetPresetPosition(cameraId,guid,ushort.Parse(SelectedPresetPosInfo.Name));
		LoadPresetPosition ();
	}

	/// <summary>
	/// 关闭当前窗口
	/// </summary>
	public void Close()
	{
		Logger.Instance.WriteLog("关闭当前窗口");
		StopCoroutine("readMemUpdate");
		if (cameraId > 0)
		CMSManageInstance.CloseCamera (cameraId,guid);
		cameraId = 0;
		guid = new CMS_GUID ();
		if(tmpText)Destroy(tmpText);
		gameObject.SetActive(false);
	}

	void OnDisable()
	{
		StopCoroutine("readMemUpdate");
		if (cameraId > 0)
		CMSManageInstance.CloseCamera (cameraId,guid);
		cameraId = 0;
		guid = new CMS_GUID ();
		if(tmpText)Destroy(tmpText);
		if(gameObject)Destroy (gameObject);
	}
	/// <summary>
	/// 创建读取内存数据的对象
	/// 并执行读取操作
	/// </summary>
	void StartReadMem(bool startReadMem)
	{
		if(startReadMem)
		{
			Logger.Instance.WriteLog("创建读取内存图像数据的对象");
			bool ReadSuccess = false;
			buff = new byte[sys_info.MAX_SHARE_MEMORY_SIZE];
			readMem = new shareMemOnlyRead("yfcamera_" + cameraId,(uint)buff.Length,ref ReadSuccess);
			if(ReadSuccess)
			{
				Hit.SetActive(false);
				StartCoroutine ("readMemUpdate");
			}
		}
	}
	/// <summary>
	/// 定期从内存中读取数据
	/// </summary>
	/// <returns>The mem update.</returns>
	IEnumerator readMemUpdate()
	{
		Logger.Instance.WriteLog("定期从内存中读取图像数据");
		while(true)
		{
			bool readSuccess = readMem.read(buff);
			if(readSuccess)
			{
				Int32 width;
				Int32 height;
				width = BitConverter.ToInt32(buff,0);
				height = BitConverter.ToInt32(buff,4);
				if(width == 0 || height == 0)
				{
					yield return new WaitForSeconds(0.01f);
					continue;
				}
				if(tmpText == null)
				{
					Logger.Instance.WriteLog("创建Texture");
					tmpText = new Texture2D(width,height,TextureFormat.RGBA32,false);
					ShowTex.mainTexture = tmpText;
				}
				if(vBuff == null)
				{
					Logger.Instance.WriteLog("创建图像数据缓存");
					vBuff = new byte[width * height * 4];
				}
				
				Buffer.BlockCopy(buff, 8, vBuff, 0, width * height * 4);
				tmpText.LoadRawTextureData(vBuff);
				tmpText.Apply();
			}
			yield return new WaitForSeconds(0.05f);
		}		
	}
	
	/// <summary>
	/// 当鼠标在UpArea上按下的时候调用
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="isPress">If set to <c>true</c> is press.</param>
	void MoveToUp(GameObject go, bool isPress)
	{
		if(isPress)
		{
			CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_UP);
		}
		else
		{
			CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_STOP_UPDOWN);
		}
		PresetInfoChanged ();
	}
	/// <summary>
	/// 当鼠标在DownArea上按下的时候调用
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="isPress">If set to <c>true</c> is press.</param>
	void MoveToDown(GameObject go, bool isPress)
	{
		if(isPress)
		{
			CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_DOWN);
		}
		else
		{
			CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_STOP_UPDOWN);
		}
		PresetInfoChanged ();
	}
	/// <summary>
	/// 当鼠标在LeftArea上按下的时候调用
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="isPress">If set to <c>true</c> is press.</param>
	void MoveToLeft(GameObject go, bool isPress)
	{
		if(isPress)
		{
			CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_LEFT);
		}
		else
		{
			CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_STOP_LEFTRIGHT);
		}
		PresetInfoChanged ();
	}
	/// <summary>
	/// 当鼠标在RightArea上按下的时候调用
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="isPress">If set to <c>true</c> is press.</param>
	void MoveToRight(GameObject go, bool isPress)
	{
		if(isPress)
		{
			CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_RIGHT);
		}
		else
		{
			CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_STOP_LEFTRIGHT);
		}
		PresetInfoChanged ();
	}
	/// <summary>
	/// 当鼠标放到MainWindow上的时候调用
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="onHover">If set to <c>true</c> on hover.</param>
	void Scale(GameObject go, bool onHover)
	{
		if(onHover)
		{
			StartCoroutine("StartScale");
		}
		else
		{
			StopCoroutine("StartScale");
		}
	}
	
	/// <summary>
	/// 执行拉近或拉远摄像机操作
	/// </summary>
	/// <returns>The scale.</returns>
	IEnumerator StartScale()
	{
		bool StopZoom = false;
		while(true)
		{
			yield return new WaitForFixedUpdate();
			if(Input.mouseScrollDelta.y > 0)
			{
				CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_ZOOM_WIDE);
				StopZoom = true;
			}
			else if(Input.mouseScrollDelta.y < 0)
			{
				CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_ZOOM_TELE);
				StopZoom = true;
			}
			if(StopZoom)
			{
				yield return new WaitForSeconds(0.5f);
				CMSManageInstance.PTZCtl(cameraId,guid,PTZOperation.PTZ_STOP_ZOOM);
				StopZoom = false;
				PresetInfoChanged ();
			}
		}
	}
}
