using UnityEngine;
using System.Collections;

public class LoginInfo : MonoBehaviour
{

    public UILabel Group;
    public UILabel RealName;
    public UILabel RealTime;
    void Start()
    {

        //test
        Group.text = DataStore.GPInfo.Name + "：";
        RealName.text = DataStore.UserInfo.RealName;
        RealTime.text = System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
    }
    void Update()
    {
        //实时显示系统时间
        RealTime.text = System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
    }

    public void UpdateInfo()
    {
        Group.text = DataStore.GPInfo.Name + "：";
        RealName.text = DataStore.UserInfo.RealName;
    }

}
