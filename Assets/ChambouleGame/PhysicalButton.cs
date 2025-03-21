using UnityEngine;

public class PhysicalButton : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand") || other.CompareTag("Controller"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }
    }
}

