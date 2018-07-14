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
    Vector3[] positions = new Vector3[5];
    private int nextPoint = 0;
    private int counter = 1;
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

    // Use this for initialization
    void Start () {
        skeletonViewer = GameObject.Find("SkeletonViewer").GetComponent<SkeletonRenderer>();
        jointPositions = new Vector3[19]; //count from Astra JointType
        ThalmicMyo = myo.GetComponent<ThalmicMyo>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ThalmicMyo.pose == Pose.Fist && !started)
        {
            started = true;
            
            ExtendUnlockAndNotifyUserAction(ThalmicMyo);
            SaveStartPositions();    
        }
        if (started)
        {
            //CheckProgress();
        }
    }


    void SaveStartPositions() {
        Astra.Body body = skeletonViewer.Bodies[0];
        Debug.Log(skeletonViewer.Bodies.Length);
        var rightHandPos = body.Joints[7].WorldPosition; //transform to local Position!!
        jointPositions[7] = new Vector3(rightHandPos.X / 1000f, rightHandPos.Y / 1000f, rightHandPos.Z / 1000f);
        var rightEllbowPos = body.Joints[6].WorldPosition; //transform to local Position!!
        jointPositions[6] = new Vector3(rightEllbowPos.X / 1000f, rightEllbowPos.Y / 1000f, rightEllbowPos.Z / 1000f);
        var rightShoulderPos = body.Joints[5].WorldPosition; //transform to local Position!!
        jointPositions[5] = new Vector3(rightShoulderPos.X / 1000f, rightShoulderPos.Y / 1000f, rightShoulderPos.Z / 1000f);

        // route gestreckter Arm, von nach unten hängen, gestreckt bis gerade nach oben
        //distance zwischen hand/ellenbogen/schulter soll gleich bleiben, anderer winkel! --> kein x movement?
        float distHandEllbow = Vector3.Distance(jointPositions[7], jointPositions[6]);
        float distEllbowShoulder = Vector3.Distance(jointPositions[6], jointPositions[5]);
        float distHandShoulder = Vector3.Distance(jointPositions[7], jointPositions[5]);

        //more positions:

        positions[0] = new Vector3(jointPositions[5].x, jointPositions[7].y, jointPositions[7].z);
        positions[1] = new Vector3(jointPositions[5].x, jointPositions[7].y + distHandEllbow, jointPositions[7].z - distHandEllbow);
        positions[2] = new Vector3(jointPositions[5].x, jointPositions[5].y, jointPositions[5].z - distHandShoulder);
        positions[3] = new Vector3(jointPositions[5].x, jointPositions[5].y + distHandEllbow, jointPositions[5].z - distHandEllbow);
        positions[4] = new Vector3(jointPositions[5].x, jointPositions[5].y + distHandShoulder, jointPositions[7].z);

        SpawnRoute();
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
