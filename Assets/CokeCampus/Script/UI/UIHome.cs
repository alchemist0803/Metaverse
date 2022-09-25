using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHome : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEnter()
    {
        
        UIWndMgr.Instance.showWindow("UIHome", false);
        UIWndMgr.Instance.showSideBar(UISideBar.SideBarType.Play);
        WorldMgr.Singleton().ActivatePlayer(true);
    }

    public void OnChat()
    {

    }
}
