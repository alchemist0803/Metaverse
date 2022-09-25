using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using agora_gaming_rtc;
using agora_utilities;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using TMPro;

public class AgoraConnector : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void drg_getUserData(int id);

    public string AppID = "6195d75b94514a34bf1ba7fde0397979";
    public string TokenID = "0066195d75b94514a34bf1ba7fde0397979IAAUecG5PI5sEFWQ9MSS7YAR7/T46Hb3nVQSMTB7eLIdUFcsjfAAAAAAEACA5jcRK0HxYQEAAQAFQfFh";
    public string ChannelID = "coke";
    public string AuthCert = "a00a92ddeea74486aed03dd33887a8f4";

    public uint ConnectedID { get; private set; }

    public GameObject _defaultScreen;
    public GameObject _defaultLoadingScreen;
    public GameObject _viewScreen;
    public GameObject[] _liveScreen;
    public GameObject[] _subLiveScreen;
    private VideoSurface m_videoSurface;
    public List<uint> m_joinerIDS;
    private IRtcEngine m_agoraEngine = null;
    private CLIENT_ROLE_TYPE m_myRole;

    private uint m_selfId;

    private bool attendInterpreter = false;
    private int interpreterId = 0;




    private int maxVideoCount = 2;

    public int currentVideoCount = 0;
    public GameObject[] VideoViewGameobject;
    public uint[] UidArray;


    // Start is called before the first frame update
    void Start()
    {
        //_defaultScreen.SetActive(true);
        //_viewScreen.SetActive(false);

        //_viewScreen = GameObject.FindGameObjectWithTag("screenvideo");
        
        VideoViewGameobject = new GameObject[6];
        UidArray = new uint[6];
    }
    private void OnEnable()
    {
    }
    // Update is called once per frame
    void Update()
    {
        /*if (!ReferenceEquals(m_agoraEngine, null))
        {
            if (WorldMgr.Singleton()._soundActive)
            {
                m_agoraEngine.EnableAudio();
            }
            else
            {
                m_agoraEngine.DisableAudio();
            }

            m_agoraEngine.EnableLocalAudio(WorldMgr.Singleton()._micActive);
            //m_agoraEngine.MuteLocalAudioStream(WorldMgr.Singleton()._micActive);
        }*/
    }

    public void SetMicActive(bool f_active)
    {
        Debug.Log("Mic Active=" + f_active);
        m_agoraEngine.EnableLocalAudio(f_active);
    }

    public void SetSoundActive(bool f_active)
    {
        //m_agoraEngine.MuteLocalAudioStream(f_active);
        //m_agoraEngine.MuteAllRemoteVideoStreams(f_active);
        //m_agoraEngine.AdjustAudioMixingVolume(f_active ? 100 : 0);
        m_agoraEngine.AdjustPlaybackSignalVolume(f_active ? 100 : 0);
    }

    public void InitAgora()
    {
        if (!ReferenceEquals(m_agoraEngine, null))
        {
            Debug.Log("Agora Engine is already inited.");
            return;
        }
#if !UNITY_EDITOR && UNITY_WEBGL
        m_agoraEngine = IRtcEngine.GetEngine(AppID);

        Debug.Log("Agora Engine is initialized.");
        // enable log
        m_agoraEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);

        m_agoraEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        Debug.Log("Register local user account.");
        m_agoraEngine.RegisterLocalUserAccount(AppID, WorldMgr.Singleton().userId);

#else

