using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMenu : MonoBehaviour
{
    [SerializeField]
    CameraMultiTarget cameraMultiTarget;

    [SerializeField]
    GameObject[] targetObjects;

    void Start()
    {
        CameraRing.instance.InitCameraRing();
        cameraMultiTarget.SetTargets(targetObjects);
    }
}
