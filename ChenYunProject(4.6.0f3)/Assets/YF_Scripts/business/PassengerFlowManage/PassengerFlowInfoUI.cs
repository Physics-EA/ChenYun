using UnityEngine;
using System.Collections;

/// <summary>
/// 人流面板UI，显示人流信息
/// </summary>
public class PassengerFlowInfoUI : MonoBehaviour
{


    /// <summary>
    /// 进入的人数直方图
    /// </summary>
    public UISprite inBar;

    /// <summary>
    /// 滞留的人数直方图
    /// </summary>
    public UISprite standBar;

    /// <summary>
    /// 离开的人数直方图
    /// </summary>
    public UISprite outBar;

    /// <summary>
    /// 用来存储进入人流标签
    /// </summary>
    public UILabel inLb;

    /// <summary>
    /// 用来存储滞留人流的标签
    /// </summary>
    public UILabel standLb;

    /// <summary>
    /// 用来存储离开人流的标签
    /// </summary>
    public UILabel outLb;

    /// <summary>
    /// 用来设置每个等级的人数
    /// </summary>
    public int grade1 = 1;
    public int grade2 = 1;
    public int grade3 = 1;
    public int grade4 = 1;

    public int test;

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="inNum">进入的人数</param>
    /// <param name="standNum">滞留的人数</param>
    /// <param name="outNum">离开的人数</param>
    public void UpdateValue(int inNum, int standNum, int outNum)
    {
        //更新人数
        inLb.text = inNum.ToString();
        //更新滞留人数
        standLb.text = standNum.ToString();
        //更新离开人数
        outLb.text = outNum.ToString();

        //如果进入的人数为三级以上
        if (inNum >= grade3)
        {
            //将进入人流的bar改为红色
            inBar.color = Color.red;
        }

        //如果进入的人数为二级以上
        else if (inNum >= grade2)
        {
            //将进入人流的bar改为黄色
            inBar.color = Color.yellow;
        }

        else
        {
            //将进入人流的bar改为绿色
            inBar.color = Color.green;
        }

        //用来判断进入的人数，以便设置进入人数的bargraph颜色
        float t = (float)inNum / (float)grade3;


        if (t > 1)
        {
            t = 1;
        }

        //进入人流的bar高度
        inBar.height = 2 + (int)(80 * t);

        if (standNum >= grade3)
        {
            //将滞留人流的bar改为红色
            standBar.color = Color.red;
        }
        else if (standNum >= grade2)
        {
            //将滞留人流的bar改为黄色
            standBar.color = Color.yellow;
        }
        else
        {
            //将滞留人流的bar改为绿色
            standBar.color = Color.green;
        }

        //如果第三等级的人数为0
        if (grade3 == 0)
        {
            grade3 = 1;
        }

        t = (float)standNum / (float)grade3;

        if (t > 1)
        {
            t = 1;
        }
        //滞留人流的bar高度
        standBar.height = 2 + (int)(80 * t);


        //如果离开的人流大于3级
        if (outNum >= grade3)
        {
            //将离开人流的bar改为红色
            outBar.color = Color.red;
        }

        //如果离开人流等级大于二级
        else if (outNum >= grade2)
        {
            //将离开人流的bar改为黄色
            outBar.color = Color.yellow;
        }
        else
        {
            //将离开人流的bar改为绿色
            outBar.color = Color.green;
        }

        t = (float)outNum / (float)grade3;

        if (t > 1)
        {
            t = 1;
        }
        //设置离开人流bar的高度
        outBar.height = 2 + (int)(80 * t);
    }
}
