using UnityEngine;

public class BowSO : WeaponSO
{
    public float holdingTime = 0, holdingTimeLimit = 2f;
    public bool holding = false;

    public override void Use(PlayerManager player, PlayerCombat combat)
    {
        holding = true;
    }

    public override void StopUsing(PlayerManager manager, PlayerCombat combat)
    {
        if (holding)
        {

        }
    }

    public override void WeaponUpdate(PlayerCombat player, PlayerManager manager)
    {
        if (holding)
        {
            holdingTime += Time.deltaTime;
        }
    }
}
