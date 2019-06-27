using UnityEngine;
using System.Collections;
using System.IO;
using System;



public class Logger
{

    enum LOG_FILE_CREATER_TIME_INTERVAL
    {
        month,
        day,
    }

    private static Logger instance;
    private StreamWriter logSW;
    private DateTime dt;
    private LOG_FILE_CREATER_TIME_INTERVAL LogFileCreaterTimeInterval;       //日志生成文件的时间间隔

    public static Logger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }
    }

    private Logger()
    {
        LogFileCreaterTimeInterval = LOG_FILE_CREATER_TIME_INTERVAL.day;

        OpenFile();
    }
    private void OpenFile()
    {
        dt = DateTime.Now;
        if (!Directory.Exists("log"))
        {
            Directory.CreateDirectory("log");
        }
        logSW = new StreamWriter(new FileStream("log/" + dt.ToString("yyyy_MM_dd") + "_操作日志.log", FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Read));
        logSW.AutoFlush = true;
        WriteLog("打开log文件");
    }
    private void CloseFile()
    {
        if (logSW != null)
        {
            logSW.Close();
        }
    }
    private void ChangeFile()
    {
        CloseFile();
        OpenFile();
    }

    public void WriteLog(string msg)
    {
        debug(msg);
    }

    public void WriteLog(string msgType, string msg)
    {
        switch (LogFileCreaterTimeInterval)
        {
            case LOG_FILE_CREATER_TIME_INTERVAL.day:
                {
                    if (DateTime.Now.ToString("yyyy_MM_dd") != dt.ToString("yyyy_MM_dd"))
                    {
                        ChangeFile();
                    }
                }
                break;
            case LOG_FILE_CREATER_TIME_INTERVAL.month:
                {
                    if (DateTime.Now.ToString("yyyy_MM") != dt.ToString("yyyy_MM"))
                    {
                        ChangeFile();
                    }
                }
                break;
            default:
                {
                    if (DateTime.Now.ToString("yyy_MM_dd") != dt.ToString("yyyy_MM_dd"))
                    {
                        ChangeFile();
                    }
                }
                break;
        }


        logSW.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "[" + msgType + "][" + msg + "]");
    }

    public void info(string msg)
    {
        WriteLog("info", msg);
    }

    public void debug(string msg)
    {
        WriteLog("debug", msg);
    }

    public void error(string msg)
    {
        WriteLog("error", msg);
    }

    ~Logger()
    {
        if (logSW != null)
        {
            CloseFile();
        }
    }
}
