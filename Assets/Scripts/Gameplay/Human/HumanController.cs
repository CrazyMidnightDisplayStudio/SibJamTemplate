using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class HumanController : MonoBehaviour
{
    [Header("AI Behaviour")]
    [SerializeField] Transform target = null;
    [SerializeField] float speed = 200f;
    [SerializeField] float nextWaypointDistance = 1f;

    Path path;
    int currentWaypoint = 0;
    Seeker seeker;
    Rigidbody2D rb;

    [Header("States")]
    [SerializeField, Range(1f, 2f)]
    private float stressLevel = 1f;

    bool isInSafePlace = true;

    public enum MovementStyle
    {
        Stealth,
        Walk,
        Run
    }

    [SerializeField]
    private MovementStyle currentMovementStyle = MovementStyle.Walk;

    private float GetMovementSpeed()
    {
        switch (currentMovementStyle)
        {
            case MovementStyle.Stealth:
                return 0.5f;
            case MovementStyle.Walk:
                return 1f;
            case MovementStyle.Run:
                return 2f;
            default:
                return 1f;
        }
    }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if ( !rb || !seeker)
        {
            Debug.LogError("Some component is missing on Human GameObject.");
        }

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (!target)
            return;

        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void FixedUpdate()
    {
        MoveNPC();
    }

    private void MoveNPC()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * (GetMovementSpeed() * stressLevel * speed * Time.deltaTime);

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (target.CompareTag("ClickPoint"))
        {
            Destroy(target.gameObject);
            target = null;
        }

        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void Scare(float terrorAmount)
    {
        stressLevel += terrorAmount;
    }

    public MovementStyle CurrentMovementStyle
    {
        get { return currentMovementStyle; }
        set { currentMovementStyle = value; }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ToggleSafeState() => isInSafePlace = !isInSafePlace;
}