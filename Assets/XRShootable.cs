using UnityEngine;
using UnityEngine.Events;

public class XRShootable : MonoBehaviour
{
    public UnityEvent onHit;
    [HideInInspector]
    public bool destroyed;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            onHit?.Invoke();
            Destroy(other.gameObject);
        }
    }

    public void DestroyThisObject()
    {
        print("destroying object" + gameObject.name);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        destroyed = true;
    }
}
