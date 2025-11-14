using UnityEngine;

public class entry_door : MonoBehaviour , IInteractable , Iscanlistener
{
    [SerializeField] bool belled = false;
    [SerializeField] bool barricaded = false;
    public void OnInteract(Player interactee)
    {
        if(interactee.Item.GetComponent<door_bell>() != null)
        {
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;            
            belled = true;

        }
        else if(interactee.Item.GetComponent<blockade_chair>() != null)
        {
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;            
            barricaded = true;
        }
    }

    public void ScanDetected()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //böyle yapılabilir
        if(belled)
        {
            
        }
        if(barricaded)
        {
            
        }
        
    }
}
