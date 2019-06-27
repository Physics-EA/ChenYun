using UnityEngine;
using System.Collections;


/// <summary>
/// 客流区域UI
/// </summary>
public class PassengerAreaUI : MonoBehaviour
{
    /// <summary>
    /// 这块客流区域的名字
    /// </summary>
    public UILabel name;

    /// <summary>
    /// 这块客流区域进入的人数
    /// </summary>
    public UILabel inNum;

    /// <summary>
    /// 这块客流区域滞留的人数
    /// </summary>
    public UILabel standNum;

    /// <summary>
    /// 这块客流区域离开的人数
    /// </summary>
    public UILabel outNum;

    /// <summary>
    /// 一个向量，用来让UI看向摄像机的位置
    /// </summary>
    private Vector3 vec;

    /// <summary>
    /// 固定UI的坐标
    /// </summary>
    private Vector3 bindVec;


    /// <summary>
    /// 约束客流统计区域的位置
    /// </summary>
    /// <param name="targ"></param>
    /// <param name="_name"></param>
    public void Bind(Vector3 targ, string _name)
    {
        //世界坐标转换成屏幕坐标
        bindVec = UICamera.mainCamera.WorldToScreenPoint(targ + new Vector3(0, 2.5f, 0));

        //屏幕坐标转换成世界坐标
        transform.position = UICamera.mainCamera.ScreenToWorldPoint(bindVec);

        //更改PassengerAreaUI的名称
        name.text = _name;
    }


    /// <summary>
    /// 更新这块统计区域的人流信息
    /// </summary>
    /// <param name="_in">进入的人数</param>
    /// <param name="_stand">滞留的人数</param>
    /// <param name="_out">离开的人数</param>
    public void UpdateArea(string _in, string _stand, string _out)
    {
        inNum.text = _in;
        standNum.text = _stand;
        outNum.text = _out;
    }


    void Update()
    {
        //UI的坐标为UI摄像机从屏幕坐标转换为世界坐标的位置
        transform.position = UICamera.mainCamera.ScreenToWorldPoint(bindVec);

        //主摄像机的位置与UI之间的向量差
        vec = Camera.main.transform.position - transform.position;

        //总之表示一个方向
        vec = transform.position - vec;

        //UI看向摄像机
        transform.LookAt(vec);
    }
}
