using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToFollow : MonoBehaviour
{
    // Whom the camera will follow
    public Transform target;
    // The speed of the camera 
    [SerializeField] public float smoothSpeed;
    //The initial transform Z value of the Main Camera
    private float ZOffset;

    void Start()
    {
        ZOffset = transform.position.z;
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        SmoothFollow();
    }

    public void SmoothFollow()
    {
        float targetPositionX = target.position.x;
        Vector3 newCameraPosition = new Vector3(targetPositionX, transform.position.y, ZOffset);
        Vector3 smoothFollow = Vector3.Lerp(transform.position, newCameraPosition, smoothSpeed);
        transform.position = smoothFollow;
    }
}
