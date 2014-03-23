﻿using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour {

    public GameObject weaponsWindow;
    public GameObject weaponsButton;
    public GameObject MP5Button;
    public GameObject shotgunButton;
    public GameObject lightningButton;
    public GameObject flameButton;
    public GameObject closeButton;

    public GameObject MP5;
    public GameObject shotgun;
    public GameObject lightningGun;
    public GameObject flamethrower;

    public Vector4 windowSizeClosed=new Vector4(-8.55f,-4.02f,2.33f,2.06f);
    private Vector4 windowSizeOpen=new Vector4(-5.50f,-4.02f,9.2f,2.06f);

    public Vector2 basePosition=new Vector2(.6f,.38f);
    public Vector2 hiddenPosition = new Vector2(-1.7f, .38f);

    public Vector2 closeButtonOpenPos = new Vector2(1.65f, -.83f);
    public Vector2 closeButtonClosedPos = new Vector2(-6.54f, -.83f);
    public float menuAnimationSpeed = 1f;
    public float animationBuffer = 0.01f;
    enum MenuState { Open, Opening, Closed, Closing }
    MenuState menuState = MenuState.Closed;

    Vector4 windowSizeCurrent;
    Vector2 closeButtonCurrentPos;
    Vector2[] currentButtonPos;

    public Camera guiCam;
    public int guiLayer = 10;

    void Awake()
    {

    }
    // Use this for initialization
	void Start () {
        windowSizeCurrent = windowSizeClosed;
        closeButtonCurrentPos = closeButtonClosedPos;
        currentButtonPos = new Vector2[5];
        for (int i = 0; i < currentButtonPos.Length; i++)
        {
            currentButtonPos[i] = new Vector2(hiddenPosition.x, hiddenPosition.y);
            
        }
        //wpns button pos
        currentButtonPos[0] = basePosition;
	}
	
	// Update is called once per frame
	void Update () {

        CheckButtonSelect();
        switch (menuState)
        {
            case MenuState.Open:
                if (Input.GetKeyDown(KeyCode.Escape))
                    menuState = MenuState.Closing;
                //ActivateOpenWeaponsMenu();
                break;
            case MenuState.Opening:
                if (Input.GetKeyDown(KeyCode.Escape))
                    menuState = MenuState.Closing;
                AnimateWindowOpening();
                AnimateWeaponsOpening();
                break;
            case MenuState.Closed:
                if (Input.GetKeyDown(KeyCode.V))
                    menuState = MenuState.Opening;
                //ActivateClosedWeaponsMenu();
                break;
            case MenuState.Closing:
                if (Input.GetKeyDown(KeyCode.V))
                    menuState = MenuState.Opening;
                AnimateWindowClosing();
                AnimateWeaponsClosing();
                break;
            default:
                break;
        }
        DrawWeaponsWindow();
        DrawWeaponButtons();   

	}
    void CheckButtonSelect()
    {
        Ray ray = guiCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f,1<<guiLayer))
        {
            if(Input.GetMouseButtonUp(0))
            {
                switch (hit.transform.gameObject.name)
                {
                    case "WeaponButton":
                        menuState = MenuState.Opening;
                        break;
                    case "MP5Button":
                        Debug.Log("mp5");
                        break;
                    case "ShotgunButton":
                        Debug.Log("shot");
                        break;
                    case "LightningButton":
                        Debug.Log("light");
                        break;
                    case "FlameButton":
                        Debug.Log("flem");
                        break;
                    case "CloseButton":
                        Debug.Log("Close");
                            menuState=MenuState.Closing;
                            break;
                    default:
                        break;
                }
            }
        }
    }

    void AnimateWeaponsOpening()
    {
        currentButtonPos[0].x = Mathf.Lerp(currentButtonPos[0].x, hiddenPosition.x, Time.deltaTime * menuAnimationSpeed);
        if (Vector2.Distance(currentButtonPos[0], hiddenPosition) < animationBuffer)
        {
            currentButtonPos[0] = hiddenPosition;
        }
        for (int i = 1; i < currentButtonPos.Length; i++)
        {
            Vector2 openPos = new Vector2(basePosition.x + (i - 1) * 1.8f, basePosition.y);
            currentButtonPos[i].x = Mathf.Lerp(currentButtonPos[i].x, openPos.x, Time.deltaTime * menuAnimationSpeed);
            if (Vector2.Distance(currentButtonPos[i], openPos) < animationBuffer)
            {
                currentButtonPos[i] = openPos;
            }
        }
        closeButtonCurrentPos.x = Mathf.Lerp(closeButtonCurrentPos.x, closeButtonOpenPos.x, Time.deltaTime * menuAnimationSpeed);
        if(Vector2.Distance(closeButtonCurrentPos,closeButtonOpenPos)<animationBuffer)
        {
            closeButtonCurrentPos = closeButtonOpenPos;
        }


    }
    void AnimateWeaponsClosing()
    {
        currentButtonPos[0].x = Mathf.Lerp(currentButtonPos[0].x, basePosition.x, Time.deltaTime * menuAnimationSpeed);
        if (Vector2.Distance(currentButtonPos[0], basePosition) < animationBuffer)
        {
            currentButtonPos[0] = basePosition;
        }
        for (int i = 1; i < currentButtonPos.Length; i++)
        {
            currentButtonPos[i].x = Mathf.Lerp(currentButtonPos[i].x, hiddenPosition.x, Time.deltaTime * menuAnimationSpeed);
            if (Vector2.Distance(currentButtonPos[i], hiddenPosition) < animationBuffer)
            {
                currentButtonPos[i] = hiddenPosition;
            }
        }
        closeButtonCurrentPos.x = Mathf.Lerp(closeButtonCurrentPos.x, closeButtonClosedPos.x, Time.deltaTime * menuAnimationSpeed);
        if (Vector2.Distance(closeButtonCurrentPos, closeButtonClosedPos) < animationBuffer)
        {
            closeButtonCurrentPos = closeButtonClosedPos;
        }
    }
    void AnimateWindowOpening()
    {
        windowSizeCurrent.x = Mathf.Lerp(windowSizeCurrent.x, windowSizeOpen.x, Time.deltaTime * menuAnimationSpeed);
        windowSizeCurrent.z = Mathf.Lerp(windowSizeCurrent.z, windowSizeOpen.z, Time.deltaTime * menuAnimationSpeed);
        windowSizeCurrent.w = Mathf.Lerp(windowSizeCurrent.w, windowSizeOpen.w, Time.deltaTime * menuAnimationSpeed);
        if (Vector4.Distance(windowSizeCurrent, windowSizeOpen) < animationBuffer)
        {
            windowSizeCurrent = windowSizeOpen;
            menuState = MenuState.Open;
        }
    }
    
    void AnimateWindowClosing()
    {
        windowSizeCurrent.x = Mathf.Lerp(windowSizeCurrent.x, windowSizeClosed.x, Time.deltaTime * menuAnimationSpeed);
        windowSizeCurrent.z = Mathf.Lerp(windowSizeCurrent.z, windowSizeClosed.z, Time.deltaTime * menuAnimationSpeed);
        windowSizeCurrent.w = Mathf.Lerp(windowSizeCurrent.w, windowSizeClosed.w, Time.deltaTime * menuAnimationSpeed);
        if (Vector4.Distance(windowSizeCurrent, windowSizeClosed) < animationBuffer)
        {
            windowSizeCurrent = windowSizeClosed;
            menuState = MenuState.Closed;
        }
    }
    void DrawWeaponButtons()
    {
        weaponsButton.transform.position = new Vector3(currentButtonPos[0].x, currentButtonPos[0].y, weaponsButton.transform.position.z);
        MP5Button.transform.position = new Vector3(currentButtonPos[1].x, currentButtonPos[1].y, MP5Button.transform.position.z);
        shotgunButton.transform.position = new Vector3(currentButtonPos[2].x, currentButtonPos[2].y, shotgunButton.transform.position.z);
        lightningButton.transform.position = new Vector3(currentButtonPos[3].x, currentButtonPos[3].y, lightningButton.transform.position.z);
        flameButton.transform.position = new Vector3(currentButtonPos[4].x, currentButtonPos[4].y, flameButton.transform.position.z);
        closeButton.transform.position = new Vector3(closeButtonCurrentPos.x, closeButtonCurrentPos.y, closeButton.transform.position.z);
    }
    void DrawWeaponsWindow()
    {
        weaponsWindow.transform.position = new Vector3(windowSizeCurrent.x, windowSizeCurrent.y, weaponsWindow.transform.position.z);
        weaponsWindow.transform.localScale = new Vector3(windowSizeCurrent.z, windowSizeCurrent.w, weaponsWindow.transform.localScale.z);
    }
    void ActivateClosedWeaponsMenu()
    {
        weaponsButton.SetActive(true);
           MP5Button.SetActive(false);
    shotgunButton.SetActive(false);
    lightningButton.SetActive(false);
    flameButton.SetActive(false);

    }
    void ActivateOpenWeaponsMenu()
    {
        weaponsButton.SetActive(false);
        MP5Button.SetActive(true);
        shotgunButton.SetActive(true);
        lightningButton.SetActive(true);
        flameButton.SetActive(true);
    }
}

