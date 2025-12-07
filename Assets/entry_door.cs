using UnityEngine;

public class entry_door : MonoBehaviour , IMission
{
    [SerializeField] bool belled = false;
    [SerializeField] bool barricaded = false;
    [SerializeField] GameObject ChairObjectToShow;
    [SerializeField] GameObject BellObjectToShow;

    float counter = 0;
    float time = 100;
    [SerializeField] Vector3 spawnPoint = new();

    public bool IsDone()
    {
        if (barricaded)
            return true;
        else
            return false;
    }

    public void SetCompletion(float degree)
    {
        if(degree>66)
        {
            barricaded=true;
            belled = true;
        }
        else if(degree>33)
            barricaded=true;
        
        else if(degree>0)
           belled=true;
        
        else if(degree<=-66)
        {
            barricaded=false;
            belled = false;
        }
        else if(degree<=-33)
        {
            barricaded=false;
            
        }
        else if(degree<=0)
        {
            belled=false;
            
        }
        _update_door();
    }

    public void OnInteract(Player interactee)
    {
        if(interactee.Item?.GetComponent<door_bell>() != null)
        {
            if(belled)
            {
                interactee.Item.Drop(interactee);
                return;
            }
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;            
            belled = true;
            BellObjectToShow.SetActive(true);


        }
        else if(interactee.Item?.GetComponent<blockade_chair>() != null)
        {
            if(barricaded)
            {
                interactee.Item.Drop(interactee);
                return;
            }
            Destroy(interactee.Item.gameObject);
            interactee.Item = null;            
            barricaded = true;
            ChairObjectToShow.SetActive(true);
            
        }
        
    }

        public void ScanDetected(Vector3? scanLocation = null, scan scan = null)
    {
        if(scan!=null)
        {
          
            scan.StartWave(position:transform.position,size:2,TriggersEnabled:false, waveIndex: 3);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _update_door();
    }

    void _update_door()
    {
        BellObjectToShow.SetActive(belled);
        ChairObjectToShow.SetActive(barricaded);

    }

    // Update is called once per frame
    void Update()
    {
       
        if(barricaded)
        {
            counter+=Time.deltaTime;
            if(counter>=time)
            {
                counter=0;
                barricaded=false;
                AudioManager.instance.Play("ChairBrokeSound", position: transform.position);
                _update_door();
            }
        }
        
    }

    public Vector3 GetSpawnPointForEnemy()
    {
        if(belled)
        {
            AudioManager.instance.Play("BellDoorSound", position: transform.position);

        }
        return spawnPoint;
    }
}
