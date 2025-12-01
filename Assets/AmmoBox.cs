using UnityEngine;

public class AmmoBox : MonoBehaviour , IInteractable, Iscanlistener
{
    scan _scan;

    public void Start()
    {
        _scan = GetComponent<scan>();
    }
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
            scan = _scan;
            scan.StartWave(position: transform.position, size: 2, TriggersEnabled: false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
