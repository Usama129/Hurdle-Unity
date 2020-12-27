using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;


public class KinectMap : MonoBehaviour
{
    protected Animator animator;
    public GameObject BodySourceManager;
    public bool ikActive = false;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;


    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };


    Dictionary<string, string> jointsMap = new Dictionary<string, string>() {
        { "SpineBase", "B-hips"},
        { "SpineMid", "B-spine" },
        { "Neck", "B-neck" },
        { "Head", "B-head" },
        { "ShoulderLeft", "B-upper_arm_L"},
        { "ElbowLeft", "B-forearm_L"},
        { "WristLeft", "B-hand_L"},
        { "ShoulderRight", "B-upper_arm_R"},
        { "ElbowRight", "B-forearm_R"},
        { "WristRight", "B-hand_R"},
        { "HipLeft", "B-thigh_L"},
        { "KneeLeft", "B-shin_L"},
        { "AnkleLeft", "B-foot_L"},
        { "FootLeft", "B-toe_L" },
        { "HipRight", "B-thigh_R"},
        { "KneeRight", "B-shin_R"},
        { "AnkleRight", "B-foot_R"},
        { "FootRight", "B-toe_R" },
        { "SpineShoulder", "B-upperChest"},
    };

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BodySourceManager == null)
        {
            Debug.Log("BodySourceManager is null");
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            Debug.Log("_BodyManager is null");
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            Debug.Log("Data from _BodyManager is null");
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {//change here
                    _Bodies[body.TrackingId] = gameObject;
                }
               

                RefreshBodyObject(body);
              

            }
        }
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject obj = Instantiate(gameObject, new Vector3(0f, 1.8f, 0f), Quaternion.identity);
        obj.name = "Body:" + id;
        Debug.Log(obj.name);
        return obj ;
    }

   

    private void RefreshBodyObject(Kinect.Body body)
    {

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            if (jt.ToString() == "HandLeft" || jt.ToString() == "HandTipLeft" || jt.ToString() == "ThumbLeft"
                     || jt.ToString() == "HandRight" || jt.ToString() == "HandTipRight" || jt.ToString() == "ThumbRight")
                        continue;
            

            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }


            if (jt.ToString() == "ElbowRight")
            {

                Vector3 RElbow = GetVector3FromJoint(body.Joints[Kinect.JointType.ElbowRight]);
                Vector3 RShoulder = GetVector3FromJoint(body.Joints[Kinect.JointType.ShoulderRight]);
                Vector3 RWrist = GetVector3FromJoint(body.Joints[Kinect.JointType.WristRight]);
                float AngleRElbow = AngleBetweenTwoVectors(RElbow - RShoulder, RElbow - RWrist);
                Transform jointObj = RecursiveFindChild(gameObject.transform, jointsMap[jt.ToString()]);
                if (jointObj.localRotation.y < AngleRElbow - 10 || jointObj.localRotation.y > AngleRElbow + 10)
                    jointObj.localRotation = Quaternion.Euler(0f, 180 - AngleRElbow, 0f);

            }
            else if (jt.ToString() == "ElbowLeft")
            {
                Vector3 LElbow = GetVector3FromJoint(body.Joints[Kinect.JointType.ElbowLeft]);
                Vector3 LShoulder = GetVector3FromJoint(body.Joints[Kinect.JointType.ShoulderLeft]);
                Vector3 LWrist = GetVector3FromJoint(body.Joints[Kinect.JointType.WristLeft]);
                float AngleLElbow = AngleBetweenTwoVectors(LElbow - LShoulder, LElbow - LWrist);
                Transform jointObj = RecursiveFindChild(gameObject.transform, jointsMap[jt.ToString()]);
                if (jointObj.localRotation.y < AngleLElbow - 10 || jointObj.localRotation.y > AngleLElbow + 10)
                    jointObj.localRotation = Quaternion.Euler(0f, 180 - AngleLElbow, 0f);
            }

            else if (jt.ToString() == "ShoulderRight")
            {


            }
            else if (jt.ToString() == "ShoulderLeft")
            {

            }

            else if (jt.ToString() == "HipRight")
            {
                Vector3 RKnee = GetVector3FromJoint(body.Joints[Kinect.JointType.KneeRight]);
                Vector3 RHip = GetVector3FromJoint(body.Joints[Kinect.JointType.HipRight]);
                Vector3 RShoulder = GetVector3FromJoint(body.Joints[Kinect.JointType.ShoulderRight]);
                float AngleHip = AngleBetweenTwoVectors(RHip - RShoulder, RHip - RKnee);
                Transform jointObj = RecursiveFindChild(gameObject.transform, jointsMap[jt.ToString()]);
                //jointObj.localRotation = Quaternion.Euler(AngleHip + 20, 0f, 0f);
            }

            else if (jt.ToString() == "HipLeft")
            {

                Vector3 LKnee = GetVector3FromJoint(body.Joints[Kinect.JointType.KneeLeft]);
                Vector3 LHip = GetVector3FromJoint(body.Joints[Kinect.JointType.HipLeft]);
                Vector3 LShoulder = GetVector3FromJoint(body.Joints[Kinect.JointType.ShoulderLeft]);

                float AngleHip = AngleBetweenTwoVectors(LHip - LShoulder, LHip - LKnee);
                Transform jointObj = RecursiveFindChild(gameObject.transform, jointsMap[jt.ToString()]);

                //jointObj.localRotation = Quaternion.Euler(AngleHip + 20, 0f, 0f);
            }

            else if (jt.ToString() == "KneeRight")
            {
                Vector3 RKnee = GetVector3FromJoint(body.Joints[Kinect.JointType.KneeRight]);
                Vector3 RHip = GetVector3FromJoint(body.Joints[Kinect.JointType.HipRight]);
                Vector3 RAnkle = GetVector3FromJoint(body.Joints[Kinect.JointType.AnkleRight]);

                float AngleKnee = AngleBetweenTwoVectors(RKnee - RHip, RKnee - RAnkle);
                Transform jointObj = RecursiveFindChild(gameObject.transform, jointsMap[jt.ToString()]);
                if (AngleKnee < jointObj.localRotation.y - 5 || AngleKnee > jointObj.localRotation.y + 5)
                {

                    jointObj.localRotation = Quaternion.Euler(0f, 180 - AngleKnee, 0f);
                }


            }

            else if (jt.ToString() == "KneeLeft")
            {

                Vector3 LKnee = GetVector3FromJoint(body.Joints[Kinect.JointType.KneeLeft]);
                Vector3 LHip = GetVector3FromJoint(body.Joints[Kinect.JointType.HipLeft]);
                Vector3 LAnkle = GetVector3FromJoint(body.Joints[Kinect.JointType.AnkleLeft]);

                float AngleKnee = AngleBetweenTwoVectors(LKnee - LHip, LKnee - LAnkle);
                Transform jointObj = RecursiveFindChild(gameObject.transform, jointsMap[jt.ToString()]);

                jointObj.localRotation = Quaternion.Euler(0f, 180 - AngleKnee, 0f);
            }
            else if (jt.ToString() == "SpineBase") {
                Transform jointObj = RecursiveFindChild(gameObject.transform, jointsMap[jt.ToString()]);
                Vector3 spine = GetVector3FromJoint(body.Joints[Kinect.JointType.SpineBase]);
                jointObj.position = new Vector3(spine.x, spine.y*2, jointObj.position.z);
            }


        }
            
    }
    private float roundout(float articulacion)
    {
        return Mathf.Round(articulacion * Mathf.Pow(10, 2)) / 100;
    }

    void LateUpdate() {
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            Debug.Log("_BodyManager is null");
            return;
        }
        Kinect.Body[] data = _BodyManager.GetData();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = gameObject;
                }

                RefreshBodyObject(body);
            }
        }
    }

    public static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        //return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
        return new Vector3(joint.Position.X * 2.8f, joint.Position.Y * 2.8f, joint.Position.Z * 2.8f);
    }

    public static Quaternion GetQuaternionJoint(Kinect.Body body, Kinect.JointType jointTd)
    {
        var orientation = body.JointOrientations[jointTd].Orientation;

        return new Quaternion(orientation.X, orientation.Y, orientation.Z, orientation.W);
    }

    public static float map(float x, float x1, float x2, float y1, float y2)
    {
        var m = (y2 - y1) / (x2 - x1);
        var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

        return m * x + c;
    }

    Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }

    public float AngleBetweenTwoVectors(Vector3 vectorA, Vector3 vectorB)
    {
        double dotProduct = 0.0;
        vectorA.Normalize();
        vectorB.Normalize();


        dotProduct = Vector3.Dot(vectorA, vectorB);

        return (float)Math.Acos(dotProduct) / (float)Math.PI * 180;
    }
}
