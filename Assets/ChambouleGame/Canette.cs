using UnityEngine;
using System.Collections;

public class Canette : MonoBehaviour
{
    [Header("Paramètres physiques de la Canette")]
    [SerializeField] private float mass = 1f;
    [SerializeField] private Vector3 canetteSize = new Vector3(0.12f, 0.07f, 0.12f);

    private bool isKnockedDown = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = mass;
        }
        transform.localScale = canetteSize;
    }

    public void KnockDown()
    {
        if (!isKnockedDown)
        {
            isKnockedDown = true;
            Debug.Log("Canette tombée!");

            int pointsPerCan = GameManager.Instance.GetPointsPerCan();
            GameManager.Instance.addScore(pointsPerCan);

            StartCoroutine(DestroyCanetteAfterDelay(3f));
        }
    }

    private IEnumerator DestroyCanetteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
