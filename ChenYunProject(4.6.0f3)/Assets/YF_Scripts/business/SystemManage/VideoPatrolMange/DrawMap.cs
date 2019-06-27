using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DrawMap : MonoBehaviour {

	public UILabel Title;
	public GameObject MapTex;
	public GameObject MapIcon;
	public float ScaleSize;
	public GameObject LinePrefab;
	private List<GameObject> MapIcos = new List<GameObject>();
	private Dictionary<string,GameObject> DicMapIcos = new Dictionary<string, GameObject>();
	private List<GameObject> Lines = new List<GameObject> ();
	private List<DeviceInfo> DeviceInfos;

	public void Draw(string title,string[] Monitors, bool drawLine = false)
	{
		Title.text = title;
		if(DeviceInfos == null || DeviceInfos.Count <= 0)
		{
			DeviceDao dDao = new DeviceDao ();
			dDao.Select001 ();
			DeviceInfos = dDao.Result;
		}

		Clear ();

		foreach(string id in Monitors)
		{
			DeviceInfo info = findDeviceInfo(id);
			if(info.Id != id || DicMapIcos.ContainsKey(id))continue;
			GameObject mapico = Instantiate (MapIcon) as GameObject;
			mapico.GetComponent<UISprite>().color = Color.yellow;
			mapico.transform.parent = MapTex.transform;
			mapico.transform.localScale = new Vector3 (ScaleSize * 0.5f,ScaleSize * 0.5f,1);
			mapico.transform.localPosition = new Vector3 ((float.Parse(info.PosX)) * ScaleSize,(float.Parse(info.PosZ)) * ScaleSize,0);
			MapIcos.Add(mapico);
			DicMapIcos.Add(id,mapico);
		}
		if(MapIcos.Count > 0)
		{
			_StartPos = MapIcos[0].transform.localPosition;
		}

		_DrawLine = drawLine;
	}

	public void Clear()
	{
		for(int i = 0; i < MapIcos.Count;i++)
		{
			Destroy(MapIcos[i]);
		}
		MapIcos.Clear();
		for(int i = 0; i < Lines.Count;i++)
		{
			Destroy(Lines[i]);
		}
		Lines.Clear ();

		DicMapIcos.Clear();
	}

	private Vector3 _StartPos;
	private bool _DrawLine;
	public void SetMapIconColor(string id,Color color,bool highLight = false)
	{
		DicMapIcos[id].GetComponent<UISprite>().color = color;
		if(highLight)
		{
			DicMapIcos[id].transform.localScale = new Vector3 (ScaleSize,ScaleSize,1);
		}
		else
		{
			DicMapIcos[id].transform.localScale = new Vector3 (ScaleSize * 0.5f,ScaleSize * 0.5f,1);
		}
		if(_DrawLine && highLight)
		{
			Vector3 _endPos = DicMapIcos[id].transform.localPosition;
			if(Lines.Count > 0)
			{
				Lines[Lines.Count - 1].GetComponent<UISprite>().color = Color.red;
			}
			DrawLine(_StartPos,_endPos);
			if(Lines.Count > 0)
			{
				Lines[Lines.Count - 1].GetComponent<UISprite>().color = Color.green;
			}
			_StartPos = _endPos;
		}
	}
	
	public void DrawLine(Vector3 startPos,Vector3 endPos)
	{
		Vector3 middlePos = (startPos + endPos) * 0.5f;
		float length = Vector3.Distance(startPos,endPos);
		
		GameObject go = Instantiate(LinePrefab) as GameObject;
		go.transform.parent = MapTex.transform;
		go.transform.localScale = new Vector3(length * 0.01f,1,1);
		go.transform.eulerAngles = Vector3.zero;
		go.transform.localPosition = middlePos;
		Vector2 dir = new Vector2(startPos.x,startPos.y) - new Vector2(endPos.x,endPos.y);
		if(startPos.y < endPos.y)
		{
			dir = -1 * dir;
		}
		float angel =  Vector2.Angle(dir,new Vector2(1,0));
		go.transform.Rotate(0,0, angel );
		
		Lines.Add(go);
	}

	private void DrawLine()
	{
		if(MapIcos.Count >= 2)
		{
			for(int i = 0; i < MapIcos.Count - 1;i++)
			{
				Vector3 startPos = MapIcos[i].transform.localPosition;
				Vector3 endPos = MapIcos[i + 1].transform.localPosition;
				Vector3 middlePos = (startPos + endPos) * 0.5f;

				float length = Vector3.Distance(startPos,endPos);

				GameObject go = Instantiate(LinePrefab) as GameObject;
				go.transform.parent = MapTex.transform;
				go.transform.localScale = new Vector3(length * 0.01f,1,1);
				go.transform.eulerAngles = Vector3.zero;
				go.transform.localPosition = middlePos;
				Vector2 dir = new Vector2(startPos.x,startPos.y) - new Vector2(endPos.x,endPos.y);
				if(startPos.y < endPos.y)
				{
					dir = -1 * dir;
				}
				float angel =  Vector2.Angle(dir,new Vector2(1,0));
				go.transform.Rotate(0,0, angel );


				Lines.Add(go);
			}
		}
	}

	private DeviceInfo findDeviceInfo(string id)
	{
		foreach(DeviceInfo info in DeviceInfos)
		{
			if(info.Id == id)
			{
				return info;
			}
		}
		return new DeviceInfo();
	}
}
