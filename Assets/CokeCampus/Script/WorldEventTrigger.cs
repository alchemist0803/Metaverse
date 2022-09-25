using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventTrigger : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        

        //collision with auditorium building
        if (collision.gameObject.tag == "Auditorium")
        {
            Debug.Log("Hi, welcome our auditorium");
            InteractiveBuilding intr_bld = collision.gameObject.GetComponent<InteractiveBuilding>();
            intr_bld?.ShowUI(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //collision with auditorium building
        if (collision.gameObject.tag == "Auditorium")
        {
            Debug.Log("Good-bye. Please visit our auditorium again.");
            InteractiveBuilding intr_bld = collision.gameObject.GetComponent<InteractiveBuilding>();
            intr_bld?.ShowUI(false);
        }
    }

}
