using UnityEngine;

public class AmmoBox : MonoBehaviour , IInteractable
{
    public void OnInteract(Player interactee)
    {
        if(interactee.Weapon?.GetComponent<Gun>()!=null)
        {
            interactee.Weapon.GetComponent<Gun>().Reload();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
