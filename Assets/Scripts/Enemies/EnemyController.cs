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
    UpgradeManager upgradeManager;

	// Use this for initialization
    void Awake()
    {
        SwarmSpawner[] spawnersArr=GameObject.FindObjectsOfType<SwarmSpawner>();
        spawners = new List<SwarmSpawner>(spawnersArr);
        soldierManager = GameObject.FindObjectOfType<SoldierManager>();
        upgradeManager = GameObject.FindObjectOfType<UpgradeManager>();
        foreach (SwarmSpawner spawner in spawners)
        {
            spawner.Attach(this);
            Health health = spawner.GetComponent<Health>();
            health.Attach(this);
            health.Attach(upgradeManager);
        }
    }
	void Start () {
        
	}
    public override void UpdateSoldierPos(Vector3[] soldierPos)
    {
        if (soldierPos.Length > 0)
        {
            soldierPosTree = KDTree.MakeFromPoints(soldierPos);
            foreach (SwarmSpawner spawner in spawners)
            {
                int nearestIndex = soldierPosTree.FindNearest(spawner.transform.position);
                Soldier nearest = soldierManager.soldiers[nearestIndex];
                if (!spawner.nearestSoldier || spawner.nearestSoldier != nearest)
                {
                    spawner.UpdateDronesTarget(nearest.transform);
                }
            }
        }
        else
        {
            foreach (SwarmSpawner spawner in spawners)
            {
                spawner.UpdateDronesTarget(null);
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
        if(spawners.Count<=0)
        {
            SceneManager.Instance.GameEnd(true);
        }
    }
    void NotifySpawnersToSpawn()
    {
        foreach (SwarmSpawner spawner in spawners)
        {
            spawner.canSpawn = canSpawnMoreDrones;
        }
    }
    
}
