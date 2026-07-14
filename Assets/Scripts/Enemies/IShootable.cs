using UnityEngine;

public enum ProjectileType
{
    Pierce,
    Fire
}

public interface IShootable
{
    public void GetShot(int damage, ProjectileType type);
}
