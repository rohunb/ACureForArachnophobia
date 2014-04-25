using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int MAX_DRONES = 100;

    EnemyController enemyController;
    SoldierManager soldierManager;

    void Awake()
    {
        {
            enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
            soldierManager = GameObject.FindObjectOfType<SoldierManager>();
            soldierManager.Attach(enemyController);
        }
    }
    void Start()
    {
        enemyController.maxDrones = MAX_DRONES;
    }

}
