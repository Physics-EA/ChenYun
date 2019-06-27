using UnityEngine;
using System.Collections;

public class VideoPatrolDetailLogItem : MonoBehaviour {

	public UILabel LBL_No;
	public UILabel LBL_Camera;
	public UILabel LBL_StartTime;
	public UILabel LBL_EndTime;
	public UILabel LBL_Memo;
	public GameObject Background1;
	public GameObject Background2;

	public void SetValue(string no,VideoPatrolDetailLogInfo info)
	{
		LBL_No.text = no;
		LBL_Camera.text = info.camera;
		LBL_StartTime.text = info.startTime;
		LBL_EndTime.text = info.endTime;
		LBL_Memo.text = info.memo;

		if(int.Parse(no) % 2 == 0)
		{
			Background2.SetActive(true);
		}
		else
		{
			Background1.SetActive(true);
		}
	}
}
