
using System;
using UnityEngine;
[System.Serializable]
    public class InteractInfo
    {
        public string name;

        public InteractInfo(string name)
        {
            this.name = name;
        }
    }
public interface IInteractable
{
    void OnHover()
    {
        Debug.Log("Item Hovered");
    }

    abstract void OnInteract(Player interactee);
    public InteractInfo Info{get; set;}
}