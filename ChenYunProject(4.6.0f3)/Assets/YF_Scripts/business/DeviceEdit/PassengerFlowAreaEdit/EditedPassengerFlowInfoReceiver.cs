using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 处理客流统计信息相关的问题
/// </summary>
public class EditedPassengerFlowInfoReceiver : MonoBehaviour {

	void OnEnable()
	{
		StartCoroutine("GetPassengerFlow");
		//StartCoroutine("Test");
	}
//	IEnumerator Test()
//	{
//		while(true)
//		{
//			Debug.Log("Test");
//			PassengerFlow(0,0,0,"172.22.20.233");
//			PassengerFlow(1,1,0,"172.22.20.233");
//			PassengerFlow(2,0,0,"172.22.20.233");
//			yield return new WaitForSeconds(0.1f);
//		}
//
//	}
	public delegate void DelEditedPassengerFlowArea(Dictionary<string,PassengerFlowData> data);

	//保存需要更新客流数据的区域
	[HideInInspector]
	public static DelEditedPassengerFlowArea PFArea;
	/// <summary>
	/// 从服务器接收客流统计信息
	/// </summary>
	/// <returns>The passenger flow.</returns>
	IEnumerator GetPassengerFlow()
	{
		while(true)
		{
			if( CMSManage.Instance != null && CMSManage.Instance.isConnecting())
			{
				CMSManage.Instance.GetPassengerFlow (PassengerFlow);
				break;
			}
			yield return new WaitForSeconds(1);
		}
		yield return null;
	}
	private int PassengerSumCount = 0;
	private int PassengerEnterCount = 0;
	private int PassengerExitCount = 0;
	private Dictionary<string,PassengerFlowData> DicPassengerFlowData= new Dictionary<string, PassengerFlowData>();

	private PassengerFlowData pfd = new PassengerFlowData();
	string DicPassengerFlowDataKey = "";
	void PassengerFlow(int channel,int ruleId,int count,string ipFrom)
	{
		DicPassengerFlowDataKey = channel + ":" + ipFrom;
		if(!DicPassengerFlowData.ContainsKey(DicPassengerFlowDataKey))
		{
			pfd.channel = channel;
			pfd.ipFrom = ipFrom;
			pfd.EnterCount = 0;
			pfd.ExitCount = 0;
			pfd.SumCount = 0;
			DicPassengerFlowData[DicPassengerFlowDataKey] = pfd;
		}
		else
		{
			pfd = DicPassengerFlowData[DicPassengerFlowDataKey];
		}
		if(ruleId == 0)
		{
			pfd.SumCount += ( 1 + count );
			PassengerEnterCount += ( 1 + count );
			pfd.EnterCount += ( 1 + count );
		}
		else if(ruleId == 1)
		{
			pfd.SumCount -= ( 1 + count);
			PassengerExitCount += ( 1 + count );
			pfd.ExitCount += ( 1 + count );
			if(pfd.SumCount < 0)
			{
				pfd.SumCount = 0;
			}
		}
		else
		{
			return;
		}
		DicPassengerFlowData[DicPassengerFlowDataKey] = pfd;
		if(PFArea != null)
			PFArea.Invoke(DicPassengerFlowData);
	}
}
