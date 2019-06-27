using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
using System.Collections.Generic;



public class StartServer : MonoBehaviour
{

    //用来标记是否开启服务端
    private bool hasOpenServer;

    void Awake()
    {
        //bool isRuned;
        //System.Threading.Mutex mutex = new System.Threading.Mutex (true, "ud00yq8quebixfet", out isRuned);
        //UnityEngine.Debug.Log ("唯一程序标示" + isRuned);
        //if(!isRuned)
        //{
        //	Application.Quit();
        //}
        System.Diagnostics.Process[] myPro = System.Diagnostics.Process.GetProcesses();
        int num = 0;
        foreach (Process pro in myPro)
        {
            try
            {
                if (pro != null)
                {
                    if (!pro.HasExited)
                    {

                        if (pro.ProcessName.ToUpper() == System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper())
                        {
                            num++;
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                //UnityEngine.Debug.Log(e);
            }

            if (num > 1)
            {
                UnityEngine.Debug.Log("程序已经打开");
                Application.Quit();
            }
        }

    }

    void Start()
    {
        OpenServer();
    }

    private void OpenServer()
    {

        System.Diagnostics.Process[] list = System.Diagnostics.Process.GetProcesses();
        foreach (Process pro in list)
        {
            try
            {
                if (pro != null)
                {
                    if (!pro.HasExited)
                    {
                        //UnityEngine.Debug.Log(pro);
                        if (pro.ProcessName.ToUpper() == "yfService".ToUpper())
                        {
                            hasOpenServer = true;
                            UnityEngine.Debug.Log("服务端已经打开");
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
        }


        if (!hasOpenServer)
        {
            UnityEngine.Debug.Log("没有打开服务端,现在打开");
            Process.Start(Application.dataPath + "/../Service/yfService.exe");
        }
    }



    void Update()
    {

    }




}
