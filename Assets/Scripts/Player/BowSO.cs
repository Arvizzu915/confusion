using UnityEngine;

[CreateAssetMenu(fileName = "NomalBow", menuName = "Bows/NormalBow")]
public class NormalBowSO : BowSO
{
    public float holdingTimeLimit = 2f;
    public float shootForce = 5f;

    public override void Use(PlayerManager player, PlayerCombat combat)
    {
        if (!combat.aiming) return;

        combat.bowManager.PlayAnimClip("PullArrow");
    }

    public override void StopUsing(PlayerManager manager, PlayerCombat combat)
    {
        if (!combat.aiming) return;

        if (combat.holding)
        {
            combat.holding = false;
            if (combat.canShoot)
            {
                Shoot(combat.holdingTime, combat);
            }
            combat.holdingTime = 0;
        }
    }

    public void Shoot(float heldTime, PlayerCombat combat)
    {
        combat.canShoot = false;
        heldTime = Mathf.Clamp(heldTime, 0, holdingTimeLimit);

        BowManager bowMan = combat.bowManager;

        Quaternion rotation = Quaternion.LookRotation(combat.aimingDirection);

        rotation *= Quaternion.Euler(90f, 0f, 0f);

        GameObject arrow = Instantiate(
            bowMan.currentArrow,
            bowMan.arrowSpwnPos.position,
            rotation
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        rb.AddForce(
            heldTime * shootForce * combat.aimingDirection,
            ForceMode.Impulse
        );

        combat.bowManager.PlayAnimClip("BowRecharge");
    }

    public override void WeaponUpdate(PlayerCombat combat, PlayerManager manager)
    {
        if (combat.holding && combat.canShoot && combat.aiming)
        {
            combat.HoldBow();
            combat.holdingTime += Time.deltaTime;
        }
    }
}
