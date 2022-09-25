using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private Vector3[] startPos1;
    private Vector3[] startPos2;
    

    [SerializeField]
    private int lineNum;
    private float[] speed = { 9f, 8f, 7f, 6f };
    [SerializeField]
    private bool leftLine;

    CarController()
    {
        startPos1 = new Vector3[4];
        startPos2 = new Vector3[4];

        for(int i=0; i<4; i++)
        {
            startPos1[i] = new Vector3(91.5f - 3 * i, 0, 105f);
            startPos2[i] = new Vector3(103.5f - 3 * i, 0, -145f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name + " arrived");
        if(other.tag == "station")
        {
            transform.Rotate(0, 180f, 0);
            //leftLine = !leftLine;
            transform.position = (leftLine ? startPos1[lineNum-1] : startPos2[lineNum-1]);
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed[lineNum - 1]);
    }
}
