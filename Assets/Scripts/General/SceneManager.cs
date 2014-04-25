using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    enum GameScene { MainMenu, Game, Win, GameOver }
    GameScene currentScene = GameScene.MainMenu;

    public GameObject menuButtonPrefab;

    float buttonSpacing = 2.4f;
    bool loading = false;

    private static SceneManager instance = null;
    public static SceneManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError(name + "error: already initialized", null);
            Destroy(gameObject);
        }
        instance = this;
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
                AudioManager.Instance.PlaySound(AudioManager.Sound.MainMenuBackgroundTrack,true);
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
                AudioManager.Instance.PlaySound(AudioManager.Sound.GameBackgroundTrack, true);
                break;
            case GameScene.Win:
                AudioManager.Instance.PlaySound(AudioManager.Sound.WinBackgroundTrack, true);
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
                AudioManager.Instance.PlaySound(AudioManager.Sound.GameOverBackgroundTrack, true);
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
        loading = false;
        GameObject button;
        MenuButton menuButton;
        switch (currentScene)
        {
            case GameScene.MainMenu:
                AudioManager.Instance.PlaySound(AudioManager.Sound.MainMenuBackgroundTrack, true);
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
                AudioManager.Instance.PlaySound(AudioManager.Sound.GameBackgroundTrack, true);
                break;
            case GameScene.Win:
                AudioManager.Instance.PlaySound(AudioManager.Sound.WinBackgroundTrack, true);
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
                AudioManager.Instance.PlaySound(AudioManager.Sound.GameOverBackgroundTrack, true);
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
    void OnGUI()
    {
        if(loading)
        {
            GUI.Label(new Rect(Screen.width / 2f - 50f, Screen.height- 50f, 300f, 100f), "<size=36><color=black>Loading...</color></size>");
        }
    }
    public void OnClick(string buttonName)
    {

        switch (buttonName)
        {
            case "Start":
                currentScene = GameScene.Game;
                loading = true;
                Application.LoadLevel("Level1");
                break;
            case "Restart":
                currentScene = GameScene.Game;
                loading = true;
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
