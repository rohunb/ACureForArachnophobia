using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int MAX_DRONES = 100;

    EnemyController enemyController;
    SoldierManager soldierManager;
    

    

    

	// Use this for initialization
	void Awake () {
        //if (currentScene == GameScene.Game)
        {
            enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
            soldierManager = GameObject.FindObjectOfType<SoldierManager>();
            soldierManager.Attach(enemyController);
        }


	}
	void Start()
    {
        enemyController.maxDrones = MAX_DRONES;
        AudioManager.Instance.PlaySound(AudioManager.Sound.BackgroundTrack, true);
    }
	void Update()
    {
        
    }
    /*
    
     * */
}
