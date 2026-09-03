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

    protected float idleTimer;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Make Shiip naturally avoid each other.
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(30, 70);

        PickRandomPoint();
    }

    protected virtual void Update()
    {
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

    protected virtual void PickRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
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
