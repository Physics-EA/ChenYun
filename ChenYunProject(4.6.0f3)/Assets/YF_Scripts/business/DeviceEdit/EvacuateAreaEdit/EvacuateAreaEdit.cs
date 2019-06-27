using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EvacuateAreaEdit : MonoBehaviour
{
	public UIGrid EvacuateAreaListGrid;
	public GameObject EditedEvacuateAreaListItemPrefab;
	public GameObject EvacuateAreaDevicePanel;

	void Awake()
	{
		EvacuateAreaDevicePanel.SetActive(false);
	}

	public void Init()
	{
		StartCoroutine(LoadData());
	}
	//从数据库加载疏散预案数据
	IEnumerator LoadData()
	{
		Logger.Instance.WriteLog("从数据库加载疏散预案数据");
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		List<EvacuateArea> eAreaList = epDao.Select001();
		foreach (var area in eAreaList)
		{
			AddItemToEvacuateAreaItemGrid(area);
		}
		if(eAreaList.Count > 0)
		{
			EvacuateAreaDevicePanel.SetActive(true);
			EvacuateAreaListGrid.GetChild(0).GetComponent<EditedEvacuateAreaListItem>().Selected();
		}
		yield return null;
	}

	private string NewAreaName = "新建疏散区域";
	public void AddNewArea()
	{
		Logger.Instance.WriteLog("新建疏散区域");
//		if(Configure.IsOperating)
//		{
//			Logger.Instance.WriteLog("正在执行其它操作");
//			return;
//		}
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		for(int i = 1; i < int.MaxValue; i++)
		{
			//如果区域名称不存在者用此名称创建一个新的区域
			if(epDao.Select005(NewAreaName + i).Count <= 0)
			{
				epDao.Insert001(NewAreaName + i,"20"," ");
				List<EvacuateArea> result = epDao.Select005(NewAreaName + i);
				if(result.Count == 1)
				{
					AddItemToEvacuateAreaItemGrid(result[0]);
					if(EvacuateAreaListGrid.transform.childCount == 1)
					{
						EvacuateAreaDevicePanel.SetActive(true);
					}
					EvacuateAreaListGrid.GetChild(EvacuateAreaListGrid.transform.childCount - 1).GetComponent<EditedEvacuateAreaListItem>().Selected();
				}
				break;
			}
		}
	}

	public void DeletArea()
	{
		if(EditedEvacuateAreaListItem.SelectedItem)
		{
			int ShowIndex = 0;
			if(EvacuateAreaListGrid.GetChild(0) == EditedEvacuateAreaListItem.SelectedItem.transform)
			{
				ShowIndex = 1;
			}
			else
			{
				ShowIndex = 0;
			}
			EditedEvacuateAreaListItem.SelectedItem.Delete();
			UIScrollView uiScrollView =EditedEvacuateAreaListItem.SelectedItem.GetComponentInParent<UIScrollView> ();
			UIScrollBar scrollBar = (UIScrollBar)uiScrollView.verticalScrollBar;
			float offset = scrollBar.value;
			uiScrollView.ResetPosition ();
			uiScrollView.verticalScrollBar.value = offset;
			EditedEvacuateAreaListItem.SelectedItem = null;
			int count = EvacuateAreaListGrid.GetChildList().Count;
			if(count > 1)
			{
				EvacuateAreaListGrid.GetChildList()[ShowIndex].GetComponent<EditedEvacuateAreaListItem>().Selected();
			}
			else
			{
				EvacuateAreaDevicePanel.SetActive(false);
			}
		}
	}

	private void AddItemToEvacuateAreaItemGrid(EvacuateArea area)
	{
		GameObject go = GameObject.Instantiate(EditedEvacuateAreaListItemPrefab) as GameObject;
		EvacuateAreaListGrid.AddChild(go.transform);
		go.transform.localScale = new Vector3(1,1,1);
		go.GetComponent<EditedEvacuateAreaListItem>().Init(area,EvacuateAreaDevicePanel);
		
	}

	void OnDisable()
	{
		EvacuateAreaListGrid.transform.DestroyChildren();
		EvacuateAreaDevicePanel.SetActive(false);
	}
}
