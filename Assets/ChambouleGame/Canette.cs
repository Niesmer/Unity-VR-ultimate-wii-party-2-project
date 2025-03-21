using UnityEngine;
using System.Collections;

public class Can : MonoBehaviour
{
    [Header("Can Properties")]
    [SerializeField] private float weight = 1f;
    [SerializeField] private Vector3 size = new Vector3(0.12f, 0.07f, 0.12f);

    private bool isDown = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb)
            rb.mass = weight;

        transform.localScale = size;
    }

    public void Fall()
    {
        if (isDown) return;

        isDown = true;
        Debug.Log("Can fell!");

        GameManager.Instance.AddScore(GameManager.Instance.GetPointsPerCan());

        StartCoroutine(DestroyAfterDelay(3f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
