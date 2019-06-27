using UnityEngine;
using System.Collections;

public class CameraUICtrl : MonoBehaviour
{

    // CameraUI 的父物体
    public Transform uiParent;

    //用来存储绑定的CameraUI
    public CameraUI ui;

    void Start()
    {
        if (ui != null && uiParent != null)
        {
            //gameObject表示当前摄像机，将当前摄像机赋值给绑定的CameraUI
            ui.bindCamera = gameObject;

            //克隆一个CameraUI，赋值给g游戏对象
            GameObject g = Instantiate(ui.gameObject) as GameObject;

            //将uiParent设置为g的的父物体
            g.transform.SetParent(uiParent);

            g.transform.localScale = Vector3.one;

            //将克隆出来的CameraUI赋值给
            ui = g.GetComponent<CameraUI>();

            //设置当前CameraUI的朝向和位置
            ui.Bind();
        }
    }


    /// <summary>
    /// 被RealTimeMonitor调用
    /// </summary>
    /// <param name="b"></param>
    public void ShowInfoSet(bool b)
    {
        ui.ActiveUI(b);
    }
}
