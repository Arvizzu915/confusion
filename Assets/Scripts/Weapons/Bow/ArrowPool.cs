using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    public static ArrowPool Instance;

    [Header("Flying Arrows")]
    [SerializeField] private ArrowManager flyingArrowPrefab;
    [SerializeField] private int flyingArrowInitialAmount = 20;

    [Header("Stuck Arrows")]
    [SerializeField] private StuckArrow stuckArrowPrefab;
    [SerializeField] private int stuckArrowInitialAmount = 30;

    private readonly Queue<ArrowManager> flyingArrows = new();
    private readonly Queue<StuckArrow> stuckArrows = new();

    private void Awake()
    {
        Instance = this;

        CreateFlyingArrows();
        CreateStuckArrows();
    }

    private void CreateFlyingArrows()
    {
        for (int i = 0; i < flyingArrowInitialAmount; i++)
        {
            ArrowManager arrow = Instantiate(flyingArrowPrefab, transform);
            arrow.gameObject.SetActive(false);
            flyingArrows.Enqueue(arrow);
        }
    }

    private void CreateStuckArrows()
    {
        for (int i = 0; i < stuckArrowInitialAmount; i++)
        {
            StuckArrow arrow = Instantiate(stuckArrowPrefab, transform);
            arrow.gameObject.SetActive(false);
            stuckArrows.Enqueue(arrow);
        }
    }

    public ArrowManager GetFlyingArrow(Vector3 position, Quaternion rotation)
    {
        ArrowManager arrow;

        if (flyingArrows.Count > 0)
        {
            arrow = flyingArrows.Dequeue();
        }
        else
        {
            arrow = Instantiate(flyingArrowPrefab, transform);
        }

        arrow.transform.SetPositionAndRotation(position, rotation);
        arrow.gameObject.SetActive(true);
        arrow.ResetArrow();

        return arrow;
    }

    public StuckArrow GetStuckArrow(Vector3 position, Quaternion rotation)
    {
        StuckArrow arrow;

        if (stuckArrows.Count > 0)
        {
            arrow = stuckArrows.Dequeue();
        }
        else
        {
            arrow = Instantiate(stuckArrowPrefab, transform);
        }

        arrow.transform.SetPositionAndRotation(position, rotation);
        arrow.gameObject.SetActive(true);
        arrow.ResetArrow();

        return arrow;
    }

    public void ReturnFlyingArrow(ArrowManager arrow)
    {
        arrow.gameObject.SetActive(false);
        arrow.transform.SetParent(transform);
        flyingArrows.Enqueue(arrow);
    }

    public void ReturnStuckArrow(StuckArrow arrow)
    {
        arrow.gameObject.SetActive(false);
        arrow.transform.SetParent(transform);
        stuckArrows.Enqueue(arrow);
    }
}