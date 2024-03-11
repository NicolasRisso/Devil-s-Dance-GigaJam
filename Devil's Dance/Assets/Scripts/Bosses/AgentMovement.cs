using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float huntSpeed;
    [Header("Detection Configuration")]
    [SerializeField] private float sightRange;
    [SerializeField] private float escapeFromSightRange;
    [SerializeField] private float touchDetection;
    [SerializeField] private float sightFOV;
    [SerializeField] private float patrolMaxDistance;
    [SerializeField] private float godsVoiceDistance;
    [SerializeField] [Range(0f, 2f)] private float randomPointAccuracyTolerance;
    [Header("Layer Masks and Tags")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private string hiddenTag;
    [SerializeField] private string hideSpotTag;

    private Transform player;
    private NavMeshAgent navMeshAgent;

    private bool playerInSightRange = false;
    private bool walkPointSet = false;

    private Vector3 walkPoint;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = patrolSpeed;
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        DetectPlayer();
        Debug.Log(playerInSightRange);
        if (!playerInSightRange) Patrolling();
        else ChasePlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        navMeshAgent.destination = walkPoint;

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < randomPointAccuracyTolerance) walkPointSet = false;
    }

    private void ChasePlayer()
    {
        navMeshAgent.destination = player.position;
    }

    private void SearchWalkPoint()
    {
        bool canReach = false;
        Vector3 randomPoint = Vector3.zero;
        if ((transform.position - player.position).magnitude >= godsVoiceDistance)
        {
            while (!canReach)
            {
                randomPoint = new Vector3(player.position.x + Random.Range(-patrolMaxDistance, patrolMaxDistance),
                    player.position.y, player.position.z + Random.Range(-patrolMaxDistance, patrolMaxDistance));
                canReach = IsPointReachable(randomPoint);
            }
        }
        else
        {
            while (!canReach)
            {
                randomPoint = new Vector3(transform.position.x + Random.Range(-patrolMaxDistance, patrolMaxDistance),
                    transform.position.y, transform.position.z + Random.Range(-patrolMaxDistance, patrolMaxDistance));
                canReach = IsPointReachable(randomPoint);
            }
        }
        walkPoint = randomPoint;
        walkPointSet = true;
    }

    private void AdjustSpeed()
    {
        if (playerInSightRange) navMeshAgent.speed = huntSpeed;
        else navMeshAgent.speed = patrolSpeed;
    }

    private void DetectPlayer()
    {
        if (IsPlayerInFieldOfView()) playerInSightRange = true;
        else if (Physics.CheckSphere(transform.position, touchDetection, playerLayerMask)) playerInSightRange = true;
        else playerInSightRange = false;
        AdjustSpeed();
    }

    private bool IsPointReachable(Vector3 point)
    {
        if (NavMesh.SamplePosition(point, out NavMeshHit _, randomPointAccuracyTolerance, NavMesh.AllAreas)) return true;
        return false;
    }

    private bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        if (!playerInSightRange)
        {
            if (directionToPlayer.magnitude > sightRange) return false;
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, directionToPlayer.magnitude, obstaclesLayerMask)) return false;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);
            return angleToPlayer <= sightFOV;
        }
        else
        {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, directionToPlayer.magnitude, obstaclesLayerMask)) return false;
            if (directionToPlayer.magnitude > escapeFromSightRange) return false;
            return true;
        }
    }
}
