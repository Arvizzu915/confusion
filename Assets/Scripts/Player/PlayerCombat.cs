using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;

    public PlayerManager playerManager;
    public InputManager inputManager;

    [Header("Weapon")]
    public WeaponSO currentWeaponSO = null, bowSO, lighterSO;
    public BowManager bowManager;
    public LighterManager lighterManager;
    public GameObject weapon, bow, lighter;
    public bool lighterObtained = false;

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
    public GameObject aimingCamera, shootingCamera;

    private Coroutine aimCoyoteCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        inputManager.inputs.Playing.Attack1.performed += Use;
        inputManager.inputs.Playing.Attack1.canceled += StopHoldingBow;
        inputManager.inputs.Playing.Aim.performed += Aim;
        inputManager.inputs.Playing.Aim.canceled += Aim;
        inputManager.inputs.Playing.Change.performed += ChangeWeapon;
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.Attack1.performed -= Use;
        inputManager.inputs.Playing.Attack1.canceled -= StopHoldingBow;
        inputManager.inputs.Playing.Aim.performed -= Aim;
        inputManager.inputs.Playing.Aim.canceled -= Aim;
        inputManager.inputs.Playing.Change.performed -= ChangeWeapon;
    }

    private void Update()
    {
        if (currentWeaponSO == null) return;

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

    public void Use(InputAction.CallbackContext ctx)
    {
        currentWeaponSO.Use(playerManager ,this);
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
            aimingCamera.SetActive(true);

            if (currentWeaponSO != null)
            {
                currentWeaponSO.Aim(playerManager, this);

                if (aimCoyoteCoroutine != null)
                    StopCoroutine(aimCoyoteCoroutine);
            }
            

            PlayerMovement.instance.StopRunning();
        }
        else if (ctx.canceled)
        {

            StopAiming();
        }
    }

    public void StopAiming()
    {
        aiming = false;
        aimingCamera.SetActive(false);

        if (currentWeaponSO == null) return;

        if (aimCoyoteCoroutine != null)
            StopCoroutine(aimCoyoteCoroutine);

        aimCoyoteCoroutine = StartCoroutine(ShootCoyoteTime());
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

    public void EquipWeapon(GameObject weapon, WeaponSO newWeaponSO)
    {
        currentWeaponSO = newWeaponSO;

        GameObject newWeapon = Instantiate(weapon, weapon.transform);
        newWeapon.transform.SetParent(weapon.transform);
        newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    private void ChangeWeapon(InputAction.CallbackContext ctx)
    {
        if (!lighterObtained) return;

        if (ctx.ReadValue<Vector2>() == Vector2.up)
        {
            currentWeaponSO = bowSO;
            lighter.SetActive(false);
            bow.SetActive(true);
        }
        else if (ctx.ReadValue<Vector2>() == Vector2.down)
        {
            currentWeaponSO = lighterSO;
            lighter.SetActive(true);
            bow.SetActive(false);
        }
        else
        {
            currentWeaponSO = null;
            lighter.SetActive(false);
            bow.SetActive(false);
        }
    }
}