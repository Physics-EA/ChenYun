using UnityEngine;
using System.Collections;

public class RoleMotion : MonoBehaviour {
	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;
	private bool isInit = true;
	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(transform.position.x,0.15f,transform.position.z);
		StartCoroutine("Init");
	}

	IEnumerator Init()
	{
		while(true)
		{
			if(transform.position.z < maxZ)
			{
				transform.Rotate(0,Random.Range(0,360),0);
				isInit = false;
				break;
			}
			yield return new WaitForEndOfFrame();
			transform.position += transform.forward * 0.06f;
			transform.position = new Vector3(transform.position.x,0.15f,transform.position.z);
		}
		yield return null;
	}
	void Update () 
	{
		if(isInit) return;
		Vector3 pos = transform.position;
		if(pos.x <= minX || pos.x >= maxX || pos.z <= minZ || pos.z >= maxZ)
		{
			transform.position -= transform.forward * 0.06f;
			transform.Rotate(0,Random.Range(10,45),0);
		}
		transform.position += transform.forward * 0.06f;
		transform.position = new Vector3(transform.position.x,0.15f,transform.position.z);
	}
	
}
