using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
    enum GameScene { MainMenu, Game, Win, GameOver }
    [SerializeField]
    GameScene currentScene = GameScene.MainMenu;
    public GUISkin guiSkin;
    public GameObject menuButtonPrefab;

    float buttonSpacing = 2.4f;

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
    void Start()
    {
        GameObject button;
        MenuButton menuButton;
        switch (currentScene)
        {
            case GameScene.MainMenu:
                //title
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("A Cure for Arachnophobia");
                menuButton.clickable = false;

                //Start button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, 0.0f, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Start");
                menuButton.clickable = true;

                //Quit button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, -buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Quit");
                menuButton.clickable = true;
                break;
            case GameScene.Game:
                break;
            case GameScene.Win:
                //title
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("You Win!");
                menuButton.clickable = false;

                //Start button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, 0.0f, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Restart");
                menuButton.clickable = true;

                //Quit button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, -buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Quit");
                menuButton.clickable = true;
                break;
            case GameScene.GameOver:
                //title
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("You Lose...");
                menuButton.clickable = false;

                //Start button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, 0.0f, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Restart");
                menuButton.clickable = true;

                //Quit button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, -buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Quit");
                menuButton.clickable = true;
                break;
            default:
                break;
        }
    }

    void OnLevelWasLoaded(int levelID)
    {
        
        GameObject button;
        MenuButton menuButton;
        switch (currentScene)
        {
            case GameScene.MainMenu:
                //title
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("A Cure for Arachnophobia");
                menuButton.clickable = false;

                //Start button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, 0.0f, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Start");
                menuButton.clickable = true;

                //Quit button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, -buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Quit");
                menuButton.clickable = true;
                break;
            case GameScene.Game:
                break;
            case GameScene.Win:
                //title
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("You Win!");
                menuButton.clickable = false;

                //Start button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, 0.0f, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Restart");
                menuButton.clickable = true;

                //Quit button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, -buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Quit");
                menuButton.clickable = true;
                break;
            case GameScene.GameOver:
                //title
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("You Lose...");
                menuButton.clickable = false;

                //Start button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, 0.0f, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Restart");
                menuButton.clickable = true;

                //Quit button
                button = Instantiate(menuButtonPrefab, new Vector3(0.0f, -buttonSpacing, -1f), Quaternion.identity) as GameObject;
                menuButton = button.GetComponent<MenuButton>();
                menuButton.SetText("Quit");
                menuButton.clickable = true;
                break;
            default:
                break;
        }
	}
    
    //void Update()
    //{
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
    public void OnClick(string buttonName)
    {
        Debug.Log("clicked +" + buttonName);
        switch (buttonName)
        {
            case "Start":
                currentScene = GameScene.Game;
                   Application.LoadLevel("Level1");
                break;
            case "Restart":
                currentScene = GameScene.Game;
                   Application.LoadLevel("Level1");
                break;
            case "Quit":
                Application.Quit();
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
