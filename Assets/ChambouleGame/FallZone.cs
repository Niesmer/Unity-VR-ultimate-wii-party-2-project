using UnityEngine;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Canette canette = other.GetComponent<Canette>();
        if (canette != null){
            canette.KnockDown();
        }
    }
}
