using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float roatationSpeed = 500f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    float ySpeed;

    Quaternion targetRoation;
    bool isPressed = false;

    CameraController cameraController;
    Animator animator;

    CharacterController characterController;
    private void Awake () {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount;

        if (isPressed) {
            moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v)) ;
            moveSpeed  = 5;
        }
        else {

            moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v)) / 100 ;
            moveSpeed  = 2 ;
        }
        var moveInput = new Vector3(h,0,v).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();
        // Debug.Log("Is Grounded: " + isGrounded);

        if (isGrounded) {
            ySpeed = -0.5f;
        }
        else {
            ySpeed += Physics.gravity.y * Time.deltaTime;            
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0) {

            targetRoation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation =Quaternion.RotateTowards(transform.rotation, targetRoation, roatationSpeed * Time.deltaTime );
        
        animator.SetFloat("moveAmount", moveAmount, 0.1f, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift)) {isPressed = true;}
        if (Input.GetKeyUp(KeyCode.LeftShift)) {isPressed = false;}

        if (Input.GetKeyDown(KeyCode.E)) // Assuming space bar is used for punching
        {
            animator.SetTrigger("punch");

        if (Input.GetKeyDown(KeyCode.Space)) {animator.SetTrigger("typing");}
    }
        if (Input.GetKeyDown(KeyCode.Q)) // Assuming space bar is used for punching
        {
            animator.SetTrigger("Hook");
        }

    }


    void GroundCheck() 
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }
    

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1,0,0, 0.5f);
        Gizmos.DrawSphere(transform.TransformDirection(groundCheckOffset), groundCheckRadius);
    }
}
