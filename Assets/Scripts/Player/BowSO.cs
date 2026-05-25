using UnityEngine;

[CreateAssetMenu(fileName = "NormalBow", menuName = "Bows/NormalBow")]
public class NormalBowSO : BowSO
{
    public float holdingTimeLimit = 2f;
    public float shootForce = 5f;

    private bool pulling = false;

    public override void Use(PlayerManager player, PlayerCombat combat)
    {
        if (!combat.CanShootWithAim()) return;
        if (!combat.canShoot) return;
        if (pulling) return;

        pulling = true;
        combat.bowManager.PlayAnimClip("PullArrow");
    }

    public override void StopUsing(PlayerManager manager, PlayerCombat combat)
    {
        if (combat.canShoot && combat.CanShootWithAim() && pulling)
        {
            Shoot(combat.holdingTime, combat);
        }

        pulling = false;
        combat.holdingTime = 0f;
    }

    private void Shoot(float heldTime, PlayerCombat combat)
    {
        combat.canShoot = false;

        heldTime = Mathf.Clamp(heldTime, 0f, holdingTimeLimit);

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
        combat.StartCoroutine(combat.RechargeWeaponCoroutine());
    }

    public override void WeaponUpdate(PlayerCombat combat, PlayerManager manager)
    {
        if (!combat.holding) return;
        if (!combat.canShoot) return;
        if (!combat.CanShootWithAim()) return;

        Use(manager, combat);
        combat.holdingTime += Time.deltaTime;
    }

    public override void CancelAiming(PlayerManager manager, PlayerCombat combat)
    {
        base.CancelAiming(manager, combat);

        pulling = false;
    }
}