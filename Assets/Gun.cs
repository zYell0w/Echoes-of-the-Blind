using UnityEngine;

public class Gun : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        Debug.Log("Interacted");
    }
}