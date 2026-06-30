using System.Collections;
using UnityEngine;

public class Safe : IInspectionable
{
    [SerializeField] private Transform safeLock;
    [SerializeField] private int[] combination;
    [SerializeField] private Collider[] coll;

    [SerializeField] private Animator animator;

    private bool moving = false, canMove = true;
    private Vector2 movingInput = Vector2.zero;

    private int currentNumber = 0, currentCorrectCombinations = 0;
    private bool left = true;

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
            left = true;

            if (currentNumber == 0)
            {
                currentNumber = 9;
            }
            else
            {
                currentNumber -= 1;
            }

            

            end = start *
            Quaternion.Euler(0f, 36f, 0f);
        }
        else
        {
            

            left = false;

            if (currentNumber == 9)
            {
                currentNumber = 0;
            }
            else
            {
                currentNumber += 1;
            }

            end = start *
            Quaternion.Euler(0f, -36f, 0f);
        }

        Debug.Log(currentNumber);
        CheckNumbers();

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

    public override void MoveInputs(Vector2 direction)
    {
        movingInput = direction;

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

    public override void Use()
    {
        CheckCombination();
    }

    private void CheckCombination()
    {
        Debug.Log(currentCorrectCombinations);

        if (currentCorrectCombinations >= 3)
        {
            OpenSafe();
        }
    }

    private void OpenSafe()
    {
        animator.SetBool("Open", true);
        foreach (Collider col in coll)
        {
            col.enabled = false;
        }

        StartCoroutine(ReturnToGameplay());
    }

    private IEnumerator ReturnToGameplay()
    {
        yield return new WaitForSeconds(.5f);
        AnalyzingMode.Instance.ExitAnalyzeMode();
    }

    private void CheckNumbers()
    {
        if (currentCorrectCombinations == 0)
        {
            if (currentNumber == combination[0] && left)
            {
                currentCorrectCombinations++;
            }
        }
        else if (currentCorrectCombinations == 1)
        {
            if (currentNumber == combination[1] && !left)
            {
                currentCorrectCombinations++;
            }
        }
        else if (currentCorrectCombinations == 2)
        {
            if (currentNumber == combination[2] && left)
            {
                currentCorrectCombinations++;
            }
        }
        else
        {
            currentCorrectCombinations = 0;
        }
    }
}
