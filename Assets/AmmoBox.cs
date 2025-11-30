using UnityEngine;

public class AmmoBox : MonoBehaviour , IInteractable, Iscanlistener
{
    public void OnInteract(Player interactee)
    {
        if(interactee.Weapon?.GetComponent<Gun>()!=null)
        {
            interactee.Weapon.GetComponent<Gun>().Reload();
            AudioManager.instance.Play("AmmoSound");
        }
    }

    public void ScanDetected(Vector3? scanLocation = null, scan scan = null)
    {
        if (scan != null)
        {
            scan.StartWave(position: transform.position, size: 2, TriggersEnabled: false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
