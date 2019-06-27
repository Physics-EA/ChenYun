using UnityEngine;
using System.Collections;

public class LogoutManage : MonoBehaviour
{

    /// <summary>
    /// 用来存储关闭按钮
    /// </summary>
	public GameObject CloseBtn;

    /// <summary>
    /// 用来存储选择窗口
    /// </summary>
    public GameObject OptionWindow;

    /// <summary>
    /// 选择窗口的左边锚点
    /// </summary>
    public GameObject OptionLeftAnchor;

    /// <summary>
    /// 选择窗口的右边锚点
    /// </summary>
    public GameObject OptionRightAnchor;

    /// <summary>
    /// 更换账户窗口
    /// </summary>
    public GameObject ChangeAcountWindow;

    /// <summary>
    /// 锁定账户窗口
    /// </summary>
    public GameObject LockAccountWindow;

    /// <summary>
    /// 
    /// </summary>
    private TweenPosition OptionTweenPosition;

    /// <summary>
    /// 用来存储选择按钮
    /// </summary>
    private BoxCollider[] OptionBtnsBC;



    void Awake()
    {
        //选择窗口的位置为右边锚点的位置
        OptionWindow.transform.localPosition = OptionRightAnchor.transform.localPosition;
    }

    void Start()
    {
        //将子物体上的BoxCollider组件赋值给
        OptionBtnsBC = OptionWindow.GetComponentsInChildren<BoxCollider>();

        OptionTweenPosition = OptionWindow.GetComponent<TweenPosition>();

        OptionTweenPosition.onFinished.Add(new EventDelegate(OnOptionWindowOpend));

        OptionTweenPosition.from = OptionRightAnchor.transform.localPosition;

        OptionTweenPosition.to = OptionLeftAnchor.transform.localPosition;

        //将触发器全部关闭
        foreach (BoxCollider collider in OptionBtnsBC)
        {
            collider.enabled = false;
        }
    }

    void Update()
    {
        if (!OptionWindowOpend) return;

        //如果单击鼠标左键
        if (Input.GetMouseButton(sys_info.MOUSE_LEFT_BUTTON))
        {
            if (UICamera.hoveredObject == CloseBtn) return;
            foreach (BoxCollider collider in OptionBtnsBC)
            {
                if (UICamera.hoveredObject == collider.gameObject)
                {
                    return;
                }
            }
            CloseOpionWindow();
        }
    }
    public void ChangeAccount()
    {
        if (!OptionWindowOpend) return;
        Logger.Instance.WriteLog("切换用户");
        ChangeAcountWindow.SetActive(true);
        CloseOpionWindow();
    }

    public void ExitSystem()
    {
        if (!OptionWindowOpend) return;
        Logger.Instance.WriteLog("退出系统");
        Application.Quit();
    }

    public void LockAccount()
    {
        if (!OptionWindowOpend) return;
        Logger.Instance.WriteLog("用户锁定");
        LockAccountWindow.SetActive(true);
        CloseOpionWindow();
    }

    private Color OldColor;
    private Color NewColor = new Color(0, 0.24f, 0.45f);
    private GameObject HoverGo;
    public void OnHoverOver(GameObject go)
    {
        if (go)
        {
            HoverGo = go;
            OldColor = go.GetComponentInChildren<UILabel>().color;
            go.GetComponentInChildren<UILabel>().color = NewColor;
        }
    }

    public void OnHoverOut(GameObject go)
    {
        if (go && HoverGo == go)
        {
            HoverGo = null;
            go.GetComponentInChildren<UILabel>().color = OldColor;
        }
    }
    /// <summary>
    /// 打开或关闭，退出选项
    /// </summary>
    bool OpenOptionWindowPlayReverse = false;
    public void OpenOptionWindow()
    {
        if (OpenOptionWindowPlayReverse)
        {
            OptionTweenPosition.PlayReverse();
            OpenOptionWindowPlayReverse = false;
            OptionWindowOpend = false;
        }
        else
        {
            OptionTweenPosition.PlayForward();
            OpenOptionWindowPlayReverse = true;
        }
    }

    private void CloseOpionWindow()
    {
        if (HoverGo)
        {
            HoverGo.GetComponentInChildren<UILabel>().color = Color.white;
            HoverGo = null;
        }
        OpenOptionWindow();
    }

    /// <summary>
    /// 标记选择窗口是否打开
    /// </summary>
    bool OptionWindowOpend = false;

    void OnOptionWindowOpend()
    {
        OptionWindowOpend = false;

        if (OpenOptionWindowPlayReverse)
        {
            foreach (BoxCollider collider in OptionBtnsBC)
            {
                collider.enabled = true;
            }
            OptionWindowOpend = true;
        }
        else
        {
            foreach (BoxCollider collider in OptionBtnsBC)
            {
                collider.enabled = false;
            }
        }
    }
}
