using System;
using UnityEngine;

public class normal_window : MonoBehaviour , IMission
{
    [SerializeField] float counter;
    const float max = 50;
    const float min = 5;
    bool holding = false;

    [SerializeField] Vector3 spawnPoint = new();


    GameObject perde ;
    Vector3 perdeScale;
    float startX;
    public void OnInteract(Player interactee)
    {
        holding = true;
        if (counter < max)
        {
            counter += Time.deltaTime;
            AudioManager.instance.Play("CurtainClosingSound");
        }

            
        
    }

   

       public void ScanDetected(Vector3? scanLocation = null, scan scan = null)
    {
        if(scan!=null)
        {
          
            scan.StartWave(position:transform.position,size:2,TriggersEnabled:false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        perde = transform.Find("perde").gameObject;
        startX = perde.transform.localScale.x;
        perdeScale = transform.localScale;
        counter = max/2;
    }

    // Update is called once per frame
    void Update()
    {
        //aha buraya azalma şeysi yapılabilir
        if(holding == false && counter > min)
            counter -= Time.deltaTime / 3;
        else
            holding = false;

        perdeScale.x = counter/max  * startX;
        perdeScale.y = perde.transform.localScale.y;
        perdeScale.z = perde.transform.localScale.z;

        perde.transform.localScale = perdeScale;       
        //değer belli bir şeyden yüksekse ya da azsa da buraya yapılabilir
        
    }

    public bool IsDone()
    {
        if(counter<min+1)
            return false;
        else
            return true;
    }

    public void SetCompletion(float degreeOutOf100)
    {
        if(degreeOutOf100>0)
        {
            counter=Math.Max(min,degreeOutOf100 * max / 100);
        }
        else
        {
            counter=Math.Min(counter,Math.Max(min,-1*degreeOutOf100 * max / 100));
            
        }
    }

    public Vector3 GetSpawnPointForEnemy()
    {
        return spawnPoint;
    }
}
