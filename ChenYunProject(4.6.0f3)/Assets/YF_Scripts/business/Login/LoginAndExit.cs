using UnityEngine;
using System.Collections;
using System;

public class LoginAndExit : MonoBehaviour
{

    /// <summary>
    /// 加载界面
    /// </summary>
    public GameObject LoadingUI;

    /// <summary>
    /// 用户名
    /// </summary>
    public UIInput UserName;

    /// <summary>
    /// 密码
    /// </summary>
    public UIInput Password;

    /// <summary>
    /// UI信息
    /// </summary>
    public UILabel Message;

    void Update()
    {
        //如果按下Tab键
        if (Input.GetKey(KeyCode.Tab))
        {
            if (UserName.isSelected)
            {
                Password.isSelected = true;
            }
        }
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    public void Login()
    {

        Logger.Instance.WriteLog("开始登录");

        //检索个人登录信息
        UserBasicDao ubdao = new UserBasicDao();

        try
        {
            ubdao.Select001(UserName.value, Password.value);
        }

        catch (Exception ex)
        {
            Logger.Instance.WriteLog("数据库连接异常");
            Message.text = "数据库连接异常,请查看output_log.txt文件查看错误原因。";
            Debug.Log(ex.ToString());
            return;
        }

        if (ubdao.Result.Count == 1)
        {
            if (ubdao.Result[0].Status == "正常")
            {
                DataStore.UserInfo = ubdao.Result[0];
                //检索用户所属组
                GroupDao gDao = new GroupDao();
                gDao.Select002(DataStore.UserInfo.ID);
                DataStore.GPInfo = gDao.Result[0];
                //检索用户所属组的权限列表
                GroupAuthorityDao gaDao = new GroupAuthorityDao();
                gaDao.Select001(DataStore.GPInfo.Id);
                //检索权限信息
                AuthorityDao aDao = new AuthorityDao();
                aDao.Select001();
                //将用户的权限详细信息保存下来
                foreach (GroupAuthorityInfo gaInfo in gaDao.Result)
                {
                    foreach (AuthorityInfo aInfo in aDao.Result)
                    {
                        if (gaInfo.AuthorityId == aInfo.Id)
                        {
                            DataStore.AuthorityInfos.Add(aInfo);
                            break;
                        }
                    }
                }
                Logger.Instance.WriteLog("登录成功");
                Message.text = "";
                LoadingUI.SetActive(true);
                LoadingUI.SendMessage("LoadLevel", "EditScene");
            }
        }
        else
        {
            Message.text = "登录失败，用户名或密码错误。";
        }
    }

    public void Exit()
    {
        Logger.Instance.WriteLog("退出程序");
        Application.Quit();
    }
}
