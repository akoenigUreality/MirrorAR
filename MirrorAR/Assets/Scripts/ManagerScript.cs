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

    private bool started = false;
    private SkeletonRenderer skeletonViewer;
    private Vector3[] jointPositions; //Nummerierung gleich Astra -> public enum JointType
    // Use this for initialization
    void Start () {
        skeletonViewer = GameObject.Find("SkeletonViewer").GetComponent<SkeletonRenderer>();
        jointPositions = new Vector3[19]; //count from Astra JointType
	}

    // Update is called once per frame
    void Update()
    {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        if (thalmicMyo.pose == Pose.Fist && !started)
        {
            started = true;
            thalmicMyo.Vibrate(VibrationType.Medium);
            ExtendUnlockAndNotifyUserAction(thalmicMyo);
            SaveStartPositions();
            SpawnRoute();
        }
    }


    void SaveStartPositions() {
        Astra.Body body = skeletonViewer.Bodies[0];
        Debug.Log(skeletonViewer.Bodies.Length);
        //for schleife, falls noch mehr joints gebraucht werden!
        var rightHandPos = body.Joints[7].WorldPosition; //transform to local Position!!
        jointPositions[7] = new Vector3(rightHandPos.X / 1000f, rightHandPos.Y / 1000f, rightHandPos.Z / 1000f);
        var rightEllbowPos = body.Joints[6].WorldPosition; //transform to local Position!!
        jointPositions[6] = new Vector3(rightEllbowPos.X / 1000f, rightEllbowPos.Y / 1000f, rightEllbowPos.Z / 1000f);
        var rightShoulderPos = body.Joints[5].WorldPosition; //transform to local Position!!
        jointPositions[5] = new Vector3(rightShoulderPos.X / 1000f, rightShoulderPos.Y / 1000f, rightShoulderPos.Z / 1000f);
    }

   public void SpawnRoute() {
        //route gestreckter Arm, von nach unten hängen, gestreckt bis gerade nach oben
        //distance zwischen hand/ellenbogen/schulter soll gleich bleiben, anderer winkel! --> kein x movement?
        float distHandEllbow = Vector3.Distance(jointPositions[7], jointPositions[6]);
        float distEllbowShoulder = Vector3.Distance(jointPositions[6], jointPositions[5]);
        float distHandShoulder = Vector3.Distance(jointPositions[7], jointPositions[5]);
        Debug.Log("dist: distHandShoulder " + distHandShoulder + jointPositions[7].y + jointPositions[5].y);


        //more positions:
        Vector3[] positions = new Vector3[5];
       // positions[0] = jointPositions[5] - (Vector3.up + Vector3.forward) *  distHandEllbow  ;
        //positions[1] = jointPositions[5] + (Vector3.up - Vector3.forward) * distHandEllbow;
        positions[0] = new Vector3(jointPositions[5].x, jointPositions[7].y + distHandEllbow, jointPositions[7].z - distHandEllbow);
        positions[1] = new Vector3(jointPositions[5].x, jointPositions[5].y + distHandEllbow, jointPositions[5].z - distHandEllbow);
        positions[2] = new Vector3(jointPositions[5].x, jointPositions[5].y + distHandShoulder, jointPositions[7].z);
        positions[3] = new Vector3(jointPositions[5].x, jointPositions[5].y, jointPositions[5].z - distHandShoulder);
        positions[4] = new Vector3(jointPositions[5].x, jointPositions[7].y, jointPositions[7].z);
        //spawn cubes:

        
        for (int i=0; i< positions.Length; i++)
        {
            Instantiate(spawn, positions[i],Quaternion.identity);
            /*
            GameObject cubes = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubes.transform.position = positions[i];
            cubes.transform.localScale = new Vector3(.1f, .1f, .1f);
            */
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
