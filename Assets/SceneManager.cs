using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
    enum GameScene { MainMenu, Game, Win, GameOver }
    GameScene currentScene = GameScene.MainMenu;
    public GUISkin guiSkin;

    private static SceneManager instance=null;
    public static SceneManager Instance
    {
        get{return instance;}
    }

    void Awake()
    {
        if(instance!=null)
        {
            Debug.LogError(name +"error: already initialized",null);
            Destroy(gameObject);
        }
        instance=this;
        DontDestroyOnLoad(gameObject);
    }


	// Use this for initialization
	void Start () {
        switch (Application.loadedLevelName)
        {
            case "Level1":
                currentScene = GameScene.Game;
                break;
            case "WinScene":
                currentScene = GameScene.Win;
                break;
            case "GameOver":
                currentScene = GameScene.GameOver;
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
    //void Update () {
    //    switch (currentScene)
    //    {
    //        case GameScene.MainMenu:

    //            break;
    //        case GameScene.Game:
    //            break;
    //        case GameScene.Win:
    //            break;
    //        case GameScene.GameOver:
    //            break;
    //        default:
    //            break;
    //    }
    //}
    void OnGUI()
    {
        GUI.skin = guiSkin;
        switch (currentScene)
        {
            case GameScene.MainMenu:
                GUI.Label(new Rect(Screen.width * .375f, Screen.height * .30f, Screen.width * .5f, Screen.height * .2f), "A Cure for Arachnophobia");
                if (GUI.Button(new Rect(Screen.width * .35f, Screen.height * .44f, Screen.width * .2f, Screen.height * .06f), "New Game"))
                {
                    currentScene = GameScene.Game;
                    Application.LoadLevel(1);
                    // fadeToGameScene = true;
                }

                if (GUI.Button(new Rect(Screen.width * .35f, Screen.height * .54f, Screen.width * .2f, Screen.height * .06f), "Quit"))
                {
                    Application.Quit();
                }
                break;
            case GameScene.Game:
                break;
            case GameScene.Win:
                GUI.Label(new Rect(Screen.width * .35f, Screen.height * .24f, Screen.width * .2f, Screen.height * .06f), "You Win!");
                if (GUI.Button(new Rect(Screen.width * .35f, Screen.height * .44f, Screen.width * .2f, Screen.height * .06f), "Restart"))
                {
                    currentScene = GameScene.Game;
                    Application.LoadLevel(1);
                    // fadeToGameScene = true;
                }

                if (GUI.Button(new Rect(Screen.width * .35f, Screen.height * .54f, Screen.width * .2f, Screen.height * .06f), "Quit"))
                {
                    Application.Quit();
                }
                break;
            case GameScene.GameOver:
                GUI.Label(new Rect(Screen.width * .35f, Screen.height * .24f, Screen.width * .2f, Screen.height * .06f), "You Lose...");
                if (GUI.Button(new Rect(Screen.width * .35f, Screen.height * .44f, Screen.width * .2f, Screen.height * .06f), "Restart"))
                {
                    currentScene = GameScene.Game;
                    Application.LoadLevel(1);
                    // fadeToGameScene = true;
                }

                if (GUI.Button(new Rect(Screen.width * .35f, Screen.height * .54f, Screen.width * .2f, Screen.height * .06f), "Quit"))
                {
                    Application.Quit();
                }
                break;
            default:
                break;
        }
    }
    public void GameEnd(bool win)
    {
        if (win)
        {
            currentScene = GameScene.Win;
            Application.LoadLevel("WinScene");
        }
        else
        {
            currentScene = GameScene.GameOver;
            Application.LoadLevel("GameOver");
        }
    }
}
