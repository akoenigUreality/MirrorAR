using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class MovementManager : MonoBehaviour {

    public GameObject myo = null;
    public Text helpText;

    private Pose _lastPose = Pose.Unknown;
    private bool started = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        if (thalmicMyo.pose == Thalmic.Myo.Pose.FingersSpread)
        {
            started = true;
        }
        if (started)
        {
            helpText.text = "started";
        }
    }
}
