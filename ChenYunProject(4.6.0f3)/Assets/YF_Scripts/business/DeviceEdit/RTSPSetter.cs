using UnityEngine;
using System.Collections;

public class RTSPSetter : MonoBehaviour {

	public UIInput URLInput;
	public UILabel UseRTSP;
	public UIInput PFURLInput;
	private string id;
	private bool UseRtsp;
	public void SetValue(string url,bool useRTSP,string pfUrl,string id)
	{
		this.id = id;
		URLInput.value = url;
		PFURLInput.value = pfUrl;
		UseRtsp = useRTSP;
		UseRTSP.text = (UseRtsp ? "是" : "否");
	}


	public void SaveData()
	{
		DeviceDao dDao = new DeviceDao();
		dDao.Update005(URLInput.value.ToString().Trim(),(UseRtsp ? "1" : "0"),PFURLInput.value,id);
		Close();
	}

	public void ChangeUseRTSP()
	{
		UseRtsp = (UseRtsp ? false : true);
		UseRTSP.text = (UseRtsp ? "是" : "否");
	}

	public void Close()
	{
		Configure.IsOperating = false;
		gameObject.SetActive(false);
	}


}
