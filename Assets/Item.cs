using UnityEngine;

public abstract class Item : MonoBehaviour ,IInteractable , IEquipable,Iscanlistener{
    private int _dropped = 0;
    [SerializeField] scan scan;

    public void Drop(Player interactee)
    {
        _dropped = 3;
        Debug.Log("Interacted");
        interactee.Item = null;
        foreach(Collider c in  GetComponents<Collider>())
            c.enabled = true;
        if(GetComponent<Collider>().bounds.center.y<transform.parent.parent.position.y-1)
            transform.position+=Vector3.up*2f;
        //transform.position =  transform.parent.position+transform.parent.parent.forward*1.3f;
        
        //transform.localRotation = Quaternion.identity;

        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<MeshRenderer>().enabled = true;



    }

    public void Equip(Player interactee)
    {
        if(interactee.Item != null)
            interactee.Item.Drop(interactee);
        interactee.Item = this;
        scan = interactee.GetComponent<scan>();
        foreach(Collider c in  GetComponents<Collider>())
            c.enabled = false;
        
        GetComponent<Rigidbody>().isKinematic = true;

        GetComponent<MeshRenderer>().enabled = false;

        transform.parent = interactee.transform.Find("Main Camera");
        if(interactee.Item.GetComponent<wood>() != null)
            AudioManager.instance.Play("WoodGrabSound");
        else
        AudioManager.instance.Play("GrabSound");

        //transform.position = transform.parent.position + transform.parent.forward*1.5f;
        
        //transform.localPosition = Vector3.forward * 2;
        //transform.localRotation = Quaternion.identity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(_dropped>0 && scan!=null)
        {
            scan.StartWave(position:collision.GetContact(0).point,size:3);
            AudioManager.instance.Play("itemDropSound");
        }
        _dropped--;
    }


    public void OnInteract(Player interactee)
    {
       Equip(interactee);
     
    }

    public void ScanDetected(Vector3 scanLocation)
    {
        Debug.LogError("Scan Detected at location: " + transform.position);
    }
}
