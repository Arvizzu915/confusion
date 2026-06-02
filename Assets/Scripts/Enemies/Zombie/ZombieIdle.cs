using UnityEngine;

public class ZombieIdle : ZombieState
{
    public override void ZombieEnter(ZombieManager manager)
    {
        manager.agent.isStopped = true;
    }

    public override void ZombieUpdate(ZombieManager manager)
    {
        Vector3 toPlayer = manager.playerTrans.position - manager.transform.position;

        if (toPlayer.sqrMagnitude <= manager.detectionRangeSqr && manager.playerInsideRoom)
        {
            manager.ChangeState(manager.stateChasing);
        }
    }

    public override void ZombieExit(ZombieManager manager)
    {

    }

    public override void ZombieGetShot(ZombieManager manager, int damage)
    {
        if (manager.playerInsideRoom)
        {
            manager.ChangeState(manager.stateChasing);
        }
    }
}
