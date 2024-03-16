using System.Collections;
using System.Collections.Generic;
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
    [Header("Persistance Configuration")]
    [SerializeField] private float timeToGiveUpSeeking;
    [SerializeField] private float timeToGiveUpSeekingAfterBeingTrapped;
    [SerializeField] private float maxDistanceToVerifyHideSpot;
    [Header("Layer Masks and Tags")]
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private LayerMask hideSpotMask;
    [SerializeField] private string hiddenTag;

    public enum State
    {
        patroling,
        chasing,
        seeking
    }

    private List<Transform> hideSpots = new List<Transform>();

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private TrapActivated trapActivated;

    private bool walkPointSet = false;

    private int hideSpotIndex = 0;
    private int maxHideSpotIndex = 0;

    private Vector3 walkPoint;

    private State state;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = patrolSpeed;
        trapActivated = GetComponent<TrapActivated>();
        player = GameObject.Find("Player").transform;
        FindAllHideSpots();
    }

    private void Update()
    {
        DetectPlayer();
        ChangeState();
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

    private void SeekPlayer()
    {
        if (!walkPointSet) SearchHideSpots();
        navMeshAgent.destination = walkPoint;

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < (randomPointAccuracyTolerance / 2f)) walkPointSet = false;
    }

    private void ChangeState()
    {
        switch (state)
        {
            case State.seeking:
                SeekPlayer();
                break;
            case State.chasing:
                ChasePlayer();
                break;
            default:
                Patrolling();
                break;
        }
    }

    private void SearchHideSpots()
    {
        if (hideSpotIndex < maxHideSpotIndex)
        {
            walkPoint = hideSpots[hideSpotIndex].position;
            walkPointSet = true;
            hideSpotIndex++;
        }
        else SearchWalkPoint();
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
        if (trapActivated.GetIsTrapped())
        {
            navMeshAgent.speed = 0f;
            return;
        }
        if (state != State.patroling) navMeshAgent.speed = huntSpeed;
        else navMeshAgent.speed = patrolSpeed;
    }

    private void DetectPlayer()
    {
        if (IsPlayerInFieldOfView() || Physics.CheckSphere(transform.position, touchDetection, playerLayerMask)) state = State.chasing;
        else if (state == State.chasing)
        { 
            state = State.seeking;
            hideSpotIndex = 0;
            walkPoint = player.transform.position;
            walkPointSet = true;
            OrderHideSpots();
            StartCoroutine(GiveUpSeeking());
        }
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
        if (state != State.chasing)
        {
            if (directionToPlayer.magnitude > sightRange) return false;
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, directionToPlayer.magnitude, obstaclesLayerMask)) return false;
            if (player.CompareTag(hiddenTag) && !transform.CompareTag(hiddenTag)) return false;
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

    private void FindAllHideSpots()
    {
        hideSpots.Clear();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) if (((1 << obj.layer) & hideSpotMask) != 0) hideSpots.Add(obj.transform);
    }

    private void OrderHideSpots()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is not set.");
            return;
        }
        hideSpots.Sort((a, b) =>
            Vector3.Distance(a.position, player.position).CompareTo(Vector3.Distance(b.position, player.position))
        );
        maxHideSpotIndex = 0;
        foreach (Transform spot in hideSpots)
        {
            if (Vector3.Distance(spot.position, player.position) <= maxDistanceToVerifyHideSpot) maxHideSpotIndex++;
        }
        //foreach (Transform hideSpot in hideSpots)
        //{
        //    Debug.Log("HideSpot: " + hideSpot.name + ", Distance: " + Vector3.Distance(hideSpot.position, player.position));
        //}
    }

    private IEnumerator GiveUpSeeking()
    {
        if (!trapActivated.GetIsTrapped()) yield return new WaitForSeconds(timeToGiveUpSeeking);
        else yield return new WaitForSeconds(timeToGiveUpSeekingAfterBeingTrapped);
        if (state != State.chasing) state = State.patroling;
    } 

    public State GetState()
    {
        return state;
    }
}
