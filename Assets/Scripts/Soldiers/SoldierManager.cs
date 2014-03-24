using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierManager : Subject {
    
    public List<Soldier> soldiers;
    public List<Soldier> selectedSoldiers;

    Soldier[] allUnitsArr;
    List<Vector3> prevSoldierPos;

	// Use this for initialization
	void Awake () {
        allUnitsArr = GameObject.FindObjectsOfType<Soldier>();
        soldiers = new List<Soldier>(allUnitsArr);
        prevSoldierPos = new List<Vector3>();
        for (int i = 0; i < soldiers.Count; i++)
        {
            soldiers[i].soldierSight.GetComponent<SoldierSight>().Attach(soldiers[i]);
            prevSoldierPos.Add(soldiers[i].transform.position);
        }
        Notify();
               
	}
	void Start()
    {
        
    }
	// Update is called once per frame
	void Update () {
        bool toNotify = false;
        for (int i = 0; i < soldiers.Count; i++)
        {
            if(soldiers[i].transform.position!=prevSoldierPos[i])
            {
                toNotify = true;
            }
            prevSoldierPos[i] = soldiers[i].transform.position;
            if (toNotify)
            {
                Notify();
            }

        }
	}
    public void AddNewSoldier(Soldier soldier)
    {
        soldiers.Add(soldier);
        
    }
    public override void Notify()
    {
        foreach (Observer obs in observers)
        {
            obs.UpdateSoldierPos(prevSoldierPos.ToArray());
        }
    }
    
    
}
