using UnityEngine;

public abstract class Item : MonoBehaviour ,IInteractable , IEquipable{
    public void Drop(Player interactee)
    {
        Debug.Log("Interacted");
        interactee.Item = null;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;

        transform.localPosition = Vector3.forward*1.5f;

        transform.localRotation = Quaternion.identity;

        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;


    }

    public void Equip(Player interactee)
    {
        interactee.Item = this;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        transform.parent = interactee.transform.Find("Main Camera");
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    
    public void OnInteract(Player interactee)
    {
       Equip(interactee);
     
    }
}
