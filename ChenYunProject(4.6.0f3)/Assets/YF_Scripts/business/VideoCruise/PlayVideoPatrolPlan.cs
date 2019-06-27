using UnityEngine;
using System.Collections;
using System;

public class PlayVideoPatrolPlan : MonoBehaviour {
	public UILabel Title;
	/// <summary>
	/// 显示图片的对象
	/// </summary>
	public UITexture ShowTex;

	public GameObject BtnNext;

	public GameObject BtnFinsed;

	public GameObject BtnExit;

	public GameObject Hit;

	public UILabel Timer;

	public GameObject lingxingPrefab;

	private string status;
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
	/// 当前打开的摄像头的ID
	/// </summary>
	int cameraId;
	/// <summary>
	/// 当前打开的摄像头的GUID
	/// </summary>
	CMS_GUID guid;
	/// <summary>
	/// 上一次打开的摄像头的GUID.
	/// </summary>
	CMS_GUID preGuid;
	/// <summary>
	/// 上一次打开的摄像头的ID
	/// </summary>
	int preCameraId;
	/// <summary>
	/// 当单击【下一个】按钮时，调用的回调函数
	/// </summary>
	public EventDelegate.Callback NextVideo;
	/// <summary>
	/// 当单击【退出】按钮时，调用的回调函数
	/// </summary>
	public EventDelegate.Callback StopVideo;
	/// <summary>
	/// 保存操作信息
	/// </summary>
	public DelWriteLog WriteLog;
	/// <summary>
	/// 保存图片信息
	/// </summary>
	public DelSavePicture SavePicture;
	private string startTime;
	private string preCamera = "";
	private bool CameraOpenFail = true;
	public Texture2D DefaultTex;
	/// <summary>
	/// 用来标示当前播放的是不是最后一个视频
	/// </summary>
	private bool isLast = false;
	/// <summary>
	/// 播放停留的最小时间
	/// </summary>
	private string playTime;
	/// <summary>
	/// 显示用的标题
	/// </summary>
	private string title;
	/// <summary>
	/// 保存图片是是否翻转图片
	/// </summary>
	private bool rotateFlip = true;
    /// <summary>
    /// 自定义跳过下一个
    /// </summary>
    public bool constomNext;
    /// <summary>
    /// 视频窗口默认位置
    /// </summary>
    public Vector4[] videoPos;
	/// <summary>
	/// 视频窗口位置参数
	/// </summary>
	private string posValue;
	void Awake()
	{
        ReadConfig();

	}

    /// <summary>
    /// 读取自定义视频窗口位置
    /// </summary>
    private void ReadConfig()
    {
        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        doc.Load(Application.dataPath + "/YF_Config/PatrolConfig.xml");
        System.Xml.XmlElement el = (System.Xml.XmlElement)doc.SelectSingleNode("config");
		posValue = el.GetAttribute("videoPos");
		if (posValue == string.Empty)
        {
			posValue = "0";
        }
		Debug.Log ("配置文件读取的视频窗口位置配置参数videoPos：" + posValue);
        UIPanel spr = gameObject.GetComponent<UIPanel>();
		switch (posValue)
        {
            case "1":
                spr.SetAnchor(transform.parent.gameObject, (int)videoPos[1].x, (int)videoPos[1].z, (int)videoPos[1].y, (int)videoPos[1].w);
                spr.leftAnchor.relative = 0;
                spr.rightAnchor.relative = 0;
                spr.topAnchor.relative = 0;
                spr.bottomAnchor.relative = 0;
                break;
            case "2":
                spr.SetAnchor(transform.parent.gameObject, (int)videoPos[2].x, (int)videoPos[2].z, (int)videoPos[2].y, (int)videoPos[2].w);
                spr.leftAnchor.relative = 1;
                spr.rightAnchor.relative = 1;
                spr.topAnchor.relative = 1;
                spr.bottomAnchor.relative = 1;
                break;
            case "3":
                spr.SetAnchor(transform.parent.gameObject, (int)videoPos[3].x, (int)videoPos[3].z, (int)videoPos[3].y, (int)videoPos[3].w);
                spr.leftAnchor.relative = 1;
                spr.rightAnchor.relative = 1;
                spr.topAnchor.relative = 0;
                spr.bottomAnchor.relative = 0;
                break;
            default:
                spr.SetAnchor(transform.parent.gameObject, (int)videoPos[0].x, (int)videoPos[0].z, (int)videoPos[0].y, (int)videoPos[0].w);
                spr.leftAnchor.relative = 0;
                spr.rightAnchor.relative = 0;
                spr.topAnchor.relative = 1;
                spr.bottomAnchor.relative = 1;
                break;
        }
    }

