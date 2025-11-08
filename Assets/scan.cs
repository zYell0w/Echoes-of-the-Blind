using Unity.Mathematics;
using UnityEngine;

public class scan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public KeyCode waveKey = KeyCode.Space;

    [SerializeField] GameObject scanObject;


    [SerializeField] float duration = 10;
    [SerializeField] float size = 5;
    [SerializeField] float simSpeed = 1;
    


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(waveKey))
        {
            StartWave();
        }
    }

    private void StartWave()
    {
        GameObject terrainscanner = Instantiate(scanObject, transform.position, quaternion.identity) as GameObject;
        ParticleSystem psys = terrainscanner.GetComponentInChildren<ParticleSystem>();

        if (psys != null)
        {
            var a = psys.main;
            a.startLifetime = duration;
            a.startSize = size;
            a.simulationSpeed = simSpeed;
            
        }
        Destroy(terrainscanner, duration + 1);
    }
}
