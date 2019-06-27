using UnityEngine;
using System.Collections;

public delegate void DelEvacuatePlanNameChanged(string newName);
public class EditedEvacuatePlanListItem : MonoBehaviour
{
	public UILabel No;
	public UILabel Name;
	/// <summary>
	/// 被选择是的背景色
	/// </summary>
	public GameObject SelectedBg;

	public static EditedEvacuatePlanListItem SelectedItem;
	private EvacuatePlan evacuatePlan;
	private GameObject areaOfPlanPanel;

	//初始化对象
	public void Init(EvacuatePlan _evacuatePlan,GameObject _areaOfPlanPanel)
	{
		Logger.Instance.WriteLog("初始化疏散预案列表项目");
		evacuatePlan = _evacuatePlan;
		areaOfPlanPanel = _areaOfPlanPanel;
		No.text = evacuatePlan.id;
		Name.text = evacuatePlan.name;
	}

	//改变名称时调用
	public void OnNameChanged(string newName)
	{
		Logger.Instance.WriteLog("更改疏散预案名称");
		if(string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newName.Trim()))
		{
			newName = evacuatePlan.name;
			areaOfPlanPanel.GetComponent<EvacuateAreaOfPlanEdit>().nameLabel.value = evacuatePlan.name;
			return;
		}
		if(evacuatePlan.name == newName)
		{
			return;
		}
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		if(epDao.Select004(newName).Count > 0)
		{
			newName = evacuatePlan.name;
			areaOfPlanPanel.GetComponent<EvacuateAreaOfPlanEdit>().nameLabel.value = evacuatePlan.name;
			//WarnWindow.Instance.Show(WarnWindow.WarnType.SameName);
			return;
		}
		epDao.Update001(evacuatePlan.id,newName);
		evacuatePlan.name = newName;
		Name.text = newName;
	}

	public void OnBind()
	{
		Logger.Instance.WriteLog("打开绑定疏散预案对应的区域面板");

		areaOfPlanPanel.GetComponent<EvacuateAreaOfPlanEdit>().Init(evacuatePlan.id,evacuatePlan.name,OnNameChanged);
	}

	//点击删除按钮时调用
	public void OnDelete()
	{
		Logger.Instance.WriteLog("删除疏散预案");

		//删除数据库中的数据
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		epDao.Delete003(evacuatePlan.id);
		epDao.Delete004(evacuatePlan.id);

		//从列表中移除自身
		GetComponentInParent<UIGrid>().RemoveChild(transform);
		GetComponentInParent<UIGrid>().repositionNow = true;

		//更新列表
		GetComponentInParent<UIWidget>().enabled = false;
		GetComponentInParent<UIWidget>().enabled = true;

		//销毁对象
		Destroy(gameObject);
	}

	private bool _selected = false;
	public void Selected()
	{
		if(SelectedItem)SelectedItem.CancelSelected();
		SelectedItem = this;
		SelectedBg.SetActive (true);
		if(!_selected)
		{
			_selected = true;
			GetComponent<BoxCollider>().enabled = false;
			OnBind();
		}
	}

	void CancelSelected()
	{
		SelectedBg.SetActive (false);
		_selected = false;
		GetComponent<BoxCollider>().enabled = true;
	}
}
