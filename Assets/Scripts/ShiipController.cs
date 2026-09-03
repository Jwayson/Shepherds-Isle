using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ShiipController : MonoBehaviour
{
    protected NavMeshAgent agent;

    public Animator anim;

    [Header("Wandering")]
    public float radius = 10f;
    public float idleTime = 3f;

    [Header("Flee")]
    public float fleeRadius = 5f;
    public float fleeDistance = 8f;
    public float fleeSpeed = 5f;

    protected float idleTimer;
    protected bool fleeing;

    private Transform player;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.obstacleAvoidanceType =
            ObstacleAvoidanceType.MedQualityObstacleAvoidance;

        agent.avoidancePriority = Random.Range(30, 70);

        PickRandomPoint();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    protected virtual void Update()
    {
        CheckForPlayer();

        if (fleeing)
            return;

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            idleTimer += Time.deltaTime;

            SetWalking(false);

            if (idleTimer >= idleTime)
            {
                idleTimer = 0f;
                PickRandomPoint();
            }
        }
        else
        {
            SetWalking(true);
        }
    }

    protected virtual void CheckForPlayer()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= fleeRadius)
        {
            FleeFromPlayer(player.transform.position);
        }
        else if (fleeing)
        {
            // Player is far enough away, resume normal behavior.
            fleeing = false;
            agent.speed = 2f;
            idleTimer = 0f;
            PickRandomPoint();
        }
    }

    protected virtual void FleeFromPlayer(Vector3 playerPosition)
    {
        fleeing = true;

        agent.speed = fleeSpeed;
        SetWalking(true);

        Vector3 awayFromPlayer =
            transform.position - playerPosition;

        awayFromPlayer.y = 0f;

        if (awayFromPlayer.sqrMagnitude < 0.01f)
            awayFromPlayer = transform.forward;

        awayFromPlayer.Normalize();

        Vector3 fleeTarget =
            transform.position + awayFromPlayer * fleeDistance;

        if (NavMesh.SamplePosition(
            fleeTarget,
            out NavMeshHit hit,
            fleeDistance,
            NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    protected virtual void PickRandomPoint()
    {
        Vector3 randomDirection =
            Random.insideUnitSphere * radius;

        randomDirection.y = 0f;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(
            randomDirection,
            out NavMeshHit hit,
            radius,
            NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            SetWalking(true);
        }
    }

    protected void SetWalking(bool walking)
    {
        if (anim != null)
            anim.SetBool("walking", walking);
    }
}
