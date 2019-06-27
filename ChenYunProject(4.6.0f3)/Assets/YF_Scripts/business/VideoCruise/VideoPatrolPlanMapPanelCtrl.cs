using UnityEngine;
using System.Collections;

public class VideoPatrolPlanMapPanelCtrl : MonoBehaviour {

	public UILabel TopTitle;
	public UITexture MapTexture;
	public UILabel BottomTitle;

	public void Maximum()
	{
		BottomTitle.alpha = 0;
		TopTitle.alpha = 1;
		MapTexture.alpha = 1;
	}

	public void Minimum()
	{
		BottomTitle.text = TopTitle.text;
		BottomTitle.alpha = 1;
		TopTitle.alpha = 0;
		MapTexture.alpha = 0;
	}

	void OnEnable()
	{
		BottomTitle.alpha = 0;
		TopTitle.alpha = 1;
		MapTexture.alpha = 1;
	}
}
