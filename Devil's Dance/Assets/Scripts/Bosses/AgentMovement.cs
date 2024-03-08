using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMovement : MonoBehaviour
{
    [Header("Detection Configuration")]
    [SerializeField] private float sightRange;
    [SerializeField] private float patrolMaxDistance;
    [SerializeField] private float godsVoiceDistance;
    [SerializeField] [Range(0f, 2f)] private float randomPointAccuracyTolerance;
    [SerializeField] private LayerMask playerLayerMask;

    private Transform player;
    private NavMeshAgent navMeshAgent;

    private bool playerInSightRange = false;
    private bool walkPointSet = false;

    private Vector3 walkPoint;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayerMask);
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

    private bool IsPointReachable(Vector3 point)
    {
        if (NavMesh.SamplePosition(point, out NavMeshHit _, randomPointAccuracyTolerance, NavMesh.AllAreas)) return true;
        return false;
    }
}
