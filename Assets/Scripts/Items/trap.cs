using UnityEngine;

public class trap : Item 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<Enemy>()!=null)
        {
            other.gameObject.GetComponent<Enemy>().Hit();
            Destroy(this.gameObject,1.0f);
        }
    }
}
