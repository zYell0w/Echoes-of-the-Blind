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
            AudioManager.instance.Play("BrokenWindowPuttingWood");
            _update_wood();

            
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
        // 1. ÖNCE SAHNEDEKİ MEVCUT DURUMU TESPİT ET
        List<GameObject> activeList = new List<GameObject>();   // Şu an AÇIK olanlar
        List<GameObject> inactiveList = new List<GameObject>(); // Şu an KAPALI olanlar

        foreach (GameObject wood in woodObjectsToShow)
        {
            if (wood.activeSelf)
                activeList.Add(wood);
            else
                inactiveList.Add(wood);
        }

     
        // 2. EĞER AÇIK SAYISI HEDEFTEN AZSA -> EKLE (WHILE DÖNGÜSÜ)
        while(activeList.Count < woodCount && inactiveList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, inactiveList.Count);
            GameObject woodToOpen = inactiveList[randomIndex];

            woodToOpen.SetActive(true); // Aç

            // Listeleri güncelle ki döngü doğru devam etsin
            activeList.Add(woodToOpen);
            inactiveList.RemoveAt(randomIndex);
        }

        // 3. EĞER AÇIK SAYISI HEDEFTEN FAZLAYSA -> KIR/KAPAT (WHILE DÖNGÜSÜ)
        while(activeList.Count > woodCount && activeList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, activeList.Count);
            GameObject woodToClose = activeList[randomIndex];

            woodToClose.SetActive(false); // Kapat

            // Listeleri güncelle
            inactiveList.Add(woodToClose);
            activeList.RemoveAt(randomIndex);
        }
        
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
                AudioManager.instance.Play("WindowWoodBrokeSound", position: transform.position);
                _update_wood();
                time = UnityEngine.Random.Range(min, max);
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
