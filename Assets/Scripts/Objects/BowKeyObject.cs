using UnityEngine;

public class BowKeyObject : KeyObject
{
    public override void TakeObject()
    {
        base.TakeObject();

        PlayerCombat.Instance.bowObtained = true;
        PlayerCombat.Instance.ChangeWeapon(0);
    }
}
