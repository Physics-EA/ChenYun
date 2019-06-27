using UnityEngine;
using System.Collections;

public class AlertLogItem : MonoBehaviour {

	public UILabel No;
	public UILabel AlertTime;
	public UILabel AlertType;
	public UILabel Person;
	public UILabel StartTime;
	public UILabel EndTime;
	public UILabel Memo;
	public GameObject Background1;
	public GameObject Background2;

	public void SetValue (string no, AlertPrecessLogInfo info) 
	{
		No.text = no;
		AlertTime.text = info.alertTime;
		AlertType.text = info.alertType;
		Person.text = info.person;
		StartTime.text = info.startTime;
		EndTime.text = info.endTime;
		Memo.text = info.memo;

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
