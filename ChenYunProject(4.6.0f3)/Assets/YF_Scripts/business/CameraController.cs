using UnityEngine;
using System.Collections;
using System.Xml;
/// <summary>
/// 对主摄像机进行控制
/// </summary>
public class CameraController : MonoBehaviour
{

    /// <summary>
    /// 摄像机移动速度
    /// </summary>
    public float MoveSpeed;

    /// <summary>
    /// 视角拉近、拉远速度
    /// </summary>
    public float ScaleSpeed;

    public float MaxHeight;

    public float MinHeight;

    /// <summary>
    /// 巡逻高度
    /// </summary>
    public float PatrolHeight;

    public Vector3 StartPos;

    public Quaternion StartRot;
    /// <summary>
    /// 摄像机旋转点（摄像机围着指定的点做圆周运动）
    /// </summary>
    public GameObject RotatePoint;
    /// <summary>
    /// 当屏幕的宽度大于等于 1600是显示的界面
    /// </summary>
    public GameObject BigUIRoot;
    /// <summary>
    /// 当屏幕的宽度小于 1600是显示的界面
    /// </summary>
    public GameObject SmallUIRoot;
    /// <summary>
    /// 记录前一帧鼠标的位置
    /// </summary>
    private Vector3 PreMousePos;
    /// <summary>
    /// 记录当前帧鼠标的位置
    /// </summary>
    private Vector3 CurrentMousePos;

    /// <summary>
    /// 标记摄像机是否能移动
    /// </summary>
    [HideInInspector]
    public static bool CanMoveable;

    public int upSpeed;
    public int downSpeed;
    public int moveSpeed;
    public float rotateSpeed;
    public float offset;
    public int interval;

    private XmlDocument xml;

    void Awake()
    {
        //摄像机能移动
        CanMoveable = true;

        if (BigUIRoot == null || SmallUIRoot == null) return;

        if (Screen.width >= 1600)
        {
            //BigUIRoot.SetActive(true);
        }

        else
        {
            //SmallUIRoot.SetActive(true);
        }

        StartPos = transform.position;
        StartRot = transform.rotation;

    }

    void Start()
    {
        //让摄像机朝向指定的旋转点
        transform.LookAt(RotatePoint.transform.position);
        xml = new XmlDocument();
        try
        {
            xml.Load(Application.dataPath + "/YF_Config/PatrolConfig.xml");

            XmlElement el = (XmlElement)xml.SelectSingleNode("config");
            string value;
            value = el.GetAttribute("maxHeight");
            MaxHeight = value != string.Empty ? float.Parse(value) : MaxHeight;
            value = el.GetAttribute("patrolHeight");
            PatrolHeight = value != string.Empty ? float.Parse(value) : PatrolHeight;
            value = el.GetAttribute("upSpeed");
            upSpeed = value != string.Empty ? int.Parse(value) : upSpeed;
            value = el.GetAttribute("downSpeed");
            downSpeed = value != string.Empty ? int.Parse(value) : downSpeed;
            value = el.GetAttribute("moveSpeed");
            moveSpeed = value != string.Empty ? int.Parse(value) : moveSpeed;
            value = el.GetAttribute("rotateSpeed");
            rotateSpeed = value != string.Empty ? float.Parse(value) : rotateSpeed;
            value = el.GetAttribute("offset");
            offset = value != string.Empty ? float.Parse(value) : offset;
            value = el.GetAttribute("interval");
            interval = value != string.Empty ? int.Parse(value) : interval;
            value = el.GetAttribute("scaleSpeed");
            ScaleSpeed = value != string.Empty ? float.Parse(value) : ScaleSpeed;
            Debug.Log("加载巡航配置文件成功");
        }
        catch (System.Exception e)
        {
            Debug.Log("加载巡航配置文件出错:" + e.Message);
        }
    }
    bool lookAtRotatePoint = false;
    // Update is called once per frame
    void Update()
    {
        if (lookAtRotatePoint)
        {
            transform.LookAt(RotatePoint.transform.position);
        }
        if (UICamera.isOverUI || CanMoveable == false) return;
        if (Input.GetMouseButtonDown(sys_info.MOUSE_LEFT_BUTTON) || Input.GetMouseButtonDown(sys_info.MOUSE_RIGHT_BUTTON))
        {
            PreMousePos = Input.mousePosition;
            CurrentMousePos = PreMousePos;
        }

        if (Input.GetMouseButton(sys_info.MOUSE_LEFT_BUTTON))
        {
            Move();
            return;
        }

        if (Input.GetMouseButton(sys_info.MOUSE_RIGHT_BUTTON))
        {
            Rotate();
            return;
        }

        ViewScale();
    }

