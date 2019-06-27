using UnityEngine;
using System.Collections;
using System;
using System.IO;
using SharpFFmpeg;
public class VideoWindow : MonoBehaviour {

	/// <summary>
	/// 显示窗口标题的对象
	/// </summary>
	public UILabel Title;
	/// <summary>
	/// 窗口标题栏
	/// </summary>
	public UISprite TitleBar;
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

	public GameObject hit;

	public GameObject PrestPositionContainer;
	/// <summary>
	/// 保存临时图片信息的对象
	/// </summary>
	private Texture2D txt2dtest = null;
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
	/// 用来保存当前窗口所显示内容的对象
	/// </summary>
	private GameObject CamaraRef;
	/// <summary>
	/// 用来保存是谁打开的窗口
	/// </summary>
	private GameObject ParentHandle;
	/// <summary>
	/// 用来管理与监控服务器通信的对象
	/// </summary>
	private CMSManage CMSManageInstance;
	/// <summary>
	/// 如果进行了PTZ操作则为true
	/// </summary>
	private bool StatusChanged = false;
	private float Tick = 60;
	void Awake()
	{
		PrestPositionContainer.SetActive (false);
		CMSManageInstance = CMSManage.Instance;
	}
	void Start()
	{
		TitleBar.GetComponent<UIDragObject> ().panelRegion = transform.parent.GetComponent<UIPanel> ();
	}

	void Update()
	{
		if(StatusChanged)
		{
			StatusChanged = false;
			Tick = 60;
		}
		if(Tick > 0)
		{
			Tick -= Time.deltaTime;
			if(Tick <= 0)
			{
				CMSManageInstance.GotoPresetPosition (cameraId,guid,1);
			}
		}
	}
	/// <summary>
	/// 绑定相关按钮事件
	/// </summary>
	void StartPlay()
	{
		UIEventListener.Get (UpArea).onPress = MoveToUp;
		UIEventListener.Get (DownArea).onPress = MoveToDown;
		UIEventListener.Get (LeftArea).onPress = MoveToLeft;
		UIEventListener.Get (RightArea).onPress = MoveToRight;
		UIEventListener.Get (MainWindow).onHover = Scale;
		UIEventListener.Get(TitleBar.gameObject).onPress = Flicker;
		cameraId = int.Parse(CamaraRef.GetComponent<MonitorInfoData> ().Data.Id);
		guid = CMSManage.StringToGUID (CamaraRef.GetComponent<MonitorInfoData> ().Data.Guid);
		if(CamaraRef.GetComponent<MonitorInfoData> ().Data.UseRTSP)
		{
			OpenVideoFromRTSP(CamaraRef.GetComponent<MonitorInfoData> ().Data.RTSPUrl);
		}
		else
		{
			CMSManageInstance.OpenCamera (cameraId,guid,StartReadMem);
		}
	}
	
	private int width;
	private int heigth;
	private Texture2D tmpTex;
	private SharpFFmpeg.SharpFFmpeg ffmpeg;
	void OpenVideoFromRTSP(string url)
	{
		ffmpeg = new SharpFFmpeg.SharpFFmpeg();
		bool ret = ffmpeg.OpenStream(url, out width, out heigth);
		if(ret == false)
		{
			return;
		}
		hit.SetActive(false);
		tmpTex = new Texture2D(width, heigth, TextureFormat.RGB24, false);
		ShowTex.mainTexture = tmpTex;
		ShowTex.shader = Shader.Find("Unlit/Masked Colored");
		samples = new byte[width * heigth * 3];
		buffer = IntPtr.Zero;
		StartCoroutine("UpdateVideo");
	}

	byte[] samples;
	IntPtr buffer;
	bool readData = true;
	IEnumerator UpdateVideo()
	{
		while (readData)
		{
			if (ffmpeg.GetPictureStream(samples))
			{
				tmpTex.LoadRawTextureData(samples);
				tmpTex.Apply();
			}
			yield return new WaitForSeconds(1.0f / 50);
		}
	}

