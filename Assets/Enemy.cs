using System.Collections;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour , Iscanlistener
{
    bool step = false;

    [SerializeField] private float stepLength = 1.0f;
    private scan scan;
    NavMeshAgent agent; 

    [SerializeField] public Vector3 target;

    float stepCounter = 0;
    float stepTime = 1.0f;

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

    void Step()
    {
        GameObject a;
        if(step)
        {
            a = Instantiate(feet[0].gameObject);
            a.transform.position  = feet[0].transform.position;
            a.transform.rotation = feet[0].transform.rotation;

            step=!step;
        }
        else
        {
            a = Instantiate(feet[1].gameObject);
            a.transform.position  = feet[1].transform.position;
            a.transform.rotation = feet[1].transform.rotation;
            step=!step;

        }
        a.SetActive(true);
        Destroy(a,1.0f);
        scan.StartWave(position:a.transform.position,size:8f);
        stepCounter = 0;
        

    }

    public void Hit()
    {
        StartCoroutine(hitCourontine());
        AudioManager.instance.Play("MonsterHitRoar");
    }

    IEnumerator hitCourontine()
    {
        float counter = 0;
        while(counter<=5)
        {
            counter+=Time.deltaTime;
            target = transform.position;
            Step();
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(agent.destination!=target)
            agent.destination = target;

        if(agent.remainingDistance!=0)
            stepCounter+=Time.deltaTime;

        

        if(stepCounter >= stepTime)
        {
            Step();
        }
    }

    public void ScanDetected(Vector3 scanLocation)
    {
        target = scanLocation;
        if(stepCounter>=stepTime/3)
            Step();
    }
}
