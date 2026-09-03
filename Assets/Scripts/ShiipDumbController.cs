using UnityEngine;
using UnityEngine.AI;

public class ShiipDumbController : ShiipController
{
    [Header("Leader")]
    public ShiipLeaderController leader;

    [Header("Herd Following")]
    public float followCheckInterval = 1f;
    public float returnDistance = 2f;

    private float followTimer;

    protected override void Start()
    {
        base.Start();

        // Automatically find a leader if one wasn't assigned.
        if (leader == null)
            leader = FindAnyObjectByType<ShiipLeaderController>();
    }

    protected override void Update()
    {
        if (leader == null)
        {
            base.Update();
            return;
        }

        followTimer += Time.deltaTime;

        // Periodically make sure we aren't outside the herd.
        if (followTimer >= followCheckInterval)
        {
            followTimer = 0f;

            if (IsOutsideHerd())
            {
                MoveBackTowardLeader();
                return;
            }
        }

        base.Update();
    }

    protected override void PickRandomPoint()
    {
        if (leader == null)
        {
            base.PickRandomPoint();
            return;
        }

        // Pick a random point inside the leader's herd radius.
        Vector3 randomDirection =
            Random.insideUnitSphere * leader.herdRadius;

        randomDirection.y = 0f;

        Vector3 target = leader.transform.position + randomDirection;

        // Find a valid NavMesh position.
        if (NavMesh.SamplePosition(
            target,
            out NavMeshHit hit,
            3f,
            NavMesh.AllAreas))
        {
            // Make sure the sampled point is actually
            // inside the leader's radius.
            if (Vector3.Distance(
                hit.position,
                leader.transform.position) <= leader.herdRadius)
            {
                agent.SetDestination(hit.position);
                SetWalking(true);
            }
        }
    }

    private bool IsOutsideHerd()
    {
        Vector3 offset = transform.position - leader.transform.position;
        offset.y = 0f;

        return offset.magnitude > leader.herdRadius;
    }

    private void MoveBackTowardLeader()
    {
        Vector3 direction =
            leader.transform.position - transform.position;

        direction.y = 0f;

        // Put the shiip somewhere safely inside the herd radius.
        Vector3 target =
            leader.transform.position -
            direction.normalized * returnDistance;

        if (NavMesh.SamplePosition(
            target,
            out NavMeshHit hit,
            5f,
            NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            SetWalking(true);
        }
    }
}
