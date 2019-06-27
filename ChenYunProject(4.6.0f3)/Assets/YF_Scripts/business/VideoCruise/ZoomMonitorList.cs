using UnityEngine;
using System.Collections;

public class ZoomMonitorList : MonoBehaviour {
	/// <summary>
	/// 碰撞器组件
	/// 用来控制监控器列表能否滑动
	/// </summary>
	public BoxCollider realTimeMonitor;
	/// <summary>
	/// TweenPosition 组件
	/// 用来展开或收缩监控器列表
	/// </summary>
	public TweenPosition tweenPos;
	/// <summary>
	/// UIScrollView 滑动列表组件
	/// </summary>
	public UIScrollView scrollView;
	/// <summary>
	/// 用来控制监控器列表是展开还是收缩
	/// </summary>
	private bool Reverse = false;
	/// <summary>
	/// 执行展开收缩操作
	/// </summary>
	public void Zoom()
	{
		if(tweenPos.enabled == false) tweenPos.enabled = true;
		if(Reverse)
		{
			scrollView.ResetPosition();
			tweenPos.PlayReverse ();
			Reverse = false;
			realTimeMonitor.enabled = false;
		}
		else
		{
			tweenPos.PlayForward();
			Reverse = true;
			realTimeMonitor.enabled = true;
		}
	}
}
