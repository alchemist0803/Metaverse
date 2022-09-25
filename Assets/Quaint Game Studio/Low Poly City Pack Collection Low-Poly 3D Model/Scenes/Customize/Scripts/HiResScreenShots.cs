using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class HiResScreenShots : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void drg_saveImage(string data, int idx, int gendor, int hair, int face, int clothes, int shoes, int skin);
    //private static extern void drg_saveCharacterImage(byte[] array, int size);

    public int[] resWidth;
    public int[] resHeight;

    private bool takeHiResShot = false;

    public Camera[] m_camera;
    public RenderTexture[] m_characterTexture;
    public CustomizeInfo modelData;

    public GameObject[] m_Boy;
    public GameObject[] m_Girl;

    public static string ScreenShotName(int f_Num, CustomizeInfo f_Data)
    {
        Debug.Log("Application.persistentDataPath=" + Application.persistentDataPath);
        return string.Format("{0}/avatar{1}_{2}{3}{4}{5}{6}{7}.png",
                             Application.persistentDataPath,
                             f_Num,
                             f_Data.gendor, f_Data.hair, f_Data.face, f_Data.clothes, f_Data.shoes, f_Data.skin);
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    private void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    public void CaptureCharacterInit(CustomizeInfo f_data)
    {
        if (f_data.gendor == 0)
        {
            m_Boy[0].SetActive(false);
            m_Girl[0].SetActive(true);
            m_Girl[0].GetComponent<CharacterInit>().CaptureInit(f_data);
        }
        else if (f_data.gendor == 1)
        {
            m_Girl[0].SetActive(false);
            m_Boy[0].SetActive(true);
            m_Boy[0].GetComponent<CharacterInit>().CaptureInit(f_data);
        }
    }

    public void ShowCharacterInit(CustomizeInfo f_data, bool custom)
    {
        for(int i=0; i<m_camera.Length; i++)
        {
            if (f_data.gendor == 0)
            {
                m_Girl[i].GetComponent<CharacterInit>().customUi = custom;
                m_Girl[i].SetActive(true);
                m_Boy[i].SetActive(false);
                m_Girl[i].GetComponent<CharacterInit>().Init();
            }
            else if (f_data.gendor == 1)
            {
                m_Boy[i].GetComponent<CharacterInit>().customUi = custom;
                m_Boy[i].SetActive(true);
                m_Girl[i].SetActive(false);
                m_Boy[i].GetComponent<CharacterInit>().Init();
            }
        }
    }

    public void InitCapture(CustomizeInfo f_data, int f_num)
    {
        //modelData = f_data;
        m_camera[f_num].enabled = true;
        RenderTexture rt = new RenderTexture(resWidth[f_num], resHeight[f_num], 32);
        m_camera[f_num].targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth[f_num], resHeight[f_num], TextureFormat.ARGB32, false);
        m_camera[f_num].Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth[f_num], resHeight[f_num]), 0, 0);
        m_camera[f_num].targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();

        string enc = Convert.ToBase64String(bytes);
        //drg_saveCharacterImage(bytes, bytes.Length);
#if !UNITY_EDITOR && UNITY_WEBGL
            drg_saveImage(enc, f_num, f_data.gendor, f_data.hair, f_data.face, f_data.clothes, f_data.shoes, f_data.skin);
#endif
        /*string filename = ScreenShotName(f_num, f_data);

        Debug.Log("filename=" + filename);
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));*/
        
        takeHiResShot = false;
        m_camera[f_num].enabled = false;
    }

    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("k");
        if (takeHiResShot)
        {
            for(int i=0;i < m_camera.Length; i++)
            {
                m_camera[i].enabled = true;
                //modelData = CustomManager.sInstance.CurrentShowCharacter().currentModelInfo;
                RenderTexture rt = new RenderTexture(resWidth[i], resHeight[i], 32);
                m_camera[i].targetTexture = rt;
                Texture2D screenShot = new Texture2D(resWidth[i], resHeight[i], TextureFormat.ARGB32, false);
                m_camera[i].Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, resWidth[i], resHeight[i]), 0, 0);
                m_camera[i].targetTexture = null;
                RenderTexture.active = null; // JC: added to avoid errors
                Destroy(rt);
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = ScreenShotName(i, modelData);

                Debug.Log("filename=" + filename);
                System.IO.File.WriteAllBytes(filename, bytes);
                Debug.Log(string.Format("Took screenshot to: {0}", filename));
                takeHiResShot = false;
                m_camera[i].enabled = false;
            }
        }
    }

}