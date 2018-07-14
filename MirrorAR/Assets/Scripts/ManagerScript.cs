using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class ManagerScript : MonoBehaviour {
    public GameObject myo = null;
    public Text helpText;
    public GameObject spawn;

    private ThalmicMyo thalmicMyo;
    private bool started = false;
    private SkeletonRenderer skeletonViewer;
    private Vector3[] jointPositions; //Nummerierung gleich Astra -> public enum JointType
    float distHandEllbow, distEllbowShoulder, distHandShoulder;
    GameObject start, end;
    Vector3[] positions = new Vector3[5];
    Vector3 posStart, posEnd;
    private int exercise = 2;
    private bool moved = false;
    
    
    //old stuff
    private int nextPoint = 0;
    private int counter = 0;
    public int NextPoint
    {
        get
        {
            return nextPoint;
        }

        set
        {
            nextPoint = value;
        }
    }

    public int Counter
    {
        get
        {
            return counter;
        }

        set
        {
            counter = value;
        }
    }

    public ThalmicMyo ThalmicMyo
    {
        get
        {
            return thalmicMyo;
        }

        set
        {
            thalmicMyo = value;
        }
    }

    public bool Moved
    {
        get
        {
            return moved;
        }

        set
        {
            moved = value;
        }
    }

    // Use this for initialization
    void Start () {
        skeletonViewer = GameObject.Find("SkeletonViewer").GetComponent<SkeletonRenderer>();
        jointPositions = new Vector3[19]; //count from Astra JointType
        thalmicMyo = myo.GetComponent<ThalmicMyo>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (thalmicMyo.pose == Pose.Fist && !started)
        {
            started = true;
            counter = 0;
            ExtendUnlockAndNotifyUserAction(thalmicMyo);
            SaveStartPositions();
            Debug.Log("start");
            Spawn();
        }
        if (started)
        {
            switch (exercise) {
                case 1:
                    ShoulderYZ();
                    break;
                case 2:
                    ShoulderYX();
                    break;
                default: break;

            }
            //Check exercise
        }
    }

    void ShoulderYZ() {
        Astra.Body body = skeletonViewer.Bodies[0];
        var rightHandPos = body.Joints[7].WorldPosition; //transform to local Position!!
        var jointPos = new Vector3(rightHandPos.X / 1000f, rightHandPos.Y / 1000f, rightHandPos.Z / 1000f);
        
        if (Vector3.Distance(new Vector3(jointPos.x,0,0), new Vector3(jointPositions[7].x,0,0)) > 0.05f) {
            Debug.Log("uffbassa! YZ");
            //thalmicMyo.Vibrate(VibrationType.Short);
        }
        //Toggle trigger boxes
        if (moved)
        {
            start.GetComponent<BoxCollider>().enabled = true;
            end.GetComponent<BoxCollider>().enabled = false;
        }
        else {
            start.GetComponent<BoxCollider>().enabled = false;
            end.GetComponent<BoxCollider>().enabled = true;
        }
        

    }
    void ShoulderYX() {
        Astra.Body body = skeletonViewer.Bodies[0];
        var rightHandPos = body.Joints[7].WorldPosition; //transform to local Position!!
        var jointPos = new Vector3(rightHandPos.X / 1000f, rightHandPos.Y / 1000f, rightHandPos.Z / 1000f);
        
        if (Vector3.Distance(new Vector3(0,0, jointPos.z), new Vector3(0,0, jointPositions[7].z)) > 0.15f) {
            Debug.Log("uffbassa! YX");
            //thalmicMyo.Vibrate(VibrationType.Short);
        }
        //Toggle trigger boxes
        if (moved)
        {
            start.GetComponent<BoxCollider>().enabled = true;
            end.GetComponent<BoxCollider>().enabled = false;
        }
        else {
            start.GetComponent<BoxCollider>().enabled = false;
            end.GetComponent<BoxCollider>().enabled = true;
        }
        

    }

    void Spawn() {
        switch (exercise) {
            case 1:
                posStart = new Vector3(jointPositions[5].x, jointPositions[7].y, jointPositions[7].z);
                posEnd = new Vector3(jointPositions[5].x, jointPositions[5].y, jointPositions[5].z - distHandShoulder);
                start = Instantiate(spawn, posStart, Quaternion.identity);
                start.GetComponent<BoxCollider>().enabled = false;
                end = Instantiate(spawn, posEnd, Quaternion.identity);
                break;
            case 2:
                posStart = new Vector3(jointPositions[5].x + distHandShoulder, jointPositions[5].y, jointPositions[5].z);
                posEnd = new Vector3(jointPositions[5].x, jointPositions[7].y, jointPositions[7].z);
                start = Instantiate(spawn, posStart, Quaternion.identity);
                start.GetComponent<BoxCollider>().enabled = false;
                end = Instantiate(spawn, posEnd, Quaternion.identity);
                break;
            default: break;
        }
    }

    void SaveStartPositions() {
        Astra.Body body = skeletonViewer.Bodies[0];
        var rightHandPos = body.Joints[7].WorldPosition; //transform to local Position!!
        jointPositions[7] = new Vector3(rightHandPos.X / 1000f, rightHandPos.Y / 1000f, rightHandPos.Z / 1000f);
        var rightEllbowPos = body.Joints[6].WorldPosition; //transform to local Position!!
        jointPositions[6] = new Vector3(rightEllbowPos.X / 1000f, rightEllbowPos.Y / 1000f, rightEllbowPos.Z / 1000f);
        var rightShoulderPos = body.Joints[5].WorldPosition; //transform to local Position!!
        jointPositions[5] = new Vector3(rightShoulderPos.X / 1000f, rightShoulderPos.Y / 1000f, rightShoulderPos.Z / 1000f);

        // route gestreckter Arm, von nach unten hängen, gestreckt bis gerade nach oben
        //distance zwischen hand/ellenbogen/schulter soll gleich bleiben, anderer winkel! --> kein x movement?
        distHandEllbow = Vector3.Distance(jointPositions[7], jointPositions[6]);
        distEllbowShoulder = Vector3.Distance(jointPositions[6], jointPositions[5]);
        distHandShoulder = Vector3.Distance(jointPositions[7], jointPositions[5]);

        //more positions:
        /*
        positions[0] = new Vector3(jointPositions[5].x, jointPositions[7].y, jointPositions[7].z);
        positions[1] = new Vector3(jointPositions[5].x, jointPositions[7].y + distHandEllbow, jointPositions[7].z - distHandEllbow);
        positions[2] = new Vector3(jointPositions[5].x, jointPositions[5].y, jointPositions[5].z - distHandShoulder);
        positions[3] = new Vector3(jointPositions[5].x, jointPositions[5].y + distHandEllbow, jointPositions[5].z - distHandEllbow);
        positions[4] = new Vector3(jointPositions[5].x, jointPositions[5].y + distHandShoulder, jointPositions[7].z);

        SpawnRoute();
        */
    }

   public void SpawnRoute()
    {
        for (int i=0; i< positions.Length; i++)
        {
            Instantiate(spawn, positions[i],Quaternion.identity);
        }       
    }

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }
}
