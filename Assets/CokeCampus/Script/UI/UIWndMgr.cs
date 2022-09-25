using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UIWndMgr : MonoBehaviour
{
   

    /// <summary>
    /// UIWndMgr's singleton instance
    /// </summary>
    public static UIWndMgr Instance {
        get;
        private set;
    }

    /// <summary>
    /// loaded UI windows objects
    /// </summary>
    Dictionary<string, GameObject> m_wndMap;

    /// <summary>
    /// loaded UI windows objects
    /// </summary>
    List<GameObject> m_listActiveWnds;
    public GameObject m_wndContainer;

    public UnityEvent<UISideBar.SideBarType> SidebarChangeEvent;
    public UIWndMgr()
    {
        m_listActiveWnds = new List<GameObject>();
        m_wndMap = new Dictionary<string, GameObject>();
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        showWindow("UILoginWnd", true);
    }

    private void OnEnable()
    {
        
    }

    
    public void loadAllWnd()
    {

        if(m_wndMap == null)
        {
            m_wndMap = new Dictionary<string, GameObject>();
            Object[] prefabs = Resources.LoadAll("UI\\Prefab\\", typeof(GameObject));
            for (int i = 0; i < prefabs.Length; i++)
            {
                m_wndMap.Add(prefabs[i].name, Instantiate(prefabs[i], this.transform) as GameObject);
                //m_wndMap.Add(prefabs[i].name, Instantiate(prefabs[i], this.transform) as GameObject);
                //m_wndMap[objects[i].name] = objects[i];
                m_wndMap[prefabs[i].name].SetActive(false);

            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void showSideBar(UISideBar.SideBarType f_sidebar)
    {

        SidebarChangeEvent?.Invoke(f_sidebar);
    }
    
    
    public void showWindow(string f_name, bool f_showhide)
    {
        
        Debug.Log("Window show" + f_name);
        for (int i = 0; i < m_listActiveWnds.Count; i++)
        {
            m_listActiveWnds[i].SetActive(false);
            Debug.Log("Window disable" + m_listActiveWnds[i].name);
        }

        m_listActiveWnds.Clear();
        if (f_showhide)
        {
            
            if (m_wndMap.ContainsKey(f_name))
            {
                m_wndMap[f_name].SetActive(f_showhide);
            }
            else
            {
                Object wnd_prfb = Resources.Load<GameObject>("UI\\Prefab\\" + f_name);
                GameObject new_wnd_obj = Instantiate(wnd_prfb, m_wndContainer.transform) as GameObject;
                m_wndMap.Add(wnd_prfb.name, new_wnd_obj);
                
                new_wnd_obj.SetActive(true);
            }
            m_listActiveWnds.Add(m_wndMap[f_name]);
        }
        
    }
}
