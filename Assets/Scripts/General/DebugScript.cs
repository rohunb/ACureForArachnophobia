using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour {

    bool debugModeOn = false;

    SoldierManager soldierManager;

    void Awake()
    {
        soldierManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoldierManager>();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
        {
            debugModeOn = !debugModeOn;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space))
        {
            debugModeOn = !debugModeOn;
        }
        if(debugModeOn)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                soldierManager.soldiers[0].GetComponent<Health>().UpdateHealth(-10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                soldierManager.soldiers[1].GetComponent<Health>().UpdateHealth(-10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                soldierManager.soldiers[2].GetComponent<Health>().UpdateHealth(-10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                soldierManager.soldiers[3].GetComponent<Health>().UpdateHealth(-10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                soldierManager.soldiers[4].GetComponent<Health>().UpdateHealth(-10);
            }

        }
	}
    void OnGUI()
    {
        if (debugModeOn)
            GUI.Label(new Rect(Screen.width - 200, Screen.height-40, 200, 40), "<color=red><size=24>Debug Mode On</size></color>");
    }

}
