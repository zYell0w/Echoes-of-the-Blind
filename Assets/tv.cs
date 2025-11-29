using UnityEngine;

public class tv : MonoBehaviour , IMission 
{
    float randomCounter = 0f;
    float chanceOutOf100 = 10f;
    float time = 40f;
    float counter = 0;
    float counterTime = 3.0f;
    private scan scan;
    float scanCounter = 0;
    float scanTimer = 2.0f;
    bool isPlayingSound = false;

    [SerializeField] Vector3 spawnPoint = new();

    bool open = false;

    public Vector3 GetSpawnPointForEnemy()
    {
        return spawnPoint;
    }

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
                AudioManager.instance.Stop("TVSound");
                isPlayingSound = false;
                counter=0;
            }
        }
        
    }

    public void ScanDetected(Vector3 scanLocation)
    {
        Debug.LogError("Scan Detected at location: " + transform.position);
    }

    public void SetCompletion(float degreeOutOf100)
    {
        if (degreeOutOf100 > 0)
        {
            open = false;
            
        }
            
        else
        {
            open = true;
            isPlayingSound=false;
        }
            
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scan = GetComponent<scan>();
        isPlayingSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(open)
        {

            if(isPlayingSound == false)
            {
                AudioManager.instance.Play("TVSound", transform.position);
                isPlayingSound = true;
            }

            Debug.Log(transform.position);
            scanCounter+=Time.deltaTime;
            if(scanCounter>=scanTimer)
            {
                scan.StartWave();
                scanCounter=0;
            }
        }

        if (!open) 
        {
            if (randomCounter >= time)
            {
                randomCounter = 0;
                var rand = Random.Range(0, 100);
                if (rand <= chanceOutOf100)
                {
                    open = true;
                    isPlayingSound = false;
                    return;

                }
            }

            randomCounter += Time.deltaTime;
        }
    }


    
}
