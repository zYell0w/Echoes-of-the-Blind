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
        AudioManager.instance.Play("DeathSound");
    }

   

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            Die();
            Debug.Log("bi bok olmuyo");
        }
    }

    public void ScanDetected(Vector3? scanLocation = null, scan scan = null)
    {
        GetComponent<scan>().StartWave(waveIndex:1);
        AudioManager.instance.Play("EnemyNearSound");    
    }
}   