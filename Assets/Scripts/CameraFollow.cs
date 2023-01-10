using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //// Whom the camera will follow
    //public Transform target;
    //// The speed of the camera 
    //[SerializeField] public float smoothSpeed;
    ////The initial transform Z value of the Main Camera
    //private float cameraZOffset;

    //public float leftXLimit = 0.0f;
    //public float rightXLimit = 100.0f;

    //void Start()
    //{
    //    cameraZOffset = transform.position.z;
    //}

    //void Update()
    //{
        
    //}

    //private void LateUpdate()
    //{
    //    SmoothFollow();
    //}

    //public void SmoothFollow()
    //{
    //    float targetPositionX = target.position.x;
    //    Vector3 newCameraPosition = new Vector3(targetPositionX, transform.position.y, cameraZOffset);
    //    if (targetPositionX < leftXLimit)
    //    {
    //        newCameraPosition.x = leftXLimit;
    //    }
    //    if (targetPositionX > rightXLimit)
    //    {
    //        newCameraPosition.x = rightXLimit;
    //    }
    //    Vector3 smoothFollow = Vector3.Lerp(transform.position, newCameraPosition, smoothSpeed);
    //    transform.position = smoothFollow;
    //}
}
