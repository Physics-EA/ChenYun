using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// 处理单独摄像机客流统计信息相关问题
/// </summary>
public class PassengerFlowInfoCamera : MonoBehaviour
{
    /// <summary>
    /// 用来存储设备信息
    /// </summary>
    DeviceInfo info;

    /// <summary>
    /// 用来控制摄像机
    /// </summary>
    CameraUICtrl ctrlUI;

    string PFIKey;

    /// <summary>
    /// 初始化客流信息，
    /// 传入设备信息（现实中的摄像机）
    /// </summary>
    /// <param name="_info">_info.</param>
    /// <param name="_PFIShowPlane">_ PFI show plane.</param>
    public void Init(DeviceInfo _info)
    {
        info = _info;

        //将PFIKey赋值为客流Url
        PFIKey = string.IsNullOrEmpty(info.PassengerFlowUrl) ? "" : info.PassengerFlowUrl.Trim();

        //将CameraUICtrl组件赋值给ctrlUI
        ctrlUI = gameObject.GetComponent<CameraUICtrl>();
    }

    /// <summary>
    /// 人流等级
    /// </summary>
    int currLevel = 0;

    /// <summary>
    /// 人流总数
    /// </summary>
    int sum = 0;

    /// <summary>
    /// 进入摄像机的人流数量
    /// </summary>
    int enter = 0;

    /// <summary>
    /// 离开生相机的人流数量
    /// </summary>
    int exit = 0;

    /// <summary>
    /// 用来标记是否显示信息
    /// </summary>
    bool ShowInfo = false;


    /// <summary>
    /// 更新客流统计信息
    /// </summary>
    /// <param name="data">Data.</param>
    public void UpdateData(Dictionary<string, PassengerFlowData> data)
    {
        //sum = 0;
        //enter = 0;
        //exit = 0;

        if (data.ContainsKey(PFIKey))
        {
            //sum += data[PFIKey].SumCount;
            enter = data[PFIKey].EnterCount;
            exit = data[PFIKey].ExitCount;

            sum += data[PFIKey].EnterCount;
            sum = enter - exit;
            if (sum < 0)
            {
                sum = 0;
            }
        }

        if (PassengerFlowInfoReceiver.showInfo)
        {
            ctrlUI.ui.SetPassengerNum(enter, sum, exit);
        }
    }

}
