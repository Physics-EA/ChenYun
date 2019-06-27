using UnityEngine;
using System.Collections;
/// <summary>
/// 挂在已经被选择的摄像头列表上的组件
/// </summary>
public class SelectedMornitorItem : MonoBehaviour {


	public UILabel No;
	public UILabel Name;
	public UIInput PatrolTimeInput;
	public GameObject GotoArrowBtn;
	public GameObject UpArrowBtn;
	public GameObject DownArrowBtn;
	public GameObject DeleteBtn;
	public GameObject Background;
	public DeviceInfo MornitorInfo;


	private DelAction DelMoveUp;
	private DelAction DelMoveDown;
	private DelAction DelDelete;
	private DelAction DelTimeChanged;

	private static SelectedMornitorItem SelectedItem;
	void Start()
	{
		UIEventListener.Get(GotoArrowBtn).onClick = GotoPosition;
		UIEventListener.Get(UpArrowBtn).onClick = MoveUp;
		UIEventListener.Get(DownArrowBtn).onClick = MoveDown;
		UIEventListener.Get(DeleteBtn).onClick = Delete;
	}

	public void Init(DeviceInfo _MornitorInfo,string _PatrolTime)
	{
		MornitorInfo = _MornitorInfo;
		No.text = MornitorInfo.Id;
		Name.text = MornitorInfo.Description;
		PatrolTimeInput.value = _PatrolTime;
	}
	public void BindAction(DelAction _DelMoveUp,DelAction _DelMoveDown,DelAction _DelDelete,DelAction _DelTimeChanged)
	{
		DelMoveUp = _DelMoveUp;
		DelMoveDown = _DelMoveDown;
		DelDelete = _DelDelete;
		DelTimeChanged = _DelTimeChanged;
	}
	/// <summary>
	/// 移动视野到摄像机的位置
	/// </summary>
	/// <param name="go">Go.</param>
	public void GotoPosition(GameObject go)
	{
		Camera.main.GetComponent<CameraController> ().GotoPosition(new Vector3( float.Parse(MornitorInfo.PosX),0,float.Parse(MornitorInfo.PosZ)),2);
		Select();
	}
	/// <summary>
	/// 将当前项目向上移动
	/// </summary>
	/// <param name="go">Go.</param>
	public void MoveUp(GameObject go)
	{
		if(DelMoveUp != null)DelMoveUp.Invoke(gameObject);
		Select();
	}
	/// <summary>
	/// 将当前项目向下移动
	/// </summary>
	/// <param name="go">Go.</param>
	public void MoveDown(GameObject go)
	{
		if(DelMoveDown != null)DelMoveDown.Invoke(gameObject);
		Select();
	}
	/// <summary>
	/// 删除当前项目
	/// </summary>
	/// <param name="go">Go.</param>
	public void Delete(GameObject go)
	{
		if(DelDelete != null)DelDelete.Invoke(gameObject);
	}
	/// <summary>
	/// 删除当前项目
	/// </summary>
	/// <param name="go">Go.</param>
	public void TimeSelected(UISprite sprite)
	{
		sprite.enabled = true;;
		Select();
	}
	/// <summary>
	/// 当鼠标放到时间上是显示
	/// </summary>
	public void OnTimeHoverOver(UISprite sprite)
	{
		if(!PatrolTimeInput.isSelected)
		{
			sprite.enabled = true;
		}
	}
	/// <summary>
	/// 当鼠标移出时间上是显示
	/// </summary>
	public void OnTimeHoverOut(UISprite sprite)
	{
		if(!PatrolTimeInput.isSelected)
		{
			sprite.enabled = false;
		}
	}
	/// <summary>
	/// 回调函数，当时间被改变的时候调用
	/// </summary>
	public void OnTimeChanged(UISprite sprite)
	{
		Logger.Instance.WriteLog("巡航时间被改变");
		sprite.enabled = false;
		UIInput time = PatrolTimeInput;
		//如果填写的时间为空，或者小于5的时候将其设为5
		if(time.value.Trim() == "" || int.Parse(time.value) < 5)
		{
			time.value = 5 + "";
		}
		//如果填写的时间小于10，且不是一位则将其转成一位
		if(int.Parse(time.value) < 10 && time.value.Length == 2)
		{
			time.value = time.value.Substring(time.value.Length - 1);
		}
		if(DelTimeChanged != null)DelTimeChanged.Invoke(gameObject);
	}

	bool isSelected = false;
	private void Select()
	{
		if(!isSelected)
		{
			if(SelectedItem != null)
			{
				SelectedItem.Deselect();
			}
			isSelected = true;
			SelectedItem = this;
			Background.SetActive(true);
		}
	}

	private void Deselect()
	{
		isSelected = false;
		Background.SetActive(false);
	}
}
