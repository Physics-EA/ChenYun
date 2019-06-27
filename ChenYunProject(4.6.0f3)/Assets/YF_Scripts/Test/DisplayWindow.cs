using UnityEngine;
using System.Collections;

public class DisplayWindow : MonoBehaviour {
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
	/// 初始化对象
	/// </summary>
	void Start () {
//		Title = transform.FindChild ("TitleBar").FindChild ("Title").GetComponent<UILabel> ();
//		TitleBar = transform.FindChild ("TitleBar").GetComponent<UISprite> ();
//		MainWindow = transform.FindChild ("MainWindow").gameObject;
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
}
