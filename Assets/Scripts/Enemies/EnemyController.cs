using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : Observer {

    public int maxDrones;
    int numDrones = 0;
    bool canSpawnMoreDrones = false;
    List<SwarmSpawner> spawners;
    KDTree soldierPosTree;
    SoldierManager soldierManager;

	// Use this for initialization
    void Awake()
    {
        SwarmSpawner[] spawnersArr=GameObject.FindObjectsOfType<SwarmSpawner>();
        spawners = new List<SwarmSpawner>(spawnersArr);
        soldierManager = GameObject.FindObjectOfType<SoldierManager>();
        foreach (SwarmSpawner spawner in spawners)
        {
            spawner.Attach(this);
            spawner.GetComponent<Health>().Attach(this);
        }
    }
	void Start () {
        
	}
    public override void UpdateSoldierPos(Vector3[] soldierPos)
    {
        soldierPosTree = KDTree.MakeFromPoints(soldierPos);
        foreach (SwarmSpawner spawner in spawners)
        {
            int nearestIndex=soldierPosTree.FindNearest(spawner.transform.position);
            Soldier nearest=soldierManager.soldiers[nearestIndex];
            if(spawner.nearestSoldier != nearest)
            {
                spawner.UpdateDronesTarget(nearest.transform);
            }
        }
    }
    
    public override void UpdateNumEnemies(int num)
    {
        numDrones+=num;
        if (numDrones >= maxDrones)
            canSpawnMoreDrones = false;
        else
            canSpawnMoreDrones = true;
        NotifySpawnersToSpawn();
    }
    public override void UpdateSpawnerDeath(SwarmSpawner spawner) 
    {
        spawners.Remove(spawner);
    }
    void NotifySpawnersToSpawn()
    {
        foreach (SwarmSpawner spawner in spawners)
        {
            spawner.canSpawn = canSpawnMoreDrones;
        }
    }
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 150, 150));
        GUILayout.BeginVertical();
        GUILayout.Label("Num Spiders: " + numDrones);
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
