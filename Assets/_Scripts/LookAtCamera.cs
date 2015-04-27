using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

    Transform myTransform;
    Transform cameraTransform;

    void Start ()
    {
        myTransform = transform;
        cameraTransform = Camera.main.transform;
    }

    void Update ()
    {
        //myTransform.localRotation = Quaternion.Lerp(myTransform.localRotation,cameraTransform.rotation, 10f * Time.deltaTime);
        myTransform.localRotation = cameraTransform.rotation;
    }
}

