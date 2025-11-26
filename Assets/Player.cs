using UnityEngine;

public class Player : MonoBehaviour , Iscanlistener
{
    [SerializeField] public Item Item {get; set;}
    [SerializeField] public Gun Weapon {get; set;}
    public float health = 100f;
    public float stamina = 100f;

    Quaternion LookDirection;
    
    public void Die()
    {
        //bruh
    }

    public void ScanDetected(Vector3 scanLocation)
    {
        GetComponent<scan>().StartWave();
    }
}   