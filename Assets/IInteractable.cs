
using UnityEngine;

interface IInteractable
{
    void OnHover()
    {
        Debug.Log("Item Hovered");
    }

    abstract void OnInteract();
    
}