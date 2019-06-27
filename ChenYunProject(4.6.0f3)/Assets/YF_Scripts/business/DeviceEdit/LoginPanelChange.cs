using UnityEngine;
using System.Collections;
using System.Drawing;
public class LoginPanelChange : MonoBehaviour {
	/// <summary>
	/// 正常登录界面
	/// </summary>
	public GameObject NormalLoginPanel;
	/// <summary>
	/// 设备管理登录界面
	/// </summary>
	public GameObject EditLoginPanel;

	private bool isLeftCtrlDown = false;
	private bool isLeftShiftDown = false;
	private bool isNumOneDown = false;
	private bool ShowEditLoginPanel = true;
	void Awake()
	{
		Logger.Instance.WriteLog("设置应用程序分辨率");
		Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
		Screen.SetResolution(rect.Width,rect.Height,true);
	}
	void Update()
	{
		if(isLeftCtrlDown && isLeftShiftDown && isNumOneDown && ShowEditLoginPanel)
		{
			Logger.Instance.WriteLog("切换到正常监控端场景");
			ShowEditLoginPanel = false;
			isLeftCtrlDown = false;
			isLeftShiftDown = false;
			isNumOneDown = false;

			NormalLoginPanel.SetActive(false);
			EditLoginPanel.SetActive(true);
		}

		if(isLeftCtrlDown && isLeftShiftDown && isNumOneDown && !ShowEditLoginPanel)
		{
			Logger.Instance.WriteLog("切换到编辑场景");
			ShowEditLoginPanel = true;
			isLeftCtrlDown = false;
			isLeftShiftDown = false;
			isNumOneDown = false;
			
			NormalLoginPanel.SetActive(true);
			EditLoginPanel.SetActive(false);
		}

		if(Input.GetKeyDown(KeyCode.LeftControl))
		{
			isLeftCtrlDown = true;
		}

		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			isLeftShiftDown = true;
		}

		if(Input.GetKeyDown(KeyCode.F1))
		{
			isNumOneDown = true;
		}

		if(Input.GetKeyUp(KeyCode.LeftControl))
		{
			isLeftCtrlDown = false;
		}
		
		if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			isLeftShiftDown = false;
		}
		
		if(Input.GetKeyUp(KeyCode.F1))
		{
			isNumOneDown = false;
		}
	}
}
