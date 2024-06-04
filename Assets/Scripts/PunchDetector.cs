using System.Collections.Generic;
using UnityEngine;

public class PunchDetector : MonoBehaviour
{
    public float punchForce = 10f; // Force applied to enemies
    public ScoreCalculator ScoreCalculator;
    private HashSet<GameObject> punchedEnemies;

    void Start()
    {
        punchedEnemies = new HashSet<GameObject>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                // Calculate direction away from player
                Vector3 forceDirection = (other.transform.position - transform.position).normalized;

                // Get the EnemyMovement script and call OnPunched
                EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.OnPunched(forceDirection, punchForce);

                    // Check if the enemy has already been punched
                    if (!punchedEnemies.Contains(other.gameObject))
                    {
                        ScoreCalculator.GetComponent<ScoreCalculator>().UpdateDecreasingConst(1.2f);
                        punchedEnemies.Add(other.gameObject);
                    }
                }
            }
        }
    }
}
