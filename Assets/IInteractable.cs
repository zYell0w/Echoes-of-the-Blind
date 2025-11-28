
using UnityEngine;

public interface IInteractable
{
    void OnHover()
    {
        Debug.Log("Item Hovered");
    }

    abstract void OnInteract(Player interactee);
    
}