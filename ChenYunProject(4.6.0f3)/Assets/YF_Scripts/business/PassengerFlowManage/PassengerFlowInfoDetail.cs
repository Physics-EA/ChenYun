using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 处理客流统计区域相关问题
/// 根据当前区域人数 调整区域显示警戒色
/// 显示当前区域人数统计信息
/// </summary>
public class PassengerFlowInfoDetail : MonoBehaviour
{

    /// <summary>
    /// 用来存放区域人流信息
    /// </summary>
    PassengerFlowAreaInfo info;


    /// <summary>
    /// 用来存放区域的颜色
    /// </summary>
    public Material mat_green;
    public Material mat_yellow;
    public Material mat_orange;
    public Material mat_red;


    /// <summary>
    /// 指的是 PassengerFlowInfoUI 面板
    /// </summary>
    public PassengerFlowInfoUI PFInfoUI;

    PassengerFlowInfoShow PFIShow;

    PassengerAreaUI PFIShowPlane;

    List<PFURLAttr> PFURLAttrLst;


    private struct PFURLAttr
    {
        public string PassengerFlowUrl;
        public string InIsIn;
        public string InIsOut;
        public string OutIsIn;
        public string OutIsOut;
    }

    /// <summary>
    /// 初始化化区域信息
    /// </summary>
    /// <param name="_info">_info.</param>
    /// <param name="_PFIShowPlane">_ PFI show plane.</param>
    public void Init(PassengerFlowAreaInfo _info, PassengerAreaUI _PFIShowPlane)
    {
        Logger.Instance.WriteLog("初始化客流统计信息");
        PFURLAttrLst = new List<PFURLAttr>();
        info = _info;

        if (PFInfoUI != null)
        {
            PFInfoUI.grade1 = int.Parse(info.WarnLevel1);
            PFInfoUI.grade2 = int.Parse(info.WarnLevel2);
            PFInfoUI.grade3 = int.Parse(info.WarnLevel3);
        }
        //transform.position = new Vector3(FLOAT(info.PosX),FLOAT(info.PosY),FLOAT(info.PosZ));
        //transform.localScale = new Vector3(FLOAT(info.ScaleX),FLOAT(info.ScaleY),FLOAT(info.ScaleZ));
        PFIShowPlane = _PFIShowPlane;
        string[] DeviceIdLst = info.CameraIdLst.Split('|');
        Logger.Instance.WriteLog("加载相关设备信息,用来初始化客流统计信息");
        DeviceDao dDao = new DeviceDao();
        Encoding defaultEncoding = System.Text.Encoding.Default;
        foreach (var id in DeviceIdLst)
        {
            string[] _id = id.Split(',');
            dDao.Select003(_id[0]);
            if (dDao.Result.Count == 1)
            {
                PFURLAttr attr = new PFURLAttr();
                attr.PassengerFlowUrl = dDao.Result[0].PassengerFlowUrl.Trim();
                attr.InIsIn = _id[1];
                attr.InIsOut = _id[2];
                attr.OutIsIn = _id[3];
                attr.OutIsOut = _id[4];
                PFURLAttrLst.Add(attr);
            }
        }
    }


    int level = 0;
    int currLevel = 0;
    public int sum = 0;
    public int enter = 0;
    public int exit = 0;
    bool ShowInfo = false;


    /// <summary>
    /// 更新区域客流信息
    /// </summary>
    /// <param name="data">Data.</param>
    public void UpdateData(Dictionary<string, PassengerFlowData> data)
    {
        sum = 0;
        enter = 0;
        exit = 0;

        foreach (var item in PFURLAttrLst)
        {
            if (data.ContainsKey(item.PassengerFlowUrl.Trim()))
            {

                if (item.InIsIn == "1")
                {
                    enter += data[item.PassengerFlowUrl.Trim()].EnterCount;
                }

                else if (item.InIsOut == "1")
                {
                    exit += data[item.PassengerFlowUrl.Trim()].EnterCount;
                }

                if (item.OutIsIn == "1")
                {
                    enter += data[item.PassengerFlowUrl.Trim()].ExitCount;
                }

                else if (item.OutIsOut == "1")
                {
                    exit += data[item.PassengerFlowUrl.Trim()].ExitCount;
                }
            }
        }
        sum = enter - exit;

        if (sum < 0)
        {
            sum = 0;
        }

        if (PFIShowPlane != null)
        {
            PFIShowPlane.UpdateArea(STRING(enter), STRING(sum), STRING(exit));
            if (!CheckWarnLevel())
            {
                transform.renderer.material = mat_green;
                return;
            }
            if (sum >= FLOAT(info.WarnLevel3))
            {
                if (level != 3)
                {
                    level = 3;
                    transform.renderer.material = mat_red;
                }
                return;
            }
            if (sum >= FLOAT(info.WarnLevel2))
            {
                if (level != 2)
                {
                    level = 2;
                    transform.renderer.material = mat_orange;
                }
                return;
            }
            if (sum >= FLOAT(info.WarnLevel1))
            {
                if (level != 1)
                {
                    level = 1;
                    transform.renderer.material = mat_yellow;
                }
                return;
            }
            if (level != 0)
            {
                level = 0;
                transform.renderer.material = mat_green;
                return;
            }
        }
        else if (PFInfoUI != null)
        {
            PFInfoUI.UpdateValue(enter, sum, exit);
        }

    }

    /// <summary>
    /// 检查是否所有的报警等级人数都大于0
    /// </summary>
    /// <returns><c>true</c>, if warn level was checked, <c>false</c> otherwise.</returns>
    private bool CheckWarnLevel()
    {
        return FLOAT(info.WarnLevel3) > 0 && FLOAT(info.WarnLevel2) > 0 && FLOAT(info.WarnLevel1) > 0;
    }

    void StopUpdateShow()
    {
        ShowInfo = false;
    }

    /// <summary>
    /// 当鼠标点击区域是信息相关信息
    /// </summary>
    void OnMouseDown()
    {
        if (UICamera.isOverUI) return;
        //PFIShowPlane.SetActive(true);
        //PFIShow = PFIShowPlane.GetComponent<PassengerFlowInfoShow>();
        //PFIShow.SwitchOwner(gameObject,info.Name);
        //PFIShow.UpdateShowInfo(STRING(sum),STRING(enter),STRING(exit));
        //ShowInfo = true;
    }

    private float FLOAT(string str)
    {
        return float.Parse(str);
    }

    private string STRING(int val)
    {
        return val.ToString();
    }
}
