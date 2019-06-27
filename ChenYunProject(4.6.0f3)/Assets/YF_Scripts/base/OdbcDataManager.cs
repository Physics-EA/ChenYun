using UnityEngine;
using System.Collections;
using OdbcHelper;
using System.Data.Odbc;
using System.Xml;
public class OdbcDataManager {


	private static OdbcDataManager instance;
	public static OdbcDataManager Instance
	{
		get{
            if (instance == null)
            {
                instance = new OdbcDataManager();
            }
            else
            {
                instance.ResetConnection();
            }
			return instance;
		}
	}

	public OdbcOra odbcOra;
	public string DataBaseType;
	private string username = "";
	private string pwd = "";
	private string database = "";
	private string driver = "";
	private string dns = "";

	OdbcDataManager()
	{
		username = "";
		pwd = "";
		try
		{
			XmlDocument xmlDoc = new XmlDocument();
			string filepath = Application.dataPath + @"/YF_Config/DatabaseConfig.xml";
			xmlDoc.Load(filepath);
			XmlNodeList nodeList = xmlDoc.SelectSingleNode("ERP").ChildNodes;
			if (nodeList.Count == 1)
			{
				XmlElement xe = (XmlElement)nodeList[0];
				database = xe.GetAttribute("database");
				username = xe.GetAttribute("username");
				pwd = xe.GetAttribute("pwd");
				driver = xe.GetAttribute("driver");
				dns = xe.GetAttribute("DNS");
				DataBaseType = xe.GetAttribute("DataBaseType");
			}
			else
			{
				Logger.Instance.error("获取数据库配置应该是 1个 当前是 ：  " + nodeList.Count.ToString());
				Application.Quit();
				return;
			}
		}
		catch(XmlException e)
		{
			Logger.Instance.error(e.ToString());
			Application.Quit();
			return;
		}
		odbcOra = OdbcOra.Instance;
		bool ret = _Connection(driver,dns,username,pwd);
		if(ret == false)
		{
			Logger.Instance.info ("数据库连接失败");
			Application.Quit();
		}
	}

	private bool _Connection(string driver, string source, string userID, string passWord)
	{
		odbcOra.connectionString = "Driver={"+driver+"};DSN="+source+";Uid=" + userID+";Pwd="+passWord+";";
		odbcOra.Connection = new System.Data.Odbc.OdbcConnection(odbcOra.connectionString);
		bool ret = false;
		try
		{
			odbcOra.Connection.Open ();
			Logger.Instance.info ("数据库连接成功");
			ret =  true;
		}
		catch(OdbcException ex)
		{
			odbcOra.Connection.Close ();
			Logger.Instance.error (ex.Message);
			ret = false;
		}
		return ret;
	}
	
	public int Reconnection(string _username,string _pwd)
	{
		try
		{
			odbcOra.CloseConnection();
			odbcOra.Destroy();
			_Connection(driver,dns,username,pwd);
			username = _username;
			pwd = _pwd;
			odbcOra.ReturnDataSet("select 1 from dual","dual");
		}
		catch
		{
			ResetConnection();
			return 1;
		}
		return 0;
	}
	
	public void ResetConnection()
	{
		odbcOra.CloseConnection();
		odbcOra.Destroy();
		_Connection(driver,dns,username,pwd);
	}
	
	~OdbcDataManager()
	{
		odbcOra.CloseConnection();
		odbcOra = null;
	}


}
