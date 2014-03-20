using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : Observer {

    public int maxDrones;
    int numDrones = 0;
    bool canSpawnMoreDrones = false;
    SwarmSpawner[] spawners;

	// Use this for initialization
    void Awake()
    {
        spawners = GameObject.FindObjectsOfType<SwarmSpawner>();
    }
	void Start () {
        foreach (SwarmSpawner spawner in spawners)
        {
            spawner.Attach(this);
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
