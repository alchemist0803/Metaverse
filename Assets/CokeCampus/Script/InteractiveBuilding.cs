using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBuilding : MonoBehaviour
{
    public List<GameObject> UIObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUI(bool f_bShow)
    {
        if(WorldMgr.Singleton().userRole=="ADMIN")
        {
            for (int i = 0; i < UIObjects.Count; i++)
            {
                UIObjects[i].SetActive(f_bShow);
            }
        }
        else if(WorldMgr.Singleton().userRole == "PRODUCER" || WorldMgr.Singleton().userRole == "INTERPRETER" || WorldMgr.Singleton().userRole == "SPEAKER")
        {
            UIObjects[0].SetActive(f_bShow);
        }
        else if(WorldMgr.Singleton().userRole == "USER" || WorldMgr.Singleton().userRole == "Colaborator")
        {
            UIObjects[1].SetActive(f_bShow);
        }
    }
}
