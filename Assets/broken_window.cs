using UnityEngine;

public class broken_window : MonoBehaviour , Iscanlistener , IInteractable
{
    [SerializeField] private int woodCount = 0;
    public void OnInteract(Player interactee)
    {
        if(interactee.Item.GetComponent<wood>()!=null)
        {
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;
            woodCount++;
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
        
    }
}
