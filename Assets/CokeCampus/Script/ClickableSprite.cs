using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableSprite : MonoBehaviour
{
    public string Message;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        //UIWndMgr.Instance.gameObject.SendMessage(Message);
        WorldMgr.Singleton().SendMessage(Message);

    }
}
