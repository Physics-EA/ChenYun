using UnityEngine;
using System.Collections;

public class business_scene : MonoBehaviour {


	public static business_scene instance;
	public static business_scene Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new business_scene();
				//////
				///建立场景的跟节点，用来管理场景的全部动态物件 
				///////
				GameObject container = new GameObject();
				container.transform.position = new Vector3(0,0,0);
				container.transform.eulerAngles = new Vector3(0,0,0);
				container.name="gameSceneContainer";
				instance=container.AddComponent(typeof(business_scene)) as business_scene;
			}
			return instance;
		}
	}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