	/// <summary>
	/// 设备下拉列表打开关闭时 调整巡航视频窗口的位置
	/// </summary>
	/// <param name="open">If set to <c>true</c> open.</param>
	public void AdjustmentPos(bool open)
	{
		switch(posValue)
		{
			case "1":
			case "0":
				if(open)
				{
					transform.localPosition += new Vector3(330,0,0);
				}
				else 
				{
					transform.localPosition -= new Vector3(330,0,0);
				}				
				break;
		}
	}

	/// <summary>
	/// 播放视频
	/// </summary>
	/// <param name="dInfo">摄像机的详细信息</param>
	/// <param name="playTime">最小播放时间</param>
	/// <param name="isLast">是否是最后一个</param>
	public void Play(DeviceInfo dInfo,string playTime,bool isLast)
	{
		Logger.Instance.WriteLog("播放视频巡航视频");
		gameObject.GetComponent<UIPanel> ().alpha = 1;
		if(preCamera != "")
		{
			isFirstTime = false;

			string endTime = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
			if(CameraOpenFail)
			{
				Logger.Instance.WriteLog("视频巡航视频打开失败");
				WriteLog(preCamera,startTime,endTime,"打开失败");
			}
			else
			{
				Logger.Instance.WriteLog("视频巡航视频正常打开");
				WriteLog(preCamera,startTime,endTime,"正常");
			}
			if(tmpText == null)
			{
				tmpText = DefaultTex;
			}
			SavePicture( preCamera + "_"  + "End_"  + System.DateTime.Parse(endTime).ToString("yyyy-MM-dd_HH-mm-ss"),tmpText.EncodeToJPG(),rotateFlip);
			startTime = endTime;
		}
		else
		{
			isFirstTime = true;
		}
		startTime = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
		preCamera = dInfo.Description;
		status = "退出";
		BtnFinsed.SetActive(false);
		BtnExit.SetActive(true);

		this.playTime = playTime;
		this.title = dInfo.Description;
		Title.text = title;
		this.isLast = isLast;
		BtnNext.SetActive(false);
		cameraId = int.Parse(dInfo.Id);
		guid = CMSManage.StringToGUID (dInfo.Guid);
		if(dInfo.UseRTSP)
		{
			rotateFlip = false;
			OpenVideoFromRTSP(dInfo.RTSPUrl);
		}
		else
		{
			rotateFlip = true;
			CMSManage.Instance.OpenCamera (cameraId,guid,StartReadMem);
		}
		GotoMonitorPos (dInfo);

		StopCoroutine ("StartTiming");
		StartCoroutine ("StartTiming",int.Parse(playTime));
	}

	private bool isFirstTime = true;
	private GameObject lingxing = null;
	void GotoMonitorPos(DeviceInfo curinfo)
	{
		Logger.Instance.WriteLog("将视角移动到指定位置");
		Camera.main.GetComponent<CameraController> ().GotoPosition (lingxing,new Vector3(float.Parse(curinfo.PosX),float.Parse(curinfo.PosY),float.Parse(curinfo.PosZ))
																	,new Vector3(float.Parse(curinfo.RotatePointPosX),0,float.Parse(curinfo.RotatePointPosZ))
		                                                            ,new Vector3(float.Parse(curinfo.CameraPosX),float.Parse(curinfo.CameraPosY),float.Parse(curinfo.CameraPosZ))
                                                                    ,Quaternion.Euler(new Vector3(float.Parse(curinfo.CameraRotatX),float.Parse(curinfo.CameraRotatY),float.Parse(curinfo.CameraRotatZ)))
                                                                    ,CameraMoveFinished);
		if(isFirstTime)
		{
			if(lingxing == null)
			{
				lingxing = Instantiate(lingxingPrefab) as GameObject;
			}
			lingxing.transform.localPosition = new Vector3(float.Parse(curinfo.PosX),float.Parse(curinfo.PosY),float.Parse(curinfo.PosZ));
		}
		else
		{
			lingxing.SetActive(true);
		}
	}

