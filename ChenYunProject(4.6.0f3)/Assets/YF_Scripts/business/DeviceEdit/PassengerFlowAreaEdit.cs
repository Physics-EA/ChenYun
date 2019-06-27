using UnityEngine;
using System.Collections;

public class PassengerFlowAreaEdit : MonoBehaviour 
{
	private Vector3 HitPoint;
	private Vector3 OffsetHitPoint;
	private Vector3 EedHitPoint;
	private int quadrant;
	private bool MouseDown;
	private Vector3 center;
	private Vector2 preMousePos;
	private Vector2 curMousePos;
	private Vector3 tmpOffsetHitPoint;
	void Awake()
	{
		center = transform.renderer.bounds.center;
	}
	void Update()
	{
		if(_Modify == false)return;
		curMousePos = Input.mousePosition;
		if(MouseDown && preMousePos != curMousePos)
		{
			preMousePos = curMousePos;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit,1000,1 << LayerMask.NameToLayer("Ground")))
			{
				EedHitPoint = hit.point;
				OffsetHitPoint = EedHitPoint - HitPoint;
				OffsetHitPoint.y = 0;
				tmpOffsetHitPoint= OffsetHitPoint;
				switch(quadrant)
				{
				case 1: break;
				case 2: OffsetHitPoint.x *= -1f;break;
				case 3: OffsetHitPoint *= -1f;break;
				case 4: OffsetHitPoint.z *= -1f;break;
				default:return;
				}
				transform.localScale += new Vector3(OffsetHitPoint.x,0.0f,OffsetHitPoint.z);
				transform.position += tmpOffsetHitPoint * 0.5f;
				HitPoint = EedHitPoint;
			}
			
		}
	}
	
	void OnMouseDown()
	{
		if(_Modify == false)return;
		quadrant = 0;
		preMousePos = Input.mousePosition;
		curMousePos = preMousePos;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit,1000,1 << LayerMask.NameToLayer("PassengerFlowArea")))
		{
			CameraController.CanMoveable = false;
			MouseDown = true;
			HitPoint = hit.point;
			Vector3 direction = HitPoint - center;
			if(direction.x > 0 && direction.z > 0)
			{
				quadrant = 1;
			}
			else if(direction.x < 0 && direction.z > 0)
			{
				quadrant = 2;
			}
			else if(direction.x < 0 && direction.z < 0)
			{
				quadrant = 3;
			}
			else if(direction.x > 0 && direction.z < 0)
			{
				quadrant = 4;
			}
			else
			{
				MouseDown = false;
			}
		}
	}
	void OnMouseUp()
	{
		if(_Modify == false)return;
		CameraController.CanMoveable = true;
		MouseDown = false;
		quadrant = 0;
		preMousePos = Input.mousePosition;
		curMousePos = preMousePos;
	}
	private bool _Modify = false;
	public void Modify()
	{
		_Modify = true;
	}

	public void FinishedModify()
	{
		CameraController.CanMoveable = true;
		_Modify = false;
	}
}
