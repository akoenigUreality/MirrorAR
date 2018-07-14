using System.Collections;
using System.Collections.Generic;
using Thalmic.Myo;
using UnityEngine;

public class ColliderBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        ManagerScript manager = GameObject.Find("Manager").GetComponent<ManagerScript>();
        if (GameObject.FindGameObjectsWithTag("WayPoint").Length == 1)
        {
            //manager.NextPoint += manager.Counter;
            // points inverten/neue fkt setzten für reihenfolge
            manager.Counter *= -1; // ivert direction
            manager.SpawnRoute();
        }
        else
        {
            manager.NextPoint += manager.Counter;
        }
        manager.ThalmicMyo.Vibrate(VibrationType.Short);
        Destroy(this.gameObject);
        Debug.Log("next: " + manager.NextPoint);
    }
    private void OnTriggerEnter()
    {
        
    }
}
