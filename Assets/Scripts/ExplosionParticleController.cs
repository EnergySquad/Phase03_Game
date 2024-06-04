using System.Collections;
using UnityEngine;

public class ExplosionParticleController : MonoBehaviour
{
    // Public variables
    public ParticleSystem bigExplosionParticleSystem; // Reference to the big explosion particle system
    public ScoreCalculator scoreCalculator; // Reference to the ScoreCalculator script

    // Private variables
    private Coroutine reducingCoroutine; // Coroutine for reducing particles
    private ParticleSystem.MainModule mainModule; // Main module of the particle system
    private ParticleSystemRenderer particleRenderer; // Renderer for the particle system

    void Start()
    {
        try
        {
            // Initialize mainModule and particleRenderer with components from bigExplosionParticleSystem
            mainModule = bigExplosionParticleSystem.main;
            particleRenderer = bigExplosionParticleSystem.GetComponent<ParticleSystemRenderer>();

            // Set initial values for max particles and max particle size
            mainModule.maxParticles = 1000;
            particleRenderer.maxParticleSize = 1.0f;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in Start method: {ex.Message}");
        }
    }

    /// <summary>
    /// Starts the coroutine to reduce particles over time.
    /// </summary>
    public void StartReducingParticles()
    {
        if (reducingCoroutine == null)
        {
            reducingCoroutine = StartCoroutine(ReduceParticlesCoroutine());
        }
    }

    /// <summary>
    /// Stops the coroutine that reduces particles over time.
    /// </summary>
    public void StopReducingParticles()
    {
        if (reducingCoroutine != null)
        {
            StopCoroutine(reducingCoroutine);
            reducingCoroutine = null;
        }
    }

    /// <summary>
    /// Reduces the number of particles and their size over time.
    /// </summary>
    /// <returns>Returns true if the minimum values are reached, false otherwise.</returns>
    public bool ReduceParticlesOverTime()
    {
        try
        {
            // Log the current state of the particle system (for debugging)
            // Debug.Log($"Before Max Particles: {mainModule.maxParticles}, Before Max Particle Size: {particleRenderer.maxParticleSize}");

            // Reduce maxParticles and maxParticleSize
            mainModule.maxParticles = Mathf.Max(mainModule.maxParticles - 25, 5);
            particleRenderer.maxParticleSize = Mathf.Max(particleRenderer.maxParticleSize - 0.025f, 0.01f);

            // Update the decreasing constant in ScoreCalculator
            scoreCalculator.GetComponent<ScoreCalculator>().UpdateDecreasingConst(1.2f);

            // Log the new state of the particle system (for debugging)
            // Debug.Log($"After Max Particles: {mainModule.maxParticles}, After Max Particle Size: {particleRenderer.maxParticleSize}");

            // Check if minimum values are reached
            if (mainModule.maxParticles == 5 && particleRenderer.maxParticleSize == 0.01f)
            {
                return true; // Minimum values reached
            }

            return false; // Minimum values not reached
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error in ReduceParticlesOverTime: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Coroutine that continuously reduces particles over time.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator ReduceParticlesCoroutine()
    {
        while (true)
        {
            if (ReduceParticlesOverTime())
            {
                yield break; // Stop the coroutine when minimum values are reached
            }
            yield return new WaitForSeconds(1); // Wait for 1 second before the next reduction
        }
    }
}
