using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInit : MonoBehaviour
{
    private DragonEventHandler dragonEventScript;

    public HiResScreenShots hiResScreenShots;
    public int cameraNum;
    public bool customUi;

    public GameObject m_body;
    public GameObject[] m_costumes;
    public GameObject[] m_eyes;
    public GameObject[] m_hairs;
    public GameObject[] m_heads;
    public GameObject[] m_shoes;

    public Material[] m_bodySkins;
    public Material[] m_faceSkins1;
    public Material[] m_faceSkins2;
    public Material[] m_faceSkins3;

    public enum DecoratePart
    {
        CLOTH,
        HAIR,
        SHOES,
        FACE,
        SKIN,
        EYE
    }
    public CustomizeInfo characterData;

    CharacterInit()
    {
        characterData = new CustomizeInfo();
    }

    public void Initialize()
    {
        //dragonEventScript = GameObject.Find("DragonEventHandler").GetComponent<DragonEventHandler>();

        Decorate(DecoratePart.CLOTH, characterData.clothes);
        Decorate(DecoratePart.HAIR, characterData.hair);
        Decorate(DecoratePart.SHOES, characterData.shoes);
        Decorate(DecoratePart.FACE, characterData.face);
        Decorate(DecoratePart.SKIN, characterData.skin, true);
    }

    private GameObject[] GetPartObjects(DecoratePart f_part)
    {
        switch (f_part)
        {
            case DecoratePart.CLOTH:
                return m_costumes;
            case DecoratePart.FACE:
                return m_heads;
            case DecoratePart.HAIR:
                return m_hairs;
            case DecoratePart.SHOES:
                return m_shoes;
            case DecoratePart.EYE:
                return m_eyes;
            default:
                return null;
        }
    }

    public void Decorate(DecoratePart f_part, int f_idx, bool f_skinFlag = false)
    {
        if (f_skinFlag)
        {
            for (int i = 0; i < m_bodySkins.Length; i++)
            {
                m_heads[0].GetComponent<Renderer>().material = m_faceSkins1[f_idx];
                m_heads[1].GetComponent<Renderer>().material = m_faceSkins2[f_idx];
                m_heads[2].GetComponent<Renderer>().material = m_faceSkins3[f_idx];
                m_body.GetComponent<Renderer>().material = m_bodySkins[f_idx];
            }
        }
        else
        {
            GameObject[] _objects = GetPartObjects(f_part);
            for (int i = 0; i < _objects.Length; i++)
            {
                _objects[i].SetActive(i == f_idx);
            }
        }

    }

    public void CaptureInit(CustomizeInfo f_data)
    {
        characterData = f_data;
        Initialize();
        if (hiResScreenShots != null)
        {
            hiResScreenShots.InitCapture(characterData, 3);
        }
    }

    public void Init()
    {
        dragonEventScript = GameObject.Find("DragonEventHandler").GetComponent<DragonEventHandler>();

        if (customUi)
        {
            //Debug.Log("1 character data current model info");
            characterData = CustomManager.sInstance.CurrentShowCharacter().currentModelInfo;
        }
        else
        {
            //Debug.Log("0 character data info");
            characterData = dragonEventScript.info;
        }
        Initialize();

        if (hiResScreenShots != null)
        {
            if (cameraNum == 0)
            {
                hiResScreenShots.InitCapture(characterData, cameraNum);
                hiResScreenShots.InitCapture(characterData, 3);
            }
            else
            {
                hiResScreenShots.InitCapture(characterData, cameraNum);
            }
        }
    }
}
