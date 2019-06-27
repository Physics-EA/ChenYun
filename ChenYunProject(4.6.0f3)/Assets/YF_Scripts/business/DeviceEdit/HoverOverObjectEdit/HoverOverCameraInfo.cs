using UnityEngine;
using System.Collections;

public class HoverOverCameraInfo : MonoBehaviour {

	public UILabel Name;
	public UILabel HasBindedCamera;

	public void Init(string _name,string _hasBindedCamera)
	{
		Name.text = _name;
		HasBindedCamera.text = _hasBindedCamera;
	}
}
