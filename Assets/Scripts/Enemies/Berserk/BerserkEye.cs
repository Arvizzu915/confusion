using UnityEngine;

public class BerserkEye : MonoBehaviour, IShootable
{
    [SerializeField] private Berserker berserkerScript;

    public void GetShot(int damage, ProjectileType type)
    {
        berserkerScript.GetShotOnEye(damage);
    }
}
