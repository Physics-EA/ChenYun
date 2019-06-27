using UnityEngine;
using UnityEditor;

public class ExportAssetBundles
{
    //在Unity编辑器中添加菜单
    [MenuItem("Assets/Build AssetBundle From Selection")]
    static void ExportResourceRGB2()
    {
        // 打开保存面板，获得用户选择的路径
        
	Debug.Log("路径是 : "+Application.streamingAssetsPath);
		Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/YF_Prefabs/ChenyunStatus.prefab");
		if(obj!=null)
		{
			BuildPipeline.BuildAssetBundle(obj, null,
		                               Application.streamingAssetsPath + "/Test.assetbundle",
		                               BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets
		                               | BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.StandaloneWindows);
		}
        
    }	
}

