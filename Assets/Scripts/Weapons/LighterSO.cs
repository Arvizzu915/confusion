using UnityEngine;

[CreateAssetMenu(fileName = "Lighter", menuName = "Weapons/Lighter")]
public class LighterSO : WeaponSO
{
    public override void Use(PlayerManager player, PlayerCombat combat)
    {
        
    }

    public override void Aim(PlayerManager manager, PlayerCombat combat)
    {
    }

    public override void CancelAiming(PlayerManager manager, PlayerCombat combat)
    {
    }
}
