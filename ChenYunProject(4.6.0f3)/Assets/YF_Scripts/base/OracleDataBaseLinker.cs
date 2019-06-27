using UnityEngine;
using System.Collections;
using System.Xml;
public class OracleDataBaseLinker
{
//	public static ConnForOracle instance;
//
//	private static string username = "";
//	private static string pwd = "";
//	private static string database = "";
//	static OracleDataBaseLinker()
//	{
//		username = "";
//		pwd = "";
//		XmlDocument xmlDoc = new XmlDocument();
//		string filepath = Application.dataPath + @"/YF_Config/DatabaseConfig.xml";
//		xmlDoc.Load(filepath);
//		XmlNodeList nodeList = xmlDoc.SelectSingleNode("ERP").ChildNodes;
//		foreach(XmlElement xe in nodeList )
//		{
//			database                   = xe.GetAttribute("database");
//			username                   = xe.GetAttribute("username");
//			pwd                  	   = xe.GetAttribute("pwd");		
//		}
//		if(ConnForOracle.Instance == null)
//		{
//			ConnForOracle.Create ();
//		}
//		ConnForOracle.Instance.Init(username,pwd,database);
//		instance = ConnForOracle.Instance;
//	}
//
//	public static int Reconnection(string _username,string _pwd)
//	{
//		try
//		{
//			instance.CloseConn();
//			instance.Destroy();
//			ConnForOracle.Create();
//			ConnForOracle.Instance.Init(_username,_pwd,database);
//			instance = ConnForOracle.Instance;
//			instance.ReturnDataSet("select 1 from dual","dual");
//		}
//		catch
//		{
//			ResetConnection();
//			return 1;
//		}
//		return 0;
//	}
//
//	public static void ResetConnection()
//	{
//		instance.Destroy();
//		ConnForOracle.Create();
//		ConnForOracle.Instance.Init(username,pwd,database);
//		instance = ConnForOracle.Instance;
//	}
//
//	~OracleDataBaseLinker()
//	{
//		instance.CloseConn ();
//	}
}
