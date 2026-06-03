using UnityEngine;
using UnityEngine.AI;

public class ZombieChasing : ZombieState
{
    private float repathTimer;

    private const float RepathRate = 0.1f;
    private const float SampleRadius = 3f;

    private Vector3 lastValidPlayerPosition;
    private bool hasValidPosition = false;

    public override void ZombieEnter(ZombieManager manager)
    {
        Debug.Log("chase");

        repathTimer = 0f;
        hasValidPosition = false;

        manager.agent.speed = manager.RunningSpeed;
        manager.agent.isStopped = false;
    }

    public override void ZombieUpdate(ZombieManager manager)
    {
        if (!manager.playerInsideRoom)
        {
            manager.ChangeState(manager.stateIdle);
        }

        repathTimer += Time.deltaTime;

        if (repathTimer < RepathRate)
            return;

        repathTimer = 0f;

        Vector3 playerPosition = manager.playerTrans.position;

        if (NavMesh.SamplePosition(playerPosition, out NavMeshHit navHit, SampleRadius, NavMesh.AllAreas))
        {
            lastValidPlayerPosition = navHit.position;
            hasValidPosition = true;
        }

        if (!hasValidPosition)
            return;

        if (!manager.agent.pathPending)
        {
            manager.agent.isStopped = false;
            manager.agent.SetDestination(lastValidPlayerPosition);
        }
    }

    public override void ZombieExit(ZombieManager manager)
    {
        manager.agent.ResetPath();
    }

    public override void ZombieGetShot(ZombieManager manager, int damage)
    {
        
    }
}
