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

    [Header("Aim")]
    public Vector3 aimingDirection;
    public bool canShoot = true;
    [SerializeField] private LayerMask detectLayer;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float aimDistance = 100f;
    [SerializeField] private GameObject aimingCamera;

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
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * aimDistance;
        }

        aimingDirection = (targetPoint - bowManager.arrowSpwnPos.position).normalized;
    }

    public void EquipWeapon(GameObject weapon, BowSO newWeaponSO)
    {
        currentWeaponSO = newWeaponSO;

        GameObject newWeapon = Instantiate(weapon, weapon.transform);
        newWeapon.transform.SetParent(weapon.transform);
        newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void TryHoldBow(InputAction.CallbackContext ctx)
    {
        holding = true;
    }

    public void HoldBow()
    {
        if (currentWeaponSO == null) return;

        currentWeaponSO.Use(playerManager, this);
    }

    public void StopHoldingBow(InputAction.CallbackContext ctx)
    {
        if (currentWeaponSO == null) return;

        currentWeaponSO.StopUsing(playerManager, this);
    }

    public void Aim(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            aiming = true;
            aimingCamera.SetActive(true);
            currentWeaponSO.Aim(playerManager, this);
        }
        else if(ctx.canceled)
        {
            aiming = false;
            aimingCamera.SetActive(false);
            currentWeaponSO.CancelAiming(playerManager, this);
        }
    }
}
