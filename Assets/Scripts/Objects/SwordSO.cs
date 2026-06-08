using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Weapons/Swords")]
public class SwordSO : WeaponSO
{
    public override void Use(PlayerManager player, PlayerCombat combat)
    {
        player.playerAnim.Play("Attack");
    }
}
