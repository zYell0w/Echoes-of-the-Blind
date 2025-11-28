using UnityEngine;

public class tv : MonoBehaviour , IMission , IInteractable
{
    float counter = 0;
    float counterTime = 3.0f;
    private scan scan;
    float scanCounter = 0;
    float scanTimer = 2.0f;

    bool open = false;
    public bool IsDone()
    {
        return !open;
    }

    public void OnInteract(Player interactee)
    {
        if(open)
        {
            counter+=Time.deltaTime;
            if(counter>=counterTime)
            {
                open = false;
                counter=0;
            }
        }
        
    }

    public void SetCompletion(float degreeOutOf100)
    {
        if(degreeOutOf100>0)
            open = false;
        else
            open=true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scan = GetComponent<scan>();
    }

    // Update is called once per frame
    void Update()
    {
        if(open)
        {
            //AudioManager.instance.Play("");
            scanCounter+=Time.deltaTime;
            if(scanCounter>=scanTimer)
            {
                scan.StartWave();
                scanCounter=0;
            }
        }
    }
}
