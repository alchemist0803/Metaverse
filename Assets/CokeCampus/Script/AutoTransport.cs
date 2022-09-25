using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoTransport : MonoBehaviour
{
    
    public Transform[] stations;

    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.Warp(this.transform.position);
        FindNextDestination();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*private void OnTriggerEnter(Collider other)
    {
        
        //check other.tag == station
        //Find Next Destination()
        if(other.tag == "station")
            FindNextDestination();
    }*/

    public void InStation(Transform[] f_station)
    {
        stations = new Transform[f_station.Length];
        for (int i=0; i<f_station.Length; i++)
        {
            stations[i] = f_station[i];
        }

        FindNextDestination();
    }

    void FindNextDestination()
    {
        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 pos = agent.destination;
        while (pos == agent.destination)
        {
            int new_station = (Random.Range(0, 1000) % stations.Length) * 5 % stations.Length;
            pos = stations[new_station].position;

        }
        agent.destination = pos;
        
    }
}
