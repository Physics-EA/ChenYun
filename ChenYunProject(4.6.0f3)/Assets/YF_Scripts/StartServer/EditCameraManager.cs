using UnityEngine;
using System.Collections;

public class EditCameraManager : MonoBehaviour {

	public static EditCameraManager instance;

	public Transform node;

	public EditCameraWindow currentEditCameraWindow;

	public GameObject editCameraWindowPrefab;

	public CAMARE_INFO currentCameraInfo;

	void Awake () {
		instance = this;
	}

	public void CloseCamera()
	{
		if(currentEditCameraWindow != null)
		{
			currentEditCameraWindow.StopCamera();
			Object.Destroy(currentEditCameraWindow.gameObject);
		}
	}

	public void OpenCamera(int index,CAMARE_INFO info)
	{
		if(currentEditCameraWindow != null)
		{
			currentEditCameraWindow.StopCamera();
			Object.Destroy(currentEditCameraWindow.gameObject);
		}

		GameObject obj = Instantiate(editCameraWindowPrefab)as GameObject;

		obj.transform.parent = node;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = new Vector3(800,300,0);
		EditCameraWindow window = obj.GetComponent<EditCameraWindow>();
		window.transform.parent = transform;
		window.transform.localScale = new Vector3(1,1,1);
		window.OpenWindow(index,info);
		currentEditCameraWindow = window;
		currentCameraInfo = info;
	}
}
