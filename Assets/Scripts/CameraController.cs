using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;

    [SerializeField] float distance = 5;

    [SerializeField] float rotationSpeed = 2;
    
    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;

    [SerializeField] Vector2 framingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;


    float rotationY;
    float rotationX;
    float invertXVal;
    float invertYVal;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() {

        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        rotationY += Input.GetAxis("Mouse X") * rotationSpeed * invertXVal;
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed * invertYVal;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle );

        var targetRotation = Quaternion.Euler(rotationX,rotationY,0);

        var focusPosition = followTarget.position + new Vector3(framingOffset.x,framingOffset.y);

        // Quaternion.Euler(0,45,0) //Create a horizontal rotation of 45 degree
        transform.position = focusPosition - targetRotation * new Vector3(0,0,distance);
        transform.rotation = targetRotation;


    }

    public Quaternion PlanarRotation => Quaternion.Euler(0,rotationY,0);

}
