using System.Collections.Generic;
using UnityEngine;

public class broken_window : MonoBehaviour , Iscanlistener , IInteractable
{
    [SerializeField] private int woodCount = 0;
    [SerializeField] List<GameObject> woodObjectsToShow = new();
    public void OnInteract(Player interactee)
    {
        
        if(interactee.Item?.GetComponent<wood>()!=null)
        {
            if(woodCount == woodObjectsToShow.Count)
            {
                interactee.Item.Drop(interactee);
                //başarısız ses efekti
                return;
            }
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;
            woodCount++;
            List<GameObject> availableWoods = new();
            foreach(GameObject wood in woodObjectsToShow)
            {
                if(wood.activeSelf == false)
                    availableWoods.Add(wood);   
            }
            int random = Random.Range(0,availableWoods.Count);
            availableWoods[random].SetActive(true);

            //başarılı ses efekti
        }
    }

    public void ScanDetected()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(GameObject wood in woodObjectsToShow)
        {
            wood.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