	/// <summary>
	/// 创建读取内存数据的对象
	/// 并执行读取操作
	/// </summary>
	void StartReadMem(bool startReadMem)
	{
		if(startReadMem)
		{
			if(CMSManageInstance.HasPTZCtl(CamaraRef.GetComponent<MonitorInfoData> ().Data.Guid))
			{
				PrestPositionContainer.SetActive (true);
			}
			bool success = false;
			buff = new byte[1280*720*4*4 + 8];
			readMem = new shareMemOnlyRead("yfcamera_" + cameraId,1280*720*4*4 + 8,ref success);
			if(success)
			{
				hit.SetActive(false);
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
		while(readData)
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
				if(txt2dtest == null)
				{
					txt2dtest = new Texture2D(width,height,TextureFormat.RGBA32,false);
					ShowTex.mainTexture = txt2dtest;
				}
				if(vBuff == null)
				{
					vBuff = new byte[width * height * 4];
				}

				Buffer.BlockCopy(buff, 8, vBuff, 0, width * height * 4);
				txt2dtest.LoadRawTextureData(vBuff);
				txt2dtest.Apply();
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
			StatusChanged = true;
		}
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
			StatusChanged = true;
		}
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
			StatusChanged = true;
		}
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
			StatusChanged = true;
		}
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
				StatusChanged = true;
			}
		}
	}

	public void GotoPresetPosition(UILabel index)
	{
		CMSManageInstance.GotoPresetPosition (cameraId,guid,(ushort)(ushort.Parse(index.text) + 1));
		if(ushort.Parse(index.text) != 0)StatusChanged = true;
	}
	/// <summary>
	/// 当鼠标在标题栏按下调用，使标题栏闪烁
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="onHover">If set to <c>true</c> on hover.</param>
	void Flicker(GameObject go ,bool onPress)
	{
		if(onPress)
		{
			ParentHandle.SendMessage("Flicker",gameObject);
			StartCoroutine("StartFlicker");
		}
	}
	/// <summary>
	/// 其它代码调用，使标题栏闪烁
	/// </summary>
	void FlickerNoParam()
	{
		StartCoroutine("StartFlicker");
	}
	/// <summary>
	/// 实现标题栏闪烁
	/// </summary>
	/// <returns>The flicker.</returns>
	IEnumerator StartFlicker()
	{
		Color color = TitleBar.color;
		for(int i = 0; i < 2;i++)
		{
			TitleBar.color = Color.black;
			yield return new WaitForSeconds(1.0f / 8);
			TitleBar.color = color;
			yield return new WaitForSeconds(1.0f / 8);
		}
	}
	/// <summary>
	/// 窗口打开索引
	/// </summary>
	/// <param name="camaraRef">camaraRef.</param>
	public void SetCamaraRef(GameObject camaraRef)
	{
		CamaraRef = camaraRef;
	}
	/// <summary>
	/// Sets the parent handle.
	/// </summary>
	/// <param name="go">Go.</param>
	public void SetParentHandle(GameObject go)
	{
		ParentHandle = go;
	}
	/// <summary>
	/// 设置标题
	/// </summary>
	/// <param name="title">Title.</param>
	public void SetTitle(string title)
	{
		Title.text = title;
	}
	/// <summary>
	/// 设置标题栏的颜色
	/// </summary>
	/// <param name="color">Color.</param>
	public void SetTitleBarColor(Color color)
	{
		
		TitleBar.color = color;
	}
	/// <summary>
	/// 设置窗口是否可以缩放
	/// </summary>
	/// <param name="scalable">If set to <c>true</c> scalable.</param>
	public void SetScalable(bool scalable)
	{
		if(scalable)
		{
			if(MainWindow.GetComponent<UIDragResize>()) return;
			MainWindow.AddComponent<UIDragResize>();
		}
		else
		{
			Destroy(MainWindow.GetComponent<UIDragResize>());
			
		}
	}
	/// <summary>
	/// 设置窗口是否可以移动
	/// </summary>
	/// <param name="movable">If set to <c>true</c> movable.</param>
	public void SetMovable(bool movable)
	{
		
		if(movable)
		{
			if(TitleBar.GetComponent<UIDragObject>()) return;
			TitleBar.gameObject.AddComponent<UIDragObject>();
		}
		else
		{
			Destroy(TitleBar.GetComponent<UIDragObject>());
			
		}
	}
	/// <summary>
	/// 设置窗口的位置
	/// </summary>
	/// <param name="pos">Position.</param>
	public void SetPosition(float[] pos)
	{
		Vector3 _pos = new Vector3 (pos[0],pos[1],pos[2]);
		transform.localPosition = _pos;
		
	}
	/// <summary>
	/// 设置窗口的大小
	/// </summary>
	/// <param name="size">Size.</param>
	public void SetMainWindowSize(int[] size)
	{
		
		MainWindow.GetComponent<UITexture> ().width = size [0];
		MainWindow.GetComponent<UITexture> ().height = size [1];
		MainWindow.transform.localPosition = Vector3.zero;
	}
	/// <summary>
	/// 显示窗口
	/// </summary>
	void Show()
	{
		CamaraRef.SendMessage ("UseNewColor");
		GetComponent<UIPanel>().alpha = 1;
	}
	/// <summary>
	/// 隐藏窗口
	/// </summary>
	void Hidden()
	{
		CamaraRef.SendMessage ("UseDefaultColor");
		GetComponent<UIPanel>().alpha = 0;
	}
	/// <summary>
	/// 向控制器提交关闭操作
	/// </summary>
	public void Close()
	{
		ParentHandle.SendMessage("StopVideo" ,CamaraRef);
		readData = false;
		StopCoroutine ("readMemUpdate");
		if (ffmpeg != null)
		{
			//StopCoroutine("UpdateVideo");
			ffmpeg.CloseStream();
			ffmpeg = null;
		}
		if(txt2dtest != null)
		{
			Destroy(txt2dtest);
		}
		if (tmpTex != null) 
		{
			Destroy(tmpTex);
			tmpTex = null;
		}
	}

	void OnDestroy()
	{
		readData = false;
		StopCoroutine ("readMemUpdate");
		if (ffmpeg != null)
		{
			//StopCoroutine("UpdateVideo");
			ffmpeg.CloseStream();
			ffmpeg = null;
		}

		if(txt2dtest != null)
		{
			Destroy(txt2dtest);
		}
		if (tmpTex != null) 
		{
			Destroy(tmpTex);
		}
	}
}