#endif
        // init engine

    }


    public void ConnectAgora(CLIENT_ROLE_TYPE f_role)
    {
        
        Debug.Log("calling join (channel = " + ChannelID + ")");

        if (m_agoraEngine == null)
            return;

        // set callbacks (optional)
        m_agoraEngine.OnLocalUserRegistered = onLocalUserRegistered;
        m_agoraEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        m_agoraEngine.OnUserJoined = onUserJoined;
        m_agoraEngine.OnUserOffline = onUserOffline;
        m_agoraEngine.OnLeaveChannel += OnLeaveChannelHandler;
        m_agoraEngine.OnWarning = (int warn, string msg) =>
        {
            Debug.LogWarningFormat("Warning code:{0} msg:{1}", warn, IRtcEngine.GetErrorDescription(warn));
        };
        m_agoraEngine.OnError = HandleError;


        m_agoraEngine.SetClientRole(f_role);
        // enable video
        if (f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER)
        {
            m_agoraEngine.EnableVideo();
            // allow camera output callback
            m_agoraEngine.EnableVideoObserver();
        }

        m_myRole = f_role;

        //mRtcEngine.SetClientRole(isHost ? CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER : CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
        var _orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_FIXED_LANDSCAPE;

        VideoEncoderConfiguration config = new VideoEncoderConfiguration
        {
            orientationMode = _orientationMode,
            degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_FRAMERATE,
            dimensions = new VideoDimensions { width = 640, height = 480 },
            //mirrorMode = VIDEO_MIRROR_MODE_TYPE.VIDEO_MIRROR_MODE_DISABLED
        };
#if !UNITY_EDITOR && UNITY_WEBGL
        m_agoraEngine.SetCloudProxy(CLOUD_PROXY_TYPE.UDP_PROXY);
#else
        m_agoraEngine.SetCloudProxy(CLOUD_PROXY_TYPE.NONE_PROXY);
        m_agoraEngine.SetCloudProxy(CLOUD_PROXY_TYPE.UDP_PROXY);

#endif

        m_agoraEngine.SetVideoEncoderConfiguration(config);
        // join channel
        //m_agoraEngine.JoinChannelByKey(TokenID, ChannelID, WorldMgr.Singleton().userName);
        m_agoraEngine.JoinChannelWithUserAccount(TokenID, ChannelID, WorldMgr.Singleton().userId);


        /*
        
        Debug.Log("calling join (channel = " + ChannelID + ")");

        if (m_agoraEngine == null)
            return;

        // set callbacks (optional)
        m_agoraEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        m_agoraEngine.OnUserJoined = onUserJoined;
        m_agoraEngine.OnUserOffline = onUserLeave;
        // m_agoraEngine.OnLeaveChannel += OnLeaveChannelHandler;
        m_agoraEngine.OnWarning = (int warn, string msg) =>
        {
            Debug.LogWarningFormat("Warning code:{0} msg:{1}", warn, IRtcEngine.GetErrorDescription(warn));
        };

        m_agoraEngine.OnError = HandleError;

        m_agoraEngine.SetClientRole(f_role);

        m_agoraEngine.EnableVideo();
        // allow camera output callback
        m_agoraEngine.EnableVideoObserver();
        // m_agoraEngine.SetClientRole(f_role);
         // enable video
         //{

         //}
         // enable video module
         //m_agoraEngine.EnableVideo();
         // if user is host, enable local video capture
         //m_agoraEngine.EnableLocalVideo(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
         // if user is audience, disable publish video stream
         //m_agoraEngine.MuteLocalVideoStream(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
         //m_agoraEngine.MuteAllRemoteVideoStreams(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);


         // if user is host, enable capture audio stream.
         //m_agoraEngine.EnableLocalAudio(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
         // if user is audience, stop publish audio stream.
         //m_agoraEngine.MuteLocalAudioStream(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
         //  if user is host, stop subscribe audio stream from audience.
         //m_agoraEngine.MuteAllRemoteAudioStreams(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
         
        var _orientationMode = ORIENTATION_MODE.ORIENTATION_MODE_ADAPTIVE;
#if !UNITY_EDITOR && UNITY_WEBGL
        m_agoraEngine.SetCloudProxy(CLOUD_PROXY_TYPE.UDP_PROXY);
#else
        m_agoraEngine.SetCloudProxy(CLOUD_PROXY_TYPE.NONE_PROXY);
        m_agoraEngine.SetCloudProxy(CLOUD_PROXY_TYPE.UDP_PROXY);
#endif
        m_myRole = f_role;
        VideoEncoderConfiguration config = new VideoEncoderConfiguration
        {
            orientationMode = _orientationMode,
            degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_FRAMERATE,
            dimensions = new VideoDimensions { width = 640, height = 480 },
            // mirrorMode = VIDEO_MIRROR_MODE_TYPE.VIDEO_MIRROR_MODE_DISABLED
            // note: mirrorMode is not effective for WebGL
        };

        m_agoraEngine.SetVideoEncoderConfiguration(config);

        ChannelMediaOptions media_options = new ChannelMediaOptions();

        media_options.autoSubscribeAudio = f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE;
        media_options.autoSubscribeVideo = f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE;
        media_options.publishLocalAudio = f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER;
        media_options.publishLocalVideo = f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER;
        

        //int channel_id_j = m_agoraEngine.JoinChannel(TokenID, ChannelID, "", 0, media_options);
        int channel_id_j = m_agoraEngine.JoinChannelByKey(TokenID, ChannelID);
        Debug.Log("join channel return " + channel_id_j);

        //mRtcEngine.JoinChannel(channel, null, 0);

         // m_agoraEngine.EnableLocalVideo(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
          // if user is audience, disable publish video stream
         // m_agoraEngine.MuteLocalVideoStream(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
         // m_agoraEngine.MuteAllRemoteVideoStreams(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
          //mRtcEngine.SetClientRole(isHost ? CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER : CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);

          // if user is host, enable capture audio stream.
          //m_agoraEngine.EnableLocalAudio(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
          // if user is audience, stop publish audio stream.
         // m_agoraEngine.MuteLocalAudioStream(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
          //  if user is host, stop subscribe audio stream from audience.
          //m_agoraEngine.MuteAllRemoteAudioStreams(f_role == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
          */
    }

    private void onLocalUserRegistered(uint uid, string userAccount)
    {
        Debug.Log("LocalUserRegisteredHandler: uid = " + uid + " userAccount = " + userAccount);
        //m_agoraEngine.JoinChannelWithUserAccount(TokenID, ChannelID, WorldMgr.Singleton().userName);
    }


    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        ConnectedID = uid;
        
        UserInfo selfData = m_agoraEngine.GetUserInfoByUid(uid);
        Debug.Log("selfData=" + selfData.userAccount);

        MakeVideoSelf(uid);
    }

    private void onUserJoined(uint uid, int elapsed)
    {

        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);

        drg_getUserData(Convert.ToInt32(uid));

        GameObject[] videos = GameObject.FindGameObjectsWithTag("screenvideo");

        Debug.Log("videos lenght=" + videos.Length);
        Debug.Log("masvideocount=" + maxVideoCount);

        if ((m_myRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE && videos.Length > maxVideoCount) || (m_myRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER && videos.Length > maxVideoCount)) return;

        if (m_joinerIDS == null) m_joinerIDS = new List<uint>();

        if (!m_joinerIDS.Contains(uid)) m_joinerIDS.Add(uid);

        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        GameObject UserVideo = Instantiate(_viewScreen, GameObject.Find("Tv").transform);
        m_videoSurface = GetChildWithName(UserVideo, "screen").GetComponent<VideoSurface>();
        if (m_videoSurface == null)
            m_videoSurface = GetChildWithName(UserVideo, "screen").AddComponent<VideoSurface>();

        UserVideo.name = uid.ToString();
        UserVideo.transform.localScale = new Vector3(.000001f, .000001f, .000001f);

        m_videoSurface.SetForUser(uid);
        m_videoSurface.SetEnable(true);
        m_videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);

        Debug.Log("make user video");
        _defaultLoadingScreen.SetActive(false);
    }

    public void MakeVideoSelf(uint f_uid)
    {
        Debug.Log("start makevideoself");


        m_selfId = f_uid;

        if (m_myRole == CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE) return;

        _defaultLoadingScreen.SetActive(false);

        drg_getUserData(Convert.ToInt32(f_uid));

        GameObject[] videos = GameObject.FindGameObjectsWithTag("screenvideo");

        if (videos.Length > maxVideoCount) return;

        GameObject SelfVideo = Instantiate(_viewScreen, GameObject.FindGameObjectWithTag("TV").transform);
        GetChildWithName(SelfVideo, "screen").AddComponent<VideoSurface>();

        SelfVideo.name = f_uid.ToString();
        SelfVideo.transform.localScale = new Vector3(.000001f, .000001f, .000001f);

        Debug.Log("end makevideoself");
    }

    public void VideoPlaneRebuild(string name = "", string role = "", string id = "")
    {

        Debug.Log("start video plane rebuild");
        GameObject[] videos = GameObject.FindGameObjectsWithTag("screenvideo");
        currentVideoCount = videos.Length;
        Debug.Log("current video count=" + currentVideoCount);

        if(attendInterpreter)
        {
            for (int j = 0; j < _subLiveScreen.Length; j++)
            {
                if (j + 1 == currentVideoCount)
                {
                    _subLiveScreen[j].SetActive(true);
                }
                else
                {
                    _subLiveScreen[j].SetActive(false);
                }
            }

            switch (currentVideoCount)
            {
                case 1:
                    videos[0].transform.localPosition = new Vector3(4.8f, .02f, 4.22f);
                    videos[0].transform.localScale = new Vector3(.8f, 1f, .85f);
                    GetChildWithName(videos[0], "Name").GetComponent<TextMeshPro>().text = name;
                    IEnumerator coroutine1 = Disappear(GetChildWithName(videos[0], "Name"));
                    StartCoroutine(coroutine1);
                    break;
                case 2:
                    for (int i = 0; i < videos.Length; i++)
                    {
                        if (videos[i].name == interpreterId.ToString())
                        {
                            videos[i].transform.localPosition = new Vector3(-3.21f, .02f, -3.6f);
                            videos[i].transform.localScale = new Vector3(.21f, 1f, .154f);
                            
                        } else
                        {
                            videos[i].transform.localPosition = new Vector3(4.87f, .02f, 4.29f);
                            videos[i].transform.localScale = new Vector3(.79f, 1f, .86f);
                        }
                        if (videos[i].name == id)
                        {
                            GetChildWithName(videos[i], "Name").GetComponent<TextMeshPro>().text = name;
                            IEnumerator coroutine2 = Disappear(GetChildWithName(videos[i], "Name"));
                            StartCoroutine(coroutine2);
                        }
                    }
                    break;
                case 3:
                    int makeVideoCount = 0;
                    for (int i = 0; i < currentVideoCount; i++)
                    {
                        if (videos[i].name == interpreterId.ToString())
                        {
                            videos[i].transform.localPosition = new Vector3(-3.21f, .02f, -3.6f);
                            videos[i].transform.localScale = new Vector3(.21f, 1f, .154f);
                        }
                        else
                        {
                            if(makeVideoCount == 0)
                            {
                                videos[i].transform.localPosition = new Vector3(4.86f, .02f, 4.81f);
                                videos[i].transform.localScale = new Vector3(.79f, 1f, .47f);
                                makeVideoCount++;
                            } else if(makeVideoCount == 1)
                            {
                                videos[i].transform.localPosition = new Vector3(4.86f, .02f, -.11f);
                                videos[i].transform.localScale = new Vector3(.79f, 1f, .47f);
                                makeVideoCount++;
                            }
                        }
                        if (videos[i].name == id)
                        {
                            GetChildWithName(videos[i], "Name").GetComponent<TextMeshPro>().text = name;
                            IEnumerator coroutine3 = Disappear(GetChildWithName(videos[i], "Name"));
                            StartCoroutine(coroutine3);
                        }
                    }
                    break;
            }
        } else
        {
            for (int j = 0; j < _liveScreen.Length; j++)
            {
                if (j + 1 == currentVideoCount)
                {
                    _liveScreen[j].SetActive(true);
                    if (j == 0)
                    {
                        GameObject timeText = _liveScreen[j].transform.Find("TimeText").gameObject;
                        timeText.GetComponent<TextMesh>().text = System.DateTime.Parse(WorldMgr.Singleton().eventDate).ToString("dd/MM/yyyy HH:mm");
                    }
                }
                else
                {
                    _liveScreen[j].SetActive(false);
                }
            }

            switch (currentVideoCount)
            {
                case 1:
                    videos[0].transform.localPosition = new Vector3(4.8f, .02f, 4.22f);
                    videos[0].transform.localScale = new Vector3(.8f, 1f, .85f);
                    if (videos[0].name == id)
                    {
                        GetChildWithName(videos[0], "Name").GetComponent<TextMeshPro>().text = name;
                        IEnumerator coroutine = Disappear(GetChildWithName(videos[0], "Name"));
                        StartCoroutine(coroutine);
                    }
                    break;
                case 2:
                    videos[0].transform.localPosition = new Vector3(4.86f, .02f, 4.82f);
                    videos[0].transform.localScale = new Vector3(.8f, 1f, .475f);
                    videos[1].transform.localPosition = new Vector3(4.8f, .02f, -.09f);
                    videos[1].transform.localScale = new Vector3(.8f, 1f, .475f);
                    for (int i = 0; i < videos.Length; i++)
                    {
                        if (videos[i].name == id)
                        {
                            GetChildWithName(videos[i], "Name").GetComponent<TextMeshPro>().text = name;
                            IEnumerator coroutine = Disappear(GetChildWithName(videos[i], "Name"));
                            StartCoroutine(coroutine);
                        }
                    }
                    break;
                    
            }
        }

        Debug.Log("end video plane rebuild");
    }

    private IEnumerator Disappear(GameObject f_textObj)
    {
        yield return new WaitForSeconds(7);
        f_textObj.SetActive(false);
    }

    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        // remove video stream
        Debug.Log("onUserOffline: uid = " + uid + " reason = " + reason);
        // this is called in main thread
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Debug.Log("go tag name=" + go.tag + "    go name=" + go.name);
            Destroy(go);

            IEnumerator f_call = CallRebuild();
            StartCoroutine(f_call);
            //VideoPlaneRebuild();
            Debug.Log("video destroyed");
        }
    }

    private IEnumerator CallRebuild()
    {
        yield return new WaitForSeconds(2);
        VideoPlaneRebuild();
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        Debug.Log("OnLeaveChannelSuccess ---- TEST");
    }

    public void MakeUid(uint f_uid)
    {
        UidArray[currentVideoCount - 1] = f_uid;
        Debug.Log("end make uid");
    }

    void onUserLeave(uint uid, USER_OFFLINE_REASON reason)
    {
        Debug.Log("OnLeaveChannelSuccess ---- TEST");
    }

    private int LastError { get; set; }
    private void HandleError(int error, string msg)
    {
        if (error == LastError)
        {
            return;
        }

        msg = string.Format("Error code:{0} msg:{1}", error, IRtcEngine.GetErrorDescription(error));

        switch (error)
        {
            case 101:
            msg += "\nPlease make sure your AppId is valid and it does not require a certificate for this demo.";
            break;
        }

        Debug.LogError(msg);

        LastError = error;
    }



    public void leave()
    {

        GameObject[] videos = GameObject.FindGameObjectsWithTag("screenvideo");
        for (int i = 0; i < videos.Length; i++)
        {
            Destroy(videos[i]);
        }

        _defaultScreen.SetActive(true);
        for (int j = 0; j < _liveScreen.Length; j++)
        {
            _liveScreen[j].SetActive(false);
        }

        m_videoSurface = null;

        Debug.Log("call leave");

        if (m_agoraEngine == null)
            return;

        // leave channel
        m_agoraEngine.LeaveChannel();
        Debug.Log("after LeaveChannel");

        // deregister video frame observers in native-c code
        m_agoraEngine.DisableVideoObserver();
        Debug.Log("after DisableVideoObserver");

        //m_agoraEngine.SetCloudProxy(CLOUD_PROXY_TYPE.NONE_PROXY);
        Debug.Log("after SetCloudProxy");
    }

    public void unloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (m_agoraEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            m_agoraEngine = null;
        }
    }

    public void ShowUserData(string data)
    {
        string[] f = data.Split(char.Parse("|"));
        Debug.Log("username = " + f[0] + " role = " + f[1] + " id = " + f[2]);
        if(f[1] == "INTERPRETER")
        {
            attendInterpreter = true;
            interpreterId = int.Parse(f[2]);
        }
        VideoPlaneRebuild(f[0], f[1], f[2]);
    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }
}
