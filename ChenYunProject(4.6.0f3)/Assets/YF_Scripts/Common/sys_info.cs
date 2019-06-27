using UnityEngine;
using System.Collections;
using System.Xml;
using System;
public class sys_info
{
    //系统相关的全局定义
    //系统的版本号
    public const string version = "0.0.1";
    //软件名称
    public const string name = "陈云故居实时监控调度系统";

    public static string CMS_ip = "";
    public static ushort CMS_port = 0;
    public static string CMS_loginName = "";
    public static string CMS_loginPassword = "";
    public static UInt16 CMS_Protocol = 0;

    //
    public const int MAX_NAME_SIZE = 36;

    public const int MAX_IPADDRESS_SIZE = 32;
    //摄像头的名字长度
    public const int MAX_CAMARENAME_SIZE = 64;
    //摄像头的描述
    public const int MAX_CAMARE_DESC_SZIE = 200;
    //共享内存的最大值
    public const int MAX_SHARE_MEMORY_SIZE = 1280 * 720 * 4 * 4 + 8;

    //输入事件相关定义
    //鼠标左键事件
    public const int MOUSE_LEFT_BUTTON = 0;
    //鼠标右键事件
    public const int MOUSE_RIGHT_BUTTON = 1;
    //鼠标中键事件
    public const int MOUSE_MIDDLE_BUTTON = 2;

    public static sys_info instance;
    public static sys_info Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new sys_info();
            }
            return instance;
        }
    }

    static sys_info()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filepath = Application.dataPath + @"/YF_Config/DatabaseConfig.xml";
        xmlDoc.Load(filepath);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("ERP").ChildNodes;
        foreach (XmlElement xe in nodeList)
        {
            CMS_ip = xe.GetAttribute("CMS_ip");
            CMS_port = ushort.Parse(xe.GetAttribute("CMS_port"));
            CMS_loginName = xe.GetAttribute("CMS_loginName");
            CMS_loginPassword = xe.GetAttribute("CMS_loginPassword");
            CMS_Protocol = UInt16.Parse(xe.GetAttribute("CMS_Protocol"));
        }

    }
}
