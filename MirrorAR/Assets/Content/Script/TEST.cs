using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {

        DataLogger.Instance().RecordTrial(1,1,"TEST");
    }
}
