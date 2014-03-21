using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public List<Subject> nodes;
    public Observer nodeWatcher;

    public int MAX_DRONES = 100;

    EnemyController enemyController;
    SoldierManager soldierManager;

	// Use this for initialization
	void Awake () {

        nodeWatcher.subjects = nodes;
        foreach (Node node in nodes)
        {
            node.Attach(nodeWatcher);
        }

        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        soldierManager = GameObject.FindObjectOfType<SoldierManager>();
	}
	void Start()
    {
        enemyController.maxDrones = MAX_DRONES;
        soldierManager.Attach(enemyController);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
