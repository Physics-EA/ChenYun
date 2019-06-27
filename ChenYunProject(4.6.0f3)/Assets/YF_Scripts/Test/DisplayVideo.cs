using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using libYunFuCSV;
/// <summary>
/// 播放视频
/// </summary>
public class DisplayVideo : MonoBehaviour {
	/// <summary>
//	/// 纹理数组用来存放播放的图片
//	/// </summary>
//	private List<Texture> Texs;
//	/// <summary>
//	/// 初始化
//	/// </summary>
//	void Start () 
//	{
//		Texs = new List<Texture> ();
//		for(int i = 1; i <= 10; i++)
//		{
//			Texture tex = Resources.Load<Texture>("image/" + i);
//			Texs.Add(tex);
//		}
//		StartCoroutine ("Display");
//	}
//	/// <summary>
//	/// 每隔 1.0f / 26 秒播放一帧动画
//	/// </summary>
//	IEnumerator Display()
//	{
//		while(true)
//		{
//			for(int i = 0; i <= 9; i++)
//			{
//				GetComponent<UITexture>().mainTexture = Texs[i];
//				yield return new WaitForSeconds(1.0f / 26);
//			}
//		}
//
//	}

	LibCSV libcsv;
	Texture2D tex;

	void Start ()
	{
		string path = Application.dataPath;
		string sdkpath = path + "/../SDKDLL";
		libcsv = LibCSV.InitLib(sdkpath);
		tex = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
		GetComponent<UITexture>().mainTexture = tex;
		StartCoroutine ("Display");
	}
	/// <summary>
	/// 每隔 1.0f / 26 秒播放一帧动画
	/// </summary>
	IEnumerator Display()
	{
		while(true)
		{
			byte[] videoData = LibCSV.GetCurrentVideoData();
			if (tex != null && videoData != null)
			{
				if(videoData.Length == tex.width * tex.height * 4)
				{
					tex.LoadRawTextureData(videoData);
					tex.Apply();
					LibCSV.SetWrite();
				}
			}
			yield return new WaitForSeconds(1.0f / 26);
		}

	}

	void OnDestroy()
	{
		if (libcsv !=  null)
		{
			LibCSV.unInitLib();
		}
		
	}
}
