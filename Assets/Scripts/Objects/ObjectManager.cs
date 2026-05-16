using UnityEngine;

public class ObjectManager : Interactuable
{
    [SerializeField] BowSO weaponSO;

    public override void Interact(PlayerManager player)
    {
        weaponSO.GetEquipped(player);
        gameObject.SetActive(false);
    }
}
