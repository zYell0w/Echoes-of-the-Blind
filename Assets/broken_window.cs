using System;
using System.Collections.Generic;
using UnityEngine;

public class broken_window : MonoBehaviour , Iscanlistener , IInteractable , IMission
{
    [SerializeField] private int woodCount = 0;
    [SerializeField] List<GameObject> woodObjectsToShow = new();

   

    public void SetCompletion(float degree)
    {
        if(degree>0)
            woodCount = (int) Math.Round(degree * (float)woodObjectsToShow.Count / 100);
        else
            woodCount = Math.Min(woodCount,Math.Abs((int) Math.Round(degree * (float)woodObjectsToShow.Count / 100)));

    }
    

    public bool IsDone()
    {
        if(woodCount>0) 
            return true;
        else
            return false;
    }
    

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
            int random = UnityEngine.Random.Range(0,availableWoods.Count);
            availableWoods[random].SetActive(true);

            //başarılı ses efekti
        }
    }

    public void ScanDetected(Vector3 scanLocation)
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _update_wood();
    }

    private void _update_wood()
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
