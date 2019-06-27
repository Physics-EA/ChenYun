using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public delegate void DelEvacuateAreaNameChanged(string newName);
public class EditedEvacuateAreaListItem : MonoBehaviour
{
	public UILabel No;
	public UILabel Name;
	public UIInput FontSize;
	public GameObject UIText;
	public GameObject SelectedBg;

	[HideInInspector]
	public static EditedEvacuateAreaListItem SelectedItem;

	private EvacuateArea evacuateArea;
	private GameObject areaDevicePanel;

	/// <summary>
	/// 疏散区域对象
	/// </summary>
	private GameObject GOArea;
	/// <summary>
	/// 疏散区域名称对象
	/// </summary>
	private GameObject NameText;
	//初始化对象
	public void Init(EvacuateArea _evacuateArea,GameObject _areaDevicePanel)
	{
		Logger.Instance.WriteLog("初始化疏散区域列表项目");
		evacuateArea = _evacuateArea;
		areaDevicePanel = _areaDevicePanel;
		No.text = evacuateArea.id;
		Name.text = evacuateArea.name;
		FontSize.value = evacuateArea.fontSize;
		DrawArea(evacuateArea.points);
	}
	

	/// <summary>
	/// 面片中心位置
	/// </summary>
	private Vector3 centerPos = Vector3.zero;
	//绘制区域
	private void DrawArea(string _points)
	{
		Logger.Instance.WriteLog("绘制疏散区域");
		if(string.IsNullOrEmpty(_points.Trim()))return;
		string[] point = _points.Split('|');
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
		GOArea = new GameObject();
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
		SetEvacuateAreaText();
	}

	//设置疏散区域名称对象
	private void SetEvacuateAreaText()
	{
		Logger.Instance.WriteLog("设置疏散区域名称对象");
		NameText = Instantiate(UIText) as GameObject;
		centerPos.y = 0.2f;
		NameText.transform.position = centerPos;
		NameText.transform.parent = GameObject.Find("SceneUI").transform;
		NameText.transform.localRotation = Quaternion.identity;
		NameText.GetComponent<Text>().text = evacuateArea.name;
		NameText.GetComponent<Text>().fontSize = int.Parse(evacuateArea.fontSize);
		AdjustTextAlignment();
	}
	//设置疏散区域名称文字对齐方式
	private void AdjustTextAlignment()
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

	public void BindArea(GameObject go,string points)
	{
		Logger.Instance.WriteLog("绑定疏散区域");
		//if(DrawBtn)DrawBtn.GetComponentInChildren<UILabel>().color = Color.white;
		if(DrawBtn)DrawBtn.GetComponentInChildren<UILabel>().text = "绘制";
		evacuateArea.points = points;
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		epDao.Update003(evacuateArea.id,evacuateArea.points);
		DrawArea(evacuateArea.points);
		Destroy(go);
	}
	
	void OnDisable()
	{
		if(GOArea)Destroy(GOArea);
		if(NameText)Destroy(NameText);
		SelectedBg.SetActive (false);
	}

	public void GotoPosition()
	{
		Logger.Instance.WriteLog("移动到统计区域位置");
		Selected();
		if(GOArea)Camera.main.GetComponent<CameraController> ().GotoPosition(centerPos,2);
	}
	//改变字体时调用
	public void OnFontSizeChanged()
	{
		Logger.Instance.WriteLog("改变疏散字体");
		if(string.IsNullOrEmpty(FontSize.value) || string.IsNullOrEmpty(FontSize.value.Trim()))
		{
			FontSize.value = evacuateArea.fontSize;
			return;
		}
		if(evacuateArea.fontSize == FontSize.value)
		{
			return;
		}
		if(int.Parse(FontSize.value) < 10)
		{
			FontSize.value = "10";
		}
		if(int.Parse(FontSize.value) > 40)
		{
			FontSize.value = "40";
		}
		//保存字体变更
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		epDao.Update004(evacuateArea.id,FontSize.value);
		evacuateArea.fontSize = FontSize.value;
		if(NameText) NameText.GetComponent<Text>().fontSize = int.Parse(evacuateArea.fontSize);
		AdjustTextAlignment();
	}

