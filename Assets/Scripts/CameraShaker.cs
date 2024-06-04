using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] public float DefaultRotationStrength = 0.15f;
    [SerializeField] public float DefaultPositionStrength = 2f;
    [SerializeField] public float DefaultDuration = 0.5f;

    public void Shake(float duration, float positionOffsetStrength, float rotationOffsetStrength)
    {
        StopAllCoroutines();
        StartCoroutine(CameraShakeCoroutine(duration, positionOffsetStrength, rotationOffsetStrength));
    }

    private IEnumerator CameraShakeCoroutine(float duration, float positionOffsetStrength, float rotationOffsetStrength)
    {
        float elapsed = 0f;
        float currentMagnitude = 1f;

        // Capture the current position and rotation of the camera
        Vector3 initialPosition = transform.localPosition;
        Quaternion initialRotation = transform.localRotation;

        while (elapsed < duration)
        {
            float x = (Random.value - 0.5f) * currentMagnitude * positionOffsetStrength;
            float y = (Random.value - 0.5f) * currentMagnitude * positionOffsetStrength;

            float lerpAmount = currentMagnitude * rotationOffsetStrength;
            Vector3 lookAtVector = Vector3.Lerp(Vector3.forward, Random.insideUnitCircle, lerpAmount);

            transform.localPosition = initialPosition + new Vector3(x, y, 0);
            transform.localRotation = initialRotation * Quaternion.LookRotation(lookAtVector);

            elapsed += Time.deltaTime;
            currentMagnitude = (1 - (elapsed / duration)) * (1 - (elapsed / duration));

            yield return null;
        }

        // Reset the camera's position and rotation to its initial state before the shake started
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }
}
