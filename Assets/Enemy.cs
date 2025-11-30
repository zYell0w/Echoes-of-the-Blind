using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour , Iscanlistener
{
    bool step = false;

    [SerializeField] private float stepLength = 1.0f;

    float baseSpeed = 2;
    private scan scan;
    private bool hitted = false;
    NavMeshAgent agent;
    private float health = 100;

    [SerializeField] public Vector3 target;

    float stepCounter = 0;
    float stepTime = 0.9f;

    [SerializeField] GameObject [] feet = {null,null};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        target = transform.position;
        //agent.isStopped = true;
        scan = GetComponent<scan>();
        foreach(GameObject foot in feet)
        {
            foot.SetActive(false);
        }
        
    }

    void Step(bool withWaves = true)
    {
        GameObject a;
        if(step)
        {
            
            a = Instantiate(feet[0].gameObject);
            a.transform.position  = feet[0].transform.position;
            a.transform.rotation = feet[0].transform.rotation;
            if(!hitted)
                AudioManager.instance.Play("EnemyStepSound", position: transform.position);

            step =!step;
        }
        else
        {

            a = Instantiate(feet[1].gameObject);
            a.transform.position  = feet[1].transform.position;
            a.transform.rotation = feet[1].transform.rotation;
            if(!hitted)
                AudioManager.instance.Play("EnemyStepSound", position: transform.position);
            step =!step;

        }
        a.SetActive(true);
        Destroy(a,1.0f);
        
        scan.StartWave(position:a.transform.position,size:8f,TriggersEnabled: withWaves);
        stepCounter = 0;
        

    }

    public void Hit()
    {
        StartCoroutine(hitCourontine());
        AudioManager.instance.Stop("MonsterHitRoar");
        AudioManager.instance.Play("MonsterHitRoar",position:transform.position);
        health -= 25f;
    }

     public void Slow()
    {
        agent.speed=baseSpeed/2;
    }
     public void UnSlow()
    {
        agent.speed=baseSpeed;
    }

    IEnumerator hitCourontine()
    {
        float counter = 0;
        hitted = true;
        Step();
        while(counter<=5)
        {
            counter+=Time.deltaTime;
            target = transform.position;
            Step(false);
            yield return null;
        }
        Step();
        hitted = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(agent.destination!=target)
            agent.destination = target;

        if(agent.remainingDistance!=0)
            stepCounter+=Time.deltaTime;

        if (hitted == false) {

            AudioManager.instance.Play("EnemyBreathing",position: transform.position);
        
        }

        if (health <= 0)
        {
            AudioManager.instance.Stop("MonsterHitRoar");
            AudioManager.instance.Play("MonsterDeath", transform.position);
            Enemy.Destroy(gameObject,0.5f);
        }

        if(stepCounter >= stepTime)
        {
            Step();
        }

        
    }

    

    public void ScanDetected(Vector3? scanLocation = null, scan scan = null)
    {
        if(scanLocation!=null)
            target = (Vector3)scanLocation;
        if(stepCounter>=stepTime/3)
            Step();
    }
}
