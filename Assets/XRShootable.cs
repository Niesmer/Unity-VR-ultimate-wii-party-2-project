using UnityEngine;
using UnityEngine.Events;

public class XRShootable : MonoBehaviour
{
    public UnityEvent onHit;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            onHit?.Invoke();
            Destroy(other.gameObject);
        }
    }
}
