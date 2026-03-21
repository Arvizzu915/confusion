using UnityEngine;

public class ObjectManager : Interactuable
{
    [SerializeField] WeaponSO weaponSO;

    public override void Interact(PlayerManager player)
    {
        weaponSO.GetEquipped(player);
        gameObject.SetActive(false);
    }
}
