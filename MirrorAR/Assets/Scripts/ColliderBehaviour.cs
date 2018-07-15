using System.Collections;
using System.Collections.Generic;
using Thalmic.Myo;
using UnityEngine;

public class ColliderBehaviour : MonoBehaviour {

    private ManagerScript _manager;

    private void Start()
    {
        _manager = GameObject.Find("Manager").GetComponent<ManagerScript>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _manager.OnColliderEntered(this.gameObject);
    }
}
