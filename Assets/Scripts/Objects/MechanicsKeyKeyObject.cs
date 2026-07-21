using UnityEngine;

public class MechanicsKeyKeyObject : KeyObject
{
    public override void TakeObject()
    {
        base.TakeObject();

        PlayerManager.instance.keyToMechanicsObtained = true;
    }
}
