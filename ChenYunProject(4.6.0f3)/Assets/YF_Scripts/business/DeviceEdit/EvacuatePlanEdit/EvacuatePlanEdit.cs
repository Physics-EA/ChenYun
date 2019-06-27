using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 疏散预案配置管理脚本
/// </summary>
public class EvacuatePlanEdit : MonoBehaviour 
{

	public GameObject EditedEvacuatePlanListItemPrefab;
	public UIGrid EvacuatePlanItemGrid;
	public GameObject EvacuateAreaOfPlanPanel;
	
	//public static bool Operating = false;
	public void  Init()
	{
		StartCoroutine(LoadData());
	}
	//从数据库加载疏散预案数据
	IEnumerator LoadData()
	{
		yield return new WaitForEndOfFrame();
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		List<EvacuatePlan> ePlanList = epDao.Select003();
		foreach (var plan in ePlanList)
		{
			AddItemToEvacuatePlanItemGrid(plan);
		}

		if(ePlanList.Count > 0)
		{
			EvacuateAreaOfPlanPanel.SetActive(true);
			EvacuatePlanItemGrid.GetChild(0).GetComponent<EditedEvacuatePlanListItem>().Selected();
		}
		else
		{
			EvacuateAreaOfPlanPanel.SetActive(false);
		}
		yield return null;
	}

	void OnDisable()
	{
		EvacuatePlanItemGrid.transform.DestroyChildren();
		EvacuateAreaOfPlanPanel.GetComponent<EvacuateAreaOfPlanEdit>().Destroy();
	}

	private string NewPlanName = "新建疏散预案";
	public void AddNewPlan()
	{
		Logger.Instance.WriteLog("新建疏散预案");
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		for(int i = 1; i < int.MaxValue; i++)
		{
			//如果预案名称不存在者用此名称创建一个新的预案
			if(epDao.Select004(NewPlanName + i).Count <= 0)
			{
				epDao.Insert003(NewPlanName + i);
				List<EvacuatePlan> result = epDao.Select004(NewPlanName + i);
				if(result.Count == 1)
				{
					AddItemToEvacuatePlanItemGrid(epDao.Select004(NewPlanName + i)[0]);

					if(EvacuatePlanItemGrid.transform.childCount == 1)
					{
						EvacuateAreaOfPlanPanel.SetActive(true);
					}
					EvacuatePlanItemGrid.GetChild(EvacuatePlanItemGrid.transform.childCount - 1).GetComponent<EditedEvacuatePlanListItem>().Selected();
				}
				break;
			}
		}
	}

	/// <summary>
	/// 删除选中的项目
	/// </summary>
	public void DeleteItem()
	{
		if(EditedEvacuatePlanListItem.SelectedItem)
		{
			int ShowIndex = 0;
			if(EvacuatePlanItemGrid.GetChild(0) == EditedEvacuatePlanListItem.SelectedItem.transform)
			{
				ShowIndex = 1;
			}
			else
			{
				ShowIndex = 0;
			}
			EditedEvacuatePlanListItem.SelectedItem.OnDelete();
			UIScrollView uiScrollView =EditedEvacuatePlanListItem.SelectedItem.GetComponentInParent<UIScrollView> ();
			UIScrollBar scrollBar = (UIScrollBar)uiScrollView.verticalScrollBar;
			float offset = scrollBar.value;
			uiScrollView.ResetPosition ();
			uiScrollView.verticalScrollBar.value = offset;
			EditedEvacuatePlanListItem.SelectedItem = null;
			if(EvacuatePlanItemGrid.transform.childCount > 1)
			{
				EvacuatePlanItemGrid.GetChild(ShowIndex).GetComponent<EditedEvacuatePlanListItem>().Selected();
			}
			else
			{
				EvacuateAreaOfPlanPanel.SetActive(false);
			}
		}
	}

	private void AddItemToEvacuatePlanItemGrid(EvacuatePlan plan)
	{
		GameObject go = GameObject.Instantiate(EditedEvacuatePlanListItemPrefab) as GameObject;
		EvacuatePlanItemGrid.AddChild(go.transform);
		go.transform.localScale = new Vector3(1,1,1);
		go.GetComponent<EditedEvacuatePlanListItem>().Init(plan,EvacuateAreaOfPlanPanel);
	}
}
