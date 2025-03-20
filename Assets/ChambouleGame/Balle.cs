using UnityEngine;
using System.Collections.Generic;

public class Balle : MonoBehaviour
{
    [Header("Paramètres de la balle")]
    [SerializeField] private float forceLancer = 10f;
    private bool aEteLancee = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
        }
    }

    private void Update()
    {
        if (!aEteLancee && rb.linearVelocity.magnitude > 0.1f)
        {
            aEteLancee = true;
            rb.useGravity = true;
        }
    }
}