    /// <summary>
    /// 拉近或拉远视角
    /// </summary>
    private void ViewScale()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            Vector3 pos = transform.position + transform.forward * ScaleSpeed;
            if (pos.y >= MinHeight)
            {
                transform.position = pos;
            }

        }

        if (Input.mouseScrollDelta.y > 0)
        {
            Vector3 pos = transform.position - transform.forward * ScaleSpeed;
            if (pos.y <= MaxHeight)
            {
                transform.position = pos;
            }
        }
    }
    /// <summary>
    /// 移动摄像机
    /// </summary>
    private void Move()
    {
        CurrentMousePos = Input.mousePosition;
        Vector3 Delta = PreMousePos - CurrentMousePos;
        float adjustX = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        float adjustZ = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

        transform.position += new Vector3(Delta.x * adjustX + Delta.y * adjustZ, 0, Delta.y * adjustX - Delta.x * adjustZ) * MoveSpeed * 0.1f;
        RotatePoint.transform.position += new Vector3(Delta.x * adjustX + Delta.y * adjustZ, 0, Delta.y * adjustX - Delta.x * adjustZ) * MoveSpeed * 0.1f;
        PreMousePos = CurrentMousePos;
    }

    /// <summary>
    /// 旋转摄像机
    /// </summary>
    private void Rotate()
    {
        CurrentMousePos = Input.mousePosition;
        Vector3 Delta = PreMousePos - CurrentMousePos;
        transform.RotateAround(RotatePoint.transform.position, Vector3.up, -Delta.x * 0.1f);
        //transform.Rotate(Vector3.up, Delta.x * 0.1f, Space.World);
        //transform.Rotate(transform.right, -Delta.y * 0.1f, Space.World);
        PreMousePos = CurrentMousePos;
    }

    private TweenPosition TP_rotatePoint;
    private TweenPosition TP_camera;
    private bool MoveToPos = true;

    /// <summary>
    /// 将摄像机移动到指定的位置
    /// </summary>
    /// <param name="targetPos">Target position.</param>
    public void GotoPosition(Vector3 targetPos, uint _time = 0)
    {
        StopMove();
        MoveToPos = true;
        Vector3 Direction = targetPos - RotatePoint.transform.position;
        Direction.y = 0;
        int distance = (int)Vector3.Distance(targetPos, RotatePoint.transform.position);
        float time = 1f * distance * 0.05f;
        if (_time > 0) time = _time;
        targetPos.y = 0;
        TP_rotatePoint = TweenPosition.Begin(RotatePoint, time, targetPos);
        Vector3 monitor = transform.position + Direction;
        TP_camera = TweenPosition.Begin(transform.gameObject, time, monitor);
        StartCoroutine("MoveToMinHighPos");
    }

    IEnumerator MoveToMinHighPos()
    {

        while (MoveToPos)
        {
            Vector3 pos = transform.position + transform.forward * ScaleSpeed;
            if (pos.y >= 60)
            {
                transform.position = pos;
            }
            else
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopMove()
    {
        if (TP_rotatePoint && TP_camera)
        {
            TP_rotatePoint.enabled = false;
            TP_camera.enabled = false;
            TP_rotatePoint = null;
            TP_camera = null;
            MoveToPos = false;
        }
    }

    Vector3 CameraRotate;
    public void GotoPosition(Vector3 RotatePointPos, Vector3 CameraPos, Vector3 CameraRotate, EventDelegate.Callback call = null, bool _lookAtRotatePoint = false)
    {
        lookAtRotatePoint = _lookAtRotatePoint;
        StartCoroutine(rotate(RotatePointPos, CameraPos, CameraRotate, call));
    }

    void StopLookAtRotatePoint()
    {
        lookAtRotatePoint = false;
    }

    IEnumerator rotate(Vector3 RotatePointPos, Vector3 CameraPos, Vector3 CameraRotate, EventDelegate.Callback call = null)
    {
        while (true)
        {
            Vector3 pos = transform.position - transform.forward * 60 * Time.deltaTime;
            if (pos.y <= MaxHeight)
            {
                transform.position = pos;
            }
            else
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        int distance = (int)Vector3.Distance(CameraPos, transform.position);
        float time = 1f * distance * 0.02f;
        //this.CameraRotate = CameraRotate;
        TweenPosition.Begin(RotatePoint, time, RotatePointPos);
        TweenPosition cameraTP;
        if (call == null)
        {
            cameraTP = TweenPosition.Begin(transform.gameObject, time, CameraPos);
            cameraTP.AddOnFinished(new EventDelegate(StopLookAtRotatePoint));
        }
        else
        {
            cameraTP = TweenPosition.Begin(transform.gameObject, time, CameraPos);
            cameraTP.onFinished.Add(new EventDelegate(call));
            cameraTP.AddOnFinished(new EventDelegate(StopLookAtRotatePoint));
        }
        yield return null;
    }

    public void GotoPosition(GameObject src, Vector3 targetPos, Vector3 RotatePointPos, Vector3 CameraPos, Quaternion targetRorate, EventDelegate.Callback call = null)
    {
        if (call != null)
        {
            CanMoveable = false;
        }
        cruiseParameter.tarRot = targetRorate;
        cruiseParameter.rotatePos = RotatePointPos;
        cruiseParameter.gotoCall = call;
        cruiseParameter.tarPos = targetPos;
        cruiseParameter.camPos = CameraPos;
        cruiseParameter.src = src;

        srcTween = null;
        camTween = null;
        camRotation = null;
        rotateTween = null;
        StartCoroutine("cruiseRotate");
    }

    //以下变量用于开启相机移动功能后  记录相关信息  以便过程中或结束时需要使用 ****************
    private Cruise cruiseParameter = new Cruise();
    struct Cruise
    {
        /// <summary>
        /// 相机目标角度
        /// </summary>
        public Quaternion tarRot;
        /// <summary>
        /// 菱形目标坐标
        /// </summary>
        public Vector3 tarPos;
        /// <summary>
        /// 旋转中心点目标坐标
        /// </summary>
        public Vector3 rotatePos;
        /// <summary>
        /// 相机目标坐标
        /// </summary>
        public Vector3 camPos;
        /// <summary>
        /// 菱形对象
        /// </summary>
        public GameObject src;

        public EventDelegate.Callback gotoCall;
    }

    /// <summary>
    /// 菱形位移控制脚本
    /// </summary>
    TweenPosition srcTween = null;
    /// <summary>
    /// 相机位移控制脚本
    /// </summary>
    TweenPosition camTween = null;
    /// <summary>
    /// 旋转点位移控制脚本
    /// </summary>
    TweenPosition rotateTween = null;
    /// <summary>
    /// 相机旋转控制脚本
    /// </summary>
    TweenRotation camRotation = null;
    bool islook;
    //以上***************************************

    /// <summary>
    /// 用户强制停止巡航后 瞬移到目标位置
    /// </summary>
    public void TeleportPosition()
    {

        StopCoroutine("cruiseRotate");
        StopCoroutine("Look");
        if (srcTween != null)
        {
            srcTween.enabled = false;
        }
        if (camTween != null)
        {
            camTween.enabled = false;
        }
        if (rotateTween != null)
        {
            rotateTween.enabled = false;
        }
        if (camRotation != null)
        {
            camRotation.enabled = false;
        }

        if (cruiseParameter.src != null)
        {
            cruiseParameter.src.transform.position = cruiseParameter.tarPos;
        }
        RotatePoint.transform.position = cruiseParameter.rotatePos;
        transform.position = cruiseParameter.camPos;
        transform.rotation = cruiseParameter.tarRot;
    }

    IEnumerator cruiseRotate()
    {

        //拉升高度
        while (true)
        {

            Vector3 pos = transform.position - transform.forward * upSpeed * Time.deltaTime;
            if (pos.y <= MaxHeight)
            {
                transform.position = pos;
            }
            else
            {
                break;
            }
            yield return null;
        }

        //调整角度
        islook = false;
        TweenRotation tr;
        if (cruiseParameter.src == null)
        {
            Vector3 vecR = cruiseParameter.tarPos - transform.position;
            Quaternion qt = Quaternion.LookRotation(vecR);
            tr = TweenRotation.Begin(gameObject, rotateSpeed, qt);
            tr.onFinished.Clear();
            tr.onFinished.Add(new EventDelegate(IsLook));
        }
        else
        {
            Vector3 vecR = cruiseParameter.src.transform.position - transform.position;
            Quaternion qt = Quaternion.LookRotation(vecR);
            tr = TweenRotation.Begin(gameObject, rotateSpeed, qt);
            tr.onFinished.Clear();
            tr.onFinished.Add(new EventDelegate(IsLook));
        }

        while (!islook)
        {
            yield return new WaitForEndOfFrame();
        }

        if (cruiseParameter.gotoCall != null)
        {
            if (cruiseParameter.src == null)
            {
                StartCoroutine("Look", gameObject);
            }
            else
            {
                StartCoroutine("Look", cruiseParameter.src);
            }
        }

        //计算降落后视点
        if (cruiseParameter.src != null && Vector3.Distance(cruiseParameter.src.transform.position, cruiseParameter.tarPos) > interval)
        {
            Vector3 disVec = (cruiseParameter.tarPos - cruiseParameter.src.transform.position).normalized;//计算下一个目标点到当前目标点的单位向量
            disVec = new Vector3(disVec.x, 0, disVec.z);//将Y轴高度清零
            disVec = cruiseParameter.src.transform.position - disVec * PatrolHeight;//将当前目标点的坐标减去某个长度倍数的单位向量，得到后视点位坐标
            disVec = new Vector3(disVec.x - offset, PatrolHeight, disVec.z + offset);//将后视点位坐标高度设定为用户给定值
            disVec = (disVec - transform.position).normalized;//计算后视点位坐标到当前时候摄像机坐标的单位向量
            while (true)
            {
                Vector3 pos = transform.position + disVec * downSpeed * Time.deltaTime;
                if (pos.y > PatrolHeight)
                {
                    transform.position = pos;
                }
                else
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {

            while (true)
            {
                Vector3 pos = transform.position + transform.forward * downSpeed * Time.deltaTime;
                if (pos.y > PatrolHeight)
                {
                    transform.position = pos;
                }
                else
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

        }

        int distance = (int)Vector3.Distance(cruiseParameter.camPos, transform.position);
        float time = distance * 0.03f;
        if (time < 2)
        {
            time = 2;
        }
        rotateTween = TweenPosition.Begin(RotatePoint, time, cruiseParameter.rotatePos);
        rotateTween.onFinished.Clear();
        if (cruiseParameter.src != null)
        {
            srcTween = TweenPosition.Begin(cruiseParameter.src, time * 0.75f * moveSpeed, cruiseParameter.tarPos);
        }

        if (cruiseParameter.gotoCall == null)
        {
            TweenPosition.Begin(transform.gameObject, time * moveSpeed, cruiseParameter.camPos).onFinished.Clear();
            TweenRotation.Begin(gameObject, time * moveSpeed, cruiseParameter.tarRot).onFinished.Clear();
        }
        else
        {
            camTween = TweenPosition.Begin(transform.gameObject, time * moveSpeed, cruiseParameter.camPos);
            camTween.onFinished.Add(new EventDelegate(StopLook));
        }
        yield return null;
    }

    private void IsLook()
    {
        islook = true;
    }

    private void StopLook()
    {
        StopCoroutine("Look");
        camRotation = TweenRotation.Begin(gameObject, rotateSpeed, cruiseParameter.tarRot);
        camRotation.onFinished.Clear();
        camRotation.onFinished.Add(new EventDelegate(cruiseParameter.gotoCall));
    }

    private IEnumerator Look(GameObject tar)
    {
        Vector3 vec = Vector3.zero;
        float dis = 0;
        float minDis = 0;
        while (true)
        {
            if (tar == gameObject)
            {
                transform.LookAt(cruiseParameter.tarPos);
            }
            else
            {
                transform.LookAt(tar.transform);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 将摄像机移动到最高处
    /// </summary>
    public void MoveToMaxHeight()
    {
        StartCoroutine("_MoveToMaxHeight");
    }

    IEnumerator _MoveToMaxHeight()
    {

        while (true)
        {
            Vector3 pos = transform.position - transform.forward * 40 * Time.deltaTime;
            if (pos.y <= MaxHeight)
            {
                transform.position = pos;
            }
            else
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
