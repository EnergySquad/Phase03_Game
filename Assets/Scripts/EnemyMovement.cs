using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public float yPosition; // Y-axis position
    public float yRotation; // Y-axis rotation
    public float destroyThreshold = 0.1f; // Threshold distance to determine when to destroy the enemy
    public CameraShaker cameraShaker; // Reference to the CameraShaker script

    private bool hasShaken = false; // Flag to ensure camera shake happens only once
    private bool isPunched = false; // Flag to check if the enemy is punched
    private ParticleSystem explosionEffect; // Reference to the Particle System
    public ScoreCalculator ScoreCalculator;

    void Start()
    {
        // ScoreCalculator.SetDecreasingConst(descantConstant);

        // Set the Y position and rotation
        Vector3 newPosition = transform.position;
        newPosition.y = yPosition;
        transform.position = newPosition;

        Quaternion newRotation = transform.rotation;
        newRotation = Quaternion.Euler(newRotation.eulerAngles.x, yRotation, newRotation.eulerAngles.z);
        transform.rotation = newRotation;

        // Get the Particle System component
        explosionEffect = GetComponentInChildren<ParticleSystem>();
        if (explosionEffect != null)
        {
            Debug.Log("This is working");
            explosionEffect.Stop();
        }
        else
        {
            Debug.LogError("No ParticleSystem found on the enemy.");
        }
    }

    void Update()
    {
        if (!isPunched)
        {
            // Calculate direction towards (0, 0, 0) while keeping the Y position constant
            Vector3 targetPosition = new Vector3(0, transform.position.y, 0);
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Move towards the target position
            transform.position += direction * speed * Time.deltaTime;

            // Check if the enemy is close enough to the target position
            if (Vector3.Distance(transform.position, targetPosition) < destroyThreshold)
            {
                // Trigger camera shake if it hasn't been triggered yet
                if (!hasShaken && cameraShaker != null)
                {
                    cameraShaker.Shake(cameraShaker.DefaultDuration, cameraShaker.DefaultPositionStrength, cameraShaker.DefaultRotationStrength);
                    hasShaken = true; // Set the flag to true to prevent further shakes
                    Destroy(gameObject);
                    ScoreCalculator.GetComponent<ScoreCalculator>().UpdateDecreasingConst(0.7f);
                }
            }
        }

    }

    public void OnPunched(Vector3 forceDirection, float force)
    {
        isPunched = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(forceDirection * force, ForceMode.Impulse);
        }

        // Play the particle system
        if (explosionEffect != null)
        {
            explosionEffect.Play();
        }

        // Destroy the enemy after a short delay to allow the particle effect to be visible
        Destroy(gameObject, 5.1f); // Adjust the delay as needed
    }
}
