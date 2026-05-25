using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public PlayerManager playerManager;
    public InputManager inputManager;

    [Header("Weapon")]
    public BowSO currentWeaponSO = null;
    public BowManager bowManager;
    public GameObject weapon;

    [Header("Bow")]
    public float holdingTime = 0;
    public bool holding = false;
    public bool aiming = false;
    public bool canShootAfterAim = false;
    public float aimCoyoteTime = 0.2f;
    public float rechargeTime = .8f;

    [Header("Aim")]
    public Vector3 aimingDirection;
    public bool canShoot = true;
    [SerializeField] private LayerMask detectLayer;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float aimDistance = 100f;
    [SerializeField] private GameObject aimingCamera;

    private Coroutine aimCoyoteCoroutine;

    private void Start()
    {
        inputManager.inputs.Playing.Attack1.performed += TryHoldBow;
        inputManager.inputs.Playing.Attack1.canceled += StopHoldingBow;
        inputManager.inputs.Playing.Aim.performed += Aim;
        inputManager.inputs.Playing.Aim.canceled += Aim;
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.Attack1.performed -= TryHoldBow;
        inputManager.inputs.Playing.Attack1.canceled -= StopHoldingBow;
        inputManager.inputs.Playing.Aim.performed -= Aim;
        inputManager.inputs.Playing.Aim.canceled -= Aim;
    }

    private void Update()
    {
        currentWeaponSO.WeaponUpdate(this, playerManager);
        ShootAimRay();
    }

    private void ShootAimRay()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, aimDistance, detectLayer))
            targetPoint = hit.point;
        else
            targetPoint = ray.origin + ray.direction * aimDistance;

        aimingDirection = (targetPoint - bowManager.arrowSpwnPos.position).normalized;
    }

    public bool CanShootWithAim()
    {
        return aiming || canShootAfterAim;
    }

    public void TryHoldBow(InputAction.CallbackContext ctx)
    {
        holding = true;
    }

    public void StopHoldingBow(InputAction.CallbackContext ctx)
    {
        if (currentWeaponSO == null) return;

        holding = false;
        currentWeaponSO.StopUsing(playerManager, this);
    }

    public void Aim(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PlayerMovement.instance.StopRunning();

            if (aimCoyoteCoroutine != null)
                StopCoroutine(aimCoyoteCoroutine);

            aiming = true;
            canShootAfterAim = true;

            aimingCamera.SetActive(true);
            currentWeaponSO.Aim(playerManager, this);
        }
        else if (ctx.canceled)
        {
            aiming = false;
            aimingCamera.SetActive(false);

            if (aimCoyoteCoroutine != null)
                StopCoroutine(aimCoyoteCoroutine);

            aimCoyoteCoroutine = StartCoroutine(ShootCoyoteTime());
        }
    }

    private IEnumerator ShootCoyoteTime()
    {
        canShootAfterAim = true;

        yield return new WaitForSeconds(aimCoyoteTime);

        canShootAfterAim = false;

        if (holding)
        {
            holding = false;
            holdingTime = 0f;
            currentWeaponSO.CancelAiming(playerManager, this);
        }
        else
        {
            currentWeaponSO.CancelAiming(playerManager, this);
        }
    }

    public IEnumerator RechargeWeaponCoroutine()
    {
        yield return new WaitForSeconds(rechargeTime);
        canShoot = true;
    }

    public void EquipWeapon(GameObject weapon, BowSO newWeaponSO)
    {
        currentWeaponSO = newWeaponSO;

        GameObject newWeapon = Instantiate(weapon, weapon.transform);
        newWeapon.transform.SetParent(weapon.transform);
        newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}