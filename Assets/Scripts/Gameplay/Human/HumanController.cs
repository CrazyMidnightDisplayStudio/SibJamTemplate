using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] int relaxFrequency = 2;
    private float timer;

    [SerializeField, Range(1f, 2f)]
    private float stressLevel = 1f;

    bool isInSafePlace = true;

    [SerializeField] Volume volume;
    private ChannelMixer channelMixer;

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

        volume.profile.TryGet<ChannelMixer>(out channelMixer);

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

        if (isInSafePlace)
        {
            timer += Time.deltaTime;
            if (timer >= 1f / relaxFrequency)
            {
                Relax(0.01f);
                timer = 0f;
            }

        }
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
        if (stressLevel > 2)
        {
            stressLevel = 2;
        }
        RedScreen(200f, 20f);
    }

    public void Relax(float relaxAmount)
    {
        stressLevel -= relaxAmount;
        if (stressLevel < 1)
        {
            stressLevel = 1;
        }
        RedScreen(100f, 1f);
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

    void RedScreen(float forRed, float forOther)
    {
        channelMixer.redOutRedIn.value = forRed * stressLevel;
        channelMixer.redOutGreenIn.value  = forOther * stressLevel;
        channelMixer.redOutBlueIn.value  = forOther * stressLevel;
    }
}