using UnityEngine;
using System.Collections;
[RequireComponent(typeof(scan))]
public class tütsü : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    
    
    public static uint tütsüCounter = 0; //4 tanesi aynı anda yanarsayı falan bu countere göre çözcez

    private Coroutine stillBurning;
    private Collider col;
    Vector3 spawnPoint;


    scan _scan;

    void Start()
    {
        _scan = GetComponent<scan>();
        col = GetComponent<Collider>();
        
        spawnPoint = new Vector3(
            col.bounds.center.x,        // X ortası local merkez
            col.bounds.max.y+0.1f,           // Y üst noktası world
            col.bounds.center.z         // Z ortası local merkez
        );
    }

    // Update is called once per frame
    void Update()
    {
   
        
    }


    public void OnInteract(Player interactee)
    {
        burn();
    }

    void burn()
    {
        if (stillBurning == null)
        {

            stillBurning = StartCoroutine(BurningCoroutine());

        }
        else if (stillBurning != null) 
        {
            Debug.Log("Zaten yanıyom amk");
        
        }

    }

    IEnumerator BurningCoroutine()
    {
        float zaman = 0f;
        tütsüCounter++;
        
        while (zaman < 10f) 
        {
            Debug.Log("yandım");
            Debug.Log(tütsüCounter + "tanemiz de yanıyor");
            _scan.StartWave(position: spawnPoint);

            yield return new WaitForSeconds(1f); 
            zaman += 1f; 
        }

        tütsüCounter--;
        stillBurning = null; 
    }


}





