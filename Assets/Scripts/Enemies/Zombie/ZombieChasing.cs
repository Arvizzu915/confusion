using UnityEngine;

public class ZombieChasing : ZombieState
{
    public override void ZombieEnter(ZombieManager manager)
    {
        Debug.Log("chase");

        manager.agent.speed = manager.RunningSpeed;
        manager.agent.SetDestination(manager.playerTrans.position);
        manager.agent.isStopped = false;
    }

    public override void ZombieUpdate(ZombieManager manager)
    {
        if (!manager.playerInsideRoom)
        {
            manager.ChangeState(manager.stateIdle);
        }

        manager.agent.SetDestination(manager.playerTrans.position);
    }

    public override void ZombieExit(ZombieManager manager)
    {
        
    }

    public override void ZombieGetShot(ZombieManager manager, int damage)
    {
        
    }
}
