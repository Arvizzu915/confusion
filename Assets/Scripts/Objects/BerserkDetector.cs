using UnityEngine;

public class BerserkDetector : RoomDetector
{
    public static BerserkDetector Instance;

    [SerializeField] private GameObject berseker;
    [SerializeField] private Door[] doors;

    private void Awake()
    {
        Instance = this;
    }

    public override void DetectPlayer()
    {
        base.DetectPlayer();

        if (PlayerManager.instance.keyToMechanicsObtained)
        {
            foreach (Door door in doors)
            {
                door.locked = true;
            }
        }
    }

    public void ActivateBerserker()
    {
        berseker.SetActive(true);
    }
}
