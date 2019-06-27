using UnityEngine;
using System.Collections;

public class VideoPatrolDetailLog : MonoBehaviour 
{

	public GameObject VideoPatrolDetailLogItemPrefab;
	public UIGrid DetailLogGrid;

	public UILabel Person;
	public UILabel PlanName;

	public void SetValue(VideoPatrolLogInfo PatrolLogInfo)
	{
		Logger.Instance.WriteLog("加载视频巡航日志");
		Person.text = PatrolLogInfo.person;
		PlanName.text = PatrolLogInfo.planName;

		DetailLogGrid.transform.DestroyChildren ();

		VideoPatrolDetailLogDao vpdlDao = new VideoPatrolDetailLogDao ();
		vpdlDao.Select001 (PatrolLogInfo.id);
		VideoPatrolDetailLogInfo info;
		GameObject go;
		for(int i = 0; i < vpdlDao.Result.Count; i++)
		{
			info = vpdlDao.Result[i];
			go = Instantiate(VideoPatrolDetailLogItemPrefab) as GameObject;
			DetailLogGrid.AddChild(go.transform);
			go.transform.localScale = new Vector3(1,1,1);
			go.GetComponent<VideoPatrolDetailLogItem>().SetValue((i + 1) + "",info);
		}
	}

	void OnEnable()
	{
		DetailLogGrid.transform.DestroyChildren ();
	}
}
