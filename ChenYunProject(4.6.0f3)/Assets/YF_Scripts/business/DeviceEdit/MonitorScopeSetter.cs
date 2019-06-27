using UnityEngine;
using System.Collections;

public class MonitorScopeSetter : MonoBehaviour {

	public UISlider RadioSlider;
	public UISlider ScopeSlider;
	public UISlider OffsetSlider;
	public UILabel RadioValue;
	public UILabel ScopeValue;
	public UILabel OffsetValue;
	private GameObject monitorScope;
	private DrawSector sector;
	private bool isInit = true;
	private string id;
	public void SetValue(GameObject monitorScope,string id)
	{
		Logger.Instance.WriteLog("初始化摄像机监控区域面板");
		this.monitorScope = monitorScope;
		this.id = id;
		sector = monitorScope.GetComponent<DrawSector>();
		RadioSlider.value = sector.Radio / 100.0f;
		ScopeSlider.value = sector.Scope / 360.0f;
		OffsetSlider.value = sector.Offset / 360.0f;
		RadioValue.text = sector.Radio.ToString();
		ScopeValue.text = sector.Scope.ToString();
		OffsetValue.text = sector.Offset.ToString();

		isInit = false;
	}

	public void RedrawRadio()
	{
		if(isInit) return;
		sector.Radio = (int)(RadioSlider.value * 100);
		RadioValue.text = sector.Radio.ToString();
		sector.ReDrawSector();
	}

	public void RedrawScope()
	{
		if(isInit) return;
		sector.Scope = (int)(ScopeSlider.value * 360);
		ScopeValue.text = sector.Scope.ToString();
		sector.ReDrawSector();
	}

	public void RedrawOffset()
	{
		if(isInit) return;
		sector.Offset = (int)(OffsetSlider.value * 360);
		OffsetValue.text = sector.Offset.ToString();
		sector.ReDrawSector();
	}

	public void SaveData()
	{
		Logger.Instance.WriteLog("保存摄像机监控区域设置");
		DeviceDao dDao = new DeviceDao();
		dDao.Update004(sector.Radio.ToString(),sector.Scope.ToString(),sector.Offset.ToString(),id);
		Close();
	}

	public void Close()
	{
		Logger.Instance.WriteLog("关闭摄像机监控区域设置");
		Configure.IsOperating = false;
		gameObject.SetActive(false);
	}
}
