using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierController : MonoBehaviour {
    
    public List<Soldier> soldiers;
    public List<Soldier> selectedSoldiers;
    
    Soldier[] allUnitsArr;

	// Use this for initialization
	void Awake () {
        allUnitsArr = GameObject.FindObjectsOfType<Soldier>();
        soldiers = new List<Soldier>(allUnitsArr);
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AddNewSoldier(Soldier soldier)
    {
        soldiers.Add(soldier);
        
    }
    
}
