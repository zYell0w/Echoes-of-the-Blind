using UnityEngine;

public class normal_window : MonoBehaviour , IInteractable , Iscanlistener
{
    [SerializeField] float counter;
    const float max = 50;
    const float min = 5;
    bool holding = false;

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

   

    public void ScanDetected(Vector3 scanLocation)
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        perde = transform.Find("perde").gameObject;
        startX = perde.transform.localScale.x;
        perdeScale = transform.localScale;
        counter = 10;
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
}
