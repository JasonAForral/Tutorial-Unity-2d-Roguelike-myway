using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

    Transform cameraTransform;

    void Start ()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update ()
    {
        //myTransform.localRotation = Quaternion.Lerp(myTransform.localRotation,cameraTransform.rotation, 10f * Time.deltaTime);
        transform.rotation = cameraTransform.rotation;
    }
}

