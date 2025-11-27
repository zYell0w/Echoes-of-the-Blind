using UnityEngine;

public class noise_maker : MonoBehaviour , IMission , IInteractable
{
    //TODO ses
    bool makingNoise = false;
    float interactCounter = 0f;
    float interactTime = 3.0f;
    public bool IsDone()
    {
        return !makingNoise;
    }

    public void OnInteract(Player interactee)
    {
        if(makingNoise)
        {
             interactCounter+=Time.deltaTime;
            if(interactCounter>=interactTime)
            {
                makingNoise = false;
                interactCounter=0f;
            }
        }
        
       
    }

    public void SetCompletion(float degreeOutOf100)
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(makingNoise)
        {
            //TODO
        }
    }
}
