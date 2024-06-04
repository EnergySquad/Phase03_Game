using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
    [SerializeField] private CameraShaker cameraShaker;
    private int collidedDrones = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Enemy"))
        {
            // Trigger the camera shake
            cameraShaker.Shake(cameraShaker.DefaultDuration, cameraShaker.DefaultPositionStrength, cameraShaker.DefaultRotationStrength);
            collidedDrones ++;
        }
    }

    void Update () {

        if (collidedDrones == 8) {
            PlayerPrefs.SetFloat("TotalScore",0f);
        }
    }
}
