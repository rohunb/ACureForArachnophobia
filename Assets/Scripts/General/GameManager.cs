using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int MAX_DRONES = 100;

    EnemyController enemyController;
    SoldierManager soldierManager;
    

    enum GameScene { MainMenu, Game, Pause, GameOver}
    GameScene currentScene = GameScene.MainMenu;

    public GUISkin guiSkin;

	// Use this for initialization
	void Awake () {
        if (currentScene == GameScene.Game)
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
        GUI.skin = guiSkin;
        switch (currentScene)
        {
            case GameScene.MainMenu:
                if (GUI.Button(new Rect(Screen.width * .35f, Screen.height * .24f, Screen.width * .2f, Screen.height * .06f), "New Game"))
            {
                currentScene = GameScene.Game;
                Application.LoadLevel(1);
               // fadeToGameScene = true;
            }
            
            if (GUI.Button(new Rect(Screen.width * .35f, Screen.height * .64f, Screen.width * .2f, Screen.height * .06f), "Quit"))
            {
                Application.Quit();
            }
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
}
