using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IShootable
{
    public bool playerInsideRoom;

    public abstract void GetShot(int damage);

    public virtual void SetEnemyInsideRoom(bool value)
    {
        playerInsideRoom = value;
    }

}
