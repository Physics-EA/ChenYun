using UnityEngine;
using System.Collections;

public class WarnWindow : MonoBehaviour {

	public GameObject SameNameWarn;
	public GameObject IsOperationWarn;
	private static WarnWindow _instance;
	public static WarnWindow Instance
	{
		get
		{
			return _instance;
		}
	}
	void Start()
	{
		_instance = this;
	}

	public void Show(WarnType type)
	{
		switch(type)
		{
			case WarnType.SameName:SameNameWarn.SetActive(true);break;
			case WarnType.IsOperation:IsOperationWarn.SetActive(true);break;
		}
	}
	public void Close()
	{
		SameNameWarn.SetActive(false);
		IsOperationWarn.SetActive(false);
	}

	public enum WarnType
	{
		SameName,
		IsOperation
	}
}
