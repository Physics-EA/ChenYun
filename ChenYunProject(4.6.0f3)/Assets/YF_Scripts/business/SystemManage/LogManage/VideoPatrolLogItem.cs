using UnityEngine;
using System.Collections;

public class VideoPatrolLogItem : MonoBehaviour {

	public UILabel No;
	public UILabel Person;
	public UILabel Date;
	public UILabel PlanName;
	public GameObject ViewPicture;
	public UIToggle Exception;

	private GameObject DetailLogPanel;
	private GameObject ShowPicturePanel;

	public GameObject backGround;

	VideoPatrolLogInfo info;
	public void SetValue(string no,VideoPatrolLogInfo info,GameObject detailLogPanel,GameObject showPicturePanel)
	{
		this.info = info;
		No.text = info.id;
		Person.text = info.person;
		Date.text = info.startDate;
		PlanName.text = info.planName;
		if(info.exception == "正常")
		{
			Exception.value = true;
		}
		else 
		{
			Exception.value = false;
		}

//		DetailLogPanel = detailLogPanel;
		ShowPicturePanel = showPicturePanel;
//		UIEventListener.Get (gameObject).onClick = ShowDetailLogPanel;
//		UIEventListener.Get (ViewPicture).onClick = OpenDirectory;
//		if(int.Parse(no) % 2 == 0)
//		{
//			Background2.SetActive(true);
//		}
//		else
//		{
//			Background1.SetActive(true);
//		}
	}

	void ShowDetailLogPanel(GameObject go)
	{
		Logger.Instance.WriteLog("打开视频巡航日志详细面板");
		DetailLogPanel.GetComponent<VideoPatrolDetailLog> ().SetValue (info);
		gameObject.transform.parent.BroadcastMessage ("OnSelected", gameObject);
	}


	public void OpenDirectory(GameObject go)
	{

		string path = Application.dataPath + "/SaveImage/" + info.planName + System.DateTime.Parse (info.startDate).ToString ("yyyyMMddHHmmss");
		ShowPicturePanel.SetActive (true);
		ShowPicturePanel.GetComponent<ShowVideoPatrolPicture> ().SetValue (path,info.id);
		//ShowDetailLogPanel (go);
	}

	void OnSelected(GameObject go)
	{
//		if(go == gameObject)
//		{
//			Background3.SetActive (true);
//		}
//		else
//		{
//			Background3.SetActive (false);
//		}
	}

	public void HoverOver()
	{
		backGround.SetActive (true);
	}

	public void HoverOut()
	{
		backGround.SetActive (false);
	}
}
