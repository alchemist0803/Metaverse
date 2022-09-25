using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{

    private bool isRotation = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isRotation)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 3);
        }
    }

    public void LeftRotation()
    {
        transform.Rotate(Vector3.up * 15);
        isRotation = false;
        Invoke("StartRotate", 30);
    }

    public void RightRotation()
    {
        transform.Rotate(Vector3.down * 15);
        isRotation = false;
        Invoke("StartRotate", 30);
    }

    private void StartRotate()
    {
        isRotation = true;
    }
}
