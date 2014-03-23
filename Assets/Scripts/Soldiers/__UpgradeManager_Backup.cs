using UnityEngine;
using System.Collections;

public class __UpgradeManager_Backup : MonoBehaviour {


    public GUISkin guiskin;


     Rect upgradeWindowRect = new Rect(10, Screen.height - 120, 120, 110);
     Rect buttonRect = new Rect(0, 33, 135, 70);
     Rect weaponTexRect = new Rect(17, 50, 95, 45);
     Rect weaponLabelRect=new Rect(22,0,95,95);

    public Texture2D weaponTex;
    public Texture2D MP5tex;
    public Texture2D shotgunTex;
    public Texture2D lightningGunTex;
    public Texture2D flameTex;

    public float menuAnimationSpeed = 1f;
    public float windowWidthClosed = 120f;
    public float windowWidthOpen = 120f * 5f;

    public GameObject MP5;
    public GameObject shotgun;
    public GameObject lightningGun;
    public GameObject flameThrower;

    public Rect[] wpnButtonPositions;

    float currentWindowWidth;

    enum MenuState { Open, Opening, Closed, Closing}
    MenuState menuState = MenuState.Closed;


    void Start()
    {
        currentWindowWidth = windowWidthClosed;
        
    }

    private int credits;
    public int GetCredits()
    {
        return credits;
    }
    public bool CreateTransaction(int amount)
    {
        if (credits + amount >= 0)
        {
            credits += amount;
            return true;
        }
        else
            return false;
    }
    void Update()
    {

        switch (menuState)
        {
            case MenuState.Open:
                if (Input.GetKeyDown(KeyCode.Escape))
                    menuState = MenuState.Closing;
                break;
            case MenuState.Opening:
                if (Input.GetKeyDown(KeyCode.Escape))
                    menuState = MenuState.Closing;
                currentWindowWidth = Mathf.Lerp(currentWindowWidth, windowWidthOpen, Time.deltaTime * menuAnimationSpeed);
                break;
            case MenuState.Closed:
                if (Input.GetKeyDown(KeyCode.V))
                    menuState = MenuState.Opening;
                break;
            case MenuState.Closing:
                if (Input.GetKeyDown(KeyCode.V))
                    menuState = MenuState.Opening;
                currentWindowWidth = Mathf.Lerp(currentWindowWidth, windowWidthClosed, Time.deltaTime * menuAnimationSpeed);
                break;
            default:
                break;
        }

        //if(Input.GetKeyDown(KeyCode.V))
        //    weaponsMenuOpen=true;
        //if(Input.GetKeyDown(KeyCode.Escape))
        //    weaponsMenuOpen=false;
    }
    void OnGUI()
    {
        GUI.skin = guiskin;
        //DisplayUpgradeButtons();

        switch (menuState)
        {
            case MenuState.Open:
                upgradeWindowRect.width = currentWindowWidth;
                upgradeWindowRect = GUI.Window(0, upgradeWindowRect, UpgradeWindowOpen, "");
                break;
            case MenuState.Opening:
                upgradeWindowRect.width = currentWindowWidth;
                upgradeWindowRect = GUI.Window(0, upgradeWindowRect, UpgradeWindowOpen, "");
                break;
            case MenuState.Closed:
                upgradeWindowRect.width = currentWindowWidth;
                upgradeWindowRect = GUI.Window(0, upgradeWindowRect, UpgradeWindowClosed, "");
                break;
            case MenuState.Closing:
                upgradeWindowRect.width = currentWindowWidth;
                upgradeWindowRect = GUI.Window(0, upgradeWindowRect, UpgradeWindowClosed, "");
                break;
            default:
                break;
        }
        
        

    }
    void UpgradeWindowOpen(int windowID)
    {

    }
    void UpgradeWindowClosed(int windowID)
    {
        
        GUI.DrawTexture(weaponTexRect, weaponTex);
        GUI.Label(weaponLabelRect, "Weapons");
        if (GUI.Button(buttonRect, ""))
        {
            menuState = MenuState.Opening;
        }
        
    }
    


}
