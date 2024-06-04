
using System.Collections;
using UnityEngine;

public class FireExtinguisherBoost : MonoBehaviour
{
    private Coroutine scoringCoroutine;
    public GameObject gameOver;
    private ParticleSystem extinguisherSmoke;
    public ExplosionParticleController explosionController;

    void Start()
    {
        // Find the child particle system
        extinguisherSmoke = GetComponentInChildren<ParticleSystem>();
        if (extinguisherSmoke != null)
        {
            extinguisherSmoke.Stop(); // Ensure the particle system is initially stopped
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (scoringCoroutine == null)
            {
                scoringCoroutine = StartCoroutine(IncrementScoreOverTime());
            }

            if (extinguisherSmoke != null)
            {
                extinguisherSmoke.Play(); // Start the particle system
            }

            if (explosionController != null)
            {
                explosionController.StartReducingParticles();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (scoringCoroutine != null)
            {
                StopCoroutine(scoringCoroutine);
                scoringCoroutine = null;
            }

            if (extinguisherSmoke != null)
            {
                extinguisherSmoke.Stop(); // Stop the particle system
            }

            if (explosionController != null)
            {
                explosionController.StopReducingParticles();
            }
        }
    }

    private IEnumerator IncrementScoreOverTime()
    {
        while (true)
        {
            if (explosionController != null)
            {
                bool reachedMinValues = explosionController.ReduceParticlesOverTime();
                if (reachedMinValues)
                {   
                    if (PlayerPrefs.GetString("MissionCompleted") == "Level2")
                        {
                            //update the is_task_1_completed to true
                            gameOver.SetActive(true);
                            // Pause the game
                            Time.timeScale = 0;
                            // Wait for the specified amount of real time
                            yield return new WaitForSecondsRealtime(2);
                            // Resume the game
                            Time.timeScale = 1;
                            gameOver.SetActive(false);
                            // Set the mission completed to Level1 and active the idle state until the player reach to next task
                            PlayerPrefs.SetString("MissionCompleted", "Level3");
                            PlayerPrefs.SetString("IdeleState", "true");
                        }

                    Destroy(gameObject);
                    yield break;
                }
            }

            yield return new WaitForSeconds(1);
        }
    }
}