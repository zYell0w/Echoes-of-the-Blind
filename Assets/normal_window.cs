using UnityEngine;

public class normal_window : MonoBehaviour , IInteractable , Iscanlistener
{
    [SerializeField] float yo = 0;
    bool holding = false;
    public void OnInteract(Player interactee)
    {
        holding = true;
        yo+=Time.deltaTime;
        
    }

    public void ScanDetected()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //aha buraya azalma şeysi yapılabilir
        if(holding == false && yo > 0)
            yo -= Time.deltaTime / 3;
        else
            holding = false;

        //değer belli bir şeyden yüksekse ya da azsa da buraya yapılabilir
        
    }
}
