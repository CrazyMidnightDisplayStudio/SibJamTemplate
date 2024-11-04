using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections;

public class Patrol : MonoBehaviour
{
    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] string highPriorityTag = "Human";

    [SerializeField] float nextPatrolPointDistance = 0.05f;

    [SerializeField] Transform[] potentialPatrolPoints;

    [SerializeField] float secToLostHuman = 5f;

    private AIDestinationSetter destinationSetter;
    private EnemyVision enemyVision;
    private int currentPointIndex = 0;
    bool isPatrol = true;

    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        enemyVision = GetComponent<EnemyVision>();
        destinationSetter.target = null;

        if (patrolPoints.Count > 0)
        {
            destinationSetter.target = patrolPoints[currentPointIndex];
        }
    }

    void Update()
    {
        if (enemyVision != null && enemyVision.TargetDetected() && enemyVision.GetDetectedTarget().gameObject.CompareTag(highPriorityTag))
        {
            if (isPatrol)
                StartCoroutine(ForgetAboutHuman());

            isPatrol = false;
            destinationSetter.target = enemyVision.GetDetectedTarget();
        }
        else if (destinationSetter.target != null)
        {
            isPatrol = true;
            float distanceToTarget = Vector3.Distance(transform.position, destinationSetter.target.position);
            if (distanceToTarget < nextPatrolPointDistance)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
                destinationSetter.target = patrolPoints[currentPointIndex];
            }
        }
    }

    public void addAllPoints()
    {
        patrolPoints.AddRange(potentialPatrolPoints);
    }

    public void addPatrolPoint(Transform newPp)
    {
        patrolPoints.Add(newPp);
    }

    public void removePatrolPoint(Transform newPp)
    {
        if (patrolPoints.Contains(newPp))
            patrolPoints.Remove(newPp);
    }

    IEnumerator ForgetAboutHuman()
    {
        yield return new WaitForSeconds(secToLostHuman);
        destinationSetter.target = patrolPoints[currentPointIndex];
        enemyVision.LostHuman();
        isPatrol = true;
    }
}