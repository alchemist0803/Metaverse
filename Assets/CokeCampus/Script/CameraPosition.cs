using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public GameObject m_orignObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GetOriginPos() {

    }

    public void ReturnOriginPos()
    {
        //transform.SetParent(m_orignObj.transform);
        transform.position = m_orignObj.transform.position;
        transform.rotation = m_orignObj.transform.rotation;
        
    }
}
