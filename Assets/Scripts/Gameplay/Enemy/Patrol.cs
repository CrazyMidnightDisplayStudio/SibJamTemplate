using UnityEngine;
using Pathfinding;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] string highPriorityTag = "Human";

    [SerializeField] float nextPatrolPointDistance = 0.05f;

    private AIDestinationSetter destinationSetter;
    private EnemyVision enemyVision;
    private int currentPointIndex = 0;

    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        enemyVision = GetComponent<EnemyVision>();
        destinationSetter.target = null;

        if (patrolPoints.Length > 0)
        {
            destinationSetter.target = patrolPoints[currentPointIndex];
        }
    }

    void Update()
    {
        if (enemyVision != null && enemyVision.TargetDetected() && enemyVision.GetDetectedTarget().gameObject.CompareTag(highPriorityTag))
        {
            if (destinationSetter.target && destinationSetter.target.gameObject.CompareTag(highPriorityTag))
                return;

            destinationSetter.target = enemyVision.GetDetectedTarget();
        }
        else if (destinationSetter.target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, destinationSetter.target.position);
            if (distanceToTarget < nextPatrolPointDistance)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                destinationSetter.target = patrolPoints[currentPointIndex];
            }
        }
    }
}