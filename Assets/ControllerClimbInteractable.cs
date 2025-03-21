using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Climbing;

public class ControllerClimbInteractable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to enable or disable all children climb interactables
    public void SetChildrenInteractable(bool isEnabled)
    {
        foreach (Transform child in transform)
        {
            var collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = isEnabled;
            }
        }
    }
}
