using System.Collections;
using UnityEngine;

public class BlackWeb : MonoBehaviour
{
    [SerializeField] private float burnTime = 5;

    private void Burn()
    {
        StartCoroutine(BurnWeb());
    }

    private IEnumerator BurnWeb()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position - new Vector3(0, 15f, 0);
        float timer = 0f;

        while (timer < burnTime)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / burnTime;

            t = Mathf.SmoothStep(0f, 1f, t);


            transform.SetPositionAndRotation(Vector3.Lerp(
                startPos,
                targetPos,
                t
            ), Quaternion.identity
            );

            yield return null;
        }

        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lighter"))
        {
            Burn();
        }
    }
}
