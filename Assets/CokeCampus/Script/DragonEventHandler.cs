using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System;
using Invector.vCharacterController;


[System.Serializable]
public struct CustomizeInfo
{
    public int id;
    public int background;
    public int gendor;
    public int clothes;
    public int hair;
    public int shoes;
    public int face;
    public int skin;
    public int eye;

    public static CustomizeInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<CustomizeInfo>(jsonString);
    }
}

[Serializable]
public class MapItem
{
    public int id;
    public string name;
}

public class MapData
{
    public MapItem[] data;

    public static MapData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MapData>(jsonString);
    }
}
public class DragonEventHandler : MonoBehaviour
{
    
    [DllImport("__Internal")]
    private static extern void drg_init();
    [DllImport("__Internal")]
    private static extern void drg_home();

    private CameraPosition cameraPos;

    public GameObject mapDialog;
    public CustomizeInfo info;
    public MapData mapData;

    DragonEventHandler()
    {
        info = new CustomizeInfo();
        mapData = new MapData();
    }


    void Start()
    {
        //if the game has to operate in react ui, get keyboard event from react. 
#if !UNITY_EDITOR && UNITY_WEBGL
        PassReact();
        drg_init();
#endif

        GameObject.DontDestroyOnLoad(this);
        //OpenMap();
        //OpenAvatarWindow("");
        //InitPlayer("");
        //InitInfo();
        //After unity loading is finished, react ui is showed.


        //StartGame("ADMIN|123|123");
        // cameraPos = GameObject.Find("FocalPoint").GetComponent<CameraPosition>();
        //cameraPos.GetOriginPos();
    }

    void Update()
    {
    
    }

  //to get keyboard event from unity. 
    public void PassUnity()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            WebGLInput.captureAllKeyboardInput = true;
        #endif
    }

  //to get keyboard event from react.
    public void PassReact()
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
            WebGLInput.captureAllKeyboardInput = false;
        #endif
    }

  //unity game start
    public void StartGame(string f_roleAndEvent)
    {
        string[] f = f_roleAndEvent.Split(char.Parse("|"));
        WorldMgr.Singleton().userRole = f[0];
        WorldMgr.Singleton().eventDate = f[1];
        WorldMgr.Singleton().userId = f[2];
#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllKeyboardInput = true;
#endif

        WorldMgr.Singleton().ActivatePlayer(true);
    }

  //return react ui and game stop
    public void StopGame()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
           WebGLInput.captureAllKeyboardInput = false;
#endif

        WorldMgr.Singleton().ActivatePlayer(false);
        mapDialog.SetActive(false);
        //Camera.main.gameObject.SendMessage("ReturnOriginPos");
    }

    public void OpenAvatarWindow(string data)
    {
        info = CustomizeInfo.CreateFromJSON(data);
        /*info.background = 0;
        info.gendor = 0;
        info.hair = 1;
        info.face = 1;
        info.clothes = 1;
        info.shoes = 1;
        info.skin = 0;*/
        SceneManager.LoadScene("Customize");
    }

    public void InitCaptureData(string f_data)
    {
        CustomizeInfo cusData = CustomizeInfo.CreateFromJSON(f_data);
        if (cusData.gendor == -1)
        {
            cusData.background = 0;
            cusData.gendor = 0;
            cusData.hair = 2;
            cusData.face = 1;
            cusData.clothes = 1;
            cusData.shoes = 1;
            cusData.skin = 0;
        }
        GameObject.Find("CaptureController").GetComponent<HiResScreenShots>().CaptureCharacterInit(cusData);
    }

    public void InitInfo(int f_customUi)
    {

        //Debug.Log("f_customUi=" + f_customUi);
        /*info.background = 0;
        info.gendor = 1;
        info.hair = 2;
        info.face = 1;
        info.clothes = 1;
        info.shoes = 1;
        info.skin = 0;*/
        if(f_customUi == 1)
        {
            GameObject.Find("CaptureController").GetComponent<HiResScreenShots>().ShowCharacterInit(info, true);
        } else if(f_customUi == 0)
        {
            GameObject.Find("CaptureController").GetComponent<HiResScreenShots>().ShowCharacterInit(info, false);
        }
    }

    public void InitPlayer(string data)
    {
        //Debug.Log("player data=" + data);
        info = CustomizeInfo.CreateFromJSON(data);
        if(info.gendor == -1)
        {
            info.background = 0;
            info.gendor = 0;
            info.hair = 2;
            info.face = 1;
            info.clothes = 1;
            info.shoes = 1;
            info.skin = 0;
        }
        WorldMgr.Singleton().SetPlayer(info);
    }

    public void GoAdminLecture()
    {
        WorldMgr.Singleton()._lock = true;
        WorldMgr.Singleton().SelectInstructor();
    }

    public void GoUserLecture()
    {
        WorldMgr.Singleton()._lock = true;
        WorldMgr.Singleton().SelectAudience();
    }

    public void ExitLecture()
    {
         WorldMgr.Singleton().m_player.GetComponent<vThirdPersonInput>().ExitRoom();
    }

    public void SoundActive(int f_active)
    {
        WorldMgr.Singleton().CallSoundActive(f_active == 1 ? true : false);
    }

    public void MicActive(int f_active)
    {
        WorldMgr.Singleton().CallMIcActive(f_active == 1 ? true : false);
    }

    public void LockPersonCamera(int f_lock)
    {
        if(f_lock == 1)
        {
            Camera.main.gameObject.GetComponent<vThirdPersonCamera>().lockCamera = true;
            WorldMgr.Singleton()._lock = true;
        } else if(f_lock == 0)
        {
            Camera.main.gameObject.GetComponent<vThirdPersonCamera>().lockCamera = false;
            WorldMgr.Singleton()._lock = false;
        }
    }

    public void OpenMainScene()
    {
        SceneManager.LoadScene("cokecampus");
        drg_home();
    }
    
    public void OpenMap()
    {
        Debug.Log("call openMap");
        //data = "{\"data\":[{\"id\":1,\"name\":\"building1\"},{\"id\":2,\"name\":\"building2\"}]}";
        //Debug.Log(MapData.CreateFromJSON(data).data[0].name);


        //data = "{\"Items\":[{\"id\":1,\"name\":\"building1\"},{\"id\":2,\"name\":\"building2\"}]}";
        //Player[] player = JsonHelper.FromJson<Player>(data);
        //mapData = MapData.CreateFromJSON(data);
        //Debug.Log(JsonUtility.ToJson(player[0].name));
        mapDialog.SetActive(true);
    }

    public void CloseMap()
    {
        mapDialog.SetActive(false);
    }
}
