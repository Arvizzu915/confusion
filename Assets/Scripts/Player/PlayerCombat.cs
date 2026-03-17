using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public PlayerManager playerManager;
    public InputManager inputManager;

    private WeaponSO currentWeaponSO = null;

    [SerializeField] private GameObject rightWeapon, leftWeapon;
    private void OnEnable()
    {
        inputManager.inputs.Playing.Attack1.performed += UseWeapon;
    }

    private void OnDisable()
    {
        inputManager.inputs.Playing.Attack1.performed -= UseWeapon;
    }

    public void EquipWeapon(GameObject weapon, WeaponSO newWeaponSO)
    {
        currentWeaponSO = newWeaponSO;

        GameObject newWeapon = Instantiate(weapon, leftWeapon.transform);
        newWeapon.transform.SetParent(leftWeapon.transform);
        newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void UseWeapon(InputAction.CallbackContext ctx)
    {
        if (currentWeaponSO == null) return;

        currentWeaponSO.Use(playerManager, this);
    }
}
