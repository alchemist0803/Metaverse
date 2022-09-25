using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISideBar : MonoBehaviour
{
    /// <summary>
    /// SideBarType
    /// </summary>
    public enum SideBarType
    {
        None = 0,
        Large = 1,
        Small = 2,
        Play = 3,
    }

    public SideBarType m_barType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSideBarChanged(SideBarType f_bar_type)
    {
        gameObject.SetActive(f_bar_type == m_barType);
    }
    public void OnToLarge()
    {
        UIWndMgr.Instance.showSideBar(SideBarType.Large);
        
    }

    public void OnToSmall()
    {
        UIWndMgr.Instance.showSideBar(SideBarType.Small);
    }

    public void OnToMinium()
    {
        UIWndMgr.Instance.showSideBar(SideBarType.Play);
    }

    public void OnProfile()
    {
        UIWndMgr.Instance.showSideBar(SideBarType.Large);
        UIWndMgr.Instance.showWindow("UIUniversity_explore",true);
    }

    public void OnUniverse()
    {

    }
    public void OnProgress()
    {

    }

    public void OnRanking()
    {

    }
    public void OnUniversity()
    {

    }
    public void OnNotification()
    {

    }
    public void OnNews()
    {

    }

    public void OnConfiguration()
    {

    }
}

