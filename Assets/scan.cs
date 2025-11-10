using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class scan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] GameObject scanObject;


    [SerializeField] public float duration = 10;
    [SerializeField] public float size = 5;
    [SerializeField] public float simSpeed = 1;

    [SerializeField] public List<Collider> colliders = new();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(float? duration = null,
    float? size = null,
    float? simSpeed = null,
    Vector3? position = null)
    {
        GameObject terrainscanner;
        if (position != null)
            terrainscanner = Instantiate(scanObject, (Vector3)position, quaternion.identity) as GameObject;
        else
            terrainscanner = Instantiate(scanObject, transform.position, quaternion.identity) as GameObject;

        ParticleSystem psys = terrainscanner.GetComponentInChildren<ParticleSystem>();

        if (psys != null)
        {
            var a = psys.main;
            colliders.ForEach(delegate(Collider col)
            {
                psys.trigger.AddCollider(col);
             });

            if (duration != null)
                a.startLifetime = (float)duration;
            else
                a.startLifetime = this.duration;

            if (size != null)
                a.startSize = (float)size;
            else
                a.startSize = this.size;

            if (simSpeed != null)
                a.simulationSpeed = (float)simSpeed;
            else
                a.simulationSpeed = this.simSpeed;



        }
        if (duration != null)
            Destroy(terrainscanner, (float)duration + 1);
        else
            Destroy(terrainscanner, this.duration + 1);

        

        
    }
    
   
}
