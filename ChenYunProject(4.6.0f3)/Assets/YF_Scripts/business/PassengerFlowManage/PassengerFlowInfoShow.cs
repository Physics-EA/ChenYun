using UnityEngine;
using System.Collections;
/// <summary>
/// 用来显示客流统计详细信息
/// </summary>
public class PassengerFlowInfoShow : MonoBehaviour {

	public UILabel Title;
	public UILabel SumCount;
	public UILabel EnterCount;
	public UILabel ExitCount;
	private GameObject Owner;
	/// <summary>
	/// 切换信息是调用
	/// </summary>
	/// <param name="_Owner">_ owner.</param>
	/// <param name="_Title">_ title.</param>
	public void SwitchOwner(GameObject _Owner,string _Title)
	{
		if(Owner != null) Owner.SendMessage("StopShowInfo",SendMessageOptions.DontRequireReceiver);
		Owner = _Owner;
		Title.text = _Title;
	}
	/// <summary>
	/// 更新显示信息
	/// </summary>
	/// <param name="sum">Sum.</param>
	/// <param name="enter">Enter.</param>
	/// <param name="exit">Exit.</param>
	public void UpdateShowInfo(string sum,string enter,string exit)
	{
		SumCount.text = "当前游客人数：" + sum + "人";
		EnterCount.text = "       进入人数：" + enter + "人";
		ExitCount.text = "       离开人数：" + exit + "人";
	}
	/// <summary>
	/// 关闭信息显示面板，并通知调用者
	/// </summary>
	public void Close()
	{
		if(Owner != null) Owner.SendMessage("StopShowInfo",SendMessageOptions.DontRequireReceiver);
		Owner = null;
		gameObject.SetActive(false);
	}
}
