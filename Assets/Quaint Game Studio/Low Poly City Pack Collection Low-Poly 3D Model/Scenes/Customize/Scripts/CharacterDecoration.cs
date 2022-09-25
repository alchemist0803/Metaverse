using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDecoration : MonoBehaviour
{
    public int f_sex = 0;
    private DragonEventHandler dragonEventScript;

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

    public enum DecoratePart{
        CLOTH,
        HAIR,
        SHOES,
        FACE,
        SKIN,
        EYE
    }
    [SerializeField]
    public CustomizeInfo currentModelInfo;
    /*{
        id = 0,
        background = 0,
        gendor = 0,
        clothes = 0,
        hair = 0,
        shoes = 0,
        face = 0,
        skin = 0,
        eye = 0
    };*/
    CharacterDecoration()
    {
        currentModelInfo = new CustomizeInfo();
    }
    public void Initialize()
    {
        //dragonEventScript = GameObject.Find("DragonEventHandler").GetComponent<DragonEventHandler>();

        Decorate(DecoratePart.CLOTH, currentModelInfo.clothes);
        Decorate(DecoratePart.HAIR, currentModelInfo.hair);
        Decorate(DecoratePart.SHOES, currentModelInfo.shoes);
        Decorate(DecoratePart.FACE, currentModelInfo.face);
        Decorate(DecoratePart.SKIN, currentModelInfo.skin, true);

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
        if(f_skinFlag)
        {
            for(int i=0; i<m_bodySkins.Length; i++)
            {
                m_heads[0].GetComponent<Renderer>().material = m_faceSkins1[f_idx];
                m_heads[1].GetComponent<Renderer>().material = m_faceSkins2[f_idx];
                m_heads[2].GetComponent<Renderer>().material = m_faceSkins3[f_idx];
                m_body.GetComponent<Renderer>().material = m_bodySkins[f_idx];
            }
        } else
        {
            GameObject[] _objects = GetPartObjects(f_part);
            for (int i = 0; i < _objects.Length; i++)
            {
                _objects[i].SetActive(i == f_idx);
            }
        }

        switch (f_part)
        {
            case DecoratePart.CLOTH:
                currentModelInfo.clothes = f_idx;
                break;
            case DecoratePart.FACE:
                currentModelInfo.face = f_idx;
                break;
            case DecoratePart.HAIR:
                currentModelInfo.hair = f_idx;
                break;
            case DecoratePart.SHOES:
                currentModelInfo.shoes = f_idx;
                break;
            case DecoratePart.EYE:
                currentModelInfo.eye = f_idx;
                break;
            case DecoratePart.SKIN:
                currentModelInfo.skin = f_idx;
                break;
            default:
                break;
        }
        CustomManager.sInstance.changeActiveToggle();
    }
}
