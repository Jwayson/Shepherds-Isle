using UnityEngine;
using UnityEngine.AI;

public class ShiipController : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator anim;
    public float radius = 10f;
    public float idleTime = 3f;
    private float idleTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        PickRandomPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.pathPending)
        {
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            idleTimer += Time.deltaTime;
            anim.SetBool("Walking", false);
            if (idleTimer >= idleTime)
            {
                idleTimer = 0f;
                PickRandomPoint();
            }
        }

        
    }

    //pickRandomPoint... picks a random point.
    void PickRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            anim.SetBool("Walking", true);
        }
    }
}