using UnityEngine;

public class LighterKeyObject : KeyObject
{
    public override void TakeObject()
    {
        base.TakeObject();

        PlayerCombat.Instance.lighterObtained = true;
        PlayerCombat.Instance.ChangeWeapon(1);
    }
}
