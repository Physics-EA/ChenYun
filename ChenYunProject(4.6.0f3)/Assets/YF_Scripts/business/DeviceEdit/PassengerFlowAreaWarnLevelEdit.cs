using UnityEngine;
using System.Collections;
/// <summary>
/// 设置客流统计区域个报警级别人数阈值设定
/// </summary>
public class PassengerFlowAreaWarnLevelEdit : MonoBehaviour {

	public UIInput level1;
	public UIInput level2;
	public UIInput level3;

	public GameObject level2Warn;
	public GameObject level3Warn;

	private PassengerFlowAreaListItem PFALItem;
	/// <summary>
	/// 初始化时调用 显示报警级别人数阈值信息
	/// </summary>
	/// <param name="_PFALItem">_ PFAL item.</param>
	public void SetValue(PassengerFlowAreaListItem _PFALItem)
	{
		Logger.Instance.WriteLog("初始化客流统计区域报警级别人数阈值设置面板");
		PFALItem = _PFALItem;
		level1.value = PFALItem.info.WarnLevel1;
		level2.value = PFALItem.info.WarnLevel2;
		level3.value = PFALItem.info.WarnLevel3;
	}
	/// <summary>
	/// 当警戒等级1被修改时调用
	/// 如果修改后的值等于0，则将警戒等级2，警戒等级3的值设置成0
	/// 如果修改后的值大于等于警戒等级2，则将警戒等级2值设置成警戒等级1的值加1
	/// 如果警戒等级2修改后的值大于等于警戒等级3，则将警戒等级3值设置成警戒等级2的值加1
	/// </summary>
	public void OnLevel1Change()
	{
		level2Warn.SetActive(false);
		level3Warn.SetActive(false);
		if(int.Parse(level2.value) <= int.Parse(level1.value))
		{
			level2Warn.SetActive(true);
		}
		if(int.Parse(level2.value) >= int.Parse(level3.value))
		{
			level3Warn.SetActive(true);
		}
	}
	/// <summary>
	/// 当警戒等级2被修改时调用
	/// 如果修改后的值小于等于警戒等级1的值，则设置成等级1的值加1
	/// 如果修改后的值大于等于警戒等级3的值，则警戒等级3的值设置成等级2的值加1
	/// </summary>
	public void OnLevel2Change()
	{
		level2Warn.SetActive(false);
		level3Warn.SetActive(false);
		if(int.Parse(level2.value) <= int.Parse(level1.value))
		{
			level2Warn.SetActive(true);
		}
		if(int.Parse(level2.value) >= int.Parse(level3.value))
		{
			level3Warn.SetActive(true);
		}
	}
	/// <summary>
	/// 当警戒等级3被修改时调用
	/// 如果修改后的值小于等于警戒等级2的值，则设置成等级2的值加1
	/// </summary>
	public void OnLevel3Change()
	{
		level3Warn.SetActive(false);
		if(int.Parse(level3.value) <= int.Parse(level2.value))
		{
			level3Warn.SetActive(true);
		}
	}
	/// <summary>
	/// 保存阈值信息
	/// 阈值要大于0
	/// 且等级二要大于等级一
	/// 等级三要大于等级二
	/// </summary>
	public void SaveData()
	{
		if(int.Parse(level1.value) > 0)
		{
			if( int.Parse(level2.value) > int.Parse(level1.value) && int.Parse(level3.value) > int.Parse(level2.value))
			{
				PFALItem.info.WarnLevel1 = level1.value;
				PFALItem.info.WarnLevel2 = level2.value;
				PFALItem.info.WarnLevel3 = level3.value;
				PFALItem.UpdateWarnLevel();
			}
		}
		Close();
	}

	public void Close()
	{
		Logger.Instance.WriteLog("关闭客流统计区域报警级别人数阈值设置面板");
		Configure.IsOperating = false;
		gameObject.SetActive(false);
	}

}
