using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
public class PlayMonitorVideo : MonoBehaviour {

	public UITexture ShowTex;

	public string CamaraName;
	private Texture2D txt2dtest = null;
	private shareMemOnlyRead readMem;
	private bool success;
	private byte[] buff;
	byte[] vBuff = null;
	static int count = 1;
	void Start()
	{
		success = false;
		buff = new byte[1280*720*4*4 + 8];
		readMem = new shareMemOnlyRead("yfcamera_" + count,1280*720*4*4 + 8,ref success);
		StartCoroutine ("readMemUpdate");
		count++;
	}

	IEnumerator readMemUpdate()
	{
		if(success)
		{
			while(true)
			{
				bool readSuccess = readMem.read(buff);
				if(readSuccess)
				{
					Int32 width;
					Int32 height;
					width = BitConverter.ToInt32(buff,0);
					height = BitConverter.ToInt32(buff,4);
					if(txt2dtest == null)
					{
						txt2dtest = new Texture2D(width,height,TextureFormat.ARGB32,false);
					}
					if(vBuff == null)
					{
						vBuff = new byte[width * height * 4];
					}
					Buffer.BlockCopy(buff, 8, vBuff, 0, width * height * 4);
					txt2dtest.LoadRawTextureData(vBuff);
					txt2dtest.Apply();
					ShowTex.mainTexture = txt2dtest;
				}
				yield return new WaitForSeconds(0.05f);
			}

		}

	}

	void OnDisable()
	{
		StopCoroutine ("readMemUpdate");
		count--;
	}
}
