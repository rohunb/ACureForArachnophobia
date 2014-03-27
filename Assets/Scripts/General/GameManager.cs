using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int MAX_DRONES = 100;

    EnemyController enemyController;
    SoldierManager soldierManager;

	// Use this for initialization
	void Awake () {

        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        soldierManager = GameObject.FindObjectOfType<SoldierManager>();
        soldierManager.Attach(enemyController);

	}
	void Start()
    {
        enemyController.maxDrones = MAX_DRONES;
    }
	
    void OnGUI()
    {
        GUI.Label(new Rect(10, 50, 100, 30), "Num Bullets: " + GameObject.FindObjectsOfType<ProjectileDamager>().Length);
    }
}
