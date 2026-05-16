using UnityEngine;

public abstract class BowSO : ScriptableObject
{
    [SerializeField] GameObject weaponGO;

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
