using UnityEngine;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Can canette = other.GetComponent<Can>();
        if (canette != null)
        {
            canette.Fall();
        }
    }
}
