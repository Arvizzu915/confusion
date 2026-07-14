using UnityEngine;

public class BerserkBody : MonoBehaviour, IShootable
{
    [SerializeField] private Berserker berserkerScript;

    public void GetShot(int damage, ProjectileType type)
    {
        berserkerScript.GetShotOnBody();
    }
}
