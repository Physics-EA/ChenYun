using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class EditCameraWindow : MonoBehaviour {



	/// <summary>
	/// 标题
	/// </summary>
	public UILabel lable_title;
	/// <summary>
	/// 主窗口，用来显示内容
	/// </summary>
	public GameObject mainWindow;

	/// <summary>
	/// 显示图片的对象
	/// </summary>
	public UITexture tex_showTex;
	/// <summary>
	/// 显示提示信息
	/// </summary>
	public GameObject lablt_tips;

	/// <summary>
	/// 保存临时图片信息的对象
	/// </summary>
	private Texture2D tex_tex2dTest = null;
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
	private byte[] vBuff = null;

	/// <summary>
	/// 当前打开的摄像机的ID
	/// </summary>
	private int currntCameraID = 1;
	/// <summary>
	/// 当前打开的摄像机的GUID
	/// </summary>
	private CMS_GUID currentCameraGUID;

	/// <summary>
	/// 用来保存当前窗口所显示内容的对象
	/// </summary>
	private GameObject cameraRef;

	/// <summary>
	/// 用来管理与监控服务器通信的对象
	/// </summary>
	private CMSManage cmsManageInstance;


	private float tick = 60;



	private int index;


	void Start () {



	}


	public void OpenWindow(int _index,CAMARE_INFO info)
	{
		index = _index;
		cmsManageInstance = CMSManage.Instance;
		currentCameraGUID = info.camareGuid;
		cmsManageInstance.OpenCamera(index,currentCameraGUID,StartReadMem);
	}


	private void StartReadMem(bool startReadMem)
	{
		if(startReadMem)
		{
			bool success =false;
			buff = new byte[1280*720*4*4 + 8];
			readMem = new shareMemOnlyRead ("yfcamera_" + index,1280*720*4*4 + 8,ref success);
			if(success)
			{
				lablt_tips.SetActive(false);
				StartCoroutine ("readMemUpdate");
			}
		}
	}

	/// <summary>
	/// 定期从内存中读取数据
	/// </summary>
	/// <returns>The mem update.</returns>
	private IEnumerator readMemUpdate()
	{
		while(true)
		{
			bool readSuccess = readMem.read(buff);
			if(readSuccess)
			{
				Int32 width;
				Int32 heigth;
				width = BitConverter.ToInt32(buff,0);
				heigth = BitConverter.ToInt32(buff,4);

				if(width == 0 || heigth == 0)
				{
					yield return new WaitForSeconds(0.01f);
					continue;
				}

				if(tex_tex2dTest == null)
				{
					tex_tex2dTest = new Texture2D(width,heigth,TextureFormat.ARGB32,false);
					tex_showTex.mainTexture = tex_tex2dTest;
				}

				if(vBuff == null)
				{
					vBuff = new byte[width * heigth * 4];
				}

				Buffer.BlockCopy(buff,8,vBuff,0,width*heigth*4);
				tex_tex2dTest.LoadRawTextureData(vBuff);
				tex_tex2dTest.Apply();
			}
			yield return new WaitForSeconds(0.05f);
		}
	}

	public void StopCamera()
	{
		StopCoroutine ("readMemUpdate");
		if(tex_tex2dTest)Destroy(tex_tex2dTest);
		cmsManageInstance.CloseCamera(index,currentCameraGUID);
	}
}
