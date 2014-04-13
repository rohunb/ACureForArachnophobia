using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int MAX_DRONES = 100;

    EnemyController enemyController;
    SoldierManager soldierManager;

    enum GameScene { MainMenu, Game, Pause, GameOver}
    GameScene currentScene = GameScene.MainMenu;

	// Use this for initialization
	void Awake () {

        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        soldierManager = GameObject.FindObjectOfType<SoldierManager>();
        soldierManager.Attach(enemyController);

	}
	void Start()
    {
        enemyController.maxDrones = MAX_DRONES;
        AudioManager.Instance.PlaySound(AudioManager.Sound.BackgroundTrack, true);
    }
	void Update()
    {
        switch (currentScene)
        {
            case GameScene.MainMenu:

                break;
            case GameScene.Game:
                break;
            case GameScene.Pause:
                break;
            case GameScene.GameOver:
                break;
            default:
                break;
        }
    }
    void OnGUI()
    {

    }
}
