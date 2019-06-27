using UnityEngine;
using System.Collections;
using System;
public class VideoPatrolPlanItem : MonoBehaviour {

	public UILabel No;
	public UILabel PlanName;
	public UILabel CreateTime;
	public UILabel Creator;
	public GameObject Background;
	public VideoPatrolPlanInfo info;
	public void Init(VideoPatrolPlanInfo _info)
	{
		info = _info;
		No.text = info.Id;
		PlanName.text = info.Name;
		CreateTime.text = DateTime.Parse(info.CreateTime).ToString("yyyMMdd");
		Creator.text = info.AccountName;
	}


	public void Selected()
	{
		Background.SetActive(true);
	}

	public void Deselected()
	{
		Background.SetActive(false);
	}

	public void UpdateName(string newName)
	{
		PlanName.text = newName;
		info.Name = newName;
	}
}
