using System;
using System.Collections.Generic;
using UnityEngine;

public class broken_window : MonoBehaviour , IMission
{
    [SerializeField] private int woodCount = 0;
    [SerializeField] List<GameObject> woodObjectsToShow = new();

    [SerializeField] Vector3 spawnPoint = new();
    
    float counter= 0;
    [SerializeField] float time;
    [SerializeField] float max = 90;
    [SerializeField] float min = 30;

    public void SetCompletion(float degree)
    {
        if(degree>0)
            woodCount = (int) Math.Round(degree * (float)woodObjectsToShow.Count / 100);
        else
            woodCount = Math.Min(woodCount,Math.Abs((int) Math.Round(degree * (float)woodObjectsToShow.Count / 100)));
        _update_wood();
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
            _update_wood();

            //başarılı ses efekti
        }
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(GameObject wood in woodObjectsToShow)
        {
            wood.SetActive(false);
        }
        time = UnityEngine.Random.Range(min, max);
        _update_wood();
    }

    private void _update_wood()
    {
        List<GameObject> availableWoods = new();
        List<GameObject> unavailableWoods = new();

            foreach(GameObject wood in woodObjectsToShow)
            {
                if(wood.activeSelf == false)
                    availableWoods.Add(wood);   
                else
                    unavailableWoods.Add(wood);
            }
        int a = woodObjectsToShow.Count - availableWoods.Count;
        bool changeDone = false;
        if(a<0)
        {
            for(int i =0;i<-a;i++)
            {
                int random = UnityEngine.Random.Range(0,unavailableWoods.Count);
                unavailableWoods[random].SetActive(false);
                unavailableWoods.RemoveAt(random);
                changeDone = true;
            }
        }
        else
        {
            for(int i =0;i<woodCount-a;i++)
            {
                int random = UnityEngine.Random.Range(0,availableWoods.Count);
                availableWoods[random].SetActive(true);
                availableWoods.RemoveAt(random);
                changeDone = true;
            }
        }
        if(changeDone)
            time = UnityEngine.Random.Range(min, max);
            
    }

    // Update is called once per frame
    void Update()
    {
        if(woodCount>0)
        {
            counter+=Time.deltaTime;
            if(counter>=time)
            {
                counter=0;
                woodCount--;
                _update_wood();
            }
        }
    }

    public Vector3 GetSpawnPointForEnemy()
    {
        return spawnPoint;
    }

    public void ScanDetected(Vector3 scanLocation)
    {
        Debug.LogError("Scan Detected at location: " + transform.position);
    }
}
