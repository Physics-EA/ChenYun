using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class EvacuateAreaOfPlanEdit : MonoBehaviour
{
	public UIInput nameLabel;
	public UISprite nameInputBox;
	public UIGrid EvacuateAreaOfPlanItemGrid;
	public GameObject EvacuateAreaOfPlanItemPrefab;
	public GameObject UIText;
	public DelEvacuatePlanNameChanged EvacuatePlanNameChanged;
	private string planId;
	public void Init(string _planId,string _planName,DelEvacuatePlanNameChanged _EvacuatePlanNameChanged)
	{
		Logger.Instance.WriteLog("初始化疏散预案区域编辑");
		EvacuatePlanNameChanged = _EvacuatePlanNameChanged;
		planId = _planId;
		nameLabel.value = _planName;
		StartCoroutine(LoadData());
	}

	IEnumerator LoadData()
	{
		Logger.Instance.WriteLog("加载疏散预案");
		yield return new WaitForEndOfFrame();
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		List<EvacuateAreaOfPlan> eaoPlanLst = epDao.Select002(planId);
		List<string> areaIdLst = new List<string>();
		foreach(var item in eaoPlanLst)
		{
			areaIdLst.Add(item.evacuateAreaId);
		}
		Logger.Instance.WriteLog("加载疏散区域");
		List<EvacuateArea> evacuateAreaLst = epDao.Select001();
		InitEvacuateAreaOfPlanItems(evacuateAreaLst,areaIdLst);
		EvacuateAreaOfPlanItemGrid.gameObject.GetComponent<UIWidget>().UpdateAnchors();
		yield return null;
	}

	/// <summary>
	/// 初始化疏散预案区域列表项目
	/// </summary>
	/// <param name="deviceInfoLst">Device info lst.</param>
	private void InitEvacuateAreaOfPlanItems(List<EvacuateArea> evacuateAreaLst,List<string> areaIdLst)
	{
		if(EvacuateAreaOfPlanItemGrid.GetChildList().Count > 0)
		{
			foreach(Transform tf in EvacuateAreaOfPlanItemGrid.GetChildList())
			{
				EvacuateAreaOfPlanItem eaopItem = tf.GetComponent<EvacuateAreaOfPlanItem>();
				eaopItem.Init(areaIdLst.Contains(eaopItem.AreaId));
			}
		}
		else
		{
			GOAreas = new GameObject();
			GOAreas.name = "临时疏散区域对象集";
			NameTexts = new GameObject();
			NameTexts.name = "临时疏散区域名称对象集";
			NameTexts.transform.parent = GameObject.Find("SceneUI").transform;
			NameTexts.transform.localRotation = Quaternion.identity;
			foreach(var area in evacuateAreaLst)
			{
				GameObject go = Instantiate(EvacuateAreaOfPlanItemPrefab) as GameObject;
				EvacuateAreaOfPlanItemGrid.AddChild(go.transform);
				go.transform.localScale = new Vector3(1,1,1);
				GameObject GoArea = DrawArea(area);
				GameObject AreaText = SetEvacuateAreaText(area,GoArea);
				AdjustTextAlignment(AreaText,GoArea);
				go.GetComponent<EvacuateAreaOfPlanItem>().Init(area,GoArea,centerPos,areaIdLst.Contains(area.id));
				if(GoArea)GoArea.transform.parent = GOAreas.transform;
				if(AreaText)AreaText.transform.parent = NameTexts.transform;
			}
		}
	}


	/// <summary>
	/// 疏散区域对象
	/// </summary>
	private GameObject GOAreas;
	/// <summary>
	/// 疏散区域名称对象
	/// </summary>
	private GameObject NameTexts;

	/// <summary>
	/// 面片中心位置
	/// </summary>
	private Vector3 centerPos = Vector3.zero;
	//绘制区域
	private GameObject DrawArea(EvacuateArea evacuateArea)
	{
		Logger.Instance.WriteLog("绘制疏散区域");
		if(string.IsNullOrEmpty(evacuateArea.points.Trim()))return null;
		string[] point = evacuateArea.points.Split('|');
		Vector3[] pts = new Vector3[point.Length / 3];
		centerPos = Vector3.zero;
		for(int i = 0; i < pts.Length; i++)
		{
			pts[i] = new Vector3(float.Parse(point[i * 3]), float.Parse(point[i * 3 + 1]), float.Parse(point[i * 3 + 2]));
			centerPos += pts[i];
		}
		centerPos /= pts.Length;
		
		int[] triangles = new int[pts.Length];
		for(int i = 0; i < triangles.Length;i++)
		{
			triangles[i] = i;
		}
		GameObject GOArea = new GameObject();
		GOArea.AddComponent<MeshCollider> ();
		MeshRenderer meshrend = GOArea.AddComponent<MeshRenderer>();
		meshrend.material.shader = Shader.Find("Particles/Alpha Blended");
		meshrend.material.SetColor("_TintColor",new Color(0,1,0,0.2f));
		MeshFilter meshFilter = GOArea.AddComponent<MeshFilter>();
		meshFilter.mesh.vertices = pts;
		meshFilter.mesh.triangles = triangles;
		meshFilter.mesh.RecalculateNormals();
		GOArea.transform.position = new Vector3(0,0.3f,0);
		GOArea.name = evacuateArea.name;
		return GOArea;
	}
	
	//设置疏散区域名称对象
	private GameObject SetEvacuateAreaText(EvacuateArea evacuateArea,GameObject GOArea)
	{
		Logger.Instance.WriteLog("设置疏散区域名称对象");
		if(GOArea == null) return null;
		GameObject NameText = Instantiate(UIText) as GameObject;
		centerPos.y = 0.2f;
		NameText.transform.position = centerPos;
		NameText.transform.parent = NameTexts.transform;
		NameText.transform.localRotation = Quaternion.identity;
		NameText.GetComponent<Text>().text = evacuateArea.name;
		NameText.GetComponent<Text>().fontSize = int.Parse(evacuateArea.fontSize);
		return NameText;
	}
	//设置疏散区域名称文字对齐方式
	private void AdjustTextAlignment(GameObject NameText,GameObject GOArea)
	{
		if(!NameText || !GOArea) return;
		Logger.Instance.WriteLog("设置疏散区域名称文字对齐方式");
		Vector3 size = GOArea.transform.renderer.bounds.size * 4;
		if(NameText.GetComponent<Text>().preferredWidth <= size.x)
		{
			NameText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
		}
		else
		{
			NameText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
		}
		
		NameText.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,size.z);
	}


	public void OnNameSelected()
	{
		nameInputBox.enabled = true;
	}

	public void OnNameDeselected()
	{
		if(EvacuatePlanNameChanged != null) EvacuatePlanNameChanged.Invoke(nameLabel.value);
		nameInputBox.enabled = false;
	}

	public void Destroy()
	{
		ClearEvacuateAreaOfPlanItemGrid();
		Destroy(GOAreas);
		Destroy(NameTexts);
	}

	public void SaveData()
	{
		Logger.Instance.WriteLog("保存疏散预案信息");
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		epDao.Delete003(planId);

		List<Transform> items = EvacuateAreaOfPlanItemGrid.GetChildList();
		foreach(var item in items)
		{
			if(item.gameObject.GetComponent<EvacuateAreaOfPlanItem>().BindStatu)
			{
				epDao.Insert002(planId,item.gameObject.GetComponent<EvacuateAreaOfPlanItem>().AreaId);
			}
		}
	}

	private void ClearEvacuateAreaOfPlanItemGrid()
	{
		EvacuateAreaOfPlanItemGrid.transform.DestroyChildren();
	}
}
