using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private float throwForce = 10f;
    private bool hasBeenThrown = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void Update()
    {
        if (!hasBeenThrown && rb.linearVelocity.magnitude > 0.1f)
        {
            hasBeenThrown = true;
            rb.useGravity = true;
        }
    }
}
