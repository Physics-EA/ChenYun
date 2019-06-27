using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {
	/// <summary>
	/// 进度条对象
	/// </summary>
	public UIProgressBar ProgressBar;
	/// <summary>
	/// 加载指定的场景
	/// </summary>
	/// <param name="name">需要加载的场景名</param>
	public void LoadLevel(string name)
	{
		StartCoroutine ("StartLoading",name);
	}
	/// <summary>
	/// 开始加载场景
	/// </summary>
	/// <returns>The loading.</returns>
	/// <param name="name">需要加载的场景名</param>
	private IEnumerator StartLoading(string name)
	{
		float startTime = Time.timeSinceLevelLoad;
		AsyncOperation LoadOperation = Application.LoadLevelAsync (name);
		LoadOperation.allowSceneActivation = false;
		while(LoadOperation.progress < 0.9f)
		{
			ProgressBar.value = LoadOperation.progress;
			yield return new WaitForEndOfFrame();
		}
		float endTime = Time.timeSinceLevelLoad;
		float deltaTime = endTime - startTime;
		if(deltaTime < 2)
		{
			while(deltaTime < 2)
			{
				ProgressBar.value = deltaTime * 0.5f;
				deltaTime += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}
		ProgressBar.value = 1.0f;
		yield return new WaitForEndOfFrame();
		LoadOperation.allowSceneActivation = true;
	}
}
