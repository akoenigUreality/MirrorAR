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
            manager.myo.GetComponent<ThalmicMyo>().Vibrate(VibrationType.Medium);
            manager.SpawnRoute();
        }
        else
        {
            manager.myo.GetComponent<ThalmicMyo>().Vibrate(VibrationType.Short);
        }
        Debug.Log("collision");
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter()
    {
        
    }
}
