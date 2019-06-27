using UnityEngine;
using System.Collections;
/// <summary>
/// 对摄像机图标相关事件的操作
/// </summary>
public class MonitorIcon : MonoBehaviour {
	/// <summary>
	/// 用来判定视频播放窗口是否已经显示
	/// </summary>
	private bool isDisplayWindowShowing;
	private GameObject MonitorCtr;
	// Use this for initialization
	void Start () 
	{
		MonitorCtr = GameObject.Find ("MonitorCtr");
		isDisplayWindowShowing = false;
		UIEventListener.Get (gameObject).onDoubleClick = OpenOrCloseDisplayWindow;
	}
	/// <summary>
	/// 当摄像机图标被双击时，调用此方法
	/// 用来控制打开或关闭播放窗口
	/// </summary>
	/// <param name="go">Go.</param>
	void OpenOrCloseDisplayWindow(GameObject go)
	{
		if(isDisplayWindowShowing)
		{
			MonitorCtr.SendMessage("StopVideo",gameObject);
		}
		else
		{
			MonitorCtr.SendMessage("PlayVideo",gameObject);
		}
	}
	/// <summary>
	/// 当播放窗口关闭时将调用次方法
	/// </summary>
	void DisplayWindowClosed()
	{
		Debug.Log (gameObject.name + "关闭");
		isDisplayWindowShowing = false;
	}
	/// <summary>
	/// 当播放窗口打开时将调用次方法
	/// </summary>
	void DisplayWindowOpend()
	{
		Debug.Log (gameObject.name + "打开");
		isDisplayWindowShowing = true;
	}
	/// <summary>
	/// 设置图标颜色
	/// </summary>
	/// <param name="color">Color.</param>
	void SetColor(Color color)
	{
		GetComponent<UISprite> ().color = color;
	}
}
