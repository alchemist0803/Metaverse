using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWndContainer : MonoBehaviour
{
    public float Max_Width;
    public float Small_Width;
    public float Large_Width;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSideBarChanged(UISideBar.SideBarType f_bar_id)
    {
        RectTransform rect_tran = this.gameObject.GetComponent<RectTransform>();
        if (f_bar_id == UISideBar.SideBarType.Large) // large side bar
        {
            this.gameObject.SetActive(true);
            rect_tran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Small_Width);
        } else if(f_bar_id == UISideBar.SideBarType.Small) // small side bar
        {
            this.gameObject.SetActive(true);
            rect_tran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Large_Width);
        } else if(f_bar_id == UISideBar.SideBarType.Play) // mini side bar
        {
            this.gameObject.SetActive(false);
        }
        else // UIWndMgr.SideBarType.none
        {
            this.gameObject.SetActive(true);
            rect_tran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Max_Width);
        }
    }
}
