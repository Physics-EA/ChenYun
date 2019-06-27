using UnityEngine;
using System.Collections;

public class SelecteMornitorItem : MonoBehaviour {

	public UILabel No;
	public UILabel Name;
	public GameObject AddBtn;
	public GameObject Background;

	public DeviceInfo info;
	public DelAction DelAddItem;

	private static SelecteMornitorItem SelectedItem;
	public void Init(DeviceInfo _info)
	{
		info = _info;
		No.text = info.Id;
		Name.text = info.Description;
	}

	public void BindAction(DelAction _DelAddItem)
	{
		DelAddItem = _DelAddItem;
	}

	public void GotoPosition(GameObject go)
	{
		Camera.main.GetComponent<CameraController> ().GotoPosition(new Vector3( float.Parse(info.PosX),0,float.Parse(info.PosZ)),2);
		Select();
	}

	public void AddItemToSelectedList(GameObject go)
	{
		if(DelAddItem != null)DelAddItem.Invoke(gameObject);
		Select();
	}

	bool Selected = false;
	private void Select()
	{
		if(!Selected)
		{
			if(SelectedItem)SelectedItem.Deselect();
			Selected = true;
			SelectedItem = this;
			Background.SetActive(true);
		}
	}

	private void Deselect()
	{
		Selected = false;
		Background.SetActive(false);
	}
}
