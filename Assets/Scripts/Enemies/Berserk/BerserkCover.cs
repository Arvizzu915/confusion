using UnityEngine;

public class BerserkCover : MonoBehaviour, IShootable
{
    [SerializeField] private Berserker berserkerScript;

    public void GetShot(int damage)
    {
        berserkerScript.GetShotOnCover();
    }
}
