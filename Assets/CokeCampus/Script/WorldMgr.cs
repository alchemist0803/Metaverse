using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WorldMgr : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void drg_showBtn(bool data);

    

    static WorldMgr sInstance;

    public Transform m_AudiencePos;
    public Transform m_InstructorPos;
    public Transform m_SchoolPos;

    public GameObject m_AgoraConnector;
    public GameObject AgoraConnectorPrefab;


    public GameObject w_defaultScreen;
    public GameObject w_defaultLoadingScreen;
    public GameObject w_viewScreen;

    public GameObject[] w_liveScreens;
    public GameObject[] w_subLiveScreens;

    public GameObject m_BoyPlayer;
    public GameObject m_GirlPlayer;

    private bool selectedAudience = false;
    public bool selectedInstructor = false;

    public string userRole;
    public string eventDate;
    public string userId;
    public bool _lock;


    static public WorldMgr Singleton()
    {
        return sInstance;
    }
    public GameObject m_player;
    // Start is called before the first frame update
    void Start()
    {

        if (sInstance == null)
        {
            sInstance = this;
        }
        _lock = true;

        InitAgora();
    }

    // Update is called once per frame
    void Update()
    {
        if(selectedAudience)
        {
            m_player.transform.rotation = m_AudiencePos.rotation;
        } else if(selectedInstructor)
        {
            if(_lock)
            {
                Camera.main.gameObject.GetComponent<vThirdPersonCamera>().defaultDistance = -2.5f;
                if(m_player.transform.position.x < 39.8f || m_player.transform.position.x > 40.2f || m_player.transform.position.z < 2.8f || m_player.transform.position.z > 3.2f)
                {
                    m_player.transform.position = m_InstructorPos.position;
                }
                m_player.transform.rotation = m_InstructorPos.rotation;
                
            } else
            {
                Camera.main.gameObject.GetComponent<vThirdPersonCamera>().defaultDistance = 2.5f;
            }
            //selectedInstructor = false;
        }
    }

    public void InitAgora()
    {
        Destroy(m_AgoraConnector);
        m_AgoraConnector = null;
        m_AgoraConnector = Instantiate(AgoraConnectorPrefab);
        m_AgoraConnector.name = "AgoraConnector";

        AgoraConnector _agoraConnector = m_AgoraConnector.GetComponent<AgoraConnector>();
        _agoraConnector._defaultScreen = w_defaultScreen;
        _agoraConnector._defaultLoadingScreen = w_defaultLoadingScreen;
        if(w_viewScreen != null)
            _agoraConnector._viewScreen = w_viewScreen;

        _agoraConnector._liveScreen = new GameObject[w_liveScreens.Length];
        for (int i = 0; i < w_liveScreens.Length; i++)
            _agoraConnector._liveScreen[i] = w_liveScreens[i];

        _agoraConnector._subLiveScreen = new GameObject[w_subLiveScreens.Length];
        for (int j = 0; j < w_subLiveScreens.Length; j++)
            _agoraConnector._subLiveScreen[j] = w_subLiveScreens[j];

        m_AgoraConnector?.SendMessage("InitAgora");
    }

    public void ActivatePlayer(bool f_showfalse)
    {
        //m_AgoraConnector = GameObject.Find("AgoraConnector");
        //m_AgoraConnector?.SendMessage("InitAgora");//InitAgora();
        Debug.Log("call ActivatePlayer" + f_showfalse);
        InitAgora();

        if (m_player == null)
            Debug.Log("m_player is null");

        if (m_player != null)
        {
            Debug.Log("m_plyaer isn't null");
            m_player.SetActive(f_showfalse);

            if(!f_showfalse)
            {
                Destroy(m_player);
            }
        }
        if(!f_showfalse)
        {
            Camera.main.GetComponent<CameraPosition>().ReturnOriginPos();
            Camera.main.gameObject.GetComponent<vThirdPersonCamera>().lockCamera = true;
            Camera.main.gameObject.GetComponent<vThirdPersonCamera>().smoothCameraRotation = 1f;
        }
        Camera.main.GetComponent<vThirdPersonCamera>().enabled = f_showfalse;
    }

    public void SetPlayer(CustomizeInfo f_data)
    {
        Destroy(m_player);
        if(f_data.gendor == 0)
        {
            m_player = Instantiate(m_GirlPlayer, new Vector3(22.05f, 0.145f, 0.34f), new Quaternion());
            //m_player = Instantiate(m_GirlPlayer, m_PlayerPos.transform);
        } else
        {
            m_player = Instantiate(m_BoyPlayer, new Vector3(22.05f, 0.145f, 1.314f), new Quaternion());
            //m_player = Instantiate(m_BoyPlayer, m_PlayerPos.transform);
        }

        m_player.GetComponent<CharacterInit>().Init();
    }

    public void SelectInstructor()
    {
        InitAgora();
        selectedInstructor = true;

        w_defaultScreen.SetActive(false);
        w_defaultLoadingScreen.SetActive(true);
        GameObject timeText = w_defaultLoadingScreen.transform.Find("TimeText").gameObject;

        timeText.GetComponent<TextMesh>().text = System.DateTime.Parse(eventDate).ToString("dd/MM/yyyy HH:mm");

        m_player.transform.position = m_InstructorPos.position;
        m_player.GetComponent<Rigidbody>().MovePosition(m_InstructorPos.position);

        m_player.GetComponent<vThirdPersonController>().InLectureRoom();

        m_AgoraConnector?.SendMessage("ConnectAgora", agora_gaming_rtc.CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
#if !UNITY_EDITOR && UNITY_WEBGL
        drg_showBtn(true);
#endif
        m_player.transform.rotation = m_InstructorPos.rotation;
        //m_AgoraConnector.ConnectAgora(agora_gaming_rtc.CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    }

    public void SelectAudience()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        drg_showBtn(true);
#endif

        InitAgora();

        selectedAudience = true;

        w_defaultScreen.SetActive(false);
        w_defaultLoadingScreen.SetActive(true);
        GameObject timeText = w_defaultLoadingScreen.transform.Find("TimeText").gameObject;
        timeText.GetComponent<TextMesh>().text = System.DateTime.Parse(eventDate).ToString("dd/MM/yyyy HH:mm");

        Camera.main.gameObject.GetComponent<vThirdPersonCamera>().lockCamera = true;
        Camera.main.gameObject.GetComponent<vThirdPersonCamera>().smoothCameraRotation = 1f;

        m_player.transform.position = m_AudiencePos.position;
        m_player.transform.rotation = m_AudiencePos.rotation;
        m_player.GetComponent<Rigidbody>().MovePosition(m_AudiencePos.position);

        m_player.GetComponent<vThirdPersonController>().Sit();
        m_player.GetComponent<vThirdPersonController>().InLectureRoom();

        m_AgoraConnector?.SendMessage("ConnectAgora", agora_gaming_rtc.CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
        /*if(callCount == 1)
            m_AgoraConnector?.SendMessage("ConnectAgora", agora_gaming_rtc.CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
        else
        {
            m_AgoraConnector?.SendMessage("setVideo", true);
            m_AgoraConnector?.SendMessage("MakeVideosurface");
        }*/
        //m_AgoraConnector.ConnectAgora(agora_gaming_rtc.CLIENT_ROLE_TYPE.CLIENT_ROLE_AUDIENCE);
    }

    public void ExitAuditorium()
    {

#if !UNITY_EDITOR && UNITY_WEBGL
        drg_showBtn(false);
#endif

        m_AgoraConnector?.SendMessage("leave");

        m_AgoraConnector?.SendMessage("unloadEngine");

        //m_AgoraConnector?.SendMessage("EnableVideo", true);

        Debug.Log("end call leave");

        selectedAudience = false;
        selectedInstructor = false;
        Camera.main.gameObject.GetComponent<vThirdPersonCamera>().lockCamera = true;
        Camera.main.gameObject.GetComponent<vThirdPersonCamera>().smoothCameraRotation = 1f;
        Camera.main.gameObject.GetComponent<vThirdPersonCamera>().defaultDistance = 2.5f;

        Debug.Log("start m_player position");

        m_player.transform.position = m_SchoolPos.position;

        Debug.Log("start m_player rigidbody position");

        m_player.GetComponent<Rigidbody>().MovePosition(m_SchoolPos.position);

        Debug.Log("start exit");

        m_player.GetComponent<vThirdPersonController>().Exit();

        Debug.Log("end");
    }

    public void CallMIcActive(bool f_a)
    {
        m_AgoraConnector?.SendMessage("SetMicActive", f_a);
    }

    public void CallSoundActive(bool f_a)
    {
        m_AgoraConnector?.SendMessage("SetSoundActive", f_a);
    }
}
