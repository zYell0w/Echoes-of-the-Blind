using UnityEngine;

public class entry_door : MonoBehaviour , IInteractable , Iscanlistener , IMission
{
    [SerializeField] bool belled = false;
    [SerializeField] bool barricaded = false;
    [SerializeField] GameObject ChairObjectToShow;
    [SerializeField] GameObject BellObjectToShow;

    public bool IsDone()
    {
        if(belled || barricaded)
            return true;
        else
            return false;
    }

    public void SetCompletion(float degree)
    {
        
        if(degree>0)
           belled=true;
        else if(degree>33)
            barricaded=true;
        else if(degree>66)
        {
            barricaded=true;
            belled = true;
        }
        else if(degree<=-66)
        {
            barricaded=false;
            belled = false;
        }
        else if(degree<=-33)
        {
            barricaded=false;
            
        }
        else if(degree<=0)
        {
            belled=false;
            
        }
    }

    public void OnInteract(Player interactee)
    {
        if(interactee.Item?.GetComponent<door_bell>() != null)
        {
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;            
            belled = true;
            BellObjectToShow.SetActive(true);


        }
        else if(interactee.Item?.GetComponent<blockade_chair>() != null)
        {
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;            
            barricaded = true;
            ChairObjectToShow.SetActive(true);
            
        }
    }

    public void ScanDetected(Vector3 scanLocation)
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChairObjectToShow.SetActive(false);
        BellObjectToShow.SetActive(false);
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
