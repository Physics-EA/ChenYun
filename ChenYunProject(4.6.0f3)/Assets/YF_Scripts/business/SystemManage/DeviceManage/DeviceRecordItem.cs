using UnityEngine;
using System.Collections;

/// <summary>
/// Device record item.
/// </summary>
public struct DeviceRecordInfo
{
	/// <summary>
	/// 显示序号
	/// </summary>
	public string No;
	/// <summary>
	/// 设备信息
	/// </summary>
	public DeviceInfo DInfo;
}
/// <summary>
/// 对设备记录进行相关的操作
/// </summary>
public class DeviceRecordItem : MonoBehaviour {
	/// <summary>
	/// 显示序号的标签
	/// </summary>
	public UILabel No;
	/// <summary>
	/// 显示描述的标签
	/// </summary>
	public GameObject Description;
	/// <summary>
	/// 预设位设置按钮
	/// </summary>
	public GameObject BtnPresetPos;
	/// <summary>
	/// 被选中是的背景图片
	/// </summary>
	public GameObject Background;
	/// <summary>
	/// 打开详细信息窗口时需要挂载的父对象
	/// </summary>
	private GameObject root;
	/// <summary>
	/// 保存当前记录的信息
	/// </summary>
	private DeviceRecordInfo RecordInfo;

	private static DeviceRecordItem HoverItem;
	/// <summary>
	/// 预设位设置窗口
	/// </summary>
	private GameObject PresetPosWindow;
	/// <summary>
	/// 将给定的数据进行设置
	/// </summary>
	/// <param name="Info">Info.</param>
	public void Init(DeviceRecordInfo Info, GameObject _PresetPosWindow, bool forbidden)
	{
		RecordInfo = Info;
		No.text = RecordInfo.No;
		initDescription = RecordInfo.DInfo.Description;
		PresetPosWindow =_PresetPosWindow;
		Description.GetComponent<UILabel>().text = RecordInfo.DInfo.Description;
		if(forbidden)
		{
			BtnPresetPos.GetComponent<BoxCollider>().enabled = false;
		}

	}
	/// <summary>
	/// 用来打开预设位管理界面
	/// </summary>
	public void ShowPrestPositionPanel()
	{
		Logger.Instance.WriteLog("打开预设位管理界面");
		PresetPosWindow.SetActive(true);
		PresetPosWindow.GetComponent<PresetPositionManage> ().SetValue (RecordInfo.DInfo);
	}
	/// <summary>
	/// 把当前摄像头视角保存到数据库中
	/// </summary>
	public void SaveCurrentPosition()
	{
		Logger.Instance.WriteLog("把当前摄像头视角保存到数据库中");
		GameObject camera = Camera.main.gameObject;
		GameObject point = camera.GetComponent<CameraController> ().RotatePoint;
		DeviceDao dDao = new DeviceDao ();

		dDao.Update001 (point.transform.position.x,point.transform.position.y,point.transform.position.z,
		                camera.transform.position.x,camera.transform.position.y,camera.transform.position.z,
		                camera.transform.rotation.eulerAngles.x,camera.transform.rotation.eulerAngles.y,camera.transform.rotation.eulerAngles.z,RecordInfo.DInfo.Id);

		dDao.Select003 (RecordInfo.DInfo.Id);
		if(dDao.Result.Count == 1)
		{
			RecordInfo.DInfo = dDao.Result[0];
		}
		Select();
	}

	bool Selected = false;
	void Select()
	{
		if(!Selected)
		{
			if(HoverItem)HoverItem.Deselected();
			HoverItem = this;
			Selected = true;
			Background.SetActive (true);
		}
	}
	/// <summary>
	/// 取消选中状态
	/// </summary>
	void Deselected()
	{
		Selected = false;
		Background.SetActive (false);
	}
	/// <summary>
	/// 将场景中的镜头移动指定设备所在位置
	/// </summary>
	/// <param name="go">Go.</param>
	public void GotoMornitorPosition(GameObject go)
	{
		Logger.Instance.WriteLog("将场景中的镜头移动指定设备所在位置");
		DeviceInfo info = RecordInfo.DInfo;
		if(info.RotatePointPosX.Trim() == "" || info.RotatePointPosY.Trim() == "" || info.RotatePointPosZ.Trim() == "" || 
		   info.CameraPosX.Trim() == "" || info.CameraPosY.Trim() == "" || info.CameraPosZ.Trim() == "" || 
		   info.CameraRotatX.Trim() == "" || info.CameraRotatY.Trim() == "" || info.CameraRotatZ.Trim() == "")
		{
			Camera.main.GetComponent<CameraController> ().GotoPosition(new Vector3(float.Parse(info.PosX),0,float.Parse(info.PosZ)));
		}
		else
		{
			Camera.main.GetComponent<CameraController> ().GotoPosition (new Vector3(float.Parse(info.RotatePointPosX),0,float.Parse(info.RotatePointPosZ))
			                                                            ,new Vector3(float.Parse(info.CameraPosX),float.Parse(info.CameraPosY),float.Parse(info.CameraPosZ))
			                                                            ,new Vector3(float.Parse(info.CameraRotatX),float.Parse(info.CameraRotatY),float.Parse(info.CameraRotatZ)),null,true);
		}
		Select();
	}

	bool inputChanged = false;
	string initDescription = "";
	public void InputValueChanged()
	{
		inputChanged = false;
		if(initDescription != Description.GetComponent<UILabel>().text)
		{
			initDescription = Description.GetComponent<UILabel>().text;
			inputChanged = true;
		}
	}

	public void SaveDescription()
	{
		if(inputChanged)
		{
			Logger.Instance.WriteLog("保存设备名称修改");
			inputChanged = false;

			DeviceDao dDao = new DeviceDao ();
			dDao.Update003(Description.GetComponent<UILabel>().text,RecordInfo.DInfo.Id);
		}
	}
}