	bool moveFinished = false;
	void CameraMoveFinished()
	{
		Logger.Instance.WriteLog("角移动完成");
        CameraController.CanMoveable = true;
		moveFinished = true;
		lingxing.SetActive(false);
        BtnNext.SetActive(true);
        if (constomNext)
        {
			//自动切换到下一个巡航视频
            PlayNextVide();
        }       
	}
	/// <summary>
	/// 协同函数，视频开始播放时调用，用来判定是否已经播放了指定时间
	/// </summary>
	/// <returns>The timing.</returns>
	/// <param name="time">需要播放的时间</param>
	IEnumerator StartTiming(int time)
	{
		moveFinished = false;
		for(int i = 0; i < time; i++)
		{
			Timer.text = (time - i) + "秒";
			yield return new WaitForSeconds(1.0f);
		}
		Timer.text = 0 + "秒";
		while(true)
		{
			if(moveFinished) break;
			yield return new WaitForEndOfFrame();
		}

		if(isLast)
		{
			status = "完成";
			BtnFinsed.SetActive(true);
			BtnExit.SetActive(false);
			BtnNext.SetActive(false);
		}
	}
	/// <summary>
	/// 点击退出或完成按钮时调用，关闭相关的摄像头，使主菜单可用，关闭当前播放窗口
	/// </summary>
	public void Exit()
	{
		constomNext = false;
		MainMenuController.canNotOpen = false;
		if(!moveFinished)
		{
			//这里需要处理巡航过程中用户点击退出问题
			Camera.main.GetComponent<CameraController> ().TeleportPosition();
			CameraMoveFinished();
		}

		Logger.Instance.WriteLog("退出视频巡航窗口，并关闭相关的摄像头");
        
		if(preCamera == "") return;
		string endTime = System.DateTime.Now.ToString ("yyyy/MM/dd HH:mm:ss");
		if(tmpText == null)
		{
			tmpText = DefaultTex;
			SavePicture( preCamera + "_"  + "End_" + System.DateTime.Parse(endTime).ToString("yyyy-MM-dd_HH-mm-ss"), tmpText.EncodeToJPG(),rotateFlip);
		}

		if(status == "完成")
		{
			if(CameraOpenFail)
			{
				WriteLog(preCamera,startTime,endTime,"打开失败");
			}
			else
			{
				WriteLog(preCamera,startTime,endTime,"正常");
			}
		}
		else 
		{
			if(CameraOpenFail)
			{
				WriteLog(preCamera,startTime,endTime,"打开失败");
			}
			else
			{
				WriteLog(preCamera,startTime,endTime,"正常");
			}
		}
		preCamera = "";
		StopVideo ();

		//如果前一个播放的摄像头Id 大于0 且跟当前的摄像头ID不相等则将其关闭
		if( preCameraId > 0 && cameraId != preCameraId)
		{
			CMSManage.Instance.CloseCamera (preCameraId,preGuid);
		}
		preCameraId = 0;
		preGuid = new CMS_GUID();
		//关闭当前的摄像头
		CMSManage.Instance.CloseCamera (cameraId,guid);
		cameraId = 0;
		guid = new CMS_GUID ();

		if(ffmpeg != null)ffmpeg.CloseStream();

		ShowTex.mainTexture = DefaultTex;
		Hit.SetActive(true);
//		gameObject.SetActive (false);
		gameObject.GetComponent<UIPanel> ().alpha = 0;
	}
	/// <summary>
	/// 点击【下一个】按钮时调用,播放下一个摄像头的视频
	/// </summary>
	public void PlayNextVide()
	{
		Logger.Instance.WriteLog("播放下一个摄像头的视频");
		NextVideo ();
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
			CameraOpenFail = false;
			bool ReadSuccess = false;
			buff = new byte[sys_info.MAX_SHARE_MEMORY_SIZE];
			readMem = new shareMemOnlyRead("yfcamera_" + cameraId,(uint)buff.Length,ref ReadSuccess);
			if(ReadSuccess)
			{
				Hit.SetActive(false);
				if(preCameraId > 0)
				{
					CMSManage.Instance.CloseCamera (preCameraId, preGuid);
				}
				preCameraId = cameraId;
				preGuid = guid;
				Title.text = title;
				StopCoroutine ("readMemUpdate");
				if(tmpText != null && tmpText != DefaultTex)
				{
					Destroy(tmpText);
					tmpText = null;
				}
				StartCoroutine ("readMemUpdate");
			}
		}
		else
		{
			Logger.Instance.WriteLog("摄像机请求打开失败");
			CameraOpenFail = true;
			ShowTex.mainTexture = DefaultTex;
			tmpText = DefaultTex;

			SavePicture( preCamera + "_"  + "Start_" + System.DateTime.Parse(startTime).ToString("yyyy-MM-dd_HH-mm-ss"),tmpText.EncodeToJPG(),rotateFlip);
			Hit.SetActive(true);
		}
	}
	/// <summary>
	/// 定期从内存中读取数据
	/// </summary>
	/// <returns>The mem update.</returns>
	IEnumerator readMemUpdate()
	{
		Logger.Instance.WriteLog("定期从内存中读取图像数据");
		tmpText = null;
		vBuff = null;
		bool savPicture = true;
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
					tmpText = new Texture2D(width,height,TextureFormat.RGBA32,false);
					ShowTex.mainTexture = tmpText;
				}
				if(vBuff == null)
				{
					vBuff = new byte[width * height * 4];
				}
				
				Buffer.BlockCopy(buff, 8, vBuff, 0, width * height * 4);
				if(tmpText != null)
				{
					tmpText.LoadRawTextureData(vBuff);
					tmpText.Apply();
					if(savPicture)
					{
						SavePicture( preCamera + "_" + "Start_" + System.DateTime.Parse(startTime).ToString("yyyy-MM-dd_HH-mm-ss"),tmpText.EncodeToJPG(),rotateFlip);
						savPicture = false;
					}
				}

				yield return tmpText;
			}
			yield return new WaitForSeconds(0.05f);
		}		
	}



	private int width;
	private int heigth;
	private SharpFFmpeg.SharpFFmpeg ffmpeg;
	/// <summary>
	/// 通过rtsp打开视频流
	/// </summary>
	/// <param name="url">URL.</param>
	void OpenVideoFromRTSP(string url)
	{
		CameraOpenFail = false;
		if(ffmpeg == null)
		{
			ffmpeg = new SharpFFmpeg.SharpFFmpeg();
		}
		else
		{
			ffmpeg.CloseStream();
			readData = false;
			StopCoroutine("UpdateVideo");
		}

		bool ret = ffmpeg.OpenStream(url, out width, out heigth);
		if(ret == false)
		{
			CameraOpenFail = true;
			ShowTex.mainTexture = DefaultTex;
			tmpText = DefaultTex;
			
			SavePicture( preCamera + "_"  + "Start_" + System.DateTime.Parse(startTime).ToString("yyyy-MM-dd_HH-mm-ss"),tmpText.EncodeToJPG(),rotateFlip);
			Hit.SetActive(true);
			return;
		}
		Hit.SetActive(false);
		tmpText = new Texture2D(width, heigth, TextureFormat.RGB24, false);
		ShowTex.mainTexture = tmpText;
		ShowTex.shader = Shader.Find("Unlit/Masked Colored");
		vBuff = new byte[width * heigth * 3];
		buffer = IntPtr.Zero;
		StartCoroutine("UpdateVideo");
	}
	

	IntPtr buffer;
	bool readData = true;
	/// <summary>
	/// 从视频流中读取图片信息，并在unity中显示
	/// </summary>
	/// <returns>The video.</returns>
	IEnumerator UpdateVideo()
	{
		readData = true;
		bool savPicture = true;
		int count = 0;
		while (readData)
		{
			if (ffmpeg.GetPictureStream(vBuff))
			{
				tmpText.LoadRawTextureData(vBuff);
				tmpText.Apply();
				count++;
				if(count >= 10 && savPicture)
				{
					savPicture = false;
					SavePicture( preCamera + "_" + "Start_" + System.DateTime.Parse(startTime).ToString("yyyy-MM-dd_HH-mm-ss"),tmpText.EncodeToJPG(),rotateFlip);
				}
			}
			yield return new WaitForSeconds(0.025f);
		}
	}
}
