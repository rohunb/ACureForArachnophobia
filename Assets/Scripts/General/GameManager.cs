using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public List<Subject> nodes;
    public Observer nodeWatcher;

    public int MAX_DRONES = 100;

    EnemyController enemyController;

	// Use this for initialization
	void Awake () {
        nodeWatcher.subjects = nodes;
        foreach (Node node in nodes)
        {
            node.Attach(nodeWatcher);
        }



        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
	}
	void Start()
    {
        enemyController.maxDrones = MAX_DRONES;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
