using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class ShowVideoPatrolPicture : MonoBehaviour 
{
	public UITexture Tex;
	public GameObject BtnPre;
	public GameObject BtnNext;

	private string[] files;
	private string[] fileName;
	private int currentIndex;
	private int pictureCount;
	private List<VideoPatrolDetailLogInfo> logInfos;
	public void SetValue(string directoryPath,string logId)
	{
		gameObject.SetActive (true);

		fileName = null;
		currentIndex = 0;
		pictureCount = 0;

		List<FileInfo> filesList = new List<FileInfo> ();
		if (Directory.Exists(directoryPath))
		{
			files = System.IO.Directory.GetFiles(directoryPath,"*.jpg");
			pictureCount = files.Length;
			fileName = new string[pictureCount];
			for (int i = 0; i < pictureCount; i++)
			{
				filesList.Add(new FileInfo(files[i]));
			}

			filesList.Sort(delegate(FileInfo x, FileInfo y) {
				return x.LastWriteTime.CompareTo(y.LastWriteTime);
			});

			for(int i = 0; i < pictureCount; i++)
			{
				fileName[i] = filesList[i].Name.Replace(".jpg","");
			}
		}
		BtnPre.GetComponent<BoxCollider>().enabled =  false;
		BtnNext.GetComponent<BoxCollider>().enabled =  false;
		if(pictureCount > 1)
		{
			BtnNext.GetComponent<BoxCollider>().enabled =  true;
		}

		if(pictureCount >= 1)
		{

			VideoPatrolDetailLogDao vpdlDao = new VideoPatrolDetailLogDao ();
			vpdlDao.Select001 (logId);
			
			logInfos = vpdlDao.Result;

			LoadPicture (currentIndex);
		}
	}

	public void PrePicture()
	{
		Logger.Instance.WriteLog("加显示前一张图片");
		if(currentIndex >= pictureCount - 1)
		{
			BtnNext.GetComponent<BoxCollider>().enabled = true;
		}
		currentIndex--;
		if(currentIndex <= 0)
		{
			BtnPre.GetComponent<BoxCollider>().enabled = false;
		}
		LoadPicture (currentIndex);
	}

	public void NextPicture()
	{
		Logger.Instance.WriteLog("显示后一张图片");
		if(currentIndex <=0)
		{
			BtnPre.GetComponent<BoxCollider>().enabled = true;
		}
		currentIndex++;
		if(currentIndex >= pictureCount - 1)
		{
			BtnNext.GetComponent<BoxCollider>().enabled = false;
		}
		LoadPicture (currentIndex);
	}

	private void LoadPicture(int index)
	{
		Logger.Instance.WriteLog("加载需要显示的图片");
//		Name.text = fileName [currentIndex];
		VideoPatrolDetailLogInfo info = logInfos.Find (delegate(VideoPatrolDetailLogInfo obj) {
			//文件名格式 摄像机名_Start_yyyy-mm-dd_dd-mm-ss 或 摄像机名_End_yyyy-mm-dd_dd-mm-ss
			string[] split = fileName [currentIndex].Split('_');
			string time = split[2].Replace('-','/') + " " + split[3].Replace('-',':');
			if(System.DateTime.Parse(time).Equals(System.DateTime.Parse(obj.startTime)) && split[1] == "Start" && obj.camera == split[0])
			{
				return true;
			}
			if(System.DateTime.Parse(time).Equals(System.DateTime.Parse(obj.endTime)) && split[1] == "End" && obj.camera == split[0])
			{
				return true;
			}
			return false;
		});
//		Hit.text = info.memo;
		FileStream fs = new FileStream (files[index], FileMode.Open);
		Texture2D Tex2D = new Texture2D (100, 50);
		byte[] image = new byte[fs.Length];
		fs.Read(image,0,image.Length);
		Tex2D.LoadImage (image);
		Tex.mainTexture = Tex2D;
		fs.Close ();
	}

	public void Close()
	{
		gameObject.SetActive (false);
	}

	void OnDisable()
	{
		gameObject.SetActive (false);
	}
}

