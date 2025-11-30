using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class mission_controller : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public List<GameObject> missions = new();
    [SerializeField] public List<scan> EnemyAttractingScanners = new();

    [SerializeField] public Enemy enemyPrefab;

    float missionCheckCounter = 0f;
    float missionCheckTime = 15f;
    GameObject player;

    [SerializeField] List<GameObject> objs = new();
    [SerializeField] List<GameObject> yers = new();

  
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        foreach(GameObject mission in GameObject.FindGameObjectsWithTag("Mission"))
        {
            missions.Add(mission);
        }
        objs = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None).ToList<GameObject>();

        foreach(GameObject obj in objs)
        {
            if(obj.GetComponent<Iscanlistener>()!=null)
            {
                player.GetComponent<scan>().colliders.Add(obj.GetComponent<Collider>());

            }
            player.GetComponent<scan>().colliders.Remove(player.GetComponent<Collider>());
           
            
        }
    
        
        objs.Clear();

        foreach(GameObject noise in GameObject.FindGameObjectsWithTag("NoiseMaker"))
                EnemyAttractingScanners.Add(noise.GetComponent<scan>());
            
            int a =Random.Range( (int)Mathf.Round((float)(missions.Count /3)),missions.Count );
            for(int i=0;i<a;i++)
            {
                int b = Random.Range(0,missions.Count);
                int c = Random.Range(0,50);

                missions[b].GetComponent<IMission>().SetCompletion(c);
            }
    }

    void SpawnEnemy(Vector3 pos)
    {
        Enemy enemy = Instantiate(enemyPrefab, pos,Quaternion.identity);
        foreach(scan scan in EnemyAttractingScanners)
        {
            scan.colliders.Add(enemy.GetComponent<Collider>());
        }
        player.GetComponent<scan>().colliders.Add(enemy.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        missionCheckCounter+=Time.deltaTime;
        if(missionCheckCounter>=missionCheckTime)
        {
            missionCheckCounter=0;
            List<GameObject> incompleteMissions = new();
            foreach(GameObject mission in missions)
            {
                if(!mission.GetComponent<IMission>().IsDone())
                    incompleteMissions.Add(mission);
            }
            if(incompleteMissions.Count>0)
            {
                int a = Random.Range(0,incompleteMissions.Count);
                Vector3 spawnPoint = incompleteMissions[a].GetComponent<IMission>().GetSpawnPointForEnemy();
                SpawnEnemy(spawnPoint);
            }
        }
        
    }
}
