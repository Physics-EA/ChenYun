using UnityEngine;
using System.Collections;
using System;
public class EvacuationCameraCtrl : MonoBehaviour {

	public GameObject PlayVideoPlane;
	public GameObject LingXing;
	public Shader shader;
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
	string cameraId;
	CMS_GUID guid;
	bool isPlay = false;

	public float MaxHeight;
	public float MinHeight;
	private float CameraFieldOfView = 45;
	private float DistanceFromCamera = 30;
	private float DefaultFieldAreaOfView;
	private float RealFieldAreaOfView;
	private Vector3 Scale = new Vector3(1f,1f,1f);

	void OnEnable()
	{
		float halfFOV = ( CameraFieldOfView * 0.5f ) * Mathf.Deg2Rad;
		float aspect = Camera.main.aspect;
		
		float height = DistanceFromCamera * Mathf.Tan( halfFOV );
		float width = height * aspect;
		DefaultFieldAreaOfView = width;
	}

	void Update()
	{
		float t = (Camera.main.transform.position.y - MinHeight) / (MaxHeight - MinHeight);
		float lerp = Mathf.Lerp (1.5f, 0.5f, t);
		float halfFOV = ( Camera.main.fieldOfView * 0.5f ) * Mathf.Deg2Rad;
		float aspect = Camera.main.aspect;
		
		float height = Vector3.Distance(transform.position, Camera.main.transform.position) * Mathf.Tan( halfFOV );
		float width = height * aspect;
		RealFieldAreaOfView = width;
		transform.localScale = lerp * Scale * (RealFieldAreaOfView / DefaultFieldAreaOfView);

		if(isPlay)
		{
			transform.LookAt (Camera.main.transform.position);
		}
	}

	void PlayEvacuationVideo()
	{
		Logger.Instance.WriteLog("播放监控视频");
		if(isPlay)
		{
			Logger.Instance.WriteLog("监控视频已经播放");
			return;
		}

		isPlay = true;
		PlayVideoPlane.SetActive (true);
		LingXing.SetActive (true);
		Logger.Instance.WriteLog("加载相关设备信息");
		DeviceDao dDao = new DeviceDao ();
		dDao.Select002 (gameObject.name);
		if(dDao.Result.Count == 1)
		{ 
			DeviceInfo info = dDao.Result[0];
			cameraId = info.Id;
			guid = CMSManage.StringToGUID(info.Guid);
			CMSManage.Instance.OpenCamera(int.Parse(cameraId),guid,StartReadMem);
		}
	}
	
	void StopEvacuationVideo()
	{
		Logger.Instance.WriteLog("停止播放监控视频");
		if(isPlay == false)
		{
			Logger.Instance.WriteLog("没有需要停止播放监控视频");
			return;
		}
		if(tmpText != null)Destroy(tmpText);
		if(readMem != null)readMem.Realease();
		isPlay = false;
		vBuff = null;
		StopCoroutine ("readMemUpdate");
		CMSManage.Instance.CloseCamera(int.Parse(cameraId),guid);
		PlayVideoPlane.SetActive (false);
		LingXing.SetActive (false);
	}
	/// <summary>
	/// 创建读取内存数据的对象
	/// 并执行读取操作
	/// </summary>
	void StartReadMem(bool startReadMem)
	{
		if(startReadMem)
		{
			Logger.Instance.WriteLog("创建读取内存数据的对象");
			bool ReadSuccess = false;
			buff = new byte[sys_info.MAX_SHARE_MEMORY_SIZE];
			readMem = new shareMemOnlyRead("yfcamera_" + cameraId,(uint)buff.Length,ref ReadSuccess);
			if(ReadSuccess)
			{
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
		Logger.Instance.WriteLog("定期从内存中读取数据");
		tmpText = null;
		vBuff = null;
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
					PlayVideoPlane.renderer.material.shader = shader;
					PlayVideoPlane.renderer.material.mainTexture = tmpText;
					PlayVideoPlane.transform.localScale = new Vector3(0.4f * ((float)width / height),0.4f,0.4f);
				}
				if(vBuff == null)
				{
					vBuff = new byte[width * height * 4];
				}
				
				Buffer.BlockCopy(buff, 8, vBuff, 0, width * height * 4);
				tmpText.LoadRawTextureData(vBuff);
				tmpText.Apply();
			}
			yield return new WaitForSeconds(0.05f);
		}		
	}
}
