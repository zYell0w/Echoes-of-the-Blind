using System;
using UnityEngine;

public class AmmoBox : MonoBehaviour , IInteractable, Iscanlistener
{
    scan _scan;
 [field:SerializeField]
    public InteractInfo Info { get; set; }

    public void Start()
    {
        Info.name = "ammo_box1";
        _scan = GetComponent<scan>();
    }
    public void OnInteract(Player interactee)
    {
        if(interactee.Weapon?.GetComponent<Gun>()!=null)
        {
            Info.name = "ammo_box";
            
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
