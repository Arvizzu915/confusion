using UnityEngine;

public abstract class WeaponSO : ScriptableObject
{
    [SerializeField] GameObject weaponGO;

    public void GetEquipped(PlayerManager player)
    {
        player.playerCombat.EquipWeapon(weaponGO, this);
    }

    public abstract void Use(PlayerManager player, PlayerCombat combat);
}
