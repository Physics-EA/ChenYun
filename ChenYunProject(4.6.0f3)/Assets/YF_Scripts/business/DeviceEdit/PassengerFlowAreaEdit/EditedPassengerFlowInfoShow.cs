using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// 处理客流统计区域相关问题
/// 根据当前区域人数 调整区域显示警戒色
/// 显示当前区域人数统计信息
/// </summary>
public class EditedPassengerFlowInfoShow : MonoBehaviour
{

    PassengerFlowAreaInfo info;
    public PassengerAreaUI pAreaUI;
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
    public void Init(PassengerFlowAreaInfo _info)
    {
        if (pAreaUI != null)
        {
            pAreaUI.UpdateArea(STRING(enter), STRING(sum), STRING(exit));
        }
        if (PFURLAttrLst != null)
        {
            PFURLAttrLst.Clear();
        }
        else
        {
            PFURLAttrLst = new List<PFURLAttr>();
        }
        info = _info;
        string[] DeviceIdLst = info.CameraIdLst.Split('|');
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
        Debug.Log("UpdateData");
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

        if (pAreaUI != null)
        {
            pAreaUI.UpdateArea(STRING(enter), STRING(sum), STRING(exit));
        }

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
