using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 对请求视频播放对象请求的视频播放窗口的控制
/// </summary>
public class MonitorVideoDisplayCtr : MonoBehaviour {
	/// <summary>
	/// 播放视频的组件引用
	/// </summary>
	public GameObject DisplayWindowRef;
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
		int pos = GetWindowShowPos ();
		ShowPositionUsable [pos] = false;
		DisplayWindowShowIndex.Add(pos);
		MonitorRefs.Add (requester);
		CreatDisplayWindow (pos,"摄像机" + requester.name);
		requester.SendMessage ("DisplayWindowOpend");
		requester.SendMessage ("SetColor",ForegroundColors[pos]);
	}
	/// <summary>
	/// 停止播放视频
	/// </summary>
	/// <param name="requester">Requester.</param>
	public void StopVideo(GameObject requester)
	{
		int count = MonitorRefs.Count;
		for(int i = 0; i < count; i++)
		{
			if(requester.GetInstanceID() == MonitorRefs[i].GetInstanceID())
			{
				Destroy(DisplayWindows[i]);
				DisplayWindows.RemoveAt(i);
				ShowPositionUsable[DisplayWindowShowIndex[i]] = true;
				DisplayWindowShowIndex.RemoveAt(i);
				MonitorRefs.RemoveAt(i);
				requester.SendMessage ("DisplayWindowClosed");
				break;
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
		int pos = DisplayWindowShowIndex [0];
		DisplayWindowShowIndex.RemoveAt (0);
		ShowPositionUsable [pos] = true;
		Destroy(DisplayWindows[0]);
		DisplayWindows.RemoveAt (0);
		MonitorRefs [0].SendMessage ("DisplayWindowClosed");
		MonitorRefs.RemoveAt (0);
		return pos;
	}

	private void CreatDisplayWindow(int ShowPos,string title)
	{
		GameObject dw = Instantiate (DisplayWindowRef) as GameObject;
		dw.transform.parent = GameObject.Find ("UI Root").transform;
		dw.transform.localScale = new Vector3 (1,1,1);
		DisplayWindows.Add (dw);
		dw.SendMessage ("SetPosition",ShowPostions[ShowPos]);
		dw.SendMessage ("SetTitleBarColor",ForegroundColors[ShowPos]);
		dw.SendMessage ("SetTitle",title);

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
		ShowPostions.Add (new float[]{-510,-260,0});
		ShowPostions.Add (new float[]{-130,-260,0});
		ShowPostions.Add (new float[]{260,-260,0});
		ShowPostions.Add (new float[]{260,32,0});
	}
}
