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
        jointPositions[5] = new Vector3(rightEllbowPos.X / 1000f, rightEllbowPos.Y / 1000f, rightEllbowPos.Z / 1000f);
    }

    void SpawnRoute() {
        //route gestreckter Arm, von nach unten hängen, gestreckt bis gerade nach oben
        //distance zwischen hand/ellenbogen/schulter soll gleich bleiben, anderer winkel! --> kein x movement?
        float distHandEllbow = Vector3.Distance(jointPositions[7], jointPositions[6]);
        float distEllbowShoulder = Vector3.Distance(jointPositions[6], jointPositions[5]);
        float distHandShoulder = Vector3.Distance(jointPositions[7], jointPositions[5]);
        Debug.Log("dist: distHandShoulder " + distHandShoulder + jointPositions[7].y + jointPositions[5].y);

        //endpos = shoulder up + dist hand-shoulder bzw y wert ändern!
        Vector3 endpos = jointPositions[5] + Vector3.up * 2 * distHandShoulder;   // Mytherious 2 Factor ... what ever!

        //middlepos shoulder forward
        Vector3 middlepos = jointPositions[5] - Vector3.forward * 2 * distHandShoulder;   // Mytherious 2 Factor ... what ever!


        //more positions:
        Vector3[] positions = new Vector3[2];
        positions[0] = jointPositions[5] - (Vector3.up + Vector3.forward) *  distHandShoulder  ;
        positions[1] = jointPositions[5] + (Vector3.up - Vector3.forward) * distHandShoulder;
        //spawn cubes:


        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = endpos;
        cube.transform.localScale = new Vector3(.1f,.1f,.1f);

        
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = middlepos;
        cube2.transform.localScale = new Vector3(.1f, .1f, .1f);

        GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube3.transform.position = jointPositions[7];
        cube3.transform.localScale = new Vector3(.1f, .1f, .1f);

        for (int i=0; i< positions.Length; i++)
        {
            GameObject cubes = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubes.transform.position = positions[i];
            cubes.transform.localScale = new Vector3(.1f, .1f, .1f);
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
