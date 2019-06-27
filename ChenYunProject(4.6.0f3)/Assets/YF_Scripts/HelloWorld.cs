using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using libYunFuCSV;


public class HelloWorld : MonoBehaviour {




    Texture2D txt2dtest;

    LibCSV libcsv;

    void OnDestroy()
    {
        if (libcsv !=  null)
        {
            LibCSV.unInitLib();
        }
       
    }

	// Use this for initialization
    public void Start()
    {
        //
        string path = Application.dataPath;
        string sdkpath = path + "/../SDKDLL";
        libcsv = LibCSV.InitLib(sdkpath);

        txt2dtest = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        GameObject.FindGameObjectWithTag("Game1BG").GetComponent<UITexture>().mainTexture = txt2dtest;
      
       
	}

    // Update is called once per frame
    private long mCurrentDataTicks = 0;
    float mTimer = 0.1f;
	void Update () {

        mTimer = mTimer - Time.deltaTime;
        if (mTimer < 0)
        {
            mTimer = 0.1f;

            if (libcsv != null)
            {
                Byte[] videoData = LibCSV.GetCurrentVideoData();
                if (txt2dtest != null && videoData != null)
                {
                    if (videoData.Length == txt2dtest.width * txt2dtest.height * 4)
                    {
                        txt2dtest.LoadRawTextureData(videoData);
                        txt2dtest.Apply();
                        LibCSV.SetWrite();
                    }

                }
            }
        }

        
        

	
	}

    void Awake()
    {
    
    }


    void OnGUI()
    {
        GUI.skin.label.fontSize = 100;
        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), "Hello World");
       
    }

}
