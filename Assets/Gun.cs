using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(scan))]

public class Gun : MonoBehaviour, IInteractable , IEquipable
{
    private uint ammo = 9999;

    [SerializeField] GameObject gunHand;
    [SerializeField] Sprite[] GunImageSprite;
    private Image ShootImage;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private float frameDelay = 0.1f;

    LayerMask layerMask;

    scan _scan;

    public void Start()
    {
        _scan = GetComponent<scan>();
        layerMask = LayerMask.GetMask("Default","Enemy");
    }

    public void Drop(Player interactee)
    {
         Debug.Log("Interacted");
        interactee.Weapon = null;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;


        transform.localPosition = Vector3.forward*1.5f;

        transform.localRotation = Quaternion.identity;

        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public bool Use()
    {
        /*
        if(ammo>0)
            ammo--;
        else
            return false;*/
        RaycastHit hit;
        StartCoroutine(ShowImage(gunHand));
        if (Physics.Raycast(transform.position, transform.forward, out hit, int.MaxValue , layerMask))
        {
            //hit.transform.gameObject.GetComponent<IInteractable>().OnInteract(_player);
            _scan.StartWave(position:hit.point);
         
            Debug.LogError("saas");
        }
        return true;
    }

    public void Equip(Player interactee)
    {
        interactee.Weapon = this;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;


        transform.parent = interactee.transform.Find("Main Camera");
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }

    //kullanıldığında çıkacak görsel değişkeni de burada olacak

    public void OnInteract(Player interactee)
    {
        Equip(interactee);
        
    }

    IEnumerator ShowImage(GameObject gunImageObject)
    {
        ShootImage = gunHand.GetComponent<Image>();
        gunImageObject.SetActive(true);
        if (isPlaying == false)
        {

            isPlaying = true;
            while (currentFrame < GunImageSprite.Length)
            {
                ShootImage.sprite = GunImageSprite[currentFrame];
                currentFrame++;
                yield return new WaitForSeconds(frameDelay);
            }
            currentFrame = 0;
            isPlaying = false;
            gunImageObject.SetActive(false);
        }

        yield return null;
    }


}