	//改变名称时调用
	public void OnNameChanged(string newName)
	{
		Logger.Instance.WriteLog("改变疏散区域名称");
		if(string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newName.Trim()))
		{
			areaDevicePanel.GetComponent<EvacuateAreaDeviceEdit>().nameLabel.value = evacuateArea.name;
			return;
		}
		if(evacuateArea.name == newName)
		{
			return;
		}
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		if(epDao.Select005(newName).Count > 0)
		{
			areaDevicePanel.GetComponent<EvacuateAreaDeviceEdit>().nameLabel.value = evacuateArea.name;
			//WarnWindow.Instance.Show(WarnWindow.WarnType.SameName);
			return;
		}
		epDao.Update002(evacuateArea.id,newName);
		evacuateArea.name = newName;
		Name.text = evacuateArea.name;
		if(NameText) NameText.GetComponent<Text>().text = evacuateArea.name;
	}
	//点击绑定按钮时调用
	public void OnBind()
	{
		Logger.Instance.WriteLog("绑定区域设备信息");
		areaDevicePanel.SetActive(true);
		areaDevicePanel.GetComponent<EvacuateAreaDeviceEdit>().Init(evacuateArea.id,OnNameChanged);
	}

	private bool isDrawingArea = false;
	private GameObject DrawBtn;
	//点击绘制按钮时调用
	public void OnDrawArea(GameObject title)
	{
		DrawBtn = title;
		if(isDrawingArea)
		{
			Logger.Instance.WriteLog("取消绘制疏散区域");
			gameObject.GetComponent<EvacuateAreaTool>().EndDraw();
			gameObject.GetComponent<EvacuateAreaTool>().enabled = false;
			DrawArea(evacuateArea.points);
			isDrawingArea = false;
			title.GetComponent<UILabel>().text = "绘制";
			//title.GetComponent<UILabel>().color = Color.white;
			return;
		}

		Logger.Instance.WriteLog("开始绘制疏散区域");

		isDrawingArea = true;
		title.GetComponent<UILabel>().text = "取消";
		//title.GetComponent<UILabel>().color = Color.black;
		if(GOArea)Destroy(GOArea);
		if(NameText)Destroy(NameText);
		gameObject.GetComponent<EvacuateAreaTool>().enabled = true;
		gameObject.GetComponent<EvacuateAreaTool>().StartDraw();
	}
	
	//点击删除按钮时调用
	public void Delete()
	{
		Logger.Instance.WriteLog("删除疏散区域");

		//删除数据库中的数据
		EvacuationPlanDao epDao = new EvacuationPlanDao();
		epDao.Delete001(evacuateArea.id);
		epDao.Delete002(evacuateArea.id);

		Camera.main.GetComponent<CameraController> ().StopMove();
	

		//从列表中移除自身
		GetComponentInParent<UIGrid>().RemoveChild(transform);
		GetComponentInParent<UIGrid>().repositionNow = true;
		
		//更新列表
		GetComponentInParent<UIWidget>().enabled = false;
		GetComponentInParent<UIWidget>().enabled = true;


		//销毁对象
		Destroy(GOArea);
		Destroy(NameText);
		Destroy(gameObject);
	}

	private bool _Selected = false;
	public void Selected()
	{
		if(_Selected)return;
		OnBind();
		if(SelectedItem)SelectedItem.CancelSelected();
		SelectedItem = this;
		SelectedBg.SetActive(true);

		if(!_Selected)
		{
			_Selected = false;
			GetComponent<BoxCollider>().enabled = false;
		}
	}
	
	void CancelSelected()
	{
		SelectedBg.SetActive (false);
		_Selected = false;
		if(isDrawingArea)OnDrawArea(DrawBtn);
		GetComponent<BoxCollider>().enabled = true;
	}
}
