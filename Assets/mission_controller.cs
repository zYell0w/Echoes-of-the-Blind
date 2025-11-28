using System.Collections.Generic;
using UnityEngine;

public class mission_controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public List<GameObject> missions = new();
    [SerializeField] public Enemy enemy_prefab;
    //rastgele görev atama<<
    void Start()
    {
        foreach(GameObject mission in GameObject.FindGameObjectsWithTag("Mission"))
            missions.Add(mission);
        Random.Range(0,missions.Count);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
