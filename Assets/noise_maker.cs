using UnityEngine;

public class noise_maker : MonoBehaviour 
{
    //TODO ses
    bool makingNoise = false;
    float counter = 0f;
    float scanCounter = 0;
    [SerializeField] float scanDelay = 1.0f;
    float noisecounter=0f;
    scan scan;

    [SerializeField] float time = 3.0f;
    [SerializeField] float chanceOutOf100 = 5.0f;
    [SerializeField] float noiseTime = 100f;
    [SerializeField] string audioString;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scan = GetComponent<scan>();
    }

    // Update is called once per frame
    void Update()
    {
        if(counter>=time)
        {
            var rand = Random.Range(0,100);
            if(chanceOutOf100>=rand)
            {
                makingNoise=true;
                counter=0;
            }
        }
        if(makingNoise && noisecounter<=noiseTime)
        {
            noisecounter+=Time.deltaTime;
            scanCounter+= Time.deltaTime;
            AudioManager.instance.Play(audioString,transform.position);
            if(scanCounter>=scanDelay)
            {
                scan.StartWave();
                scanCounter=0;
            }
            
        }
        else
        {
            noisecounter=0;
            makingNoise=false;
            counter+=Time.deltaTime;

        }
    }
}
