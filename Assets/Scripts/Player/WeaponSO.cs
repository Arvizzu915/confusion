using UnityEngine;

public abstract class WeaponSO : ScriptableObject
{
    [SerializeField] GameObject weaponGO;

    public int damage = 1;

    public void GetEquipped(PlayerManager player)
    {
        player.playerCombat.EquipWeapon(weaponGO, this);
    }

    public virtual void WeaponUpdate(PlayerCombat player, PlayerManager manager)
    {

    }

    public abstract void Use(PlayerManager player, PlayerCombat combat);

    public virtual void StopUsing(PlayerManager manager, PlayerCombat combat) 
    {
        
    }
}
