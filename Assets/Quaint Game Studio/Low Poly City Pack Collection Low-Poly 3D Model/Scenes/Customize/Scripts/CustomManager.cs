using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class CustomManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void drg_save(int _background, int _gendor, int _clothes, int _hair, int _shoes, int _face, int _skin);

    private DragonEventHandler dragonEventScript;
    private GameObject dragonEventHandler;
    private bool isClicked = true;


    public GameObject main;
    [SerializeField]
    public GameObject[] m_pages;
    public GameObject male;
    public GameObject female;
    public GameObject m_showCharacter;
    public GameObject plane;
    [SerializeField]
    public Toggle[] m_toggles;
    public Texture[] m_planeMaterial;

    public GameObject[] m_boyImage;
    public GameObject[] m_girlImage;

    public Toggle[] m_t_gendor;
    public Toggle[] m_t_hair;
    public Toggle[] m_t_face;
    public Toggle[] m_t_clothes;
    public Toggle[] m_t_shoes;
    public Toggle[] m_t_skin;
    public bool isMain = true;

    private Vector3 mouseDownPos;

    //public CharacterDecoration currentCharacter;

    public static CustomManager sInstance { get; private set; }
    CustomManager()
    {
        sInstance = this;
    }
    void Start()
    {
        dragonEventHandler = GameObject.Find("DragonEventHandler");
        main.SetActive(true);

        switch (dragonEventHandler.GetComponent<DragonEventHandler>().info.gendor)
        {
            case 0:
                changeCharacter(false, true);
                break;
            case 1:
            default:
                changeCharacter(true, true);
                break;
        }

        changeBackground(dragonEventHandler.GetComponent<DragonEventHandler>().info.background);

        changeActiveToggle();

        //CharacterDecoration currentCharacter = CurrentShowCharacter();
    }


    public void GotoPage()
    {
        if (isClicked)
        {
            main.SetActive(false);
            for(int i = 0; i < m_pages.Length; i++)
            {
                m_pages[i].SetActive(m_toggles[i].isOn);
            }
            isMain = false;
        }
    }
    
    public void GotoMain(GameObject fromPage)
    {
        isClicked = false;
        main.SetActive(true);
        fromPage.SetActive(false);
        for (int i = 0; i < m_toggles.Length; i++)
        {
            m_toggles[i].isOn = false;
        }
        isMain = true;
        isClicked = true;
    }

    public void GotoHome()
    {
        dragonEventHandler.SendMessage("OpenMainScene");
    }

    public void rotation(bool left, int degree)
    {
        if (left)
        {
            m_showCharacter.transform.Rotate(Vector3.up * degree);
        } else
        {
            m_showCharacter.transform.Rotate(Vector3.down * degree);
        }
    }

    public void SaveModel()
    {
        Debug.Log("unity customize data=" + CurrentShowCharacter().currentModelInfo);
        GameObject.Find("CaptureController").GetComponent<HiResScreenShots>().ShowCharacterInit(CurrentShowCharacter().currentModelInfo, true);
#if !UNITY_EDITOR && UNITY_WEBGL
        drg_save(
            CurrentShowCharacter().currentModelInfo.background,
            CurrentShowCharacter().currentModelInfo.gendor,
            CurrentShowCharacter().currentModelInfo.clothes,
            CurrentShowCharacter().currentModelInfo.hair,
            CurrentShowCharacter().currentModelInfo.shoes,
            CurrentShowCharacter().currentModelInfo.face,
            CurrentShowCharacter().currentModelInfo.skin
        );
#endif
    }

    public void changeCharacter(bool gendor, bool firstCall = false)
    {
        female.SetActive(!gendor);
        male.SetActive(gendor);

        for(int i=0; i<m_boyImage.Length; i++)
        {
            m_boyImage[i].SetActive(gendor);
            m_girlImage[i].SetActive(!gendor);
        }
        if (!firstCall)
        {
            CustomizeInfo modeldata = CurrentShowCharacter().currentModelInfo;

            m_showCharacter = (gendor ? male: female);
            modeldata.gendor = (gendor ? 1 : 0);
            CurrentShowCharacter().currentModelInfo = modeldata;
        } else
        {
            m_showCharacter = (gendor ? male : female);

            CurrentShowCharacter().currentModelInfo = dragonEventHandler.GetComponent<DragonEventHandler>().info;
        }
        CurrentShowCharacter().Initialize();
    }

    public void changeBackground(int idx)
    {
        CharacterDecoration currentCharacter = CurrentShowCharacter();
        currentCharacter.currentModelInfo.background = idx;
        switch (idx)
        {
            case 0:
                plane.GetComponent<Renderer>().material.SetTexture("_MainTex", m_planeMaterial[0]);
                break;
            case 1:
                plane.GetComponent<Renderer>().material.SetTexture("_MainTex", m_planeMaterial[1]);
                break;
            case 2:
                plane.GetComponent<Renderer>().material.SetTexture("_MainTex", m_planeMaterial[2]);
                break;
            default:
                break;
        }
    }

    public void changeActiveToggle()
    {
        CharacterDecoration currentCharacter = CurrentShowCharacter();

        m_t_gendor[currentCharacter.currentModelInfo.gendor].isOn = true;
        m_t_hair[currentCharacter.currentModelInfo.hair].isOn = true;
        m_t_face[currentCharacter.currentModelInfo.face].isOn = true;
        m_t_clothes[currentCharacter.currentModelInfo.clothes].isOn = true;
        m_t_shoes[currentCharacter.currentModelInfo.shoes].isOn = true;
        m_t_skin[currentCharacter.currentModelInfo.skin].isOn = true;
    }

    public CharacterDecoration CurrentShowCharacter()
    {
        return m_showCharacter.GetComponent<CharacterDecoration>();
    }

}
