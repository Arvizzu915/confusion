using UnityEngine;

public class BerserkCover : MonoBehaviour, IShootable
{
    [SerializeField] private Berserker berserkerScript;

    public void GetShot(int damage, ProjectileType type)
    {
        berserkerScript.GetShotOnCover();
    }
}
