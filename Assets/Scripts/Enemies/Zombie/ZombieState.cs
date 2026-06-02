using UnityEngine;

public abstract class ZombieState
{
    public abstract void ZombieEnter(ZombieManager manager);

    public abstract void ZombieUpdate(ZombieManager manager);

    public abstract void ZombieExit(ZombieManager manager);

    public abstract void ZombieGetShot(ZombieManager manager, int damage);
}
