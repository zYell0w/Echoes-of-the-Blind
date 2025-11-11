using UnityEngine;

public class Player : MonoBehaviour
{
    public Item Item {get; set;}
    public Gun Weapon {get; set;}
    public float health = 100f;
    public float stamina = 100f;

    Quaternion LookDirection;
    
    public void Die()
    {
        //bruh
    }
}   