using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

// Head of the enemyAI
public class AiAgent : MonoBehaviour
{
    [HideInInspector] public AiStateMachine stateMachine;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Rigidbody2D playerRigidbody;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public AISensor sensor;

    [HideInInspector] public Seeker agentSeeker;
    [HideInInspector] public Path path;
    [HideInInspector] public bool reachedEndOfPath;
    [HideInInspector] public int currentWaypoint = 0;
    [HideInInspector] public int currentPointIndex = 1;
    [HideInInspector] public bool once;
    [HideInInspector] public bool walk;
    [HideInInspector] public Vector2 targetPoint;
    [HideInInspector] public Vector2 patrolCenter;
    public AiStateId initialState;

    [Space(15f)]
    public LayerMask groundLayers;
    public AiAgentConfig config;

    [HideInInspector] public float initialX;

    protected virtual void Start()
    {
        initialX = transform.position.x;
        patrolCenter = transform.position;
        patrolCenter += new Vector2(0, 40);
        sensor = GetComponent<AISensor>();
        animator = GetComponentInChildren<Animator>();
        agentSeeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = playerTransform.gameObject.GetComponent<PlayerHealth>();
        playerRigidbody = playerTransform.gameObject.GetComponent<Rigidbody2D>();

        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AIChaseState());
        stateMachine.RegisterState(new AiPatrolState());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.RegisterState(new AiAFK());
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void UpdatePath()
    {
        if (agentSeeker.IsDone())
        {
            agentSeeker.StartPath(rb.position, playerTransform.position, OnPathComplete);
        }
    }

    public void UpdatePathToPoint()
    {
        if (agentSeeker.IsDone())
        {
            agentSeeker.StartPath(rb.position, targetPoint, OnPathComplete);
        }
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
