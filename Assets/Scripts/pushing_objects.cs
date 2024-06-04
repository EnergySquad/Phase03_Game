using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class moveableObject : MonoBehaviour
{
    public float pushForce = 0.003f;
    public GameObject Box1;
    public GameObject Box2;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

 
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject == Box1 || hit.gameObject == Box2)
        {
            Rigidbody _rigg = hit.collider.attachedRigidbody;

            if (_rigg != null)
            {
                Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                // Trigger the pushing animation
                animator.SetTrigger("pushing");
                Debug.Log("Pushing");

                _rigg.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);
            }
        }
    }
 
 
}