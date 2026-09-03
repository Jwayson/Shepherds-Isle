using UnityEngine;

public class ShiipLeaderController : ShiipController
{
    [Header("Herd")]
    public float herdRadius = 15f;

    protected override void Start()
    {
        base.Start();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, herdRadius);
    }
}