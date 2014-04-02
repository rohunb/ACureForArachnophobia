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
               
	}
	void Start()
    {
        Notify();
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
    public Soldier FindNearestInjuredSoldierWithinRange(Soldier soldier, float range)
    {
        Soldier lowestHPSoldier=null;
        List<Soldier> soldiersInRange = new List<Soldier>();
        List<float> distances=new List<float>();
        float lowestDist;
        Vector3 originSoldierPos = soldier.transform.position;
        
        for (int i = 0; i < soldiers.Count; i++)
        {
            if (soldiers[i] != soldier)
            {
                Vector3 targetSoldierPos = soldiers[i].transform.position;
                float xDiff = Mathf.Abs(targetSoldierPos.x - originSoldierPos.x);
                if (xDiff <= range)
                {
                    float yDiff = Mathf.Abs(targetSoldierPos.y - originSoldierPos.y);
                    if (yDiff <= range)
                    {
                        float dist = Vector3.Distance(targetSoldierPos, originSoldierPos);
                        if (dist <= range)
                        {
                            if (soldiers[i].GetComponent<Health>().Alive)
                            {
                                soldiersInRange.Add(soldiers[i]);
                                distances.Add(dist);
                            }
                        }
                    }
                }
            }
        }
        if (soldiersInRange.Count > 0)
        {
            lowestHPSoldier = soldiersInRange[0];
            lowestDist = distances[0];
        }
        else
            return null;

        for (int i = 0; i < soldiersInRange.Count; i++)
        {
            int currentSoldierHP = soldiersInRange[i].GetComponent<Health>().GetHealth;
            int lowestHPSoldierHP = lowestHPSoldier.GetComponent<Health>().GetHealth;
            if (currentSoldierHP < lowestHPSoldierHP)
            {
                lowestHPSoldier = soldiersInRange[i];
                break;
            }
            else if (currentSoldierHP == lowestHPSoldierHP)
            {
                //float distToCurrent = Vector3.Distance(soldier.transform.position, soldiersInRange[i].transform.position);
                //float distToLowest = Vector3.Distance(soldier.transform.position, lowestHPSoldier.transform.position);
                float distToCurrent = distances[i];
                if (distToCurrent < lowestDist)
                {
                    lowestHPSoldier = soldiersInRange[i];
                    lowestDist = distances[i];
                }
            }
        }
        return lowestHPSoldier;
    }
    public void RunInjuredSoldierCheck()
    {
        foreach (Soldier soldier in soldiers)
        {
            if(soldier.currentWeapon.wpnName=="HealingBeam")
            {
                soldier.UpdateLowestHPSoldier();
                return;
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
