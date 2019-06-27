using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 对请求视频播放对象请求的视频播放窗口的控制
/// </summary>
public class MonitorVideoDisplayController : MonoBehaviour {
	/// <summary>
	/// 播放视频的组件预制体
	/// </summary>
	public GameObject DisplayWindowPrefab;
	/// <summary>
	/// 用来保存发送播放视频请求的对象
	/// </summary>
	private List<GameObject> MonitorRefs;
	/// <summary>
	/// 用来保存已经打开的视频窗口对象
	/// </summary>
	private List<GameObject> DisplayWindows;
	/// <summary>
	/// 用来保存已经打开的视频窗口对象所对应的位置顺序
	/// </summary>
	private List<int> DisplayWindowShowIndex;
	/// <summary>
	/// 保存用来显示的颜色
	/// </summary>
	private List<Color> ForegroundColors;
	/// <summary>
	/// 保存每个位置是否可用的状态
	/// </summary>
	private List<bool> ShowPositionUsable;
	/// <summary>
	/// 保存窗口第一次打开时的初始位置
	/// </summary>
	private List<float[]> ShowPostions;
	// Use this for initialization
	void Start () {
		InitMonitorRefs ();
		InitDisplayWindows ();
		InitDisplayWindowShowIndex ();
		InitForegroundColors ();
		InitShowPositionUsable ();
		InitShowPostions ();
	}
	/// <summary>
	/// 播放视频
	/// </summary>
	/// <param name="requester">Requester.</param>
	public void PlayVideo(GameObject requester)
	{
		Logger.Instance.WriteLog("播放视频");
		int pos = GetWindowShowPos ();
		ShowPositionUsable [pos] = false;
		DisplayWindowShowIndex.Add(pos);
		MonitorRefs.Add (requester);
		CreatDisplayWindow (pos,requester.GetComponent<MonitorInfoData>().Data.Description,requester);
		requester.SendMessage ("DisplayWindowOpend");
		requester.SendMessage ("SetColor",ForegroundColors[pos]);
	}
	/// <summary>
	/// 停止播放视频
	/// </summary>
	/// <param name="requester">Requester.</param>
	public void StopVideo(GameObject requester)
	{
		Logger.Instance.WriteLog("停止播放视频");
		int cmaeraId = int.Parse(requester.GetComponent<MonitorInfoData> ().Data.Id);
		CMS_GUID guid = CMSManage.StringToGUID (requester.GetComponent<MonitorInfoData> ().Data.Guid);
		CMSManage.Instance.CloseCamera (cmaeraId,guid);

		int count = MonitorRefs.Count;
		for(int i = 0; i < count; i++)
		{
			if(requester.GetInstanceID() == MonitorRefs[i].GetInstanceID())
			{
				StopVideo(i);
				break;
			}
		}
	}
	/// <summary>
	/// 停止播放指定的视频
	/// </summary>
	/// <param name="index">Index.</param>
	public void StopVideo(int index)
	{
		GameObject Monitor = MonitorRefs [index];
		Monitor.SendMessage ("DisplayWindowClosed");
		Destroy(DisplayWindows[index]);
		DisplayWindows.RemoveAt(index);
		ShowPositionUsable[DisplayWindowShowIndex[index]] = true;
		DisplayWindowShowIndex.RemoveAt(index);
		MonitorRefs.RemoveAt(index);
	}
	/// <summary>
	/// 根据传入的对象，是相关联的对象闪烁
	/// </summary>
	/// <param name="requester">Requester.</param>
	public void Flicker(GameObject requester)
	{
		int count = MonitorRefs.Count;
		for(int i = 0; i < count; i++)
		{
			if(requester == MonitorRefs[i])
			{
				DisplayWindows[i].SendMessage ("FlickerNoParam");
				return;
			}
		}

		for(int i = 0; i < count; i++)
		{
			if(requester == DisplayWindows[i])
			{
				MonitorRefs[i].SendMessage ("FlickerNoParam");
				return;
			}
		}
	}
	/// <summary>
	/// 获得可用的窗口显示位置顺序,
	/// 如果可用位置已经用完，则置换掉最早的一个
	/// 并向被关闭的对象发送DisplayWindowClosed消息
	/// </summary>
	/// <returns>The window show position.</returns>
	private int GetWindowShowPos()
	{
		for(int i = 0; i < ShowPositionUsable.Count; i++)
		{
			if(ShowPositionUsable[i]) return i;
		}
		StopVideo(MonitorRefs [0]);
		for(int i = 0; i < ShowPositionUsable.Count; i++)
		{
			if(ShowPositionUsable[i]) return i;
		}
		//int pos = DisplayWindowShowIndex [0];
		//DisplayWindowShowIndex.RemoveAt (0);
		//ShowPositionUsable [pos] = true;
		//Destroy(DisplayWindows[0]);
		//DisplayWindows.RemoveAt (0);
		//MonitorRefs [0].SendMessage ("DisplayWindowClosed");
		//MonitorRefs.RemoveAt (0);
		return 0;
	}
	/// <summary>
	/// 创建一个新的播放窗口
	/// </summary>
	/// <param name="ShowPos">Show position.</param>
	/// <param name="title">Title.</param>
	/// <param name="requester">Requester.</param>
	private void CreatDisplayWindow(int ShowPos,string title,GameObject requester)
	{
		GameObject dw = Instantiate (DisplayWindowPrefab) as GameObject;
//		GameObject root = GameObject.Find ("UI Root B");
//		if(root == null)
//		{
//			root = GameObject.Find("UI Root S");
//		}
		dw.transform.parent = transform;
		dw.transform.localScale = new Vector3 (1,1,1);
		DisplayWindows.Add (dw);
		dw.SendMessage ("SetPosition",ShowPostions[ShowPos],SendMessageOptions.DontRequireReceiver);
		dw.SendMessage ("SetTitleBarColor",ForegroundColors[ShowPos],SendMessageOptions.DontRequireReceiver);
		dw.SendMessage ("SetTitle",title,SendMessageOptions.DontRequireReceiver);
		dw.SendMessage ("SetCamaraRef",requester,SendMessageOptions.DontRequireReceiver);
		dw.SendMessage ("SetParentHandle",gameObject,SendMessageOptions.DontRequireReceiver);
		dw.SendMessage ("StartPlay",gameObject,SendMessageOptions.DontRequireReceiver);
		
	}
	/// <summary>
	/// 初始化 MonitorRefs.
	/// </summary>
	private void InitMonitorRefs ()
	{
		MonitorRefs = new List<GameObject> ();
	}
	/// <summary>
	/// 初始化 DisplayWindows.
	/// </summary>
	private void InitDisplayWindows ()
	{
		DisplayWindows = new List<GameObject> ();
	}
	/// <summary>
	/// 初始化 DisplayWindowShowIndex.
	/// </summary>
	private void InitDisplayWindowShowIndex()
	{
		DisplayWindowShowIndex = new List<int> ();
	}
	/// <summary>
	/// 初始化 ForegroundColors.
	/// </summary>
	private void InitForegroundColors()
	{
		ForegroundColors = new List<Color> ();
		ForegroundColors.Add (Color.red);
		ForegroundColors.Add (Color.green);
		ForegroundColors.Add (Color.blue);
		ForegroundColors.Add (Color.cyan);
	}
	/// <summary>
	/// 初始化 howPositionUsable.
	/// </summary>
	private void InitShowPositionUsable()
	{
		ShowPositionUsable = new List<bool> ();
		ShowPositionUsable.Add (true);
		ShowPositionUsable.Add (true);
		ShowPositionUsable.Add (true);
		ShowPositionUsable.Add (true);
	}
	/// <summary>
	/// 初始化 ShowPostions.
	/// </summary>
	private void InitShowPostions()
	{
		ShowPostions = new List<float[]> ();
		if(Screen.width >= 1600)
		{
			int offset = (Screen.width - 1280) / 5;
			ShowPostions.Add (new float[]{-358 - (int)(offset * 1.5f),-260,0});
			ShowPostions.Add (new float[]{-38- (int)(offset / 2),-260,0});
			ShowPostions.Add (new float[]{282 + (int)(offset / 2),-260,0});
			ShowPostions.Add (new float[]{602 + (int)(offset * 1.5f),-260,0});
			return;
		}
		if(Screen.width >= 1280)
		{
			int offset = (Screen.width - 1280) / 5;
			ShowPostions.Add (new float[]{-358 - (int)(offset * 1.5f),-252,0});
			ShowPostions.Add (new float[]{-38- (int)(offset / 2),-252,0});
			ShowPostions.Add (new float[]{282 + (int)(offset / 2),-252,0});
			ShowPostions.Add (new float[]{602 + (int)(offset * 1.5f),-252,0});

		}
		else
		{
			int offset = (Screen.width - 960) / 5;
			ShowPostions.Add (new float[]{-358 - (int)(offset * 1.5f),-244,0});
			ShowPostions.Add (new float[]{-38- (int)(offset / 2),-244,0});
			ShowPostions.Add (new float[]{282 + (int)(offset / 2),-244,0});
			ShowPostions.Add (new float[]{602 + (int)(offset * 1.5f),-244,0});
		}
	}
}
