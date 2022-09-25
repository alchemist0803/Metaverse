using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{

    public Transform[] m_CanStation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "car")
        {
            InsertStation(other.gameObject);
        }
    }

    public void InsertStation(GameObject f_car)
    {
        f_car.GetComponent<AutoTransport>().InStation(m_CanStation);
    }
}
