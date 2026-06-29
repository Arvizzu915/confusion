using System.Collections;
using UnityEngine;

public class Safe : IInspectionable
{
    [SerializeField] private Transform safeLock;

    private bool moving = false, canMove = true;
    private Vector2 movingInput = Vector2.zero;

    private void Update()
    {
        if (moving && canMove)
        {
            StartCoroutine(MoveSafe(movingInput));
        }
    }

    private IEnumerator MoveSafe(Vector2 direction)
    {
        
        canMove = false;

        Quaternion start = safeLock.localRotation;
        Quaternion end;

        if (direction.x < 0)
        {
            end = start *
            Quaternion.Euler(0f, -15f, 0f);
        }
        else
        {
            end = start *
            Quaternion.Euler(0f, 15f, 0f);
        }



            float timer = 0f;

        while (timer < 0.2f)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.SmoothStep(0f, 1f, timer / 0.2f);

            safeLock.localRotation = Quaternion.Lerp(start, end, t);

            yield return null;
        }

        safeLock.localRotation = end;

        canMove = true;
    }

    public override void Inspect()
    {
        base.Inspect();

        AnalyzingMode.Instance.EnterAnalyzeMode(false);
    }

    public override void UseItem(int itemIndex, ItemSlotUI itemSlot)
    {
        
    }

    public override void MoveInputs()
    {
        
        moving = true;
    }

    public override void MoveDirection(Vector2 direction)
    {
        movingInput = direction;
    }

    public override void CancelInput()
    {
        moving = false;
    }
}
