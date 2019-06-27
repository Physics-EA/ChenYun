using UnityEngine;
using System.Collections;

public class MotionAction : MonoBehaviour {

	public float MaxHeight;
	public float MinHeight;
	private float CameraFieldOfView = 45;
	private float DistanceFromCamera = 30;
	private float DefaultFieldAreaOfView;
	private float RealFieldAreaOfView;
	private Vector3 Scale = new Vector3(1f,1f,1f);
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update () {
		float t = (Camera.main.transform.position.y - MinHeight) / (MaxHeight - MinHeight);
		float lerp = Mathf.Lerp (0.5f, 0.2f, t);
		float halfFOV = ( Camera.main.fieldOfView * 0.5f ) * Mathf.Deg2Rad;
		float aspect = Camera.main.aspect;
		
		float height = Vector3.Distance(transform.position, Camera.main.transform.position) * Mathf.Tan( halfFOV );
		float width = height * aspect;
		RealFieldAreaOfView = width;
		transform.localScale = lerp * Scale * (RealFieldAreaOfView / DefaultFieldAreaOfView);
	}

	void OnEnable()
	{
		transform.rotation = Quaternion.Euler(0,0,0);

		float halfFOV = ( CameraFieldOfView * 0.5f ) * Mathf.Deg2Rad;
		float aspect = Camera.main.aspect;
		
		float height = DistanceFromCamera * Mathf.Tan( halfFOV );
		float width = height * aspect;
		DefaultFieldAreaOfView = width;
		StartCoroutine ("Move");
		StartCoroutine ("Rotate");
	}
	/// <summary>
	/// 上下移动物体
	/// </summary>
	IEnumerator Move()
	{
		while(true)
		{
			float t = (Camera.main.transform.position.y - MinHeight) / (MaxHeight - MinHeight);
			float lerp = Mathf.Lerp (0.5f, 1f, t);
			for(int i = 0; i < 50; i++)
			{
				transform.position += new Vector3(0,0.01f * lerp,0);
				yield return new WaitForSeconds(0.02f);
			}
			
			for(int i = 0; i < 50; i++)
			{
				transform.position += new Vector3(0,-0.01f * lerp,0);
				yield return new WaitForSeconds(0.02f);
			}
		}
	}
	/// <summary>
	/// 位置Y轴自选转
	/// </summary>
	IEnumerator Rotate()
	{
		while(true)
		{
			transform.Rotate(new Vector3(0,0.5f,0));
			yield return new WaitForSeconds(0.01f);
		}
	}

	void OnDisable()
	{
		StopCoroutine ("Move");
		StopCoroutine ("Rotate");
	}
}
