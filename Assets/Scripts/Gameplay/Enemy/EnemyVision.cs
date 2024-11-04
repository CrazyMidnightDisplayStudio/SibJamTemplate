using Pathfinding;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] float viewDistance = 4f;
    [SerializeField] float viewAngle = 45f;
    [SerializeField] string[] targetTags;

    [SerializeField] LayerMask rayCastMask;

    Collider2D detectedTarget;

    private Vector3 lastDirection;

    void Update()
    {
        DetectTargets();
    }

    void DetectTargets()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewDistance);

        foreach (Collider2D target in targetsInViewRadius)
        {
            if (IsTargetInSight(target))
                CheckTarget(target);
        }
    }

    void CheckTarget(Collider2D target)
    {
        foreach (string tag in targetTags)
        {
            if (target.CompareTag(tag))
            {
                detectedTarget = target;
                break;
            }
        }
    }

    bool IsTargetInSight(Collider2D target)
    {
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

        lastDirection = directionToTarget;

        if (Vector3.Angle(transform.up, directionToTarget) < viewAngle / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, rayCastMask);

            if (hit.collider != null && hit.collider.CompareTag(target.tag))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, viewAngle / 2) * transform.up * viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.up * viewDistance;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        Gizmos.color = Color.green; // Цвет для луча
        Gizmos.DrawLine(transform.position, transform.position + lastDirection * viewDistance);
    }

    public bool TargetDetected()
    {
        return detectedTarget != null; // Возвращает true, если цель обнаружена
    }

    public void LostHuman()
    {
        detectedTarget = null;
    }

    public Transform GetDetectedTarget()
    {
        return detectedTarget.transform; // Возвращает обнаруженную цель
    }
}
