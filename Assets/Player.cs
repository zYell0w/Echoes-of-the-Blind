using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour , Iscanlistener
{
     public Item Item {get; set;}
     public Gun Weapon {get; set;}
    [SerializeField] private GameObject finishScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private int nightTime;

    private float counter = 0;
    public float health = 100f;
    public float stamina = 100f;

    Quaternion LookDirection;
    public void Start()
    {
        counter = 0;
    }
    public void Update()
    {
        if (counter < nightTime)
        {
            counter += Time.deltaTime;
        }
        else if (counter >= nightTime) { 
        
            counter = 0;
            AudioManager.instance.Play("WinSound");
            finishScreen.SetActive(true);
            winScreen.SetActive(true);
            StartCoroutine(BekleVeSahneDegis());
        }

    }
    public void Die()
    {
        //bruh
        AudioManager.instance.Play("DeathSound");
        finishScreen.SetActive(true);
        loseScreen.SetActive(true);
        StartCoroutine(BekleVeSahneDegis());
        
    }


    public void ScanDetected(Vector3? scanLocation = null, scan scan = null)
    {
        GetComponent<scan>().StartWave(waveIndex:1);
        AudioManager.instance.Play("EnemyNearSound");    
    }

    IEnumerator BekleVeSahneDegis()
    {
        
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(5f);

        Time.timeScale = 1;
        SceneManager.LoadScene("entry");
    }




}   