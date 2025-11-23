using UnityEngine;

public class entry_door : MonoBehaviour , IInteractable , Iscanlistener
{
    [SerializeField] bool belled = false;
    [SerializeField] bool barricaded = false;
    [SerializeField] GameObject ChairObjectToShow;
    [SerializeField] GameObject BellObjectToShow;

    public void OnInteract(Player interactee)
    {
        interactee.Item.TryGetComponent<door_bell>(out door_bell bell);
        interactee.Item.TryGetComponent<blockade_chair>(out blockade_chair chair);

        if(bell != null)
        {
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;            
            belled = true;

        }
        else if(chair != null)
        {
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;            
            barricaded = true;
            ChairObjectToShow.SetActive(true);
            
        }
    }

    public void ScanDetected()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChairObjectToShow.SetActive(false);
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
