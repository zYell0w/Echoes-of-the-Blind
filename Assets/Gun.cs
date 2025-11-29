using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(scan))]

public class Gun : MonoBehaviour, IInteractable , IEquipable , Iscanlistener
{
    private int ammo = 6;

    [SerializeField] GameObject gunHand;
    [SerializeField] Sprite[] GunImageSprite;
    private Image ShootImage;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private float frameDelay = 0.1f;

    private float cooldown = 1f;
    private float cooldownCounter = 0f;

    private bool ready = true;

    LayerMask layerMask;

    scan _scan;

    public void Start()
    {
        _scan = GetComponent<scan>();
        layerMask = LayerMask.GetMask("Default","Enemy");
    }

    void Update()
    {
        if(!ready)
        {
            cooldownCounter+=Time.deltaTime;
        }
        if(cooldownCounter >= cooldown)
        {
            cooldownCounter = 0;
            ready=true;
        }

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
        if(!ready)
            return false;
        
        if(ammo>0)
            ammo--;
        else
        {
            AudioManager.instance.Play("NoAmmoSound");
            return false;
        }
            
        
        RaycastHit hit;
        StartCoroutine(ShowImage(gunHand));
        AudioManager.instance.Play("PistolShootingSound");
        if (Physics.Raycast(transform.position, transform.forward, out hit, int.MaxValue , layerMask))
        {
            //hit.transform.gameObject.GetComponent<IInteractable>().OnInteract(_player);
            _scan.StartWave(position:hit.point);
            if(hit.transform.gameObject.GetComponent<Enemy>()!=null)
            {
                hit.transform.gameObject.GetComponent<Enemy>().Hit();
            }
        }
        ready = false;
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

        public void ScanDetected(Vector3? scanLocation = null, scan scan = null)
    {
        if(scan!=null)
        {
          
            scan.StartWave(position:transform.position,size:2,TriggersEnabled:false);
        }
    }
}


