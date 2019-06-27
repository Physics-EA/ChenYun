using UnityEngine;
using System.Collections;
/// <summary>
/// 切换显示窗口
/// </summary>
public class SwitchWindows : MonoBehaviour {
	/// <summary>
	/// 场景主摄像机，用来显示场景中的对象
	/// </summary>
	public Camera MainCamera;
	/// <summary>
	/// UI主摄像机，用来显示最终的显示画面
	/// </summary>
	public UICamera MainUICamera;
	/// <summary>
	/// UI辅助摄像机
	/// </summary>
	public UICamera SubUICamera;
	/// <summary>
	/// 摄像机目标渲染纹理
	/// </summary>
	public RenderTexture tex;
	/// <summary>
	/// 用来显示场景的对象
	/// </summary>
	public GameObject SceneWindow;
	/// <summary>
	/// 用来显示视频的对象
	/// </summary>
	public GameObject DisplayWindow;
	/// <summary>
	/// 用来表示窗口是否已经被切换
	/// </summary>
	private bool isChange;
	// Use this for initialization
	void Start () {
		isChange = true;
		MainCamera.targetTexture = tex;
		SubUICamera.camera.targetTexture = tex;
		DisplayWindow.SendMessage("SetTitle","测试窗口");
	}
	
	// 切换窗口
	public void Switch () 
	{
		if(isChange)
		{
			/// <summary>
			/// 切换到主场景窗口
			/// </summary>
			isChange = false;
			MainCamera.targetTexture = null;
			SubUICamera.camera.targetTexture = null;
			SceneWindow.SetActive(false);
			DisplayWindow.SendMessage("SetMainWindowSize",new int[]{400,300});
			DisplayWindow.SendMessage("SetPosition",new float[]{535,-265,0});
			DisplayWindow.SendMessage("SetMovable",false);
			DisplayWindow.SendMessage("SetScalable",false);
		}
		else
		{
			/// <summary>
			/// 去换到视频播放窗口
			/// </summary>
			isChange = true;
			MainCamera.targetTexture = tex;
			SubUICamera.camera.targetTexture = tex;
			SceneWindow.SetActive(true);
			SceneWindow.transform.localPosition = new Vector3(SceneWindow.transform.localPosition.x,SceneWindow.transform.localPosition.y,-1);
			DisplayWindow.SendMessage("SetMainWindowSize",new int[]{1800,850});
			DisplayWindow.SendMessage("SetPosition",new float[]{0,-50,0});

		}
	}
}
